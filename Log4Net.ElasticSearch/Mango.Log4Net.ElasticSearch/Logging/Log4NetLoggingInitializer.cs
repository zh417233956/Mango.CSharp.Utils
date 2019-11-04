using System.Reflection;

namespace Mango.Log4Net.ElasticSearch.Logging
{
    public class Log4NetLoggingInitializer
    {
        /// <summary>
        /// 开始初始化基础日志
        /// </summary>
        /// <param name="config">日志配置信息</param>
        public void Initialize()
        {
            LoggingInitializerBase logInit = new LoggingInitializerBase();
            logInit.SetLoggingFromAdapterConfig();
        }
    }
    /// <summary>
    /// 日志初始化器基类
    /// </summary>
    public class LoggingInitializerBase
    {
        /// <summary>
        /// 获取或设置 服务提供者
        /// </summary>
        public AssemblyName[] ServiceProvider { get { return System.Reflection.Assembly.GetEntryAssembly().GetReferencedAssemblies(); } }

        /// <summary>
        /// 从日志适配器配置节点初始化日志适配器
        /// </summary>
        /// <param name="config">日志适配器配置节点</param>
        public void SetLoggingFromAdapterConfig()
        {
            ILoggerAdapter adapter = new Log4NetLoggerAdapter();

            if (adapter == null)
            {
                return;
            }
            LogManager.AddLoggerAdapter(adapter);
        }
    }
}
