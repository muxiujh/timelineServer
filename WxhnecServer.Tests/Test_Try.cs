using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WxhnecServer.Logics;
using WxhnecServer.Models;
using System.Text.RegularExpressions;
using System.Reflection;
using WxhnecServer.Logics.Attributes;
using System.Collections.Specialized;

namespace WxhnecServer.Tests
{
    [TestClass]
    public class Test_Try
    {
        [TestMethod]
        public void testValidate() {
            pre_activity row = new pre_activity();
            row.ctime = 1488643200;
            row.is_del = 0;
            row.status = 0;
            row.place = "cool";
            row.title = "1";
            row.summary = "373259115@qq.com";
            row.url = "url mine";
            Assert.IsTrue(true);

        }
        
        [TestMethod]
        public void try_reg() {
            bool result;
            object obj = "啦啦cool";
            string pattern = @"^([\x7f-\xff]|[a-zA-Z0-9_])*$";
            string pattern2 = @"^([\u4E00-\uFA29]|[\uE7C7-\uE7F3]|[a-zA-Z0-9_])*$";
            result = Regex.Match(obj.ToString(), pattern).Success;
            result = Regex.Match(obj.ToString(), pattern2).Success;
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void try_field() {

            TEntityLogic<pre_activity> entity = new TEntityLogic<pre_activity>();

            pre_activity row = entity.FindRow(2, nameof(row.pre_activity_detail));

            TFieldLogic<pre_activity> fields = new TFieldLogic<pre_activity>();

            var propertyList = fields.GetPropertyList();

            foreach(PropertyInfo pro in propertyList) {
                // if virtual && showUI, then rend
                if (pro.GetMethod.IsVirtual) {
                    var ele = pro.GetCustomAttribute<TElement>();
                    var subRow = pro.GetValue(row);
                    var propertyList2 = subRow.GetType().GetProperties();
                }
            }
        }

        [TestMethod]
        public void try_ui_modify() {
            TFieldLogic<pre_activity> fields = new TFieldLogic<pre_activity>();
            TEntityLogic<pre_activity> entity = new TEntityLogic<pre_activity>();
            NameValueCollection collection = new NameValueCollection();
            collection["id"] = "2";
            collection["title"] = "cool";
            collection["content"] = "content cool";
            var row = fields.UI2RowRaw(collection, fields.TType);
            int result = entity.Modify(row);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void try_ui_add() {
            TFieldLogic<pre_activity> fields = new TFieldLogic<pre_activity>();
            TEntityLogic<pre_activity> entity = new TEntityLogic<pre_activity>();
            NameValueCollection collection = new NameValueCollection();
            collection["title"] = "cool";
            //collection["is_del"] = "1";
            //collection["place"] = "place";
            //collection["summary"] = "summary";
            collection["content"] = "content cool";
            var row = fields.UI2RowRaw(collection, fields.TType);
            int result = entity.Add(row);
            Assert.IsTrue(true);
        }


    }
}
