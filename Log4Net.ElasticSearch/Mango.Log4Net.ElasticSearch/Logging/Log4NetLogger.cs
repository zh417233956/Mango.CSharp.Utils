using log4net.Core;
using System;

namespace Mango.Log4Net.ElasticSearch.Logging
{
    /// <summary>
    /// log4net 日志输出者适配类
    /// </summary>
    internal class Log4NetLogger : LogBase
    {
        private static readonly Type DeclaringType = typeof(Log4NetLogger);
        private readonly log4net.Core.ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="Log4NetLog"/>类型的新实例
        /// </summary>
        public Log4NetLogger(ILoggerWrapper wrapper)
        {
            _logger = wrapper.Logger;
        }

        /// <summary>
        /// 获取日志输出级别
        /// </summary>
        /// <param name="level">日志输出级别枚举</param>
        /// <returns>获取日志输出级别</returns>
        private static Level GetLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.All:
                    return Level.All;
                case LogLevel.Trace:
                    return Level.Trace;
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Warn:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Fatal:
                    return Level.Fatal;
                case LogLevel.Off:
                    return Level.Off;
                default:
                    return Level.Off;
            }
        }


        #region 日志格式化

        //日志内容格式模板
        private const string _LoggerTextFormat = "日志内容：{0}";

        /// <summary>
        /// 方法名
        /// </summary>
        private string methodName { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string SerializeObject(object message)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(message);
        }
        /// <summary>
        /// 日志格式化
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="message"></param>
        /// <returns></returns>       
        private string TextFormat(object message)
        {
            System.Diagnostics.StackTrace stackTrance = new System.Diagnostics.StackTrace(true);
            var _frame = stackTrance.GetFrame(4);
            if (_frame != null)
            {
                var msgModel = (Models.LogEvent)message;
                Models.LogEventClassInfo _message = new Models.LogEventClassInfo();
                _message.uuid = msgModel.uuid;
                _message.module = msgModel.module;
                _message.uuidtag = msgModel.uuidtag;
                _message.message = msgModel.message;
                _message.request = msgModel.request;
                _message.response = msgModel.response;
                _message.otherMsg = msgModel.otherMsg;

                _message.methodName = _frame.GetMethod().Name;
                _message.className = _frame.GetMethod().DeclaringType?.FullName;
                _message.fileName = _frame.GetFileName();
                _message.lineNumber = _frame.GetFileLineNumber();
                var fullinfoFormat = "{0}.{1}({2}:{3})";
                _message.fullInfo = string.Format(fullinfoFormat, _message.className, _message.methodName, _message.fileName, _message.lineNumber);

                // .NET Compact Framework 1.0 has no support for ASP.NET
                // SSCLI 1.0 has no support for ASP.NET
#if !NETCF && !SSCLI && !NETSTANDARD
                //ASP.NET,获取上下文
                System.Web.HttpContext context = null;
                if (System.Web.HttpContext.Current != null)
                {
                    context = System.Web.HttpContext.Current;
                    _message.SERVER_NAME = context.Request.Params["SERVER_NAME"];
                    _message.HTTP_HOST = context.Request.Params["HTTP_HOST"];
                    _message.SERVER_PORT = context.Request.Params["SERVER_PORT"];
                    _message.LOCAL_ADDR= context.Request.Params["LOCAL_ADDR"];
                    _message.Appl_Physical_Path= context.Request.Params["Appl_Physical_Path"];
                }
                
#elif NETSTANDARD
                //NETSTANDARD
                var context = ExtensionMethods.GetHttpContext();
                if (context != null)
                {
                    _message.SERVER_NAME = context.Request.Host.Value;
                    _message.HTTP_HOST = context.Request.Host.Host;
                    _message.SERVER_PORT = context.Request.Host.Port.ToString();
                    _message.LOCAL_ADDR = context.Connection.LocalIpAddress.ToString();
                    _message.Appl_Physical_Path = AppDomain.CurrentDomain.BaseDirectory;
                }
                else
                {
                    _message.SERVER_NAME = AppDomain.CurrentDomain.FriendlyName;
                    _message.Appl_Physical_Path = AppDomain.CurrentDomain.BaseDirectory;
                }
#endif
                return SerializeObject(_message);

            }
            else
            {
                return SerializeObject(message);
            }

        }
        #endregion

        #region 定义信息二次处理        
        private void HandMessage(object Msg)
        {
            //System.Diagnostics.StackTrace stackTrance = new System.Diagnostics.StackTrace(true);
            //methodName = stackTrance.GetFrame(4).GetMethod().Name;            
        }
        #endregion

        #region Overrides of LogBase

        /// <summary>
        /// 获取日志输出处理委托实例
        /// </summary>
        /// <param name="level">日志输出级别</param>
        /// <param name="message">日志消息</param>
        /// <param name="exception">日志异常</param>
        /// <param name="isData">是否数据日志</param>
        protected override void Write(LogLevel level, object message, Exception exception, bool isData = false)
        {
            if (isData)
            {
                return;
            }
            HandMessage(message);
            Level log4NetLevel = GetLevel(level);
            if (message.GetType() != typeof(string))
            {
                message = TextFormat(message);
            }
            _logger.Log(DeclaringType, log4NetLevel, message, exception);
        }

        /// <summary>
        /// 获取 是否数据日志对象
        /// </summary>
        public override bool IsDataLogging
        {
            get { return false; }
        }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Trace"/>级别的日志
        /// </summary>
        public override bool IsTraceEnabled
        {
            get { return _logger.IsEnabledFor(Level.Trace); }
        }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Debug"/>级别的日志
        /// </summary>
        public override bool IsDebugEnabled
        {
            get { return _logger.IsEnabledFor(Level.Debug); }
        }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Info"/>级别的日志
        /// </summary>
        public override bool IsInfoEnabled
        {
            get { return _logger.IsEnabledFor(Level.Info); }
        }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Warn"/>级别的日志
        /// </summary>
        public override bool IsWarnEnabled
        {
            get { return _logger.IsEnabledFor(Level.Warn); }
        }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Error"/>级别的日志
        /// </summary>
        public override bool IsErrorEnabled
        {
            get { return _logger.IsEnabledFor(Level.Error); }
        }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Fatal"/>级别的日志
        /// </summary>
        public override bool IsFatalEnabled
        {
            get { return _logger.IsEnabledFor(Level.Fatal); }
        }

        #endregion
    }
}