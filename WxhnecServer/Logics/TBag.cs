using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxhnecServer.Models;
using System.Reflection;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using WxhnecServer.Logics.Enums;
using WxhnecServer.Logics.Attributes;

namespace WxhnecServer.Logics
{
    public struct TBag
    {
        public PropertyInfo[] propertyList;
        public object row;
        public bool isSub;
    }
}
