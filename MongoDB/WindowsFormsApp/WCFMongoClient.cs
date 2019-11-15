using Mango.Nosql.Mongo;
using Mango.Nosql.Mongo.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp.Models;

namespace WindowsFormsApp
{
    public class WCFMongoClient<T> where T : MongoEntity
    {
        #region 构造函数
        string mongodburl = "mongodb://localhost:27017";
        public WCFMongoClient()
        {
            InitMongoRepository();
        }
        public WCFMongoClient(string mongodburl)
        {
            this.mongodburl = mongodburl;
            InitMongoRepository();
        }
        public MongoRepository mongoRepository;
        private void InitMongoRepository()
        {           
            mongoRepository = new MongoRepository(mongodburl);
        }
        #endregion 构造函数

        #region 分页数据

        /// <summary>
        /// 通过分页、条件、排序查询数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="filters"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public DefaultResult<List<T>> GetListByQuery(int page, int pagesize, List<CommonFilterModel> filters, List<CommonOrderModel> orders)
        {
            var dtstart = DateTime.Now;
            Expression<Func<T, bool>> predicate = GenerateCommonFilterModel<T>(filters);

            Func<Sort<T>, Sort<T>> sort = GenerateCommonOrderModel<T>(orders);
            var dt1 = DateTime.Now;
            var result = mongoRepository.PageList<T>(predicate, sort, page, pagesize);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<List<T>>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"查询Lambda表达式：{predicate.Body.ToString()}");
            debugeInfo.Append($",查询耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = result.Total;
            mongoRet.Data = result.Items;
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }
        public DefaultResult<int> GetCountByQueryLock(List<CommonFilterModel> filters)
        {
            var dtstart = DateTime.Now;
            Expression<Func<T, bool>> predicate = GenerateCommonFilterModel<T>(filters);
            var dt1 = DateTime.Now;
            var result = mongoRepository.Count<T>(predicate);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<int>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"查询Lambda表达式：{predicate.Body.ToString()}");
            debugeInfo.Append($",查询耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = result;
            mongoRet.Data = result;
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }
        
        #endregion

        #region 增改查
        public DefaultResult<int> Add(T entity)
        {
            var dtstart = DateTime.Now;

            var dt1 = DateTime.Now;
            var result = mongoRepository.Add<T>(entity);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<int>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"插入耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = 1;
            mongoRet.Data = result ? 1 : 0;
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }

        public DefaultResult<int> Update(T entity)
        {
            var dtstart = DateTime.Now;

            var dt1 = DateTime.Now;
            var result = mongoRepository.Update<T>(entity);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<int>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"更新耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = 1;
            mongoRet.Data = result ? 1 : 0;
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }

        public DefaultResult<int> UpdateByFilters(T entity, List<CommonFilterModel> filters)
        {
            var dtstart = DateTime.Now;
            Expression<Func<T, bool>> predicate = GenerateCommonFilterModel<T>(filters);
            var dt1 = DateTime.Now;
            var result = mongoRepository.Update<T>(predicate, u => entity);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<int>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"查询Lambda表达式：{predicate.Body.ToString()}");
            debugeInfo.Append($",更新耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = 1;
            mongoRet.Data = Convert.ToInt32(result);
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }
        public DefaultResult<T> GetModelByID(string entityId)
        {
            var dtstart = DateTime.Now;

            var dt1 = DateTime.Now;
            var result = mongoRepository.Get<T>(p => p.Id == entityId);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<T>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"查询耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = 1;
            mongoRet.Data = result;
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }
        public DefaultResult<T> GetModel(List<CommonFilterModel> filters)
        {
            var dtstart = DateTime.Now;
            Expression<Func<T, bool>> predicate = GenerateCommonFilterModel<T>(filters);
            var dt1 = DateTime.Now;
            var result = mongoRepository.Get<T>(predicate);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<T>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"查询Lambda表达式：{predicate.Body.ToString()}");
            debugeInfo.Append($",查询耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = 1;
            mongoRet.Data = result;
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }
        public DefaultResult<T> GetModel(List<CommonFilterModel> filters, List<CommonOrderModel> orders)
        {
            var dtstart = DateTime.Now;
            Expression<Func<T, bool>> predicate = GenerateCommonFilterModel<T>(filters);
            Func<Sort<T>, Sort<T>> sort = GenerateCommonOrderModel<T>(orders);
            var dt1 = DateTime.Now;
            var result = mongoRepository.Get<T>(predicate, sort);
            var dt2 = DateTime.Now;

            var mongoRet = new DefaultResult<T>();
            StringBuilder debugeInfo = new StringBuilder();
            debugeInfo.Append($"查询Lambda表达式：{predicate.Body.ToString()}");
            debugeInfo.Append($",查询耗时：{(dt2 - dt1).TotalMilliseconds}ms");
            mongoRet.Debug = debugeInfo.ToString();
            mongoRet.RetInt = 1;
            mongoRet.Data = result;
            mongoRet.RunTime = (DateTime.Now - dtstart).TotalMilliseconds;
            return mongoRet;
        }

        #endregion

        #region 生成查询表达式

