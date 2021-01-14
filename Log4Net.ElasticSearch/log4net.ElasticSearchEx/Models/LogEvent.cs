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
        }
        #region 扩展 接口级日志
        public string ltctraid { get; set; }
        public string requestid { get; set; }
        public string startTime { get; set; }     
        public string hostId { get; set; }
        public string hostName { get; set; }
        public string domain { get; set; }   
        public string project { get; set; }
        public string level { get; set; }
        public string timestamp { get; set; }
        public object Exception { get; set; }
        public int RequestUserId { get; set; }
        public string RequestRaw { get; set; }
        public string RequestIp { get; set; }
        public string RequestAgent { get; set; }
        public string RequestAssembly { get; set; }
        public string CustomLogMessage { get; set; }
        public string CustomLogName { get; set; }

        #endregion 扩展 接口级日志

        public static IEnumerable<logEvent> CreateMany(IEnumerable<LoggingEvent> loggingEvents)
        {
            return loggingEvents.Select(@event => Create(@event)).ToArray();
        }

        static logEvent Create(LoggingEvent loggingEvent)
        {
            var logEvent = new logEvent
            {
                domain = loggingEvent.Domain,
                timestamp = loggingEvent.TimeStamp.ToUniversalTime().ToString("O"),
                Exception = loggingEvent.ExceptionObject == null ? new object() : JsonSerializableException.Create(loggingEvent.ExceptionObject),
                CustomLogMessage = loggingEvent.RenderedMessage,
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
                        logEvent.ltctraid = mangoLogEvent.ltctraid;
                        logEvent.requestid = mangoLogEvent.requestid;
                        logEvent.startTime = mangoLogEvent.startTime.ToUniversalTime().ToString("O");
                        logEvent.CustomLogName = mangoLogEvent.CustomLogName;
                        logEvent.CustomLogMessage = mangoLogEvent.CustomLogMessage;
                        logEvent.RequestUserId = 0;
                        logEvent.RequestRaw = mangoLogEvent.RequestRaw;
                        logEvent.RequestIp = mangoLogEvent.RequestIp;
                        logEvent.RequestAgent = mangoLogEvent.RequestAgent;
                        //类信息
                        logEvent.RequestAssembly = mangoLogEvent.fullInfo;


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
                            if (appl_Path_Param.Length < 2)
                            {
                                appl_Path_Param = mangoLogEvent.Appl_Physical_Path.Split('/');
                                if (appl_Path_Param.Length > 3)
                                {
                                    logEvent.project = appl_Path_Param[3];
                                }
                            }
                            else if (appl_Path_Param.Length > 3)
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
            if (string.IsNullOrEmpty(logEvent.RequestAssembly) && loggingEvent.LocationInformation != null)
            {
                logEvent.RequestAssembly = loggingEvent.LocationInformation.FullInfo;
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