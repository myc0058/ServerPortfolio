using Engine.Framework;
using log4net;
using Schema.Protobuf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.MockDedicateServer.Utils;
using static Engine.Framework.Api;

namespace Tool.MockDedicateServer
{
    public partial class Api
    {
        private static log4net.ILog Log;//LogManager.GetLogger("MainLog", "MainLog");

        public static int ListenPortForClient { get; set; } = 0;

        public static void StartUp()
        {
            Utils.Config.Instance = Utils.Config.ReadFromFile();

            Engine.Framework.Api.Add(Singleton<Entities.Game.Layer>.Instance);
            Singleton<Scheduler.MainTimer>.Instance.Run(50);

            AppDomain.CurrentDomain.UnhandledException += App_UnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            WriteDebugLog("================= OnUnobservedTaskException BEGIN ==============");
            WriteDebugLog(e.Exception.StackTrace);
            WriteDebugLog("================= OnUnobservedTaskException END ==============");
        }

        private static void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteDebugLog("================= OnUnobservedTaskException BEGIN ==============");
            if (e.ExceptionObject is Exception)
                WriteDebugLog((e.ExceptionObject as Exception).StackTrace);
            else
                WriteDebugLog(e.ToString());
            WriteDebugLog("================= OnUnobservedTaskException END ==============");
        }

        public static void WriteDebugLog(string str)
        {
            //Log.Debug(str);
            Console.WriteLine(str);
        }

        public static void SetModeByMapname(string name)
        {
            if (Basis.Metadata.Config.FreeForAll.LevelName == name)
                BattleMode = EBattleMode.FreeForAll;
            else if (Basis.Metadata.Config.Tag.LevelName == name)
                BattleMode = EBattleMode.Tag;
            else if (Basis.Metadata.Config.RandomTrio.LevelName == name)
                BattleMode = EBattleMode.RandomTrio;
            else
                BattleMode = EBattleMode.Editor;
        }

        public static EBattleMode BattleMode { get; set; }
    }
}
