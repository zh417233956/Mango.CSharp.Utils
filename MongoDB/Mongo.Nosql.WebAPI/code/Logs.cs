using Mango.Nosql.Mongo.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo.Nosql.WebAPI.code
{
    #region model
    [Mongo("test", "testlogs")]
    public class Logs : MongoEntity
    {
        public Logs()
        {
            OtherMsg = new Dictionary<string, string>();
            CreateTime = DateTime.Now;
        }
        public string Project { get; set; }
        public string HostId { get; set; }
        public string Level { get; set; }
        public string LogName { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public string RawUrl { get; set; }
        public string UrlReferrer { get; set; }
        public string IP { get; set; }
        public Exception Exception { get; set; }
        public string Msg { get; set; }
        public IDictionary<string, string> OtherMsg { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }

    }

    #endregion model
}
