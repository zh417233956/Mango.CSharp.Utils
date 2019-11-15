using Mango.Nosql.Mongo.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo.Nosql.WebAPI.code
{
    [Mongo("test", "loginfohouse")]
    public class Log_Info_House : MongoEntity
    {
        public Log_Info_House()
        {
            AddTime = DateTime.Now;
            adddate = AddTime.Date;
        }
        /// <summary>
        /// 日志类别,应有独立字典表(DicClass=110);0=不详;1=房源;2=客源;3=小区;4=合同;5=组织;6=经纪人
        /// </summary>
        public int LogClassID { get; set; }
        public int mapid { get; set; }
        public int mapid2 { get; set; }
        public string LogMemo { get; set; }
        public string LogMemoBak { get; set; }
        public int IsStar { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime AddTime { get; set; }
        public int userid { get; set; }
        public int orgid { get; set; }
        public int cityid { get; set; }
        public int isdel { get; set; }
        public int LogIDOld { get; set; }
        public int LogTypeId { get; set; }
        public int IsChufa { get; set; }
        public int isadmin { get; set; }
        public int adduserzhuliid { get; set; }
        public int DBSource { get; set; }
        public string AddFromMethod { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime adddate { get; set; }
        public int ucid { get; set; }

    }
}
