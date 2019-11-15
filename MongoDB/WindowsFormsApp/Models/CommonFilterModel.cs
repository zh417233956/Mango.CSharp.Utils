using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.Models
{
    //
    // 摘要:
    //     通用生成查询条件
    //
    public class CommonFilterModel
    {
        //
        // 摘要:
        //     构造函数
        public CommonFilterModel()
        { }
        //
        // 摘要:
        //     初始化一个 TT.Common.Frame.Model.CommonFilterModel 查询对象(单值).
        //
        // 参数:
        //   name:
        //     字段名
        //
        //   filter:
        //     判断条件(=>....like in not in)
        //
        //   value:
        //     检索值
        public CommonFilterModel(string name, string filter, string value)
        {
            Name = name;
            Filter = filter;
            Value = value;
        }
        //
        // 摘要:
        //     初始化一个 TT.Common.Frame.Model.CommonFilterModel 查询对象(多值).
        //
        // 参数:
        //   name:
        //     字段名
        //
        //   filter:
        //     判断条件(=>....like in not in)
        //
        //   values:
        //     检索值
        public CommonFilterModel(string name, string filter, List<object> values)
        {
            Name = name;
            Filter = filter;
            ListValue = values;
        }

        //
        // 摘要:
        //     字段名(对就实体类属性名)
        public string Name { get; set; }
        //
        // 摘要:
        //     查询条件(=>....like in not in)
        public string Filter { get; set; }
        //
        // 摘要:
        //     用来查询的值
        public string Value { get; set; }
        //
        // 摘要:
        //     采用In时的列表数据
        public List<object> ListValue { get; set; }
    }
}
