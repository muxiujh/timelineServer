using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxhnecServer.Models;
using System.Reflection;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace WxhnecServer.Logics
{

    public class TEntityTestLogic<T> : TEntityBaseLogic<T> where T : class
    {
        Random m_random;

        public TEntityTestLogic() {
            m_random = new Random();
        }

        int getRandomInt() {
            return m_random.Next(1, 100);
        }

        bool autoIncrease(PropertyInfo pro) {
            var result = true;
            foreach (CustomAttributeData attr in pro.CustomAttributes) {
                Type attrType = attr.AttributeType;
                if (attrType == typeof(DatabaseGeneratedAttribute)) {
                    result = false;
                }
            }
            return result;
        }

        T FillRow(T prow = null) {
            T row = null;
            bool modeCreate = prow == null;
            if (modeCreate) {
                row = (T)Activator.CreateInstance(m_type, null);
            }
            else {
                row = prow;
            }
            PropertyInfo[] propertys = m_type.GetProperties();
            foreach (PropertyInfo pro in propertys) {
                if (pro.Name == "id") {
                    if (autoIncrease(pro)) {
                        continue;
                    }
                    else if (!modeCreate) {
                        continue;
                    }
                }

                object val = null;
                if (pro.PropertyType == typeof(Int32?)) {
                    val = getRandomInt();
                }
                else if (pro.PropertyType == typeof(Int64?)) {
                    val = Convert.ToInt64(getRandomInt());
                }
                else if (pro.PropertyType == typeof(String)) {
                    val = pro.Name + " " + getRandomInt();
                }
                else if (pro.PropertyType == typeof(DateTime?)) {
                    val = DateTime.Now;
                }

                if (val != null) {
                    pro.SetValue(row, val);
                }
            }
            return row;
        }

        public int Add() {
            m_row = FillRow();
            m_dbset.Add(m_row);
            int result = m_db.SaveChanges();
            return result;
        }

        public int Modify() {
            m_row = FillRow(m_row);
            int result = m_db.SaveChanges();
            return result;
        }

        public int Remove() {
            m_dbset.Remove(m_row);
            int result = m_db.SaveChanges();
            return result;
        }
    }
}