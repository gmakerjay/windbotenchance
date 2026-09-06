using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace YgoAiPlatform.Core
{
    public class IpcWorker : IDisposable
    {
        private readonly int _repPort;
        private readonly int _pubPort;
        private readonly string _duelId;
        private readonly EventBus? _eventBus;
        private readonly Channel<string>? _eventChannel;
        
        private ResponseSocket? _repSocket;
        private PublisherSocket? _pubSocket;
        private CancellationTokenSource? _cts;
        private Task? _workerTask;
        private Task? _publisherTask;

        private volatile bool _isPaused = false;
        private volatile bool _isRunning = false;

        // CB-001: เพิ่ม property และ method เพื่อให้ pause ทำงานจริงและ testable ได้
        public bool IsPaused => _isPaused;

        /// <summary>
        /// ตั้งค่า pause state — ใช้ในการทดสอบหรือการสั่งคำสั่งจากภายนอก
        /// </summary>
        public void SetPaused(bool paused) => _isPaused = paused;

        public bool IsRunning => _isRunning && (_cts == null || !_cts.IsCancellationRequested);

        public IpcWorker(int repPort, int pubPort, string duelId, EventBus? eventBus = null)
        {
            _repPort = repPort;
            _pubPort = pubPort;
            _duelId = duelId;
            _eventBus = eventBus;

            if (_eventBus != null)
            {
                // Create a bounded channel to buffer events for ZMQ publishing
                _eventChannel = Channel.CreateBounded<string>(
                    new BoundedChannelOptions(1000)
                    {
                        FullMode = BoundedChannelFullMode.DropOldest,
                        SingleReader = true,
                        SingleWriter = false
                    });
            }
        }

        public void Start()
        {
            if (_isRunning) return;
            
            _isRunning = true;
            _cts = new CancellationTokenSource();

            // 1. เริ่ม Socket REQ/REP
            _repSocket = new ResponseSocket();
            _repSocket.Bind($"tcp://127.0.0.1:{_repPort}");

            // 2. เริ่ม Socket PUB/SUB
            _pubSocket = new PublisherSocket();
            _pubSocket.Bind($"tcp://127.0.0.1:{_pubPort}");

            Console.WriteLine($"[IpcWorker] Bound REP socket to port {_repPort}");
            Console.WriteLine($"[IpcWorker] Bound PUB socket to port {_pubPort}");

            // Subscribe to all EventBus event types if EventBus is provided
            if (_eventBus != null && _eventChannel != null)
            {
                foreach (DuelEventType eventType in Enum.GetValues(typeof(DuelEventType)))
                {
                    _eventBus.Subscribe(eventType, (duelEvent) =>
                    {
                        string topic = IsAiDecisionEvent(duelEvent) ? "ai.decision" : "duel.event";
                        string json = JsonSerializer.Serialize(duelEvent);
                        string message = $"{topic}|{json}";
                        _eventChannel.Writer.TryWrite(message);
                    });
                }
            }

            // สปอน Thread ทำงานหลัก
            _workerTask = Task.Run(() => ProcessRepMessages(_cts.Token));
            _publisherTask = Task.Run(() => ProcessPubStreaming(_cts.Token));
        }

        private void ProcessRepMessages(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // รอรับข้อความ (ใช้ TimeSpan เพื่อไม่ให้ Thread บล็อกถาวรและรับ Cancellation ได้)
                    if (_repSocket!.TryReceiveFrameString(TimeSpan.FromMilliseconds(500), out string? message))
                    {
                        if (message == null) continue;

                        string cmd = message.ToLower();
                        Console.WriteLine($"[IpcWorker] [Port {_repPort}] Received Command: {message} (paused={_isPaused})");
                        string response = "Unknown Command";

                        switch (cmd)
                        {
                            case "health-check":
                                // CB-001: health-check ต้องตอบได้เสมอ แม้ paused
                                response = "OK";
                                break;

                            case "pause":
                                _isPaused = true;
                                response = "Paused";
                                Console.WriteLine($"[IpcWorker] [Port {_repPort}] Process Paused.");
                                break;

                            case "resume":
                                // CB-001: resume ต้องตอบได้แม้ paused (เพื่อ un-pause ได้)
                                _isPaused = false;
                                response = "Resumed";
                                Console.WriteLine($"[IpcWorker] [Port {_repPort}] Process Resumed.");
                                break;

                            case "terminate":
                                response = "Terminated";
                                Console.WriteLine($"[IpcWorker] [Port {_repPort}] Terminate signal received. Shutting down.");
                                _repSocket!.SendFrame(response);
                                _cts?.Cancel(); // สั่งยกเลิกลูปทั้งหมด
                                return;

                            default:
                                // CB-001: คำสั่ง game-specific อื่นๆ จะ skip เมื่อ paused
                                if (_isPaused)
                                {
                                    response = "Paused";
                                    Console.WriteLine($"[IpcWorker] [Port {_repPort}] Command '{message}' skipped (paused).");
                                }
                                break;
                        }

                        _repSocket!.SendFrame(response);
                    }
                    else if (_isPaused)
                    {
                        // CB-001: ไม่มีข้อความเข้ามา + paused → สลีป 100ms ลดการ spin-wait
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[IpcWorker Error] In REP loop: {ex.Message}");
                    break;
                }
            }
        }

        private async Task ProcessPubStreaming(CancellationToken token)
        {
            if (_eventBus != null && _eventChannel != null)
            {
                await ProcessRealEventStreaming(token);
            }
            // If no EventBus is configured, PUB streaming is disabled (no legacy fallback)
        }

        private async Task ProcessRealEventStreaming(CancellationToken token)
        {
            try
            {
                while (await _eventChannel!.Reader.WaitToReadAsync(token))
                {
                    while (_eventChannel.Reader.TryRead(out string? message))
                    {
                        if (token.IsCancellationRequested) return;

                        int separatorIdx = message.IndexOf('|');
                        if (separatorIdx > 0)
                        {
                            string topic = message.Substring(0, separatorIdx);
                            string payload = message.Substring(separatorIdx + 1);

                            try
                            {
                                _pubSocket!.SendMoreFrame(topic).SendFrame(payload);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[IpcWorker Error] Failed to publish event: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"[IpcWorker Error] In real event streaming: {ex.Message}");
            }
        }


        private static bool IsAiDecisionEvent(DuelEvent duelEvent)
        {
            return duelEvent.AiReasoning != null &&
                   duelEvent.AiReasoning.Reason != null &&
                   duelEvent.AiReasoning.Reason.Count > 0;
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _eventChannel?.Writer.TryComplete();
            
            try
            {
                // รอให้ Task ทำงานเสร็จ
                _workerTask?.Wait(1000);
                _publisherTask?.Wait(1000);
            }
            catch {}

            _repSocket?.Close();
            _pubSocket?.Close();
            _repSocket?.Dispose();
            _pubSocket?.Dispose();
            _cts?.Dispose();
            
            
            Console.WriteLine($"[IpcWorker] Disposed Ports {_repPort}/{_pubPort}");
        }
    }
}
