using System.Web;
using System.Web.Mvc;
using JCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Routing;

namespace WxhnecServer
{
    public class UploadController : BaseController
    {
        UploadLogic m_upload;

        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);
            m_upload = new UploadLogic(m_serverDir);
        }        

        [HttpPost]
        public string Index() {
            HttpPostedFileBase httpFile = Request.Files[0];
            string index = Request.Params["index"];

            var jo = new JObject();
            var path = m_upload.Upload(httpFile);
            if(path != null) {
                jo["index"] = index;
                jo["success"] = true;
                jo["url"] = "/Upload/Show?pic=" + path;
                jo["path"] = path;
            }
            else {
                path = m_upload.Error;
                jo["error"] = m_upload.Error;
            }

            return JsonConvert.SerializeObject(jo);
        }

        [HttpGet]
        public ActionResult Upload() {
            return View();
        }

        [HttpGet]
        public ActionResult Show() {
            var collection = Request.Params;

            var result = m_upload.Show(collection.Get("pic"), collection.Get("size"));
            return Redirect(result);
        }
    }
}