using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WxhnecServer.Logics;
using WxhnecServer.Models;
using System.Reflection;

namespace WxhnecServer.Tests
{
    [TestClass]
    public class Test_TEntityTest
    {
        void basicTest<T>() where T : class {

            TEntityTestLogic<T> obj = new TEntityTestLogic<T>();
            int result;

            result = obj.Add();
            Assert.IsTrue(result > 0);

            result = obj.Modify();
            Assert.IsTrue(result > 0);

            result = obj.Remove();
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void test_pre_activity() {
            basicTest<pre_activity>();
        }

        [TestMethod]
        public void test_pre_activity_detail() {
            basicTest<pre_activity_detail>();
        }

        [TestMethod]
        public void test_pre_config() {
            basicTest<pre_config>();
        }        
        
    }
}
