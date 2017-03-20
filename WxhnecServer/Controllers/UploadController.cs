using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JCore;

namespace WxhnecServer
{
    public class UploadController : Controller
    {
        UploadLogic m_upload = new UploadLogic();

        [HttpPost]
        public string Index() {
            HttpPostedFileBase httpFile = Request.Files[0];

            var result = m_upload.Upload(httpFile);
            if(result == null) {
                result = m_upload.Error;
            }

            return result;
        }

        [HttpGet]
        public ActionResult Upload() {
            
            return View();
        }

        [HttpGet]
        public string Show() {
            var collection = Request.Params;

            var result = m_upload.Show(collection.Get("pic"), collection.Get("size"));
            return result;
        }
    }
}