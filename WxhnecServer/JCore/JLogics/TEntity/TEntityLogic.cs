using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace JCore
{
    public class TEntityLogic<T> : TEntityBaseLogic<T> where T : class
    {
        public Dictionary<string, string> Errors = new Dictionary<string, string>();

        //
        // only the changed field will be validate and update
        //
        bool checkModify(ref object row) {
            if(row == null) {
                return false;
            }

            bool bResult = false;
            Type type = THelper.GetBaseType(row);
            foreach (PropertyInfo pro in type.GetProperties()) {
                if (PropertyHelper.IsVirtual(pro)) {
                    if (PropertyHelper.HasElement(pro)) {
                        var value = pro.GetValue(row);
                        if (checkModify(ref value)) {
                            // set modify
                            pro.SetValue(row, value);
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
                        DbPropertyEntry proEntry = m_db.Entry(row).Property(pro.Name);
                        proEntry.IsModified = true;
                        bResult = true;
                    }
                }
            }
            return bResult;
        }

        bool checkAdd(ref object row) {
            if (row == null) {
                return false;
            }

            bool bResult = true;
            Type type = THelper.GetBaseType(row);
            foreach (PropertyInfo pro in type.GetProperties()) {
                if (PropertyHelper.IsVirtual(pro)) {
                    if (PropertyHelper.HasElement(pro)) {
                        var value = pro.GetValue(row);
                        if (value == null) {
                            value = Activator.CreateInstance(pro.PropertyType, null);
                        }
                        if (!checkAdd(ref value)) {
                            // set error
                            bResult = false;
                        }
                    }
                }
                else if ((row == null || pro.GetValue(row) == null) && PropertyHelper.IsRequired(pro)) {
                    // set error
                    Errors[pro.Name] = "is required";
                    bResult = false;
                }
            }
            return bResult;
        }

        bool checkRow(ref T row) {
            bool bResult = true;
            if (row == null) {
                Errors["db"] = "row is null";
                bResult = false;
            }
            return bResult;
        }

        //
        // return
        //      -1: DbError
        //      0~n: Success
        //
        int saveChanges() {
            int result = -1;
            try {
                result = m_db.SaveChanges();
            }
            catch (DbEntityValidationException ex) {
                var validationResult = ex.EntityValidationErrors.First();
                var iterator = validationResult.ValidationErrors.GetEnumerator();
                while (iterator.MoveNext()) {
                    var item = iterator.Current;
                    Errors[item.PropertyName] = item.ErrorMessage;
                }
            }
            catch{
                Errors["db"] = "Unknown error";
            }
            return result;
        }

        public T Add(T row) {
            if (!checkRow(ref row)) {
                return null;
            }

            T result = null;
            object rowReal = row;
            m_dbset.Add(row);
            if (checkAdd(ref rowReal)) {
                if (saveChanges() >= 0) {
                    result = row;
                }
            }
            return result;
        }

        public bool Modify(T row) {
            if (!checkRow(ref row)) {
                return false;
            }

            bool bResult = false;
            object rowReal = row;
            m_dbset.Attach(row);
            if (checkModify(ref rowReal)) {
                if (saveChanges() >= 0) {
                    bResult = true;
                }
            }
            else {
                bResult = true;
            }
            return bResult;
        }

        public bool Remove(T row) {
            if (!checkRow(ref row)) {
                return false;
            }

            bool bResult = false;
            m_db.Entry(row).State = EntityState.Deleted;
            bResult = saveChanges() >= 0;
            return bResult;
        }

        public bool RemoveId(int id) {
            return Remove(FindRow(id));
        }

        public bool ValidateField(string name, object value, Type type = null) {
            bool result = true;
            while (true) {
                if (string.IsNullOrEmpty(name)) {
                    break;
                }

                if (type == null) {
                    type = TType;
                }

                PropertyInfo property = type.GetProperty(name);

                // no property
                if (property == null) {
                    // level 1 search level 2
                    if (type == TType) {
                        foreach (var pro in type.GetProperties()) {
                            if (PropertyHelper.IsVirtual(pro)) {
                                if (PropertyHelper.HasElement(pro)) {
                                    if (!ValidateField(name, value, pro.PropertyType)) {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                }

                // has property
                var validates = property.GetCustomAttributes<TValidate>();
                foreach (var validate in validates) {
                    ValidationContext context = new ValidationContext(new Object());
                    ValidationResult vResult = validate.GetValidationResult(value, context);
                    if (vResult != null) {
                        Errors[name] = vResult.ErrorMessage;
                        result = false;
                        break;
                    }
                }

                break;
            }
            return result;
        }

    }
    
}