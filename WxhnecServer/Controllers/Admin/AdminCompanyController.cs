using JCore;
using System.Collections.Generic;

namespace WxhnecServer
{
    public class AdminCompanyController : AdminCompanyBaseController
    {
        const string c_count = "count";
        const string c_limit = "limit";

        protected override Dictionary<string, object> getPreset() {
            var dict = new Dictionary<string, object>();
            dict[G.companyid] = m_companyid;
            return dict;
        }

        protected override bool checkTable() {
            m_menuLeft = "MenuLeft2";
            return base.checkTable();
        }

        protected override bool checkModify(int id) {
            bool result = false;
            while (true) {
                if (!base.checkModify(id)) {
                    break;
                }

                // check m_companyid == Row.companyid
                var pro = m_logic.Row.GetType().GetProperty(G.companyid);
                if(pro == null) {
                    break;
                }

                var value = pro.GetValue(m_logic.Row);
                if(m_companyid != THelper.StringToInt(value)) {
                    m_error = G.L["no_permission"];
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        protected override bool checkAdd() {
            // company
            var companyEntity = new TEntityLogic<pre_company>();
            var company = companyEntity.FindRow(m_companyid);
            var count = getNumber(company, c_count);

            // level
            var levelLogic = new TEntityLogic<pre_company_level>();
            var levelid = company.levelid == null ? c_levelDefault : company.levelid.Value;
            var level = levelLogic.FindRow(levelid);
            var limit = getNumber(level, c_limit);

            if (count >= limit) {
                m_error = string.Format(G.L[m_table + "_limit"], limit) + G.L["push_limit"];
                return false;
            }

            return true;
        }

        protected override bool finishAdd() {
            var companyEntity = new TEntityLogic<pre_company>();
            var company = companyEntity.FindRow(m_companyid);
            getNumber(company, c_count, true);
            return companyEntity.Modify(company);
        }

        protected int getNumber(object row, string pre, bool increase = false) {
            var result = 0;
            while (true) {
                if (row == null) {
                    break;
                }

                string content = m_table;
                content = content.Replace(c_pre, "");
                content = THelper.UpFirst(content);
                content = pre + content;
                var pro = row.GetType().GetProperty(content);
                if (pro == null) {
                    break;
                }

                var value = pro.GetValue(row);
                result = THelper.StringToInt(value);
                if (increase) {
                    pro.SetValue(row, ++result);
                }
                break;
            }
            return result;
        }
    }
}