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
    public class TEntityBaseLogic<T> where T : class
    {
        protected JDb m_db;

        protected DbSet<T> m_dbset;
        public T Row { get; }
        public Type TType { get; set; }

        public TEntityBaseLogic() {
            m_db = new JDb();
            TType = typeof(T);
            initDbSet();
        }

        ~TEntityBaseLogic() {
            if (m_db != null) {
                m_db.Dispose();
            }
        }

        void initDbSet() {
            // get property pre_activity using typeof(T).Name as "pre_activity"
            PropertyInfo property = typeof(JDb).GetProperty(TType.Name);
            m_dbset = (DbSet<T>)property.GetValue(m_db);
        }

        public T FindRow(int id, string reference = null) {
            T row = m_dbset.Find(id);
            if (row != null &&　reference != null) {
                m_db.Entry(row).Reference(reference).Load();
            }
            return row;
        }

    }
}