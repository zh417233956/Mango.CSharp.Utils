using System;
using System.Collections.Generic;

namespace log4net.ElasticSearch.Models
{
    /// <summary>
    /// 芒果接口级日志
    /// </summary>
    public class MangoLogEvent
    {
        public MangoLogEvent()
        {
            this.CustomLogMessage = "";
        }
        private string _ltctraid;

        public string ltctraid
        {
            get
            {
                if (string.IsNullOrEmpty(_ltctraid))
                {
                    _ltctraid = Guid.NewGuid().ToString();
                }
                return _ltctraid;
            }
            set { _ltctraid = value; }
        }
        public string requestid { get; set; }
        public string CustomLogName { get; set; }
        public string CustomLogMessage { get; set; }

        #region 调用类信息
        public string className { get; set; }
        public string fileName { get; set; }
        public int lineNumber { get; set; }
        public string fullInfo { get; set; }
        public string methodName { get; set; }
        #endregion 调用类信息

        #region 服务器信息
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string LOCAL_ADDR { get; set; }
        public string RequestIp { get; set; }
        public string RequestAgent { get; set; }
        public string RequestRaw { get; set; }
        /// <summary>
        /// 获取域名
        /// </summary>
        public string SERVER_NAME { get; set; }
        /// <summary>
        /// 获取端口
        /// </summary>
        public string SERVER_PORT { get; set; }
        /// <summary>
        /// 服务器地址,IP+PORT
        /// </summary>
        public string HTTP_HOST { get; set; }
        /// <summary>
        /// 与应用程序元数据库路径相应的物理路径
        /// </summary>
        public string Appl_Physical_Path { get; set; }
        #endregion

        private DateTime _startTime = DateTime.Now;

        public DateTime startTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
    }
}
