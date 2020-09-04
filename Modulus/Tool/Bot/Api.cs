using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tool.Bot.Entities;
using Tool.Bot.UI;
using Tool.Bot.Utils;
using static Engine.Framework.Api;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Tool.Bot
{
    public static class Api
    {
        private static ILoggerRepository loggerRepository = null;
        private static log4net.ILog log = null;

        public static int HeartBeatInterval { get; set; } = 15;

        public static void StartUp()
        {

            loggerRepository = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log = LogManager.GetLogger(loggerRepository.Name, "MainLog");

            Utils.Config.Instance = Utils.Config.ReadFromFile();

            Engine.Framework.Api.Add(Singleton<Entities.User.Layer>.Instance);
            Singleton<Scheduler.MainTimer>.Instance.Run(50);

            AppDomain.CurrentDomain.UnhandledException += App_UnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        public static IBotUI BotUI = null;

        public static void WriteDebugLog(string str)
        {
            log.Debug(str);
            BotUI?.ShowLog(str);
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

        
    }
}
