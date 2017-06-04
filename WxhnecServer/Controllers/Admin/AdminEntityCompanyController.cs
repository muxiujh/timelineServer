using JCore;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Routing;

namespace WxhnecServer
{
    [AdminCompanyFilter]
    public class AdminEntityCompanyController : AdminEntityController
    {
        int m_companyid;

        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);

            while (true) {
                m_companyid = THelper.StringToInt(Session[G.companyid]);
                if (m_companyid > 0) {
                    break;
                }

                var companyidConfig = ConfigurationManager.AppSettings[G.companyid];
                m_companyid = THelper.StringToInt(companyidConfig);
                break;
            }
            m_pageList = "/AdminEntityCompany/List";
        }

        protected override Dictionary<string, object> getPreset(string table) {
            var dict = new Dictionary<string, object>();

            if (table == nameof(pre_company)) {
                dict[c_id] = m_companyid;
            }
            else {
                dict[G.companyid] = m_companyid;
            }
            return dict;
        }
    }
}