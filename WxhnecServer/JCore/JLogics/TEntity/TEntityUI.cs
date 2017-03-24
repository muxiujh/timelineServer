using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Specialized;

namespace JCore
{
    using TFDictionary = Dictionary<TF, object>;

    public class TEntityUI
    {

        public TFDictionary Field2UI(PropertyInfo pro, object value) {
            if (PropertyHelper.IsVirtual(pro)) {
                return null;
            }

            TFDictionary dict = pro.GetCustomAttributes<TField>()
                                            .ToDictionary(p => p.Key, p => p.Value);

            // Reserved return
            if (dict.ContainsKey(TF.reserved)) {
                return null;
            }

            // begin fill dictionary
            PropertyHelper.FillTFDictionary(ref dict);

            // element
            TElement element = pro.GetCustomAttribute<TElement>();
            if (element == null) {
                TE te = PropertyHelper.IsKey(pro) ? TE.hidden : TE.text;
                element = new TElement(te);
            }
            dict[TF.element] = element.Key;

            // dataSource
            dict[TF.dataSource] = element.Value;

            // widget
            string widget;
            switch (element.Key) {
                case TE.text:
                case TE.password:
                case TE.url:
                case TE.tel:
                    widget = TE.text.ToString();
                    break;
                default:
                    widget = element.Key.ToString();
                    break;
            }
            dict[TF.widget] = widget;

            // name
            dict[TF.name] = pro.Name;

            // value
            THelper.ConvertToUI(element.Key, ref value);
            dict[TF.value] = value;

            return dict;
        }

        public void Row2UI(object row, Action<TFDictionary> act, bool isSub = false) {
            Type type = THelper.GetBaseType(row);
            List<PropertyInfo> propertyListVirtual = new List<PropertyInfo>();

            // propertyList
            var propertyList = type.GetProperties();
            List<TFDictionary> list = new List<TFDictionary>();
            foreach (PropertyInfo pro in propertyList) {
                if (PropertyHelper.IsVirtual(pro)) {
                    if (PropertyHelper.HasElement(pro)) {
                        propertyListVirtual.Add(pro);
                    }
                }
                else {
                    if(isSub && PropertyHelper.IsKey(pro)) {
                        continue;
                    }
                    var value = pro.GetValue(row);
                    var dict = Field2UI(pro, value);
                    // action
                    act(dict);
                }
            }

            // propertyListVirtual
            foreach (PropertyInfo pro in propertyListVirtual) {
                var value = pro.GetValue(row);
                if (value == null) {
                    value = Activator.CreateInstance(pro.PropertyType, null);
                }
                Row2UI(value, act, true);
            }
        }

        public object UI2Row(NameValueCollection collection, Type type) {
            var row = Activator.CreateInstance(type, null);
            PropertyInfo[] propertyList = type.GetProperties();
            foreach (PropertyInfo pro in propertyList) {

                if (PropertyHelper.IsVirtual(pro)) {
                    if (PropertyHelper.HasElement(pro)) {
                        var subRow = UI2Row(collection, pro.PropertyType);
                        pro.SetValue(row, subRow);
                    }
                }
                else if (collection.AllKeys.Contains(pro.Name)) {
                    object val = collection[pro.Name];
                    THelper.ConvertToType(pro.PropertyType, ref val);
                    pro.SetValue(row, val);                    
                }                    
            }
            return row;
        }
    }
}