        /// <summary>
        /// 根据条件数据动态生成连接条件
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        private Expression<Func<TSource, bool>> GenerateCommonFilterModel<TSource>(List<CommonFilterModel> filters)
        {
            Expression<Func<TSource, bool>> andExp = null;
            Expression body = null;
            var p = Expression.Parameter(typeof(TSource), "p");
            foreach (var item in filters)
            {
                Expression bodyItem = null;
                var propertyName = Expression.Property(p, item.Name);
                object itemValue = null;
                if (propertyName.Type == typeof(DateTime))
                {
                    itemValue = Convert.ToDateTime(item.Value);
                }
                else if (propertyName.Type == typeof(int))
                {
                    itemValue = Convert.ToInt32(item.Value);
                }
                else if (propertyName.Type == typeof(decimal))
                {
                    itemValue = Convert.ToDecimal(item.Value);
                }
                else
                {
                    itemValue = Convert.ChangeType(item.Value, propertyName.Type);
                }

                switch (item.Filter)
                {
                    case "=":
                        bodyItem = Expression.Equal(propertyName, Expression.Constant(itemValue));
                        break;
                    case ">":
                        bodyItem = Expression.GreaterThan(propertyName, Expression.Constant(itemValue));
                        break;
                    case "<":
                        bodyItem = Expression.LessThan(propertyName, Expression.Constant(itemValue));
                        break;
                    case ">=":
                        bodyItem = Expression.GreaterThanOrEqual(propertyName, Expression.Constant(itemValue));
                        break;
                    case "<=":
                        bodyItem = Expression.LessThanOrEqual(propertyName, Expression.Constant(itemValue));
                        break;
                    case "!=":
                        bodyItem = Expression.NotEqual(propertyName, Expression.Constant(itemValue));
                        break;
                    case "like":
                        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        bodyItem = Expression.Equal(Expression.Call(propertyName, method, Expression.Constant(item.Value)), Expression.Constant(true));
                        break;
                    case "in":
                        Expression expression = Expression.Constant(false);
                        var methodEquals = (propertyName.Type).GetMethod("Equals", new Type[] { propertyName.Type });
                        foreach (var optionName in item.ListValue)
                        {
                            itemValue = Convert.ChangeType(optionName, propertyName.Type);
                            Expression right = Expression.Call
                                   (
                                      propertyName,  //p.DataSourceName
                                      methodEquals,// 反射使用.Contains()方法                         
                                      Expression.Constant(itemValue)
                                   );
                            expression = Expression.Or(right, expression);//p.DataSourceName.contain("") || p.DataSourceName.contain("") 
                        }
                        bodyItem = expression;

                        break;
                    case "not in":
                        Expression expression2 = Expression.Constant(true);
                        foreach (var optionName in item.ListValue)
                        {
                            itemValue = Convert.ChangeType(optionName, propertyName.Type);
                            Expression right = Expression.NotEqual(propertyName, Expression.Constant(itemValue));
                            expression2 = Expression.And(right, expression2);
                        }
                        bodyItem = expression2;

                        break;
                }

                if (body == null)
                {
                    body = bodyItem;
                }
                else if (bodyItem != null)
                {
                    body = Expression.AndAlso(body, bodyItem);
                }
            }

            andExp = Expression.Lambda<Func<TSource, bool>>(body, p);

            return andExp;
        }
        /// <summary>
        /// 根据排序条件动态生成排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="orders"></param>
        /// <returns></returns>
        private Func<Sort<TSource>, Sort<TSource>> GenerateCommonOrderModel<TSource>(List<CommonOrderModel> orders)
        {
            Func<Sort<TSource>, Sort<TSource>> orderExp = null;
            var p = Expression.Parameter(typeof(TSource), "p");
            MemberExpression propertyName = null;
            orderExp = (o) =>
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    propertyName = Expression.Property(p, orders[i].Name);

                    if (orders[i].Order == 1)
                    {
                        if (propertyName.Type == typeof(DateTime))
                        {
                            o = o.Desc(Expression.Lambda<Func<TSource, DateTime>>(propertyName, p));
                        }
                        else if (propertyName.Type == typeof(int))
                        {
                            o = o.Desc(Expression.Lambda<Func<TSource, int>>(propertyName, p));
                        }
                        else if (propertyName.Type == typeof(decimal))
                        {
                            o = o.Desc(Expression.Lambda<Func<TSource, decimal>>(propertyName, p));
                        }
                        else if (propertyName.Type == typeof(string))
                        {
                            o = o.Desc(Expression.Lambda<Func<TSource, string>>(propertyName, p));
                        }
                    }
                    else if (orders[i].Order == 0)
                    {
                        if (propertyName.Type == typeof(DateTime))
                        {
                            o = o.Asc(Expression.Lambda<Func<TSource, DateTime>>(propertyName, p));
                        }
                        else if (propertyName.Type == typeof(int))
                        {
                            o = o.Asc(Expression.Lambda<Func<TSource, int>>(propertyName, p));
                        }
                        else if (propertyName.Type == typeof(decimal))
                        {
                            o = o.Asc(Expression.Lambda<Func<TSource, decimal>>(propertyName, p));
                        }
                        else if (propertyName.Type == typeof(string))
                        {
                            o = o.Asc(Expression.Lambda<Func<TSource, string>>(propertyName, p));
                        }
                    }
                }
                return o;
            };
            return orderExp;
        }

        #endregion 生成查询表达式
    }
}
