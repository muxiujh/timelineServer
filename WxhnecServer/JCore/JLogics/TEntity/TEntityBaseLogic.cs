﻿using System;
using System.Reflection;
using System.Data.Entity;

namespace JCore
{
    public class TEntityBaseLogic<T> where T : class
    {
        protected DbContext m_db;
        protected Type m_dbType;

        protected DbSet<T> m_dbset;
        public T Row { get; }
        public Type TType { get; set; }

        public TEntityBaseLogic() {
            m_dbType = Type.GetType("JCore.JDb", true);
            m_db = Activator.CreateInstance(m_dbType, null) as DbContext;
            //m_db = new JDb();
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
            PropertyInfo property = m_dbType.GetProperty(TType.Name);
            m_dbset = (DbSet<T>)property.GetValue(m_db);
        }

        public T FindRow(int id, string reference = null) {
            T row = m_dbset.Find(id);
            if (row != null && reference != null) {
                m_db.Entry(row).Reference(reference).Load();
            }
            return row;
        }

        public bool DetachRow(object row) {
            var result = false;
            while (true) {
                if (row == null) {
                    break; ;
                }

                if (THelper.GetBaseType(row) != TType) {
                    break;
                }

                m_db.Entry(row).State = EntityState.Detached;
                result = true;
                break;
            }
            return result;
        }

        public string GetTitle() {
            TEntity entity = TType.GetCustomAttribute<TEntity>();
            string title = entity != null ? entity.Value : null;
            return THelper.Lang(title, TType.Name);
        }

    }
}