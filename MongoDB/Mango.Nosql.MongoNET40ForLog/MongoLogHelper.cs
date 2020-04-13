using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;

namespace Mango.Nosql.MongoNET40ForLog
{
    public class MongoLogHelper
    {
        public MongoClientHelper MongoClient;
        public MongoLogHelper()
        {
            MongoClient = new MongoClientHelper();
        }
        public bool Log2Mongo(Log_Info entity)
        {
            bool result = false;
            try
            {
                var mongo_entity = new Log_Info_Mongo();
                mongo_entity.LogClassID = entity.LogClassID;
                mongo_entity.LogTypeId = entity.LogTypeId;
                mongo_entity.mapid = entity.mapid;
                mongo_entity.mapid2 = entity.mapid2;
                mongo_entity.LogMemo = entity.LogMemo;
                mongo_entity.IsStar = entity.IsStar;
                mongo_entity.AddTime = entity.AddTime;
                mongo_entity.userid = entity.userid;
                mongo_entity.orgid = entity.orgid;
                mongo_entity.cityid = entity.cityid;
                mongo_entity.isdel = entity.isdel;
                mongo_entity.LogIDOld = entity.LogIDOld;
                mongo_entity.isadmin = entity.isadmin;
                mongo_entity.DBSource = entity.DBSource;
                mongo_entity.AddFromMethod = entity.AddFromMethod;
                //获取集合,库名：test，表名：Log_Info
                var collection = MongoClient.GetCollection<Log_Info_Mongo>("test", "testloginfo");
                //插入数据到表Log_Info
                var write_result = collection.Insert(mongo_entity);
                if (write_result.Ok)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 存入mongodb中Log_Info的实体
        /// </summary>
        public class Log_Info_Mongo
        {
            private string _id;
            [BsonElement("_id")]
            public string Id
            {
                set => _id = value;
                get
                {
                    _id = _id ?? Guid.NewGuid().ToString("N");
                    return _id;
                }
            }

            [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
            public DateTime AddTime { get; set; }

            public Log_Info_Mongo()
            {
                AddTime = DateTime.Now;
            }
            public int LogClassID { get; set; }
            public int LogTypeId { get; set; }
            public int mapid { get; set; }
            public int mapid2 { get; set; }
            public string LogMemo { get; set; }
            public bool IsStar { get; set; }            
            public int userid { get; set; }
            public int orgid { get; set; }
            public int cityid { get; set; }
            public int isdel { get; set; }
            public int LogIDOld { get; set; }
            public int isadmin { get; set; }
            public int DBSource { get; set; }
            public string AddFromMethod { get; set; }
        } 
    }

    /// <summary>
    /// Frame中Log_Info的实体，集成时请删除
    /// </summary>
    public class Log_Info
    {
        public Log_Info()
        {
            AddTime = DateTime.Now;
            adddate = AddTime.Date;
        }
        public int LogClassID { get; set; }
        public int LogTypeId { get; set; }
        public int mapid { get; set; }
        public int mapid2 { get; set; }
        public string LogMemo { get; set; }
        public bool IsStar { get; set; }
        public DateTime AddTime { get; set; }
        public int userid { get; set; }
        public int orgid { get; set; }
        public int cityid { get; set; }
        public int isdel { get; set; }
        public int LogIDOld { get; set; }
        public int isadmin { get; set; }
        public int DBSource { get; set; }
        public string AddFromMethod { get; set; }
        public DateTime adddate { get; set; }
    }

    /// <summary>
    /// Client连接帮助类
    /// </summary>
    public class MongoClientHelper
    {
        #region 初始化        
        private readonly MongoServer _mongoServer;

        public MongoClientHelper()
        {
            _mongoServer = MongoDBConnect.Server;
        }

        static MongoClientHelper()
        {
            ConventionRegistry.Register("IgnoreExtraElements",
                new ConventionPack { new IgnoreExtraElementsConvention(true) }, type => true);
        }
        #endregion

        public MongoCollection<T> GetCollection<T>(string database, string collection)
        {
            var db = _mongoServer.GetDatabase(database);
            return db.GetCollection<T>(collection);
        }
    }

    #region mongodb连接配置类
    /// <summary>
    /// 连接地址配置
    /// </summary>
    public static class MongoDBConfig
    {
        private static string connectionString;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new Exception("MongoDB的连接字符串不能为空");
                }
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }
    }
    /// <summary>
    /// Mongodb连接
    /// </summary>
    public static class MongoDBConnect
    {
        public static MongoClient Client;
        public static MongoServer Server;

        // static constructor
        static MongoDBConnect()
        {
            //var connectionString = ConnectionString ?? "mongodb://localhost/";
            var connectionString = MongoDBConfig.ConnectionString;
            var mongoUrl = new MongoUrl(connectionString);
            var clientSettings = MongoClientSettings.FromUrl(mongoUrl);
            if (Server != null)
            {
                Server.Disconnect();
            }
            Client = new MongoClient(clientSettings);
            Server = Client.GetServer();

            // connect early so BuildInfo will be populated
            Server.Connect();
        }
    }

    #endregion

    /*使用示例
     * 
        //初始化连接串
        //MongoDBConfig.ConnectionString = "mongodb://zhh:123456@192.168.50.190:27017";
        MongoDBConfig.ConnectionString = "mongodb://mongodb2.db.517.jiali:27017,mongodb1.db.517.jiali:27017";


        //插入示例
        MongoLogHelper mongolog = new MongoLogHelper();
        var entity = new Log_Info();
        entity.AddTime = DateTime.Now;
        entity.adddate = DateTime.Now;
        entity.IsStar = false;
        entity.cityid = 0;
        entity.LogClassID = 1;
        entity.LogTypeId = 0;
        entity.LogIDOld = 0;
        entity.userid = 0;
        entity.isdel = 0;
        entity.orgid = 22;
        entity.mapid = 884444;
        entity.LogMemo = "测试Log_Info插入";
        entity.isadmin = 0;
        entity.mapid2 = 884444;
        int dbSource=5;
        entity.DBSource = dbSource;
        entity.AddFromMethod = "zhonghai.mongodbtest.testinsert";
        mongolog.Log2Mongo(entity);

        //查询示例
        var log_table = mongolog.MongoClient.GetCollection<MongoCollection<BsonDocument>>("test","testloginfo");
        var Queryable_result = log_table.AsQueryable<MongoLogHelper.Log_Info_Mongo>().Where(w => w.LogClassID == entity.LogClassID);
        var str = Newtonsoft.Json.JsonConvert.SerializeObject(Queryable_result.ToList());
        Response.Write(str);

     * 
     * 
     */

}
