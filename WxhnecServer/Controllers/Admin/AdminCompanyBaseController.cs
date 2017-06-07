using JCore;
using System.Configuration;
using System.Web.Routing;

namespace WxhnecServer
{
    [AdminCompanyFilter]
    public abstract class AdminCompanyBaseController : AdminEntityController
    {
        protected const int c_levelDefault = 1;
        protected int m_companyid;

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
            m_pageList = "/AdminCompany/List";
        }
    }
}