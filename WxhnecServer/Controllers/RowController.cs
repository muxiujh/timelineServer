using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxhnecServer.Models;
using WxhnecServer.Logics;
using System.Reflection;
using WxhnecServer.Logics.Enums;

namespace WxhnecServer.Controllers
{
    public class RowController : Controller
    {
        [HttpPost]
        public int Add2(FormCollection collection) {
            TFieldLogic<pre_activity> fields = new TFieldLogic<pre_activity>();
            TEntityLogic<pre_activity> entity = new TEntityLogic<pre_activity>();
            pre_activity row = fields.UI2Row(collection);
            int result = entity.Modify(row);
            return result;
        }

        public ActionResult Add() {
            ViewBag.ui = "http://localhost/ui";
            ViewBag.resource = "Resources";

            TEntityLogic<pre_activity> entity = new TEntityLogic<pre_activity>();
            ViewBag.entity = entity.GetTitle();

            pre_activity row = entity.FindRow(2, nameof(row.pre_activity_detail));
            var propertyList = typeof(pre_activity).GetProperties();
            TBag bag = new TBag() {
                row = row,
                propertyList = propertyList
            };

            return View(bag);
        }


        public ActionResult AddSub(object val) {
            var propertyList = val.GetType().GetProperties();
            TBag bag = new TBag() {
                row = val,
                propertyList = propertyList,
                isSub = true
            };
            return View("Add", bag);
        }


        public ActionResult Field(PropertyInfo pro, object val) {
            
            TFieldLogic<pre_activity> field = new TFieldLogic<pre_activity>();
            var dict = field.Field2UI(pro, val);

            return dict == null ? null : View(dict[TF.type].ToString(), dict);
        }
    }
}