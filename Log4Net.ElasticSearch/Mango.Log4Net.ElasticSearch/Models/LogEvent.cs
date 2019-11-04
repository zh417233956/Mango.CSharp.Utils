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
        /// <param name="uuid">业务编号</param>
        /// <param name="msg">消息描述</param>
        /// <param name="request">请求消息</param>
        /// <param name="response">响应消息</param>
        /// <param name="otherMsg">其它追加消息</param>
        public LogEvent(string uuid, string msg, string request = "", string response = "", Dictionary<string, string> otherMsg = null)
        {
            this._uuid = uuid;
            this.message = msg;
            this.request = request;
            this.response = response;
            this.uuidtag = "";
            this.otherMsg = otherMsg;
            if (otherMsg == null)
            {
                this.otherMsg = new Dictionary<string, string>();
            }
        }
        private string _uuid;

        public string uuid
        {
            get
            {
                if (string.IsNullOrEmpty(_uuid))
                {
                    _uuid = Guid.NewGuid().ToString();
                    uuidtag = "new";
                }
                return _uuid;
            }
        }

        public string uuidtag { get; set; }
        public string message { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public IDictionary<string, string> otherMsg { get; set; }
    }

}
