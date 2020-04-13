namespace Mango.Log4Net.ElasticSearch.Models
{
    /// <summary>
    /// 携带类信息
    /// </summary>
    public class LogEventClassInfo : LogEvent
    {
        public LogEventClassInfo() : base("", "","")
        {

        }
        public new string uuid { get; set; }
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
    }
}
