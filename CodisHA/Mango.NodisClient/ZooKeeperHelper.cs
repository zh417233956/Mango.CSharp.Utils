using Mango.NodisClient.Rabbit.ZookeeperExt;
using System;
using System.Collections.Generic;

namespace Mango.NodisClient
{
    /// <summary>
    /// zk配置及建立监听
    /// </summary>
    public class ZooKeeperHelper : IDisposable
    {
        public ZooKeeperClient _zk;
        private CodisWatcher.DeleteNodeDel deleteNodeDel;
        private CodisWatcher.AddNodeDel addNodeDel;
        public CodisWatcher codiswatcher;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="log"></param>
        /// <param name="connectionString"></param>
        /// <param name="proxy"></param>
        /// <param name="SessionTimeout">默认20(秒)</param>
        /// <param name="addNodeDel"></param>
        /// <param name="deleteNodeDel"></param>
        public ZooKeeperHelper(string connectionString, string proxy, double SessionTimeout = 20, CodisWatcher.AddNodeDel addNodeDel = null, CodisWatcher.DeleteNodeDel deleteNodeDel = null)
        {           
            _zk = new ZooKeeperClient(connectionString, SessionTimeout);
            this.addNodeDel = addNodeDel;
            this.deleteNodeDel = deleteNodeDel;
            codiswatcher = new CodisWatcher(_zk, proxy, addNodeDel, deleteNodeDel);
            codiswatcher.ProcessWatched();
        }
        /// <summary>
        /// 当前的节点列表
        /// </summary>
        public List<CodisProxyInfo> pools => AsyncUtil.RunSync(()=> codiswatcher.GetPools());

        /// <summary>
        /// 强制从服务器获取的节点列表
        /// </summary>
        public List<CodisProxyInfo> refreshPools => AsyncUtil.RunSync(() => codiswatcher.GetAllPools());

        /// <summary>
        /// 执行与释放或重置非托管资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 获取或设置一个值。该值指示资源已经被释放。
        /// </summary>
        private bool _disposed;
        /// <summary>
        /// 由终结器调用以释放资源。
        /// </summary>
        ~ZooKeeperHelper()
        {
            Dispose(false);
        }
        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
            }
            // 标记已经被释放。
            _disposed = true;
        }
    }
}
