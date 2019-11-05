using log4net.Repository;
using System.IO;


namespace Mango.Log4Net.ElasticSearch.Logging
{
    /// <summary>
    /// log4net 日志输出适配器
    /// </summary>
    public class Log4NetLoggerAdapter : LoggerAdapterBase
    {
        //log4net日志
        public static ILoggerRepository repository { get; set; }

        /// <summary>
        /// 初始化一个<see cref="Log4NetLoggerAdapter"/>类型的新实例
        /// </summary>
        public Log4NetLoggerAdapter()
        {
            #region 初始化配置 XML形式
            //            var assembly = Assembly.GetExecutingAssembly();
            //            //var resourceName = "Log4Net.Mango.log4net.config";
            //            var resourceName = "Log4Net.Mango.log4net.ElasticSearch.config";
            //            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //            {
            //#if !NETSTANDARD
            //                // 从 Stream 中读取 log4net 的配置信息
            //                log4net.Config.XmlConfigurator.Configure(stream);
            //#else
            //                 // 加载log4net日志配置文件
            //                repository = log4net.LogManager.CreateRepository("MangoRepository");
            //                 // 从 Stream 中读取 log4net 的配置信息
            //                log4net.Config.XmlConfigurator.Configure(repository,stream);
            //#endif
            //            }
            //            //启用log4net的调试信息
            //            log4net.Util.LogLog.InternalDebugging = true;
            #endregion
#if NETSTANDARD
            try
            {
                repository = log4net.LogManager.GetRepository("log4net-default-repository");
            }
            catch (System.Exception)
            {
                repository = log4net.LogManager.CreateRepository("log4net-default-repository");
            }
#endif

            string fileName = "Configs/log4net.config";
            string configFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(configFile))
            {
                fileName = "log4net.config";
                configFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName);
            }

            if (File.Exists(configFile))
            {
#if !NETSTANDARD

                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
#else
                try
                {
                    if (repository == null)
                    {
                        repository = log4net.LogManager.CreateRepository("log4net-default-repository");
                    }
                }
                catch (System.Exception)
                {
                }

                log4net.Config.XmlConfigurator.ConfigureAndWatch(repository, new FileInfo(configFile));
#endif
                return;
            }

#region appender
            //RollingFileAppender appender = new RollingFileAppender
            //{
            //    Name = "root",
            //    File = "logs\\log_",
            //    AppendToFile = true,
            //    LockingModel = new FileAppender.MinimalLock(),
            //    RollingStyle = RollingFileAppender.RollingMode.Date,
            //    DatePattern = "yyyyMMdd-HH\".log\"",
            //    StaticLogFileName = false,
            //    MaxSizeRollBackups = 10,
            //    Layout = new PatternLayout("[%d{yyyy-MM-dd HH:mm:ss.fff}] %-5p %c.%M %t %w %n%m%n")
            //    //Layout = new PatternLayout("[%d [%t] %-5p %c [%x] - %m%n]")
            //};
            //appender.ClearFilters();
            //appender.AddFilter(new LevelMatchFilter { LevelToMatch = Level.Info });
            ////PatternLayout layout = new PatternLayout("[%d{yyyy-MM-dd HH:mm:ss.fff}] %c.%M %t %n%m%n");
            ////appender.Layout = layout;
            //BasicConfigurator.Configure(appender);
            //appender.ActivateOptions();

#endregion
        }

#region Overrides of LoggerAdapterBase

        /// <summary>
        /// 创建指定名称的缓存实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        protected override ILog CreateLogger(string name)
        {
#if !NETSTANDARD
            log4net.ILog log = log4net.LogManager.GetLogger(name);
#else
            log4net.ILog log = log4net.LogManager.GetLogger("log4net-default-repository", name);
#endif
            return new Log4NetLogger(log);
        }

#endregion
    }
}