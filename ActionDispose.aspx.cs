using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc;
using System.Configuration;
using System.Text;

namespace AssetManagement
{
    public partial class ActionDispose : System.Web.UI.Page
    {
        public int entryId, AssetID;
        public String nextPage, tableName, command, dbtype;
        Data dataObj = new Data();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["cmd"])) { command = Request.QueryString["cmd"]; }

            if (Convert.ToInt32(Request.QueryString["id"].ToString()) > 0) { entryId = Convert.ToInt32(Request.QueryString["id"]); }

            if (!string.IsNullOrEmpty(Request.QueryString["page"])) { nextPage = Request.QueryString["page"].Replace("[TTTTT]", "&"); }

            // if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Gen.isSpecificType(Request.QueryString["id"], "int"))
            //{
            //    AssetID = Convert.ToInt32(Request.QueryString["id"]);
            //}

            
            
            String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];

            StringBuilder outStr = new StringBuilder();
            string queryParam = string.Empty;

            outStr.Append("update tbl_assets set ");

            
             switch (command)
             {
                 case "d": //Dispose
                     outStr.Append("Disposed = 1,");
                     break;

                 case "u": //Un-Dispose
                     outStr.Append("Disposed = 0,");
                     outStr.Append("Active = 1,");
                     break;
             }

            //outStr.Append("update tblAssets set Disposed = 1,ToBeVerified = 1,ToBeApproved = 0,");

            outStr.Append("CurrentUserId='0', ");
            outStr.Append("ModifiedDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
            outStr.Append("ModifiedBy='" + curUser + "' ");
            outStr.Append("where Id =" + entryId);

            dataObj.SqlQuery.CommandText = outStr.ToString();
            dataObj.SetMySqlDbConn(1, dataObj.dbConnAssetMan);
            dataObj.SqlQuery.ExecuteNonQuery();
            dataObj.SetMySqlDbConn(0, dataObj.dbConnAssetMan);
            Response.Redirect(nextPage);
        }
    }
}