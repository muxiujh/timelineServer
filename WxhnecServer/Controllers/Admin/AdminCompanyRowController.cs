
namespace WxhnecServer
{
    public class AdminCompanyRowController : AdminCompanyBaseController
    {
        protected override bool checkTable() {
            m_menuLeft = "MenuLeft1";
            return base.checkTable();
        }

        protected override bool checkModify(int id) {
            return base.checkModify(m_companyid);
        }

        protected override bool checkAdd() {
            return false;
        }
    }
}