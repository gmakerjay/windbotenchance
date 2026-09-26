using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Web;
using System.Diagnostics;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper;
using YgoAiPlatform.Core;

namespace WindBot
{
    public class Program
    {
        public static string AssetPath;

        internal static Random Rand;
        public static bool ServerMode;

        public static EventBus EventBus;
        private static IpcWorker _zmqWorker;

        internal static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                try { _zmqWorker?.Dispose(); } catch {}
            };

            Logger.WriteLine("WindBot starting...");

            // Wire audit trail delegates (Layer 1/2/3 โ’ Logger)
            DecisionTracer.WriteTrace = Logger.WriteTraceLine;
            DecisionTracer.WriteError = Logger.WriteErrorLine;
            HeuristicGuard.WriteTrace = Logger.WriteTraceLine;
            HeuristicGuard.WriteError = Logger.WriteErrorLine;
            GameStateSnapshot.WriteTrace = Logger.WriteTraceLine;
            GameStateSnapshot.WriteError = Logger.WriteErrorLine;
            GameStateSnapshot.GetLogDir = Logger.GetCurrentLogDir;

            Config.Load(args);

            bool enableLog = Config.GetBool("Log", true);
            Logger.FileLogEnabled = enableLog;
            DecisionTracer.Enabled = enableLog;

            // ZMQ Integration for Spectator
            int zmqPubPort = Config.GetInt("ZmqPubPort", 0);
            int zmqRepPort = Config.GetInt("ZmqRepPort", 0);
            string duelId = Config.GetString("DuelId", "windbot");

            if (zmqPubPort > 0)
            {
                int repPort = zmqRepPort > 0 ? zmqRepPort : zmqPubPort - 1000;
                EventBus = new EventBus();
                _zmqWorker = new IpcWorker(repPort, zmqPubPort, duelId, EventBus);
                _zmqWorker.Start();
                Logger.WriteLine($"[ZMQ Spectator] Started ZMQ Publisher on PUB:{zmqPubPort} / REP:{repPort}");
            }

            Logger.WriteLine(Config.GetString("Deck"));

            AssetPath = Config.GetString("AssetPath", "");
            BotConfig.AssetPath = AssetPath;

            string databasePath = Config.GetString("DbPath", "cards.cdb");

            InitDatas(databasePath);

            ServerMode = Config.GetBool("ServerMode", false);

