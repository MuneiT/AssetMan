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
    public partial class AssetCategoryAddEdit : System.Web.UI.Page
    {
        public bool IsNew = false;
        public int catID = 0;
        public CategoryItem thisEntry;
        public string thisUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["new"]) && Gen.isSpecificType(Request.QueryString["new"], "bool"))
            {
                IsNew = Convert.ToBoolean(Request.QueryString["new"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Gen.isSpecificType(Request.QueryString["id"], "int"))
            {
                catID = Convert.ToInt32(Request.QueryString["id"]);
            }

            thisUrl = Gen.GetCurrentPageName(Request.Url.Query, false);

            if (!Page.IsPostBack)
            {
                thisEntry = new CategoryItem(catID);

                if (!IsNew)
                {
                    if (thisEntry.Id > 0)
                    {
                        this.f_CatDesc.Text = thisEntry.Description;
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

            if (EntriesBL.ValidateCategory(this.f_CatDesc.Text))
            {
                String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];
                String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                CategoryItem thisCategory = new CategoryItem();
                thisCategory.Description = this.f_CatDesc.Text.Replace("'", "''");
                thisCategory.Active = f_active.Checked;

                if (!IsNew)
                {
                    thisCategory.Id = catID;
                    thisCategory.ModifiedBy = curUser;
                    thisCategory.ModifiedDate = Convert.ToDateTime(curDateTime);
                }
                else
                {
                    thisCategory.Guid = Gen.generateGUID();
                    thisCategory.CreatedBy = curUser;
                    thisCategory.CreatedDate = Convert.ToDateTime(curDateTime);
                    thisCategory.ModifiedBy = curUser;
                    thisCategory.ModifiedDate = Convert.ToDateTime(curDateTime);
                }

                String curRecID = string.Empty;

                if (IsNew)
                {
                    if (!thisCategory.CheckIfExist())
                    {
                        thisCategory.CreateUpdate(IsNew);
                    }
                }
                else
                {
                    thisCategory.CreateUpdate(IsNew);
                }


                if (reload)
                {
                    Gen.refreshParent(this, string.Empty);
                }
            }
        }
    }
}