using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc;

namespace AssetManagement
{
    public partial class appError : System.Web.UI.Page
    {
        string err = "The system process could not be completed";
        bool sb = false;
        String errorMessage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["err"]))
            {
                err = Request.QueryString["err"];
            }

            if (string.IsNullOrEmpty(Request.QueryString["sb"]) == false && Gen.isSpecificType(Request.QueryString["sb"], "bool") == true)
            {
                sb = Convert.ToBoolean(Request.QueryString["sb"]);
            }

            if (string.IsNullOrEmpty(Request.QueryString["errMsg"]))
            {
                errorMessage = Request.QueryString["errMsg"];
            }

            switch (err)
            {
                case "userpermission":
                    ltlErrMsg.Text = "User '" + Request.ServerVariables["REMOTE_USER"].ToString() + "' does not have permission to use " + Environment.NewLine + "<strong>ASSET MANAGEMENT SYSTEM</strong>.";
                    break;

                case "sys":
                    ltlErrMsg.Text = "Error Occured on <strong>ASSET MANAGEMENT SYSTEM</strong> : " + errorMessage + ".";
                    break;
            }

            pnlButton.Visible = sb;
        }
    }
}