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
        protected T m_row;
        protected DbSet<T> m_dbset;
        protected Type m_type;

        public TEntityBaseLogic() {
            m_db = new JDb();
            m_type = typeof(T);
            initDbSet();
        }

        void initDbSet() {
            PropertyInfo property = typeof(JDb).GetProperty(m_type.Name);
            m_dbset = (DbSet<T>)property.GetValue(m_db);
        }

        public T FindRow(int id, string reference = null) {
            T row = m_dbset.Find(id);
            if (reference != null) {
                m_db.Entry(row).Reference(reference).Load();
            }
            return row;
        }

    }
}