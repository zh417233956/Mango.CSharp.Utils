using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mango.NodisClient;
#if NETFRAMEWORK
using ServiceStack.Redis;
#else
using StackExchange.Redis;
#endif

namespace Mango.CodisHA
{
    /// <summary>
    /// Redis连接池帮助类
    /// </summary>
    public class RedisPool
    {
#if NETFRAMEWORK
        #region NETFRAMEWORK

        private IRedisClientsManager Manager;

        int maxWritePoolSize = 1;
        int defaultDb = 0;
        /// <summary>
        /// 连接池设置
        /// </summary>
        /// <param name="poolSize">连接池大小</param>
        /// <param name="defaultdb"></param>
        /// <returns></returns>
        public RedisPool PoolConfig(int poolSize, int defaultdb = 0)
        {
            this.maxWritePoolSize = poolSize;
            this.defaultDb = defaultdb;
            return this;
        }
        /// <summary>
        /// 构建一个监听zk变化自动更新的连接池
        /// </summary>
        /// <returns></returns>
        public RedisPool Build()
        {
            #region zk配置获取及建立监听
            validate();
            if (zkhelper != null)
            {
                zkhelper.Dispose();
            }
            zkhelper = new ZooKeeperHelper(zkAddr, zkProxyDir, zkSessionTimeout,
                (nodes) =>
                {
                    CreateManager();
                },
                (nodes) =>
                {
                    CreateManager();
                });

            #endregion zk配置获取及建立监听

            CreateManager();

            return this;
        }

        /// <summary>
        /// 获取Redis连接
        /// </summary>
        /// <returns></returns>
        public IRedisClient GetClient()
        {
            try
            {
                var client = Manager.GetClient();
                return client;
            }
            catch (System.Exception)
            {
                return null;
            }
        }       
        /// <summary>
        /// Codis建立连接池
        /// </summary>
        private void CreateManager()
        {
            // 读取Redis主机IP配置信息
            // 有密码的格式：redis:password@127.0.0.1:6379
            // 无密码的格式：127.0.0.1:6379
            var pools = zkhelper.pools;
            string[] redisMasterHosts = pools.Select(m => m.Addr).ToArray();
            // 如果Redis服务器是主从配置，则还需要读取Redis Slave机的IP配置信息
            string[] redisSlaveHosts = null;
            //string slaveConnection = null;
            //if (!string.IsNullOrWhiteSpace(slaveConnection))
            //{
            //    redisSlaveHosts = slaveConnection.Split(',');
            //}

            var redisClientManagerConfig = new RedisClientManagerConfig
            {
                MaxWritePoolSize = maxWritePoolSize,
                DefaultDb = defaultDb,
                AutoStart = true
            };
            // 创建Redis连接池
            if (Manager != null)
            {
                Manager.Dispose();
            }

            #region 只为打日志
            //string redisMasterHostsStr = "";
            //foreach (var itemHost in redisMasterHosts)
            //{
            //    redisMasterHostsStr += itemHost + ",";
            //}
            //log.InfoFormat("创建Redis连接池，RedisMasterHosts：{0}", redisMasterHostsStr.TrimEnd(','));
            #endregion 只为打日志

            Manager = new PooledRedisClientManager(redisMasterHosts, redisSlaveHosts, redisClientManagerConfig)
            {
                PoolTimeout = 2000,
                ConnectTimeout = 1000
            };
        }

        #region Redis集群

        /// <summary>
        /// 构建一个监听zk变化自动更新的连接池
        /// </summary>
        /// <returns></returns>
        public RedisPool BuildRedis()
        {
            validateRedis();

            CreateManagerByRedis();

            return this;
        }

        string readWriteHosts = "";
        string readOnlyHosts = "";
        /// <summary>
        /// 设置Redis主机IP配置信息
        /// 有密码的格式：redis:password@127.0.0.1:6379
        /// 无密码的格式：127.0.0.1:6379
        /// </summary>
        /// <param name="readWriteHosts">读写，主</param>
        /// <param name="readOnlyHosts">只读，从</param>
        /// <returns></returns>
        public RedisPool RedisConfig(string readWriteHosts, string readOnlyHosts = "")
        {
            this.readWriteHosts = readWriteHosts;
            this.readOnlyHosts = readOnlyHosts;
            return this;
        }
        /// <summary>
        /// Redis建立连接池
        /// </summary>
        private void CreateManagerByRedis()
        {
            // 读取Redis主机IP配置信息
            // 有密码的格式：redis:password@127.0.0.1:6379
            // 无密码的格式：127.0.0.1:6379
            string[] redisMasterHosts = readWriteHosts.Split(',');
            // 如果Redis服务器是主从配置，则还需要读取Redis Slave机的IP配置信息
            string[] redisSlaveHosts = null;
            if (!string.IsNullOrWhiteSpace(readOnlyHosts))
            {
                redisSlaveHosts = readOnlyHosts.Split(',');
            }

            var redisClientManagerConfig = new RedisClientManagerConfig
            {
                MaxWritePoolSize = maxWritePoolSize,
                DefaultDb = defaultDb,
                AutoStart = true
            };

            Manager = new PooledRedisClientManager(redisMasterHosts, redisSlaveHosts, redisClientManagerConfig)
            {
                PoolTimeout = 2000,
                ConnectTimeout = 1000
            };
        }
        /// <summary>
        /// 参数校验检查
        /// </summary>
        private void validateRedis()
        {
            if (string.IsNullOrEmpty(readWriteHosts))
            {
                throw new Exception("readWriteHosts can not be null");
            }           
        }
        #endregion Redis集群

