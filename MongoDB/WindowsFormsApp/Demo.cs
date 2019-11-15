using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Models;

namespace WindowsFormsApp
{
    public class Demo
    {
        string mongodburl = "mongodb://192.168.50.138:27020";
        public string Demo1()
        {
            WCFMongoClient<Logs> client = new WCFMongoClient<Logs>(mongodburl);
            //排序方式
            var orderby = new List<CommonOrderModel>() { new CommonOrderModel() { Name = "CreateTime", Order = 1 } };
            //wcf查询过滤条件
            var filterList = new List<CommonFilterModel>();
            //增加过滤条件
            filterList.Add(new CommonFilterModel("LogName", "=", "FormDeubgeLog"));
            filterList.Add(new CommonFilterModel("UserId", "=", "58988"));
            filterList.Add(new CommonFilterModel("CreateTime", ">", "2019-9-19"));
            //filterList.Add(new CommonFilterModel("UserId", "in", new List<object>() { 58988, 58999 }));

            //GetListByQuery
            var res = client.GetListByQuery(1, 3, filterList, orderby);

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
    }
}
