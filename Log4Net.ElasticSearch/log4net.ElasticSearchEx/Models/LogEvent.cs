using log4net.Core;
using log4net.ElasticSearch.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace log4net.ElasticSearch.Models
{
    /// <summary>
    /// Primary object which will get serialized into a json object to pass to ES. Deviating from CamelCase
    /// class members so that we can stick with the build in serializer and not take a dependency on another lib. ES
    /// exepects fields to start with lowercase letters.
    /// </summary>
    public class logEvent
    {
        public logEvent()
        {
            //properties = new Dictionary<string, string>();
        }

        //public string timeStamp { get; set; }

        //public string message { get; set; }

        //public object messageObject { get; set; }

        //public object exception { get; set; }

        //public string loggerName { get; set; }

        //public string domain { get; set; }

        //public string identity { get; set; }

        //public string level { get; set; }

        //public string className { get; set; }

        //public string fileName { get; set; }

        //public string lineNumber { get; set; }

        //public string fullInfo { get; set; }

        //public string methodName { get; set; }

        //public string fix { get; set; }

        //public IDictionary<string, string> properties { get; set; }

        //public string userName { get; set; }

        //public string threadName { get; set; }

        //public string hostName { get; set; }

        #region 扩展 接口级日志

        public string uuid { get; set; }
        public object exception { get; set; }
        public string uuidtag { get; set; }
        public string message { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public IDictionary<string, string> otherMsg { get; set; }
        public string project { get; set; }
        public string hostId { get; set; }
        public string hostName { get; set; }
        public string domain { get; set; }
        public string level { get; set; }
        public string fullInfo { get; set; }
        public string threadName { get; set; }
        public string timeStamp { get; set; }

        #endregion 扩展 接口级日志

        public static IEnumerable<logEvent> CreateMany(IEnumerable<LoggingEvent> loggingEvents)
        {
            return loggingEvents.Select(@event => Create(@event)).ToArray();
        }

        static logEvent Create(LoggingEvent loggingEvent)
        {
            var logEvent = new logEvent
            {
                //loggerName = loggingEvent.LoggerName,
                domain = loggingEvent.Domain,
                //identity = loggingEvent.Identity,
                threadName = loggingEvent.ThreadName,
                //userName = loggingEvent.UserName,
                timeStamp = loggingEvent.TimeStamp.ToUniversalTime().ToString("O"),
                exception = loggingEvent.ExceptionObject == null ? new object() : JsonSerializableException.Create(loggingEvent.ExceptionObject),
                message = loggingEvent.RenderedMessage,
                //fix = loggingEvent.Fix.ToString(),
                hostName = Environment.MachineName,
                level = loggingEvent.Level == null ? null : loggingEvent.Level.DisplayName
            };

            #region 扩展 接口级日志
            MangoLogEvent mangoLogEvent = null;
            try
            {
                if (loggingEvent.MessageObject != null)
                {
                    if (loggingEvent.MessageObject.GetType() == typeof(string))
                    {
                        mangoLogEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<MangoLogEvent>(loggingEvent.MessageObject.ToString());
                    }
                    else
                    {
                        mangoLogEvent = Newtonsoft.Json.JsonConvert.DeserializeObject<MangoLogEvent>(loggingEvent.MessageObject.ToJson());
                    }
                    if (mangoLogEvent != null)
                    {
                        logEvent.uuid = mangoLogEvent.uuid;
                        logEvent.uuidtag = mangoLogEvent.uuidtag;
                        logEvent.message = mangoLogEvent.message;
                        logEvent.request = mangoLogEvent.request;
                        logEvent.response = mangoLogEvent.response;
                        if (mangoLogEvent.otherMsg.Count > 0)
                        {
                            logEvent.otherMsg = mangoLogEvent.otherMsg;
                        }
                        else
                        {
                            logEvent.otherMsg = new Dictionary<string, string>();
                        }
                        //类信息
                        //logEvent.className = mangoLogEvent.className;
                        //logEvent.fileName = mangoLogEvent.fileName;
                        //logEvent.lineNumber = mangoLogEvent.lineNumber.ToString();                       
                        //logEvent.methodName = mangoLogEvent.methodName;
                        logEvent.fullInfo = mangoLogEvent.fullInfo;


                        //上下文信息
                        if (!string.IsNullOrEmpty(mangoLogEvent.SERVER_PORT))
                        {
                            var ipParam = mangoLogEvent.LOCAL_ADDR.Split('.');
                            logEvent.hostId = mangoLogEvent.HTTP_HOST;
                            if (ipParam.Length == 4)
                            {
                                //logEvent.hostId = ipParam[3] + "-" + mangoLogEvent.SERVER_PORT;
                                logEvent.hostId = mangoLogEvent.LOCAL_ADDR + ":" + mangoLogEvent.SERVER_PORT;
                            }
                        }
                        if (!string.IsNullOrEmpty(mangoLogEvent.SERVER_NAME))
                        {
                            logEvent.domain = mangoLogEvent.SERVER_NAME;
                        }

                        //项目名
                        logEvent.project = logEvent.hostId;
                        if (!string.IsNullOrEmpty(mangoLogEvent.Appl_Physical_Path))
                        {
                            var appl_Path_Param = mangoLogEvent.Appl_Physical_Path.Split('\\');
                            if (appl_Path_Param.Length > 3)
                            {
                                logEvent.project = appl_Path_Param[2];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion 扩展 接口级日志

            //if (loggingEvent.LocationInformation != null)
            //扩展 新增了判断条件
            if (string.IsNullOrEmpty(logEvent.fullInfo) && loggingEvent.LocationInformation != null)
            {
                //logEvent.className = loggingEvent.LocationInformation.ClassName;
                //logEvent.fileName = loggingEvent.LocationInformation.FileName;
                //logEvent.lineNumber = loggingEvent.LocationInformation.LineNumber;
                logEvent.fullInfo = loggingEvent.LocationInformation.FullInfo;
                //logEvent.methodName = loggingEvent.LocationInformation.MethodName;
            }

            AddProperties(loggingEvent, logEvent);
            //throw new Exception("终止提交ES");

            return logEvent;
        }

        static void AddProperties(LoggingEvent loggingEvent, logEvent logEvent)
        {
            //loggingEvent.Properties().Union(AppenderPropertiesFor(loggingEvent)).
            //             Do(pair => logEvent.properties.Add(pair));
        }

        static IEnumerable<KeyValuePair<string, string>> AppenderPropertiesFor(LoggingEvent loggingEvent)
        {
            yield return Pair.For("@timestamp", loggingEvent.TimeStamp.ToUniversalTime().ToString("O"));
        }
    }
}