        #endregion NETFRAMEWORK
#else
        #region NETSTANDARD


        int defaultDb = 0;
        /// <summary>
        /// 默认redis连接DB设置
        /// </summary>
        /// <param name="defaultdb"></param>
        /// <returns></returns>
        public RedisPool DefaultDB(int defaultdb = 0)
        {
            this.defaultDb = defaultdb;
            return this;
        }
        /// <summary>
        /// 构建一个监听zk变化自动更新的连接实例
        /// </summary>
        /// <returns></returns>
        public RedisPool Build()
        {
        #region zk配置获取及建立监听
            validate();
            if (zkhelper != null)
            {
                zkhelper.Dispose();
            }
            zkhelper = new ZooKeeperHelper(zkAddr, zkProxyDir, zkSessionTimeout,
                (nodes) =>
                {                   
                    CreateInstance();
                },
                (nodes) =>
                {
                    CreateInstance();
                });       

        #endregion zk配置获取及建立监听

            CreateInstance();

            return this;
        }

        /// <summary>
        /// 获取Redis连接实例
        /// </summary>
        /// <returns></returns>
        public ConnectionMultiplexer GetInstance()
        {
            try
            {
                if (Instance.IsConnected)
                {
                    return Instance;
                }
                else
                {
                    throw new Exception("连接实例已断开");
                }
            }
            catch (System.Exception ex)
            {
                //log.ErrorFormat("获取Redis连接实例异常:{0}", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 连接实例
        /// </summary>
        private ConnectionMultiplexer Instance = null;

        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public void CreateInstance()
        {
            string redisMasterHostsStr = "";
            foreach (var itemHost in zkhelper.pools)
            {
                redisMasterHostsStr += itemHost.Addr + ",";
            }
            var constr = "{0}DefaultDatabase={1}";

            if (Instance != null)
            {
                ConnectionMultiplexer oldInstance = null;
                lock (Instance)
                {
                    oldInstance = Instance;                   
                    //log.InfoFormat("重新创建Redis实例，RedisHosts：{0}", redisMasterHostsStr.TrimEnd(','));
                    Instance = ConnectionMultiplexer.Connect(string.Format(constr, redisMasterHostsStr, defaultDb));
                }
                System.Threading.Thread.Sleep(1000);
                oldInstance.CloseAsync();
                oldInstance.Dispose();
                //log.InfoFormat("销毁Redis实例完成");
            }
            else
            {
        #region 只为打日志         
                //log.InfoFormat("创建Redis实例，RedisHosts：{0}", redisMasterHostsStr.TrimEnd(','));
        #endregion 只为打日志            
                Instance = ConnectionMultiplexer.Connect(string.Format(constr, redisMasterHostsStr, defaultDb));
            }
        }
        #endregion

        #region Redis集群

        /// <summary>
        /// 构建一个监听zk变化自动更新的连接池
        /// </summary>
        /// <returns></returns>
        public RedisPool BuildRedis()
        {
            validateRedis();

            CreateInstanceByRedis();

            return this;
        }

        string configurationStr = "";
        string redisMasterHostsStr = "";
        string connectStr = "";
        /// <summary>
        /// 设置Redis主机IP配置信息,多个用逗号分隔
        /// </summary>
        /// <param name="redisMasterHostsStr"></param>
        /// <returns></returns>
        public RedisPool RedisConfigHost(string redisMasterHostsStr)
        {
            this.redisMasterHostsStr = redisMasterHostsStr;
            var constr = "{0},DefaultDatabase={1}";
            connectStr = string.Format(constr, redisMasterHostsStr, defaultDb);
            return this;
        }

        /// <summary>
        /// 设置RedisSe的configuration连接
        /// </summary>
        /// <param name="configurationStr"></param>
        /// <returns></returns>
        public RedisPool RedisConfiguration(string configurationStr)
        {
            connectStr = configurationStr;
            return this;
        }

        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public void CreateInstanceByRedis()
        {
            Instance = ConnectionMultiplexer.Connect(connectStr);
        }

        /// <summary>
        /// 参数校验检查
        /// </summary>
        private void validateRedis()
        {
            if (string.IsNullOrEmpty(redisMasterHostsStr) && string.IsNullOrEmpty(configurationStr))
            {
                throw new Exception("redisMasterHostsStr or configurationStr can not be null");
            }
        }
        #endregion Redis集群
#endif

        #region ZK配置
        private string zkAddr;
        private int zkSessionTimeout;
        private string zkProxyDir;
        private ZooKeeperHelper zkhelper;
        /// <summary>
        /// ZK信息配置
        /// </summary>
        /// <param name="zkAddr"></param>
        /// <param name="zkSessionTimeout"></param>
        /// <returns></returns>
        public RedisPool CuratorClient(string zkAddr, int zkSessionTimeout = 20)
        {
            this.zkAddr = zkAddr;
            this.zkSessionTimeout = zkSessionTimeout;
            return this;
        }
        /// <summary>
        /// CodisProxy实例节点设置
        /// </summary>
        /// <param name="zkProxyDir"></param>
        /// <returns></returns>
        public RedisPool ZkProxyDir(string zkProxyDir)
        {
            this.zkProxyDir = zkProxyDir;
            return this;
        }
        #endregion ZK配置
        /// <summary>
        /// 参数校验检查
        /// </summary>
        private void validate()
        {
            if (string.IsNullOrEmpty(zkProxyDir))
            {
                throw new Exception("zkProxyDir can not be null");
            }
            if (string.IsNullOrEmpty(zkAddr))
            {
                throw new Exception("zk client can not be null");
            }
        }
    }
}
