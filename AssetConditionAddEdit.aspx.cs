using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc;
using AssetManagement.Proc.DAL;
using AssetManagement.Proc.BL;
using System.Configuration;

namespace AssetManagement
{
    public partial class AssetConditionAddEdit : System.Web.UI.Page
    {
        public bool IsNew = false;
        public int conID = 0;
        public Condition thisEntry;
        public string thisUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["new"]) && Gen.isSpecificType(Request.QueryString["new"], "bool"))
            {
                IsNew = Convert.ToBoolean(Request.QueryString["new"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Gen.isSpecificType(Request.QueryString["id"], "int"))
            {
                conID = Convert.ToInt32(Request.QueryString["id"]);
            }

            thisUrl = Gen.GetCurrentPageName(Request.Url.Query, false);

            if (!Page.IsPostBack)
            {
                thisEntry = new Condition(conID);

                if (!IsNew)
                {
                    if (thisEntry.Id > 0)
                    {
                        this.f_ConDesc.Text = thisEntry.Description;
                        this.f_active.Checked = Convert.ToBoolean(thisEntry.Active);
                    }
                }
            }
            
        }

        protected void btnCommit_Click(object sender, EventArgs e)
        {
            try
            {
                saveData(true);
            }
            catch (AppExceptions ex)
            {
                string output = string.Empty;

                foreach (string msg in ex.ErrorMessage)
                {
                    output += msg + "<br />";
                }

                lblException.Text = output;
                pnlException.Visible = true;
                pnlInterface.Visible = false;
            }
        }

        protected void btnCloseError_Click(object sender, EventArgs e)
        {
            pnlInterface.Visible = true;
            pnlException.Visible = false;
        }

        protected void saveData(bool reload)
        {
            List<string> ErrorMessage = new List<string>();

            if (EntriesBL.ValidateCategory(this.f_ConDesc.Text))
            {
                String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];
                String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Condition thisCondition = new Condition();
                thisCondition.Description = this.f_ConDesc.Text.Replace("'", "''");
                thisCondition.Active = f_active.Checked;

                if (!IsNew)
                {
                    thisCondition.Id = conID;
                    thisCondition.ModifiedBy = curUser;
                    thisCondition.ModifiedDate = Convert.ToDateTime(curDateTime);
                }
                else
                {
                    thisCondition.Guid = Gen.generateGUID();
                    thisCondition.CreatedBy = curUser;
                    thisCondition.CreatedDate = Convert.ToDateTime(curDateTime);
                    thisCondition.ModifiedBy = curUser;
                    thisCondition.ModifiedDate = Convert.ToDateTime(curDateTime);
                }

                String curRecID = string.Empty;

                if (IsNew)
                {
                    if (!thisCondition.CheckIfExist())
                    {
                        thisCondition.CreateUpdate(IsNew);
                    }
                }
                else
                {
                    thisCondition.CreateUpdate(IsNew);
                }


                if (reload)
                {
                    Gen.refreshParent(this, string.Empty);
                }
            }
        }
    }
}