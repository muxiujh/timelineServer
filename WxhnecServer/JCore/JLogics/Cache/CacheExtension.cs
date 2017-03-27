using System;
using System.Web;
using System.Web.Caching;

namespace JCore
{
    public static class CacheExtension
    {
        // CacheList
        public static dynamic Cache(this ICacheList cacheLogic) {
            string cacheKey = cacheLogic.GetType().Name;
            return cacheRaw(cacheKey, () => {
                return cacheLogic.GetList();
            });
        }

        public static void RemoveCache(this ICacheList cacheLogic) {
            string cacheKey = cacheLogic.GetType().Name;
            removeCacheRaw(cacheKey);
        }

        // CacheRow
        public static dynamic Cache(this ICacheRow cacheLogic, string key) {
            if (string.IsNullOrEmpty(key)) {
                return null;
            }

            string cacheKey = cacheLogic.GetType().Name + "_" + key;
            return cacheRaw(cacheKey, () => {
                return cacheLogic.GetRow(key);
            });
        }

        public static void RemoveCache(this ICacheRow cacheLogic, string key) {
            if (string.IsNullOrEmpty(key)) {
                return;
            }

            string cacheKey = cacheLogic.GetType().Name + "_" + key;
            removeCacheRaw(cacheKey);
        }

        // Remove All
        public static void RemoveAll() {
            Cache cache = getCache();
            var cacheEnumer = cache.GetEnumerator();
            while (cacheEnumer.MoveNext()) {
                cache.Remove(cacheEnumer.Key.ToString());
            }
        }

        // Raw
        static object cacheRaw(string cacheKey, Func<object> func) {
            Cache cache = getCache();
            var result = cache.Get(cacheKey);
            if (result == null) {
                result = func();
                if (result == null) {
                    cache.Insert(cacheKey, false);
                }
                else {
                    cache.Insert(cacheKey, result);
                }
            }
            else if (result.Equals(false)) {
                result = null;
            }
            return result;
        }

        static void removeCacheRaw(string cacheKey) {
            Cache cache = getCache();
            var result = cache.Remove(cacheKey);
        }

        static Cache getCache() {
            return HttpRuntime.Cache;
        }
    }
}