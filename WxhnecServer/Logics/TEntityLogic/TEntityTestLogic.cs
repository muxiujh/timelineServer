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
using WxhnecServer.Tools;

namespace WxhnecServer.Logics
{

    public class TEntityTestLogic<T> : TEntityBaseLogic<T> where T : class
    {
        Random m_random;
        T m_row;

        public TEntityTestLogic() {
            m_random = new Random();
        }

        T FillRow(T prow = null) {
            T row = null;
            bool modeCreate = prow == null;
            if (modeCreate) {
                row = (T)Activator.CreateInstance(TType, null);
            }
            else {
                row = prow;
            }
            PropertyInfo[] propertys = TType.GetProperties();
            foreach (PropertyInfo pro in propertys) {
                if (pro.Name == "id") {
                    if (PropertyHelper.IsAutoIncrease(pro)) {
                        continue;
                    }
                    else if (!modeCreate) {
                        continue;
                    }
                }

                object val = null;
                if (pro.PropertyType == typeof(Int32?)) {
                    val = THelper.GetRandom();
                }
                else if (pro.PropertyType == typeof(Int64?)) {
                    val = Convert.ToInt64(THelper.GetRandom());
                }
                else if (pro.PropertyType == typeof(String)) {
                    val = pro.Name + " " + THelper.GetRandom();
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