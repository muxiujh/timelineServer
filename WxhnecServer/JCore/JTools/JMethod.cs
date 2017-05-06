using System;
using System.Reflection;

namespace JCore
{
    public class JMethod
    {
        public static object Run<T>(string method, object[] paramList) {
            BindingFlags flags = BindingFlags.Static;
            Type type = typeof(T);
            return runMethod(type, method, null, paramList, flags);
        }

        public static object Run(object instance, string method, object[] paramList) {
            BindingFlags flags = BindingFlags.Instance;
            return runMethod(instance.GetType(), method, instance, paramList, flags);
        }

        static object runMethod(Type type, string method, object instance, object[] paramList, BindingFlags flags) {
            flags = flags | BindingFlags.NonPublic | BindingFlags.Public;
            object result = null;
            try {
                MethodInfo mInfo = type.GetMethod(method, flags);
                if (mInfo != null) {
                    result = mInfo.Invoke(instance, paramList);
                }
            }
            catch { }
            return result;
        }
    }
}