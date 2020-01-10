#if NETFRAMEWORK
using ServiceStack.Redis;
#else
using StackExchange.Redis;
#endif

namespace Mango.CodisHA
{
    /// <summary>
    /// Redis连接池构建类
    /// </summary>
    public static class RedisPoolBuilder
    {
#if NETFRAMEWORK
        private static RedisPool redisPool;
        /// <summary>
        /// 基于ZK的Codis初始化
        /// </summary>
        /// <param name="zkhosts">ZK地址，多个以“,”分割</param>
        /// <param name="db_proxy">CodisProxy实例节点</param>
        /// <param name="poolSize">连接池大小,默认1个</param>
        /// <param name="defaultdb">默认redis连接DB</param>
        public static void Init(string zkhosts, string db_proxy, int poolSize = 1, int defaultdb = 0)
        {
            redisPool = RoundRobinSSRedisPool.Create().CuratorClient(zkhosts, 60).ZkProxyDir(db_proxy).PoolConfig(poolSize, defaultdb).Build();
        }
        /// <summary>
        /// 基于Redis集群的Redis初始化
        /// </summary>
        /// <param name="readWriteHosts">设置Redis主机IP配置信息(读写)，多个逗号英文分隔.有密码的格式：redis:password@127.0.0.1:6379,无密码的格式：127.0.0.1:6379</param>
        /// <param name="readOnlyHosts">设置Redis主机IP配置信息(只读)</param>
        /// <param name="poolSize">连接池大小,默认1个</param>
        /// <param name="defaultdb">默认redis连接DB</param>
        public static void InitRedis(string readWriteHosts, string readOnlyHosts = "", int poolSize = 1, int defaultdb = 0)
        {
            redisPool = RoundRobinSSRedisPool.Create().RedisConfig(readWriteHosts, readOnlyHosts).PoolConfig(poolSize, defaultdb).BuildRedis();
        }
        /// <summary>
        /// 获取Redis连接
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            return redisPool.GetClient();
        }
#else
        private static RedisPool instance;
        /// <summary>
        /// <summary>
        /// 基于ZK的Codis初始化
        /// </summary>
        /// <param name="zkhosts">ZK地址，多个以“,”分割</param>
        /// <param name="db_proxy">CodisProxy实例节点</param>
        /// <param name="defaultdb">默认redis连接DB</param>
        public static void Init(string zkhosts, string db_proxy, int defaultdb = 0)
        {
            instance = SERedisClient.Create().CuratorClient(zkhosts, 60).ZkProxyDir(db_proxy).DefaultDB(defaultdb).Build();
        }
        /// <summary>
        /// <summary>
        /// 基于Redis集群的初始化
        /// </summary>
        /// <param name="redisMasterHostsStr">设置Redis主机IP配置信息，多个逗号英文分隔</param>
        /// <param name="defaultdb">默认redis连接DB</param>
        public static void InitRedis(string redisMasterHostsStr, int defaultdb = 0)
        {
            instance = SERedisClient.Create().RedisConfigHost(redisMasterHostsStr).DefaultDB(defaultdb).BuildRedis();
        }
        /// <summary>
        /// <summary>
        /// 基于Redis集群的初始化
        /// </summary>
        /// <param name="configurationStr">设置RedisSe的configuration连接</param>
        public static void InitRedisFullConn(string configurationStr)
        {
            instance = SERedisClient.Create().RedisConfiguration(configurationStr).BuildRedis();
        }
        /// <summary>
        /// 获取Redis连接实例
        /// </summary>
        /// <returns></returns>
        public static ConnectionMultiplexer GetInstance()
        {
            return instance.GetInstance();
        }
        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="db">-1为默认连接的数据库</param>
        /// <returns></returns>
        public static IDatabase GetDatabase(int db = -1)
        {
            var client = GetInstance();
            if (client == null)
            {
                return null;
            }
            return client.GetDatabase(db);
        }
#endif
    }
}
