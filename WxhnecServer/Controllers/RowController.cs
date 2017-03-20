using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JCore;
using System.Reflection;

namespace WxhnecServer
{
    public class RowController : Controller
    {
        //[HttpPost]
        //public bool Add2(FormCollection collection) {
        //    TFieldLogic<pre_activity> fields = new TFieldLogic<pre_activity>();
        //    TEntityLogic<pre_activity> entity = new TEntityLogic<pre_activity>();
        //    pre_activity row = fields.UI2Row(collection);
        //    var result = entity.Modify(row);
        //    return result;
        //}

        public ActionResult Row() {
            ViewBag.ui = "http://localhost/ui";
            ViewBag.resource = "Resources";

            var entity = new TEntityLogic<pre_activity>();
            ViewBag.entity = entity.GetTitle();

            var row = entity.Row;            
            row = entity.FindRow(2, nameof(row.pre_activity_detail));

            return View(row);
        }
    }
}