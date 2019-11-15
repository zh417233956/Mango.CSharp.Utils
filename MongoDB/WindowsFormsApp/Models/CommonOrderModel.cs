using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp.Models
{
    //
    // 摘要:
    //     通用排序字段
    public class CommonOrderModel
    {
        //
        // 摘要:
        //     排序名(对就实体类属性名)
        public string Name;
        //
        // 摘要:
        //     排序(0 ASC 1 DESC)
        public int Order;

        public CommonOrderModel() { }
    }
}
