using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace JCore
{
    static public class ControllerExtension
    {
        static public string U(this Controller controller, string key, object value) {

            RouteValueDictionary paramDict = controller.ViewBag.paramDict;
            if (paramDict == null) {
                paramDict = controller.ViewBag.paramDict = controller.RouteData.Values;

                var queryString = controller.Request.QueryString;
                var paramEnum = queryString.GetEnumerator();
                while (paramEnum.MoveNext()) {
                    var pKey = paramEnum.Current.ToString();
                    paramDict[pKey] = queryString.Get(pKey);
                }
            }

            RouteValueDictionary url = new RouteValueDictionary(paramDict);
            url[key] = value;
            return controller.Url.RouteUrl(url);
        }

        static public Dictionary<string, object> FilterSession(this Controller controller, List<string> keys) {
            var result = new Dictionary<string, object>();
            while (true) {
                if (keys == null) {
                    break;
                }

                foreach (var key in keys) {
                    var value = controller.Session[key];
                    if (value != null) {
                        result.Add(key, value);
                    }
                }

                break;
            }                        
            return result;
        }
    }
}