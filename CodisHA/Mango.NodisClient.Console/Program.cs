using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mango.NodisClient.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var helper = new UnitTest();
                helper.TestMethod1();

                var pools = helper.zkhelper.pools;

                foreach (var item in pools)
                {

                    System.Console.WriteLine($"节点：{item.Addr}");
                }
            }
            catch (Exception ex)
            {

                System.Console.WriteLine($"异常：{ex.Message}");
            }


            System.Console.ReadKey();
        }
    }

    public class UnitTest
    {
        private string zkAddr;
        private int zkSessionTimeout;
        private string zkProxyDir;
        public ZooKeeperHelper zkhelper;
        public UnitTest()
        {
            zkAddr = "localhost:20000";
            zkSessionTimeout = 5000;
            zkProxyDir = "mango";
        }
        public void TestMethod1()
        {
            zkhelper = new ZooKeeperHelper(zkAddr, zkProxyDir, zkSessionTimeout,
               (nodes) =>
               {
                   foreach (var item in nodes)
                   {
                       System.Console.WriteLine($"新增节点：{item.Addr}");
                   }
               },
               (nodes) =>
               {
                   foreach (var item in nodes)
                   {
                       System.Console.WriteLine($"删除节点：{item.Addr}");
                   }
               });

            //var pools = zkhelper.pools;

        }
    }
}
