using System.Web.Routing;

namespace WxhnecServer
{
    [AdminSuperFilter]
    public class AdminEntitySuperController : AdminEntityController
    {
        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);

            m_pageList = "/AdminEntitySuper/List";
        }
    }
}