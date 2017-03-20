﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JCore;

namespace WxhnecServer
{
    public class ConfigModel : TEntityBaseLogic<pre_config>, ICacheList, ICacheRow
    {
        public dynamic GetList() {
            var result = new Dictionary<string, dynamic>();
            var list = m_dbset.Select(r => new { r.name, r.type, r.value }).ToList();
            for(int i=0; i< list.Count; ++i) {
                var row = list[i];
                var value = convertValue(row.value, row.type);
                result.Add(row.name, value);
            }
            return result;
        }

        public dynamic GetRow(string key) {
            if (string.IsNullOrEmpty(key)) {
                return null;
            }

            var row =  m_dbset.Where(r => r.name == key).Select(r =>new { r.type, r.value}).FirstOrDefault();
            if(row == null) {
                return null;
            }
            return convertValue(row.value, row.type);
        }

        dynamic convertValue(string content, long? type) {
            if (string.IsNullOrEmpty(content)){
                return null;
            }

            dynamic result = null;
            if (type == 1) {
                result = new Dictionary<dynamic, string>();
                string[] lines = content.Split('\n');
                for (int i = 0; i < lines.Length; ++i) {
                    string[] temp = lines[i].Split(':');
                    if (temp.Length > 1) {
                        int? key = THelper.StringToInt(temp[0]);
                        if (key != null) {
                            result[key.Value] = temp[1];
                        }
                        else {
                            result[temp[0]] = temp[1];
                        }
                    }
                    else {
                        result[i + 1] = temp[0];
                    }
                }
            }
            else {
                result = content;
            }
            return result;
        }
    }
}