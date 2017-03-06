using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WxhnecServer.Logics;
using WxhnecServer.Models;
using System.Reflection;

namespace WxhnecServer.Tests
{
    [TestClass]
    public class Test_TEntity
    {
        [TestMethod]
        public void test_modifyField() {
            Random random = new Random();
            TEntityLogic<pre_activity> obj = new TEntityLogic<pre_activity>();

            pre_activity row = new pre_activity();
            row.title = "new " + random.Next();
            //var result = obj.ModifyField(2, nameof(row.title), row.title);
        }

        [TestMethod]
        public void test_modify() {
            Random random = new Random();
            TEntityLogic<pre_activity> obj = new TEntityLogic<pre_activity>();

            var row = new pre_activity() { id = 2};
            row.title = "new " + random.Next();
            row.date = DateTime.Now;
            var result = obj.Modify(row);
            Assert.AreNotEqual(result, -1);
        }

        [TestMethod]
        public void test_add() {

            Random random = new Random();
            TEntityLogic<pre_activity> obj = new TEntityLogic<pre_activity>();

            var row = new pre_activity();
            row.title = "new " + random.Next();
            row.place = "";
            var result = obj.Add(row);
            Assert.AreNotEqual(result, -1);
        }

    }
}
