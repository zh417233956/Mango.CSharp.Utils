using Mango.Nosql.Mongo;
using Mango.Nosql.Mongo.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.Models;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitMongoRepository();
        }

        public MongoRepository mongoRepository;
        string mongodburl = "mongodb://192.168.50.138:27020";
        private void InitMongoRepository()
        {
            mongoRepository = new MongoRepository(mongodburl);
        }
        private void btn_query_Click(object sender, EventArgs e)
        {
            var result = new Demo().Demo1();
            this.textBox1.Text = result;
        }
        private void btn_query_old_Click(object sender, EventArgs e)
        {
            Expression<Func<Logs, bool>> predicate = w => w.UserId == 58988 && w.LogName == "FormDeubgeLog";
            Func<Sort<Logs>, Sort<Logs>> sort = o => o.Desc(b => b.CreateTime);
            var log = mongoRepository.Get<Logs>(predicate, sort);
            if (log != null)
            {
                this.textBox1.Text = $"查询成功：{ Newtonsoft.Json.JsonConvert.SerializeObject(log) }";
            }
            else
            {
                this.textBox1.Text = $"查询成功：暂无数据";
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var addForm = new AddForm(this);
            addForm.Show();
        }
        public bool add_Data(string msg)
        {
            var log = new Logs
            {
                Project = "misapi2018",
                HostId = "192.168.4.144:8008",
                LogName = "FormDeubgeLog",
                Level = Level.Debug,
                UserId = 58988,
                Url = "http://misapi2018ali.517api.cn:8110/api/House/GetHouse_List_V1",
                RawUrl = "/api/House/GetHouse_List_V1",
                UrlReferrer = "",
                IP = "175.161.71.161, 111.202.96.71",
                OtherMsg = new Dictionary<string, string>(){
                    { "content-type", "application/json; charset=utf-8"},
                    { "method", "post"},
                },
                Msg = msg
            };
            var result = mongoRepository.Add(log);
            var quernLog = mongoRepository.Get<Logs>(w => w.UserId == 58988 && w.LogName == "FormDeubgeLog", o => o.Desc(b => b.CreateTime));
            if (quernLog != null)
            {
                this.textBox1.Text = $"添加成功：{ quernLog.Msg }";
            }
            else
            {
                this.textBox1.Text = $"添加失败";
            }
            return result;

        }
    }
}
