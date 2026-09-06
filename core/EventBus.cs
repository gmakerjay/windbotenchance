using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public class EventBus
    {
        // เก็บลายเซ็นต์ Callback แบบ Task เพื่อรองรับทั้งแบบ Async และ Sync (แปลงเป็น CompletedTask)
        private readonly ConcurrentDictionary<DuelEventType, List<Func<DuelEvent, Task>>> _subscribers = new();
        
        // สำหรับล็อกขบวนการลงทะเบียน
        private readonly object _lockObj = new();

        // เก็บสถิติจำนวนครั้งการส่งข่าวสารแยกตามประเภท
        private readonly ConcurrentDictionary<DuelEventType, long> _stats = new();

        // CB-003: Event callback เมื่อ subscriber โยน exception — ใช้สำหรับ monitoring / การนับ error
        // signature: (DuelEventType eventType, Exception ex)
        public event Action<DuelEventType, Exception>? OnSubscriberError;

        public EventBus()
        {
            // เคลียร์สถิติเริ่มต้น
            foreach (DuelEventType type in Enum.GetValues(typeof(DuelEventType)))
            {
                _stats[type] = 0;
                _subscribers[type] = new List<Func<DuelEvent, Task>>();
            }
        }

        /// <summary>
        /// บอกรับกิจกรรม (Subscribe) ในรูปแบบประสานเวลา (Synchronous)
        /// </summary>
        public void Subscribe(DuelEventType eventType, Action<DuelEvent> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            lock (_lockObj)
            {
                var list = _subscribers.GetOrAdd(eventType, _ => new List<Func<DuelEvent, Task>>());
                list.Add(ev =>
                {
                    handler(ev);
                    return Task.CompletedTask;
                });
            }
        }

        /// <summary>
        /// บอกรับกิจกรรม (Subscribe) ในรูปแบบอซิงโครนัส (Asynchronous)
        /// </summary>
        public void SubscribeAsync(DuelEventType eventType, Func<DuelEvent, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            lock (_lockObj)
            {
                var list = _subscribers.GetOrAdd(eventType, _ => new List<Func<DuelEvent, Task>>());
                list.Add(handler);
            }
        }

        /// <summary>
        /// ยกเลิกการบอกรับกิจกรรม (Unsubscribe) ทั้งหมดของประเภทที่กำหนด
        /// </summary>
        public void Unsubscribe(DuelEventType eventType)
        {
            lock (_lockObj)
            {
                if (_subscribers.TryGetValue(eventType, out var list))
                {
                    list.Clear();
                }
            }
        }

        /// <summary>
        /// ยกเลิกการบอกรับกิจกรรมทุกประเภท (Unsubscribe All)
        /// </summary>
        public void UnsubscribeAll()
        {
            lock (_lockObj)
            {
                foreach (var kvp in _subscribers)
                {
                    kvp.Value.Clear();
                }
            }
        }

        /// <summary>
        /// เผยแพร่กิจกรรม (Publish) ในรูปแบบประสานเวลา (Synchronous)
        /// </summary>
        public void Publish(DuelEvent duelEvent)
        {
            if (duelEvent == null) return;

            // บันทึกสถิติ
            _stats.AddOrUpdate(duelEvent.EventType, 1, (_, count) => count + 1);

            List<Func<DuelEvent, Task>> subs;
            lock (_lockObj)
            {
                if (!_subscribers.TryGetValue(duelEvent.EventType, out var list) || list.Count == 0)
                {
                    LogDiagnostic(duelEvent, 0);
                    return;
                }
                // คัดลอกรายการป้องกันการแก้ไขระหว่างประมวลผล (Snapshot)
                subs = new List<Func<DuelEvent, Task>>(list);
            }

            LogDiagnostic(duelEvent, subs.Count);

            // รันทีละ subscriber แบบ Synchronous (รอให้ทำงานเสร็จทีละตัว)
            foreach (var sub in subs)
            {
                try
                {
                    sub(duelEvent).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    // CB-003: fire error event เพื่อให้ caller นับ error ได้ และยัง continue ต่อไปได้
                    try { OnSubscriberError?.Invoke(duelEvent.EventType, ex); } catch { }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[EventBus Error] Subscriber failed in Sync Publish: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// เผยแพร่กิจกรรม (Publish) ในรูปแบบอซิงโครนัส (Asynchronous)
        /// </summary>
        public async Task PublishAsync(DuelEvent duelEvent)
        {
            if (duelEvent == null) return;

            _stats.AddOrUpdate(duelEvent.EventType, 1, (_, count) => count + 1);

            List<Func<DuelEvent, Task>> subs;
            lock (_lockObj)
            {
                if (!_subscribers.TryGetValue(duelEvent.EventType, out var list) || list.Count == 0)
                {
                    LogDiagnostic(duelEvent, 0);
                    return;
                }
                subs = new List<Func<DuelEvent, Task>>(list);
            }

            LogDiagnostic(duelEvent, subs.Count);

            // CB-003: รัน subscribers ทั้งหมดแบบขนานและรอคอยพร้อมกัน
            // แต่ละ subscriber ถูก wrap ใน try/catch แยกต่างหาก — subscriber ผิดพลาดหนึ่งไม่ส่งผลกระทบ subscriber อื่น
            var tasks = subs.Select(sub => Task.Run(async () =>
            {
                try
                {
                    await sub(duelEvent);
                }
                catch (Exception ex)
                {
                    // fire error event เพื่อให้ caller นับ error ได้
                    try { OnSubscriberError?.Invoke(duelEvent.EventType, ex); } catch { }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[EventBus Error] Subscriber failed in Async Publish: {ex.Message}");
                    Console.ResetColor();
                }
            }));

            // Task.WhenAll จะไม่ throw เพราะ exception ถูกจัดการภายใน subscriber task แล้ว
            await Task.WhenAll(tasks);
        }

        private void LogDiagnostic(DuelEvent duelEvent, int subCount)
        {
            string ts = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Console.WriteLine($"[EventBus] [{ts}] Publish: {duelEvent.EventType,-15} | Card: {duelEvent.CardName,-20} | Subscribers: {subCount}");
        }

        /// <summary>
        /// ดึงสถิติจำนวนครั้งในการส่งกิจกรรมแต่ละประเภท
        /// </summary>
        public Dictionary<DuelEventType, long> GetStats()
        {
            return new Dictionary<DuelEventType, long>(_stats);
        }
    }
}
