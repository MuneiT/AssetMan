using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc.DAL;
using AssetManagement.Proc;
using System.Configuration;
using System.Text;
using AssetManagement.Proc.BL;
using System.IO;

namespace AssetManagement
{
    public partial class AssetMovement : System.Web.UI.Page
    {
        public bool IsNew = false , moveAssigned = false, moveStore = false ; 
        //VerifyAsset = false, showLostAsset = false, showDisposedAsset = false, mustbeActive = false;
        public int assetID = 0 , curStoreRec = 0;
        public Assets assetEnrty;
        public string thisUrl = string.Empty;
        public string currentAssetUsernameId = string.Empty, movingAssetUsernameId = string.Empty;
        public int PageCurr = 0;
        public String nextPage;
        public string strDefaultReportPath = "MovementForms";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["page"])) { nextPage = Request.QueryString["page"].Replace("[TTTTT]", "&"); }

            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Gen.isSpecificType(Request.QueryString["id"], "int"))
            {
                assetID = Convert.ToInt32(Request.QueryString["id"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["moveAssigned"]) && Gen.isSpecificType(Request.QueryString["moveAssigned"], "bool"))
            {
                moveAssigned = Convert.ToBoolean(Request.QueryString["moveAssigned"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["moveStore"]) && Gen.isSpecificType(Request.QueryString["moveStore"], "bool"))
            {
                moveStore = Convert.ToBoolean(Request.QueryString["moveStore"]);
            }


            thisUrl = Gen.GetCurrentPageName(Request.Url.Query, false);

            if (!Page.IsPostBack)
            {
                

                PopulateDropDownOptions();
                f_movementDate.Attributes.Add("readonly", "readonly");
                if (assetID > 0)
                {
                    assetEnrty = new Assets(assetID);
                    buildAssetListList(assetEnrty);
                    AssetMovementItem MovementEntry = new AssetMovementItem(assetID);
                    //Must be Global


                    if (assetEnrty.Id > 0)
                    {
                        if (assetEnrty.CurrentUserId > 0)
                        {
                            currentAssetUsernameId = assetEnrty.CurrentUserId.ToString();
                            ViewState["currentAssetUsernameId"] = currentAssetUsernameId;
                            curStoreRec = 0;
                        }
                        else
                        {
                            currentAssetUsernameId = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["id"];
                            ViewState["currentAssetUsernameId"] = currentAssetUsernameId;
                            curStoreRec = 1;
                        }


                        //Get Issuing from user detail
                        User currentUser = new User(currentAssetUsernameId, false, false);

                        if (currentUser.Id > 0 && assetEnrty.CurrentUserId != 0)
                        {
                            //this.panelddlMovingTo.Visible = false;
                            this.panelToStoreRoom.Visible = true;
                            this.panelTemp1.Visible = true;
                            this.panelTemp2.Visible = true;
                            this.panelTemp3.Visible = true;
                            this.panelTemp4.Visible = true;
                            pnlCondition.Visible = true;
                            this.panelReceiverEmail.Visible = false;
                            this.panelFullNames.Visible = false;
                            this.panelTelNumber.Visible = false;
                            this.panelRoomNumber.Visible = false;
                            this.panelDirectorate.Visible = false;

                            this.f_fromPersal.Text = currentUser.Ad;
                            this.f_fromFullName.Text = currentUser.FullName;
                            this.f_fromRoom.Text = currentUser.OfficeNo;
                            this.f_fromTel.Text = currentUser.Telephone;

                            if (MovementEntry.AssetId == assetEnrty.Id)

                            {
                                ddl_stafflist.SelectedValue = (MovementEntry.Technician).ToString();
                            }
                            

                            if (currentUser.DivisionId > 0)
                            {
                                Division userDirectorate = new Division(currentUser.DivisionId);

                                if (userDirectorate.Id > 0)
                                {
                                    this.f_fromDirectorate.Text = userDirectorate.DivisionName;
                                }
                            }
                        }
                        else if (assetEnrty.CurrentUserId == 0)
                        {
                            //this.panelddlMovingTo.Visible = false;
                            this.panelFromStoreRoom.Visible = true;
                            this.panelTemp1.Visible = true;
                            this.panelTemp2.Visible = true;
                            this.panelTemp3.Visible = true;
                            this.pnlCondition.Visible = false;
                            this.panelTemp4.Visible = true;
                            this.panelFromPersal.Visible = false;
                            this.panelFromFullNames.Visible = false;
                            this.panelFromTelNumber.Visible = false;
                            this.panelFromRoomNumber.Visible = false;
                            this.panelFromDirectorate.Visible = false;
                        }
                        
                    }
                    else
                    {
                        ViewState["currentAssetUsernameId"] = string.Empty;
                    }


                }
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            //this.ToolkitScriptManager1.RegisterPostBackControl(btnMoveAsset);
        }

        protected void btnProcessMovement_Click(object sender, EventArgs e)
        {
            try
            {
                //Session["selectedFilename"] = moveFileUploder.PostedFile.FileName; 
                processData(true);
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

        protected void processData(bool reload) 
        {
            List<string> ErrorMessage = new List<string>();

            string _currentAssetUsernameId = string.Empty;
            string _movingAssetUsernameId = string.Empty;
         
            Assets AssetEntry = new Assets(assetID);


            if (AssetEntry.CurrentUserId == 0)
            {
                _currentAssetUsernameId = Convert.ToInt32(0).ToString();
                curStoreRec = 1;
            }
            else if (ViewState["currentAssetUsernameId"] != null)
            {
                _currentAssetUsernameId = (string)ViewState["currentAssetUsernameId"];
                curStoreRec = 1;
            }

            if (ViewState["movingAssetUsernameId"] != null)
            {
                _movingAssetUsernameId = (string)ViewState["movingAssetUsernameId"];
                curStoreRec = 0;
            }

         
                if ( Convert.ToInt32(_currentAssetUsernameId) >  0)
                {
                    _movingAssetUsernameId = Convert.ToInt32(0).ToString();
                }
             


            //if (EntriesBL.ValidateAssetMovement(this.assetID.ToString(), this.f_movementDate.Text, _movingAssetUsernameId, f_movementReason.Text))
            if (EntriesBL.ValidateAssetMovement(this.assetID.ToString(), this.f_movementDate.Text, f_movementReason.Text , ddl_stafflist.SelectedValue)) 
            {
                String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];
                String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                String moveDateTime = string.Empty;

                Data dataObj = new Data();
                dataObj.SetMySqlDbConn(1, dataObj.dbConnAssetMan);
                Assets thisAsset = new Assets();
                StringBuilder outStr = new StringBuilder();
                
                String PersalNo, EmpName, EmpSurname;

                if (Convert.ToInt32(_movingAssetUsernameId) > 0)
                {
                    User thisOfficial = new User(Convert.ToInt32(_movingAssetUsernameId));
                    PersalNo = thisOfficial.Ad;
                    EmpName = thisOfficial.Name;
                    EmpSurname = thisOfficial.Surname;
                }
                else
                {
                    PersalNo = string.Empty;
                    EmpName = string.Empty;
                    EmpSurname = string.Empty;
                 }

                if (_currentAssetUsernameId != _movingAssetUsernameId)
                {
                    if (!string.IsNullOrEmpty(f_movementDate.Text.Trim()))
                    {
                        moveDateTime = this.f_movementDate.Text + " 00:00:00";
                    }
                    else
                    {
                        moveDateTime = "1900-01-01 00:00:00";
                    }

                    

                    outStr.Clear();
                    outStr.Append("Update dcs_assets_management.tbl_assets Set ");
                    outStr.Append("CurrentUserId = '" + _movingAssetUsernameId + "', ");
                    outStr.Append("MovedDate = '" + moveDateTime + "', ");
                    outStr.Append("PersalNo = '" + PersalNo + "', ");
                    outStr.Append("EmpName = '" + EmpName + "', ");
                    outStr.Append("EmpSurname = '" + EmpSurname + "', ");
                    outStr.Append("StoreRoom = '" + curStoreRec + "', ");
                    if (rdbCondition.Checked)
                    {
                        outStr.Append("assCondition = '" + "repairs" + "', ");
                    }
                    
                    outStr.Append("ModifiedBy = '" + curUser + "', ");
                    outStr.Append("ModifiedDate = '" + curDateTime + "' ");
                    outStr.Append("WHERE Id = " + this.assetID + "");

                    dataObj.SqlQuery.CommandText = outStr.ToString();
                    dataObj.SqlQuery.ExecuteNonQuery();
                    //thisAsset.GeneralInsertUpdate(outStr.ToString(),"","");

                    

                    string guidToPass = Gen.generateGUID();
                    string tableToQuery = "tbl_asset_movement_history";
                    string returnedMovementId = string.Empty;


                    outStr.Clear();
                    outStr.Append("Insert Into dcs_assets_management.tbl_asset_movement_history (AssetId, prevOfficial, curOfficial, movedDate, Technician, movementReason, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Guid) ");
                    outStr.Append(" values (");
                    outStr.Append("'" + this.assetID + "','" + _currentAssetUsernameId + "','" + _movingAssetUsernameId + "',");
                    outStr.Append("'" + moveDateTime + "', ");
                    outStr.Append("'" + this.ddl_stafflist.SelectedValue.Replace("'", "''") + "', ");
                    outStr.Append("'" + f_movementReason.Text.Replace("'", "''") + "', ");
                    outStr.Append("'" + curUser.Replace("'", "''") + "',");
                    outStr.Append("'" + curDateTime + "',");
                    outStr.Append("'" + curUser.Replace("'", "''") + "',");
                    outStr.Append("'" + curDateTime + "', ");
                    outStr.Append("'" + guidToPass + "' ");
                    outStr.Append(")");
                    returnedMovementId = thisAsset.GeneralInsertUpdate(outStr.ToString(), guidToPass, tableToQuery);

                    if (!string.IsNullOrEmpty(returnedMovementId) && moveFileUploder.HasFile)
                    {
                        uploadAttachments(returnedMovementId);
                    }

                    AssetMoveParamsItem thisParameters = new AssetMoveParamsItem();
                    AssetMovementItem thisAssetM = new AssetMovementItem();


                    if (_movingAssetUsernameId != Convert.ToInt32(0).ToString())
                    {
                        string UserEmail = string.Empty;
                        string Name = string.Empty;

                        outStr.Clear();
                        outStr.Append("SELECT DISTINCT tUSE.Email ");
                        outStr.Append("FROM dcssysman.tbl_AppsUsers tUSE ");
                        outStr.Append("INNER JOIN dcs_assets_management.tbl_asset_movement_history tAH ON tAH.curOfficial = tUSE.Id ");
                        outStr.Append("where tAH.curOfficial = " + _movingAssetUsernameId + "");
                        UserEmail = thisAsset.GeneralSelect(outStr.ToString(), "", "");

                        outStr.Clear();
                        outStr.Append("SELECT DISTINCT tUSE.Name ");
                        outStr.Append("FROM dcssysman.tbl_AppsUsers tUSE ");
                        outStr.Append("INNER JOIN dcs_assets_management.tbl_asset_movement_history tAH ON tAH.curOfficial = tUSE.Id ");
                        outStr.Append("where tAH.curOfficial = " + _movingAssetUsernameId + "");
                        Name = thisAsset.GeneralSelect(outStr.ToString(), "");



                        Gen.sendNotificationMessage(_movingAssetUsernameId, UserEmail, Name, assetID, "Please Note that you have a new asset allocated to you");
                    }

                }


                if (moveAssigned)
                {
                    Gen.refreshParent(this, "Asset_List.aspx?mustbeActive=true");
                }
                else
                {
                    Gen.refreshParent(this, "Asset_List.aspx?StoreRoom=true");
                }

                    //ScriptManager.RegisterStartupScript(Page, this.GetType(), "script", "gotoParentURL('" + nextPage + "');", true);
                
            }
        }

        protected void f_toPersal_TextChanged(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(),"alert", "alert('TextChanged event fired " + f_toPersal.Text + "');", true);

            if (!string.IsNullOrEmpty(this.f_toPersal.Text))
            {
                movingAssetUsernameId = string.Empty;
                User existingUser = new User(this.f_toPersal.Text.Replace("'", "''"), true, false);
                if (existingUser.Id > 0)
                {
                    userProfileError.Text = string.Empty;
                    movingAssetUsernameId = existingUser.Id.ToString();
                    ViewState["movingAssetUsernameId"] = movingAssetUsernameId;
                    this.f_toFullname.Text = existingUser.FullName;
                    this.f_toTel.Text = existingUser.Telephone;
                    this.f_toRoom.Text = existingUser.OfficeNo;

                    if (existingUser.DivisionId > 0)
                    {
                        Division userDirectorate = new Division(existingUser.DivisionId);

                        if (userDirectorate.Id > 0)
                        {
                            this.f_toDirectorate.Text = userDirectorate.DivisionName;
                        }
                    }
                }
                else
                {
                    clearValues();
                    ViewState["movingAssetUsernameId"] = string.Empty;
                    userProfileError.Text = "No profile found for user Persal:" + this.f_toPersal.Text.Replace("'", "''");
                }
            }
            else
            {
                ViewState["movingAssetUsernameId"] = string.Empty;
            }
        }

        //protected void ddlMovingTo_SelectedIndexChanged(object sender, EventArgs e)
        //{

    

        //    if (!string.IsNullOrEmpty(ddlMovingTo.SelectedValue))
        //    {
        //        if (Convert.ToInt32(ddlMovingTo.SelectedValue) == 0)
        //        {
        //            this.panelReceiverEmail.Visible = true;
        //            this.panelFullNames.Visible = true;
        //            this.panelTelNumber.Visible = true;
        //            this.panelRoomNumber.Visible = true;
        //            this.panelDirectorate.Visible = true;

        //            panelToStoreRoom.Visible = false;
        //        }
        //        else if (Convert.ToInt32(ddlMovingTo.SelectedValue) == 1)
        //        {
        //            this.panelToStoreRoom.Visible = true;
        //            f_ToStoreRoom.Enabled = false;
        //            f_ToStoreRoom.Text = "ASSET MOVING TO ICT STOREROOM ";
                 
        //            this.panelReceiverEmail.Visible = false;
        //            this.panelFullNames.Visible = false;
        //            this.panelTelNumber.Visible = false;
        //            this.panelRoomNumber.Visible = false;
        //            this.panelDirectorate.Visible = false;
        //        }
        //    }
            

        //}

        void clearValues()
        {
            f_toDirectorate.Text = "";
            f_toFullname.Text = "";
            f_toRoom.Text = "";
            f_toTel.Text = "";
        }

        protected void btnCloseError_Click(object sender, EventArgs e)
        {
            pnlInterface.Visible = true;
            pnlException.Visible = false;
        }

        protected void buildAssetListList(Assets selAsset)
        {
            if (selAsset.Id > 0)
            {
                string rowClass = "txt_blue_cgothic_13";

                lit_assetlist.Text += "<div class=\"datagrid\">" + Environment.NewLine;
                lit_assetlist.Text += "   <table width=\"100%\" border=\"0\" cellpadding=\"3\" cellspacing=\"1\" bgcolor=\"#CCCCCC\">" + Environment.NewLine;
                lit_assetlist.Text += "	    <thead><tr class=\"txt_white_12\">" + Environment.NewLine;
                lit_assetlist.Text += "		    <th width=\"100\" align=\"left\" valign=\"top\"><strong>ASSET NO</strong></td>" + Environment.NewLine;
                lit_assetlist.Text += "		    <th align=\"left\" valign=\"top\"><strong>DESCRIPTION</strong></th>" + Environment.NewLine;
                lit_assetlist.Text += "		    <th width=\"250\" align=\"left\" valign=\"top\"><strong>SERIAL NO</strong></th>" + Environment.NewLine;
                lit_assetlist.Text += "      </tr></thead>" + Environment.NewLine;

                lit_assetlist.Text += "<tbody>";
                lit_assetlist.Text += "       <tr class=\"" + rowClass + "\">" + Environment.NewLine;
                lit_assetlist.Text += "           <td align=\"left\" valign=\"top\" class=\"datagridRow txt_blue_cgothic_13\" bgcolor=\"#FFFFFF\">" + selAsset.AssetNo + "</td>" + Environment.NewLine;
                lit_assetlist.Text += "          <td align=\"left\" valign=\"top\" class=\"datagridRow txt_blue_cgothic_13\" bgcolor=\"#FFFFFF\">" + selAsset.Description + "</td>" + Environment.NewLine;
                lit_assetlist.Text += "          <td align=\"left\" valign=\"top\" class=\"datagridRow txt_blue_cgothic_13\" bgcolor=\"#FFFFFF\">" + selAsset.SerialNo + "</td>" + Environment.NewLine;
                lit_assetlist.Text += "       </tr>" + Environment.NewLine;

                lit_assetlist.Text += "</tbody></table></div>" + Environment.NewLine;
            }
            else
            {
                lit_assetlist.Text += "<div class=\"datagrid\">" + Environment.NewLine;
                lit_assetlist.Text += "   No Asset Selected" + Environment.NewLine;
                lit_assetlist.Text += "</div>" + Environment.NewLine;
            }
        }

        protected void uploadAttachments(string passedAssetMoveID)
        {
            Data dataObj = new Data();

            String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String createdby = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["fullname"];

            String ulDir = MapPath(strDefaultReportPath);
            ulDir = ulDir.Replace("\\", "\\") + "\\";
            String query;
            String title;
            String selectedfile;
            String ext = "";

            bool err;
            err = false;

            if (moveFileUploder.HasFile)
            {
                selectedfile = moveFileUploder.FileName;
                title = moveFileUploder.FileName.ToUpper();

                selectedfile = (passedAssetMoveID.ToString() + "_" + DateTime.Now.ToString("yyMMddHHmmss") + "_" + selectedfile).ToUpper();

                try
                {
                    moveFileUploder.PostedFile.SaveAs(ulDir + selectedfile);
                    ext = System.IO.Path.GetExtension(moveFileUploder.PostedFile.FileName);
                    ext = (ext.Replace(".", "")).ToUpper();
                }
                catch (Exception ex)
                {
                    err = true;
                }

                if (!err)
                {
                    query = "insert into tbl_asset_movement_attachement (movement_id,filename,title,extention,createddate,createdby) values ('" + passedAssetMoveID + "','" + selectedfile + "','" + title.Replace("'", "''") + "','" + ext + "','" + curDateTime + "','" + createdby + "')";

                    dataObj.SetMySqlDbConn(1, dataObj.dbConnAssetMan);
                    dataObj.SqlQuery.CommandText = query;
                    dataObj.SqlQuery.ExecuteNonQuery();
                    dataObj.SetMySqlDbConn(0, dataObj.dbConnAssetMan);
                }
            }
        }

        void PopulateDropDownOptions()
        {
            SupportStaffList supportStaffData = new SupportStaffList(true);
            ddl_stafflist.DataSource = supportStaffData.Listing;
            ddl_stafflist.DataTextField = "FullName";
            ddl_stafflist.DataValueField = "FullName";
            ddl_stafflist.DataBind();
            ddl_stafflist.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All Technician...", "0"));
        }

        //protected void UploadFile(object sender, EventArgs e)
        //{
        //    if (assetID > 0)
        //    {
        //        string filename = Path.GetFileName(f_fileToUpload.PostedFile.FileName);
        //        string contentType = f_fileToUpload.PostedFile.ContentType;
        //        using (Stream fs = f_fileToUpload.PostedFile.InputStream)
        //        {
        //            using (BinaryReader br = new BinaryReader(fs))
        //            {
        //                byte[] bytes = br.ReadBytes((Int32)fs.Length);

        //                String curUser = Request.Cookies[ConfigurationManager.AppSettings["PrimaryCookie"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["PrimaryCookie"]]["surname"];
        //                String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //                //CategoryItem thisCat = new CategoryItem();
        //                //thisCat.Id = CatId;
        //                //thisCat.Picture = bytes;
        //                //thisCat.Modifiedby = curUser;
        //                //thisCat.ModifiedDate = Convert.ToDateTime(curDateTime);
        //                //thisCat.UploadPicture();
        //            }
        //        }
        //    }

        //    Response.Redirect(Request.Url.AbsoluteUri);
        //}
    }
}