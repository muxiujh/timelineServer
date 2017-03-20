using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JCore
{
    public class TListLogic<T> : TEntityBaseLogic<T> where T : class
    {
        Expression m_query;
        int? m_skip;
        int? m_take;

        public TListLogic() {
            m_query = m_dbset.AsQueryable().Expression;
        }

        public TListLogic<T> TOrder(string key = null, bool isAsc = true) {
            while (true) {
                TListOrder listOrder = null;
                if (!string.IsNullOrWhiteSpace(key)) {
                    listOrder = new TListOrder(key, isAsc);
                }
                else {
                    listOrder = TType.GetCustomAttribute<TListOrder>();
                }

                // check key input
                if (listOrder == null) {
                    break;
                }

                // check key property
                PropertyInfo property = TType.GetProperty(listOrder.Key);
                if (property == null) {
                    break;
                }

                ParameterExpression parameter = Expression.Parameter(TType);
                MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
                LambdaExpression lambda = Expression.Lambda(propertyAccess, parameter); // pre_department.Name
                string order = listOrder.IsAsc ? "OrderBy" : "OrderByDescending";

                m_query = Expression.Call(
                    typeof(Queryable),
                    order,
                    new Type[] { TType, property.PropertyType },
                    m_query,
                    lambda
                    );

                break;
            }
            return this;
        }

        public TListLogic<T> TLimit(int pageSize, int page = 1) {
            while (true) {
                // take
                if(pageSize <= 0) {
                    break;
                }
                m_take = pageSize;

                // skip
                page = page - 1;
                if(page <= 0) {
                    break;
                }
                m_skip = page * pageSize;

                break;
            }
            return this;
        }


        public TListLogic<T> TWhere(string key, object value, string operate = op.eq) {
            
            while (true) {
                // check key input
                if (string.IsNullOrWhiteSpace(key)) {
                    break;
                }

                // check key property
                PropertyInfo property = TType.GetProperty(key);
                if (property == null) {
                    break;
                }

                ParameterExpression parameter = Expression.Parameter(TType);
                Expression left = Expression.Property(parameter, property);
                Expression right = Expression.Constant(value);
                if (value.GetType() != property.PropertyType) {
                    right = Expression.Convert(right, property.PropertyType);
                }

                Expression condition = null;
                switch (operate) {
                    case op.gt:
                        condition = Expression.GreaterThan(left, right);
                        break;
                    case op.gte:
                        condition = Expression.GreaterThanOrEqual(left, right);
                        break;
                    case op.lt:
                        condition = Expression.LessThan(left, right);
                        break;
                    case op.lte:
                        condition = Expression.LessThanOrEqual(left, right);
                        break;
                    case op.like:
                        Type stringType = typeof(string);
                        condition = Expression.Call(
                            left,
                            stringType.GetMethod("Contains", new Type[] { stringType }),
                            right);
                        break;
                    case op.eq:
                    default:
                        condition = Expression.Equal(left, right);
                        break;
                }
                
                LambdaExpression lambda = Expression.Lambda(condition, parameter);
                m_query = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new Type[] { TType },
                    m_query,
                    lambda
                    );

                break;
            }
            return this;
        }

        public List<T> ToList() {
            IQueryable<T> query = m_dbset.AsQueryable().Provider.CreateQuery<T>(m_query);
            if(m_skip != null) {
                query = query.Skip(m_skip.Value);
            }
            if(m_take != null) {
                query = query.Take(m_take.Value);
            }            
            return query.ToList();
        }

        public int Count() {
            IQueryable<T> query = m_dbset.AsQueryable().Provider.CreateQuery<T>(m_query);
            return query.Count();
        }

    }
}