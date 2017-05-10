using System;
using System.Collections.Generic;
using System.Reflection;

namespace JCore
{
    using TFDictionary = Dictionary<TF, object>;

    public class TListUI : TEntityUI
    {
        public void List2UI(object list, Action rowBegin, Action<object> filedAction, Action<object> rowEnd) {
            dynamic listDynamic = list;
            foreach (object row in listDynamic) {
                // row
                if (rowBegin != null) {
                    rowBegin();
                }

                PropertyInfo[] propertyList = THelper.GetBaseType(row).GetProperties();   
                object key = null;
                foreach (PropertyInfo pro in propertyList) {
                    // field
                    if (PropertyHelper.IsKey(pro)) {
                        key = pro.GetValue(row);
                    }

                    if (PropertyHelper.HasAttribute<TListShow>(pro)) {
                        var dict = field2UI(pro, pro.GetValue(row));
                        field2UIMore(ref dict, pro);
                        filedAction(dict);
                    }
                }

                if (rowEnd != null) {
                    rowEnd(key);
                }
            }
        }

        public List<TFDictionary> Class2UI(string tNamespaceClass) {
            List<TFDictionary> list = null;
            while (true) {
                if (string.IsNullOrEmpty(tNamespaceClass)) {
                    break;
                }

                Type type = Type.GetType(tNamespaceClass);
                if(type == null) {
                    break;
                }

                list = new List<TFDictionary>();
                var propertyList = type.GetProperties();
                foreach (PropertyInfo pro in propertyList) {
                    if (PropertyHelper.HasAttribute<TListShow>(pro)) {
                        var dict = field2UI(pro, null);
                        list.Add(dict);
                    }
                }

                break;
            }
            return list;
        }

        public List<SCompare> Class2Search(string tNamespaceClass) {
            var list = new List<SCompare>();
            while (true) {
                if (string.IsNullOrEmpty(tNamespaceClass)) {
                    break;
                }

                Type type = Type.GetType(tNamespaceClass);
                if (type == null) {
                    break;
                }

                var propertyList = type.GetProperties();
                foreach (PropertyInfo pro in propertyList) {
                    if (PropertyHelper.HasAttribute<TListSearch>(pro)) {
                        var title = PropertyHelper.GetTFieldValue(pro, TF.title);
                        if(string.IsNullOrEmpty(title)) {
                            title = pro.Name;
                        }
                        SCompare compare = new SCompare(pro.Name, null, null, title);
                        list.Add(compare);
                    }
                }

                break;
            }
            return list;
        }

        void field2UIMore(ref TFDictionary dict, PropertyInfo pro) {
            if(dict == null) {
                return;
            }

            var elementKey = (TE)dict[TF.element];
            // listElement
            TE listElement;
            switch (elementKey) {
                case TE.date:
                case TE.hidden:
                    listElement = TE.text;
                    break;
                case TE.select: {
                        var dataSource = G.Config[dict[TF.dataSource].ToString()];
                        object valueShow = null;
                        try {
                            var value = dict[TF.value];
                            if (value != null) {
                                valueShow = dataSource[value];
                            }
                        }
                        catch { }
                        dict[TF.value] = valueShow;
                    }
                    listElement = TE.text;
                    break;
                case TE.picture:
                case TE.pictureList:
                    listElement = TE.picture;
                    break;
                default:
                    listElement = TE.textarea;
                    break;
            }
            dict[TF.listElement] = listElement;
        }
    }
}