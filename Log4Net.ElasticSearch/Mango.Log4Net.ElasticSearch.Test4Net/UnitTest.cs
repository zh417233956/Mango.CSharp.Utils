using System;
using Mango.Log4Net.ElasticSearch.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mango.Log4Net.ElasticSearch.Test4Net
{
    [TestClass]
    public class UnitTest
    {
        ILogger log = LogManager.GetLogger(typeof(UnitTest));
        [TestMethod]
        public void TestMethod()
        {
            var msg = new Models.LogEvent(Guid.NewGuid().ToString(), "测试.net","net_unittest", "hello Mango.Log4Net.ElasticSearch");
            log.Error(msg, new Exception("手动抛出异常"));
        }
    }
}