            if (ServerMode)
            {
                // Run in server mode, provide a http interface to create bot.
                int serverPort = Config.GetInt("ServerPort", 2399);
                RunAsServer(serverPort);
            }
            else
            {
                // Join the host specified on the command line.
                if (args.Length == 0)
                {
                    Logger.WriteErrorLine("=== WARN ===");
                    Logger.WriteLine("No input found, tring to connect to localhost YGOPro host.");
                    Logger.WriteLine("If it fail, the program will quit sliently.");
                }
                RunFromArgs();
            }
        }

        public static void InitDatas(string databasePath)
        {
            Rand = new Random();
            DecksManager.Init();

            string[] dbPaths;
            //If databasePath is an absolute path like "โ€ชC:/ProjectIgnis/expansions/cards.cdb",
            //then Path.GetFullPath("../โ€ชC:/ProjectIgnis/expansions/cards.cdb" would give an error,
            //due to containing a colon that's not part of a volume identifier.
            if (Path.IsPathRooted(databasePath)) dbPaths = new string[] { databasePath };
            else dbPaths = new string[]{
                Path.GetFullPath(databasePath),
                Path.GetFullPath("../" + databasePath),
                Path.GetFullPath("../expansions/" + databasePath)
            };

            string primaryPath = null;
            foreach (var absPath in dbPaths)
            {
                if (File.Exists(absPath))
                {
                    primaryPath = absPath;
                    break;
                }
            }

            if (primaryPath == null)
            {
                Logger.WriteErrorLine("Can't find cards database file.");
                Logger.WriteErrorLine("Please place cards.cdb next to WindBot.exe or Bot.exe .");
                return;
            }

            // Load the primary database
            NamedCardsManager.Init(primaryPath);

            // Also load any other .cdb files in the same directory
            try
            {
                string dir = Path.GetDirectoryName(primaryPath);
                if (Directory.Exists(dir))
                {
                    string[] otherDbs = Directory.GetFiles(dir, "*.cdb");
                    foreach (string db in otherDbs)
                    {
                        if (Path.GetFullPath(db) != Path.GetFullPath(primaryPath))
                        {
                            Logger.WriteLine("Loading extra database: " + Path.GetFileName(db));
                            NamedCardsManager.Init(db);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLine("Error loading extra databases: " + ex.Message);
            }
        }

        private static void RunFromArgs()
        {
            WindBotInfo Info = new WindBotInfo();
            Info.Name = Config.GetString("Name", Info.Name);
            Info.Deck = Config.GetString("Deck", Info.Deck);
            Info.Dialog = Config.GetString("Dialog", Info.Dialog);
            Info.Host = Config.GetString("Host", Info.Host);
            Info.Port = Config.GetInt("Port", Info.Port);
            Info.HostInfo = Config.GetString("HostInfo", Info.HostInfo);
            Info.Version = Config.GetInt("Version", Info.Version);
            Info.Hand = Config.GetInt("Hand", Info.Hand);
            Info.Debug = Config.GetBool("Debug", Info.Debug);
            Info.Chat = Config.GetBool("Chat", Info.Chat);
            Info.ReplayPath = Config.GetString("ReplayPath", Info.ReplayPath);
            Info.DuelId = Config.GetString("DuelId", Info.DuelId);
            Run(Info);
        }

        private static void RunAsServer(int ServerPort)
        {
            using (HttpListener MainServer = new HttpListener())
            {
                MainServer.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                MainServer.Prefixes.Add("http://+:" + ServerPort + "/");
                MainServer.Start();
                Logger.WriteLine("WindBot server start successed.");
                Logger.WriteLine("HTTP GET http://127.0.0.1:" + ServerPort + "/?name=WindBot&host=127.0.0.1&port=7911 to call the bot.");
                while (true)
                {
                    try
                    {
                        HttpListenerContext ctx = MainServer.GetContext();
                        var queryParams = HttpUtility.ParseQueryString(ctx.Request.Url.Query);

                        WindBotInfo Info = new WindBotInfo();
                        Info.Name = queryParams.Get("name");
                        Info.Deck = queryParams.Get("deck");
                        Info.Host = queryParams.Get("host");
                        string port = queryParams.Get("port");
                        if (port != null)
                            Info.Port = Int32.Parse(port);
                        string dialog = queryParams.Get("dialog");
                        if (dialog != null)
                            Info.Dialog = dialog;
                        string version = queryParams.Get("version");
                        if (version != null)
                            Info.Version = Int16.Parse(version);
                        string password = queryParams.Get("password");
                        if (password != null)
                            Info.HostInfo = password;
                        string hand = queryParams.Get("hand");
                        if (hand != null)
                            Info.Hand = Int32.Parse(hand);
                        string debug = queryParams.Get("debug");
                        if (debug != null)
                            Info.Debug = bool.Parse(debug);
                        string chat = queryParams.Get("chat");
                        if (chat != null)
                            Info.Chat = bool.Parse(chat);

                        if (Info.Name == null || Info.Host == null || port == null)
                        {
                            ctx.Response.StatusCode = 400;
                            ctx.Response.Close();
                        }
                        else
                        {
                            ctx.Response.StatusCode = 200;
                            try
                            {
                                Thread workThread = new Thread(new ParameterizedThreadStart(Run));
                                workThread.Start(Info);
                            }
                            catch (Exception ex)
                            {
                                if (Debugger.IsAttached)
                                    throw;
                                Logger.WriteErrorLine("Start Thread Error: " + ex);
                                ctx.Response.StatusCode = 500;
                            }
                            ctx.Response.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Debugger.IsAttached)
                            throw;
                        Logger.WriteErrorLine("Parse Http Request Error: " + ex);
                    }
                }
            }
        }

        private static void Run(object o)
        {
            // All errors should be caught instead of causing the program to crash.
            try
            {
                WindBotInfo Info = (WindBotInfo)o;
                GameClient client = new GameClient(Info);
                client.Start();
                Logger.DebugWriteLine(client.Username + " started.");
                while (client.Connection.IsConnected)
                {
                    try
                    {
                        client.Tick();
#if DEBUG
                        Thread.Sleep(1);
#else
                        Thread.Sleep(30);
#endif
                    }
                    catch (Exception ex)
                    {
                        if (Debugger.IsAttached)
                            throw;
                        Logger.WriteErrorLine("Tick Error: " + ex);
                    }
                }
                Logger.DebugWriteLine(client.Username + " end.");
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    throw;
                Logger.WriteErrorLine("Run Error: " + ex);
            }
        }
    }
}
