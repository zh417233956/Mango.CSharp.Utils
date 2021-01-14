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
            //var msg = new Models.LogEvent("net_unittest", "hello Mango.Log4Net.ElasticSearch");
            //log.Error(msg, new Exception("手动抛出异常"));

            var msg2 = new Models.LogEvent("core_unittest2", "2hello Mango.Log4Net.ElasticSearch", "11d7d4c051f148f79925d5ca4ac0fa88", "11d7d4c051f148f79925d5ca4ac0fa89");
            log.Error(msg2, new Exception("手动抛出异常"));
        }
    }
}
