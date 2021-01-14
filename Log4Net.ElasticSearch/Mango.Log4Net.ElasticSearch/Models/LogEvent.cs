using System;
using System.Collections.Generic;

namespace Mango.Log4Net.ElasticSearch.Models
{
    /// <summary>
    /// 接口级日志Model
    /// </summary>
    public class LogEvent
    {
        /// <summary>
        /// 消息实体
        /// </summary>
        /// <param name="CustomLogName">业务类型</param>
        /// <param name="CustomLogMessage">消息描述</param>
        public LogEvent( string CustomLogName, string CustomLogMessage)
        {
            this.CustomLogName = CustomLogName;
            this.CustomLogMessage = CustomLogMessage;
        }
        public string CustomLogName { get; set; }
        public string CustomLogMessage { get; set; }

        /// <summary>
        /// 消息实体
        /// </summary>
        /// <param name="CustomLogName">业务类型</param>
        /// <param name="CustomLogMessage">消息描述</param>
        /// <param name="ltctraid">ltctraid</param>
        /// <param name="requestid">requestid</param>
        public LogEvent(string CustomLogName, string CustomLogMessage,string ltctraid,string requestid)
        {
            this.CustomLogName = CustomLogName;
            this.CustomLogMessage = CustomLogMessage;
            ltcid = ltctraid;
            reqid = requestid;
        }
        public string ltcid { get; set; }
        public string reqid { get; set; }
    }

}
