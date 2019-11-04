using Mango.Log4Net.ElasticSearch.Logging;
using System;
using Xunit;

namespace Mango.Log4Net.ElasticSearch.Test
{
    public class UnitTest
    {
        ILogger log = LogManager.GetLogger(typeof(UnitTest));
        [Fact]
        public void Test()
        {
            var msg = new Models.LogEvent(Guid.NewGuid().ToString(), "测试.net core", "hello Mango.Log4Net.ElasticSearch");
            log.Error(msg, new Exception("手动抛出异常"));
        }
    }
}
