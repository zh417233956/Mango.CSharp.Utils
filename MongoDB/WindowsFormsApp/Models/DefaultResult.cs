using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.Models
{
    public class DefaultResult<MT>
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 数据集合，添加数据时为主键
        /// </summary>
        public MT Data { get; set; }
        /// <summary>
        /// 加密数据字符串
        /// </summary>
        public string RETData { get; set; }

        private string _debug;
        /// <summary>
        /// 调试信息
        /// </summary>
        public string Debug
        {
            get
            {
                return _debug ?? "";
            }
            set { _debug = value; }
        }
        /// <summary>
        /// 执行耗时
        /// </summary>
        public double RunTime { get; set; }
        /// <summary>
        /// 数据影响行数
        /// </summary>
        public int RetInt { get; set; }
    }
}
