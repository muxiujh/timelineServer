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
    }
}