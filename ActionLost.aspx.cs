using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc;
using System.Text;
using System.Configuration;

namespace AssetManagement
{
    public partial class ActionLost : System.Web.UI.Page
    {
        public int entryId;
        public String nextPage, tableName, command, dbtype;
        Data dataObj = new Data();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect("Asset_list.aspx");

            if (!string.IsNullOrEmpty(Request.QueryString["cmd"])) { command = Request.QueryString["cmd"]; }

            if (Convert.ToInt32(Request.QueryString["id"].ToString()) > 0) { entryId = Convert.ToInt32(Request.QueryString["id"]); }

            if (!string.IsNullOrEmpty(Request.QueryString["page"])) { nextPage = Request.QueryString["page"].Replace("[TTTTT]", "&"); }

            String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];

            StringBuilder outStr = new StringBuilder();
            string queryParam = string.Empty;

            outStr.Append("update tbl_assets set ");
            

            switch (command)
            {
                case "l": //Lost
                    outStr.Append("Lost = 1,");
                   break;

                case "u": //Un-Lost
                    outStr.Append("Lost = 0,");
                    outStr.Append("Active = 1,");


                    break;
            }

            outStr.Append("CurrentUserId='0', ");
            outStr.Append("ModifiedDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
            outStr.Append("ModifiedBy='" + curUser + "' ");
            outStr.Append("where Id =" + entryId);

            dataObj.SqlQuery.CommandText = outStr.ToString();
            dataObj.SetMySqlDbConn(1, dataObj.dbConnAssetMan);
            dataObj.SqlQuery.ExecuteNonQuery();
            dataObj.SetMySqlDbConn(0, dataObj.dbConnAssetMan);


            string guidToPass = Gen.generateGUID();

            outStr.Clear();
            outStr.Append("Insert Into tbl_asset_movement_history (AssetId, FoundAsset, movedDate,CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Guid) ");
            outStr.Append(" values (");
            outStr.Append("'" + entryId  + "',");
            outStr.Append("'" + 1 + "', ");
            outStr.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
            outStr.Append("'" + curUser.Replace("'", "''") + "',");
            outStr.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
            outStr.Append("'" + curUser.Replace("'", "''") + "',");
            outStr.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
            outStr.Append("'" + guidToPass + "' ");
            outStr.Append(")");

            dataObj.SqlQuery.CommandText = outStr.ToString();
            dataObj.SetMySqlDbConn(1, dataObj.dbConnAssetMan);
            dataObj.SqlQuery.ExecuteNonQuery();
            dataObj.SetMySqlDbConn(0, dataObj.dbConnAssetMan);

            Response.Redirect(nextPage);
        }
    }
}