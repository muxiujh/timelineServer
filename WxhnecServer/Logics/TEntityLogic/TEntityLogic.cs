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
using WxhnecServer.Logics.Enums;
using WxhnecServer.Logics.Attributes;
using WxhnecServer.Tools;

namespace WxhnecServer.Logics
{
    public class TEntityLogic<T> : TEntityBaseLogic<T> where T : class
    {
        public List<DbEntityValidationResult> EntityValidationErrors;
        public List<string> RequiredErrors = new List<string>();
        public bool Success {
            get {
                return (EntityValidationErrors == null || EntityValidationErrors.Count == 0)
                    && (RequiredErrors == null || RequiredErrors.Count == 0);
            }
        }

        bool checkModify(Type type, ref object row) {
            bool bResult = false;
            DbEntityEntry rowEntry = m_db.Entry(row);
            foreach (PropertyInfo pro in type.GetProperties()) {
                if (THelper.IsVirtual(pro)) {
                    if (THelper.HasElement(pro)) {
                        var subRow = pro.GetValue(row);
                        if (checkModify(pro.PropertyType, ref subRow)) {
                            // set modify
                            pro.SetValue(row, subRow);
                            bResult = true;
                        }
                    }
                }
                else {
                    if (pro.Name == "id") {
                        continue;
                    }
                    if (pro.GetValue(row) != null) {
                        // set modify
                        DbPropertyEntry proEntry = rowEntry.Property(pro.Name);
                        proEntry.IsModified = true;
                        bResult = true;
                    }
                }
            }
            return bResult;
        }

        //
        // only the changed field will be validate and update
        //
        public int Modify(object row) {
            int result = -1;
            m_dbset.Attach((T)row);
            if (checkModify(m_type, ref row)) {
                result = saveChanges();
            }
            return result;
        }

        int saveChanges() {
            int result = -1;
            try {
                result = m_db.SaveChanges();
            }
            catch (DbEntityValidationException ex) {                
                EntityValidationErrors = (List<DbEntityValidationResult>)ex.EntityValidationErrors;
            }
            return result;
        }

        bool isRequired(PropertyInfo pro) {
            bool bResult = false;
            var attrs = pro.GetCustomAttributes<TValidate>();
            foreach (TValidate tv in attrs) {
                if (tv.key == TV.required) {
                    bResult = true;
                    break;
                }
            }
            return bResult;
        }

        bool checkAdd(Type type, ref object row) {
            bool bResult = true;
            DbEntityEntry rowEntry = m_db.Entry(row);
            foreach (PropertyInfo pro in type.GetProperties()) {
                if (THelper.IsVirtual(pro)) {
                    if (THelper.HasElement(pro)) {
                        var subRow = pro.GetValue(row);
                        if (!checkAdd(pro.PropertyType, ref subRow)) {
                            // set error
                            bResult = false;
                        }
                    }
                }
                else if (pro.GetValue(row) == null && isRequired(pro)) {
                    // set error
                    RequiredErrors.Add(pro.Name);
                    bResult = false;
                }
            }
            return bResult;
        }

        public int Add(object row) {
            int result = -1;
            m_dbset.Add((T)row);
            if (checkAdd(m_type, ref row)) {
                result = saveChanges();
            }
            return result;
        }

        public string GetTitle() {
            TEntity entity = m_type.GetCustomAttribute<TEntity>();
            string name = entity != null ? entity.Name : "";
            return name;
        }
    }
    
}