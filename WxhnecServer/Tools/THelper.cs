using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;
using WxhnecServer.Logics.Attributes;
using WxhnecServer.Logics.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace WxhnecServer.Tools
{
    public class THelper
    {
        static public Dictionary<int, string> GetList(string key) {
            Dictionary<int, string> list = new Dictionary<int, string> {
                { 1, "yes" },
                { 2, "no" }
            };
            return list;
        }

        static public Type GetBaseType(object obj) {
            if(obj == null) {
                return null;
            }
            
            Type type = obj.GetType();
            if(type.BaseType != typeof(Object)) {
                type = type.BaseType;
            }

            return type;
        }

        static Random m_random = new Random();
        static public int GetRandom(int min = 1, int max = 100) {
            return m_random.Next(min, max);
        }


        static public void ConvertToUI(TE key, ref object value) {
            if (value == null) {
                return;
            }

            switch (key) {
                case TE.date:
                    DateToString(ref value);
                    break;
            }
        }

        static public void DateToString(ref object value) {
            if (value == null) {
                return;
            }

            value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
        }

        static public void ConvertToZero(ref object value) {
            if(value == null) {
                return;
            }

            if (value.ToString() == "") {
                value = 0;
            }
        }

        static public void ConvertToType(Type type, ref object value) {
            if (value == null) {
                return;
            }

            try {
                if (type == typeof(Int32?)) {
                    ConvertToZero(ref value);
                    value = Convert.ToInt32(value);
                }
                else if (type == typeof(Int64?)) {
                    ConvertToZero(ref value);
                    value = Convert.ToInt64(value);
                }
                else if (type == typeof(DateTime?)) {
                    value = Convert.ToDateTime(value);
                }
            }
            catch (FormatException) {
                value = null;
            }
        }
    }
}