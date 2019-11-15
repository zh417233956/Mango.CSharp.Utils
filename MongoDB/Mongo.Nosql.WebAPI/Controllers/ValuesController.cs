using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Nosql.Mongo.Base;
using Mango.Wcf.Util.WCF.Models;
using Microsoft.AspNetCore.Mvc;
using Mongo.Nosql.WebAPI.code;
using MongoDB.Driver;
using Mango.Nosql.Mongo.Extension;
using System.Linq.Expressions;

namespace Mongo.Nosql.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<object> Get()
        {
            return Mongodb_House_Get();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<DefaultResult<int>> Get(int id)
        {
            return Mongodb_House_Set();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        #region 房源
        public DefaultResult<List<Log_Info_House>> Mongodb_House_Get()
        {
            WCFMongoClient<Log_Info_House> client = new WCFMongoClient<Log_Info_House>(Startup.MongodbUrl);

            //排序方式
            var orderby = new List<CommonOrderModel>() { new CommonOrderModel() { Name = "AddTime", Order = 1 } };
            //wcf查询过滤条件
            var filterList = new List<CommonFilterModel>();
            //增加过滤条件
            filterList.Add(new CommonFilterModel("userid", "=", "58989"));
            filterList.Add(new CommonFilterModel("AddTime", ">", DateTime.Now.AddMinutes(-10).ToString()));
            var res = client.GetListByQuery(1, 1, filterList, orderby);

            //Expression<Func<Log_Info_House, bool>> predicate = u => u.userid == 58989;
            //Func<Sort<Log_Info_House>, Sort<Log_Info_House>> sort = b => b.Desc(c => c.AddTime);
            //var res = client.mongoRepository.ToList<Log_Info_House>(predicate, sort, 1);

            return res;

        }
        public DefaultResult<int> Mongodb_House_Set()
        {
            WCFMongoClient<Log_Info_House> client = new WCFMongoClient<Log_Info_House>(Startup.MongodbUrl);
            var log = new Log_Info_House
            {
                LogMemo = "(Attachment_Sync_PicCount审核附件触发)修改了房源图片数量由[5]修改为[6](此条日志仅供研发部参考使用)",
                LogTypeId=32,
                LogClassID=2,
                userid = 58989,
                cityid = 1,
                orgid = 22,
                DBSource=5,
                AddFromMethod= "MIS2014",
                mapid= 3257279
            };
            var result = client.Add(log);


            return result;
        }
        #endregion

        #region testLog
        public DefaultResult<List<Logs>> mongodbGet()
        //public List<Logs> mongodbGet()
        {
            WCFMongoClient<Logs> client = new WCFMongoClient<Logs>(Startup.MongodbUrl);

            //排序方式
            var orderby = new List<CommonOrderModel>() { new CommonOrderModel() { Name = "CreateTime", Order = 1 } };
            //wcf查询过滤条件
            var filterList = new List<CommonFilterModel>();
            //增加过滤条件
            filterList.Add(new CommonFilterModel("UserId", "=", "58988"));
            var res = client.GetListByQuery(1, 1, filterList, orderby);

            //Expression<Func<Logs, bool>> predicate = u => u.UserId == 58988;
            //Func<Sort<Logs>, Sort<Logs>> sort = b => b.Desc(c => c.CreateTime);
            //var res = client.mongoRepository.ToList<Logs>(predicate, sort, 1);

            return res;

            #region 测试
            //var res = client.mongoRepository.PageList<Logs>(u => u.UserId == 58988, b => b.Desc(c => c.CreateTime), 1, 1);
            //GetListByQuery
            //var res = client.GetModel(filterList, orderby);
            //var res = client.GetModelByID("ed13c7e7b4cd478ca0fe37a7a709d6ec");
            //var rep = client.mongoRepository;
            //IMongoCollection<T> collection2 = GetCollection<T>(database, collection);
            //int totalCount = (int)IMongoCollectionExtensions.CountDocuments(collection2, predicate);
            //IFindFluent<T, T> findFluent = collection2.Find(predicate);
            //if (sort != null)
            //{
            //    findFluent = findFluent.Sort(sort.GetSortDefinition());
            //}
            //findFluent = findFluent.Skip((pageIndex - 1) * pageSize).Limit(pageSize);
            //List<TResult> items = findFluent.Project(selector).ToList();

            //return  rep.Get<Logs>(w => w.UserId == 58988,b=>b.Desc(c=>c.CreateTime));


            ////无任何转换查询

            //Expression<Func<Logs, bool>> predicate = u => u.UserId == 58988;
            //var _mongoClient = new MongoClient(Startup.MongodbUrl);
            //IMongoCollection<Logs> collection2 = _mongoClient.GetDatabase("test").GetCollection<Logs>("testlogs");
            //int totalCount = (int)IMongoCollectionExtensions.CountDocuments(collection2, predicate);
            //IFindFluent<Logs, Logs> findFluent = collection2.Find(predicate);
            //Func<Sort<Logs>, Sort<Logs>> sort = b => b.Desc(c => c.CreateTime);
            //if (sort != null)
            //{
            //    findFluent = findFluent.Sort(sort.GetSortDefinition());
            //}
            //findFluent = findFluent.Skip((1 - 1) * 1).Limit(1);
            //List<Logs> items = findFluent.Project((Logs a) => a).ToList();

            //return items;
            #endregion 测试
        }
        public DefaultResult<int> mongodbSet()
        {
            WCFMongoClient<Logs> client = new WCFMongoClient<Logs>(Startup.MongodbUrl);
            var log = new Logs
            {
                Project = "misapi2018",
                HostId = "192.168.4.144:8008",
                LogName = "FormDeubgeLog",
                Level = "debug",
                UserId = 58988,
                Url = "http://misapi2018ali.517api.cn:8110/api/House/GetHouse_List_V1",
                RawUrl = "/api/House/GetHouse_List_V1",
                UrlReferrer = "",
                IP = "175.161.71.161, 111.202.96.71",
                OtherMsg = new Dictionary<string, string>(){
                    { "method", "post"},
                },
                Msg = "压力测试"
            };
            var result = client.Add(log);


            return result;
        }
        #endregion

    }
}
