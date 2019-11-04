#if NETSTANDARD
using Microsoft.AspNetCore.Http;
#endif
namespace Mango.Log4Net.ElasticSearch.Logging
{
    public class ExtensionMethods
    {
#if NETSTANDARD
        public static IHttpContextAccessor Accessor;
        public static HttpContext GetHttpContext()
        {
            if (Accessor != null)
            {
                return Accessor.HttpContext;
            }
            else
            {
                return null;
            }
        }
#endif
    }
}
