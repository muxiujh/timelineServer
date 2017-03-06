using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WxhnecServer.Logics;
using WxhnecServer.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Generic;
using WxhnecServer.Logics.Enums;
using WxhnecServer.Logics.Attributes;

namespace WxhnecServer.Tests
{
    [TestClass]
    public class Test_TValidate
    {
        public void basic(object obj, object configVal, string method) {
            string[] tempList = method.Split('_');
            string v1 = tempList[0];
            string v2 = tempList.Length > 1 ? tempList[tempList.Length - 1] : null;
            TV tv = (TV)Enum.Parse(typeof(TV), v1);
            bool expect = v2 != "fail";

            TValidate validate = new TValidate(tv, configVal);
            ValidationContext context = new ValidationContext(new Object());
            ValidationResult vResult = validate.GetValidationResult(obj, context);
            bool result = vResult == null;
            if (result == false) {
                Console.WriteLine(vResult.ErrorMessage);
            }
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void required_string_fail() {
            object obj = "";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void required_string() {
            object obj = "haha";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void required_int_fail() {
            int? obj = 0;
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void required_int() {
            int? obj = 1;
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void required_long_fail() {
            long? obj = 0;
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void required_long() {
            long? obj = 1;
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void minlength_fail() {
            object obj = "12";
            basic(obj, 3, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void minlength() {
            object obj = "123";
            basic(obj, 3, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void maxlength_fail() {
            object obj = "1234567890";
            basic(obj, 9, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void maxlength() {
            object obj = "123456789";
            basic(obj, 9, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void email_fail() {
            object obj = "lala.lala";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        // more test
        [TestMethod]
        public void email() {
            object obj = "373259115@qq.com";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }


        [TestMethod]
        public void idcard_fail() {
            object obj = "33048119840101001";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        // more test
        [TestMethod]
        public void idcard() {
            object obj = "330481198401010011";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void phone_fail() {
            object obj = "aje5454";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        // more test
        [TestMethod]
        public void phone() {
            object obj = "0573-965842";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void mobile_fail() {
            object obj = "11222222255";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void mobile() {
            object obj = "13567383828";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void number_fail() {
            object obj = "fe6263";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        // more test
        [TestMethod]
        public void number() {
            object obj = "123569";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void name_fail() {
            object obj = "he(12";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        // more test
        [TestMethod]
        public void name() {
            object obj = "he_12";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void uname_fail() {
            object obj = "he(12";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        // more test
        [TestMethod]
        public void uname() {
            object obj = "啦啦cool";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void dateiso_fail() {
            object obj = "202";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }

        // more test
        [TestMethod]
        public void dateiso() {
            object obj = "2017-2-2";
            basic(obj, null, MethodBase.GetCurrentMethod().Name);
        }
    }
}
