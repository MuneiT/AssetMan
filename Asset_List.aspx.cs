using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc.DAL;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Configuration;
using AssetManagement.Proc;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace AssetManagement
{
    public partial class Asset_List : System.Web.UI.Page
    {
        public string thisURL = string.Empty;
        public string OrderBy = string.Empty;
        public string delParam = string.Empty;

        public bool? CapexRecords = null;
        public bool DisposedRecords = false;
        public bool LostRecords = false, StoreRecords = false;
        public bool thisDisposedRecords = false;

        public bool? awaitVerify = null;
        public bool? awaitApproval = null;

        public bool? passedCapex { get { return CapexRecords; } }
        public bool? passedDisposed { get { return DisposedRecords; } }
        public bool? passedLostAsset { get { return LostRecords; } }

        public string selDCSAssNo = string.Empty;
        public string selConditions = string.Empty;
        public string selCategories = string.Empty;

        public string SelAssetStatus = string.Empty;
        //public string selDisposed = string.Empty;
        //public string selLost = string.Empty;
        //public string selMarkAsNew = string.Empty;
        public bool mustbeActive = false, InStoreRoom = false;

        public string SearchBy = string.Empty;
        public string SearchText = string.Empty;

        //calculating asset duration.
        private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private DateTime purchasedDate;
        private DateTime CurrentDate;
        private int year;
        private int month;
        private int day;
        private int increment;
        public Assets assetEnrty;
        public string Duration = string.Empty;
        

        Data dataObj = new Data();

        public int pageSize = 25;
        public int PageCurr = 0;
        public string OrderByType = string.Empty;
        public string selOrderBy = string.Empty, dateRangeType = "ld";
        public string purchaseStartDate = string.Empty, purchaseEndDate = string.Empty, movStartDate = string.Empty, movEndDate = string.Empty;
        public int thisAssetID = 0;

        
       
        public AssetsList thisList;

        public CultureInfo cultureInfo = new CultureInfo(ConfigurationManager.AppSettings["GlobalLocation"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.BodyTag.Attributes.Add("OnResize", "CenterDiv('divPopupDialog')");
            thisURL = Gen.GetCurrentPageName(Request.Url.Query, false);

            if (!string.IsNullOrEmpty(Request.QueryString["SearchBy"])) { SearchBy = Request.QueryString["SearchBy"].ToString(); ViewState["selectedSearchColumn"] = SearchBy; }
            if (!string.IsNullOrEmpty(Request.QueryString["SearchText"])) { SearchText = Request.QueryString["SearchText"].ToString(); ViewState["selectedSearchText"] = SearchText; }
            if (!string.IsNullOrEmpty(Request.QueryString["SearchBy"])) { SearchBy = Request.QueryString["SearchBy"].ToString(); ViewState["selectedSearchColumn"] = SearchBy; }

            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]) && Gen.isSpecificType(Request.QueryString["startDate"], "date"))
            {
                purchaseStartDate = Convert.ToDateTime(Request.QueryString["startDate"]).ToString("yyyy-MM-dd");
            }

            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]) && Gen.isSpecificType(Request.QueryString["endDate"], "date"))
            {
                purchaseEndDate = Convert.ToDateTime(Request.QueryString["endDate"]).ToString("yyyy-MM-dd");
            }

            // moved date

            if (!string.IsNullOrEmpty(Request.QueryString["mStartDate"]) && Gen.isSpecificType(Request.QueryString["mstartDate"], "date"))
            {
                movStartDate = Convert.ToDateTime(Request.QueryString["mstartDate"]).ToString("yyyy-MM-dd");
            }

            if (!string.IsNullOrEmpty(Request.QueryString["mEndDate"]) && Gen.isSpecificType(Request.QueryString["mEndDate"], "date"))
            {
                movEndDate = Convert.ToDateTime(Request.QueryString["mEndDate"]).ToString("yyyy-MM-dd");
            }
            bool ReportAdmin = Convert.ToBoolean(Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["roleID_ReportAdmin_147"]);
            if (ReportAdmin)
            {
                pnlExports.Visible = true;
            }


            if (!string.IsNullOrEmpty(Request.QueryString["pg"]) && Gen.isSpecificType(Request.QueryString["pg"], "int"))
            {
                PageCurr = Convert.ToInt32(Request.QueryString["pg"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["selCategories"]))
            {
                selCategories = Request.QueryString["selCategories"].ToString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["selConditions"]))
            {
                selConditions = Request.QueryString["selConditions"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["SelAssetStatus"]))
            {
                SelAssetStatus = Request.QueryString["SelAssetStatus"].ToString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["OrderBy"])) { OrderBy = Request.QueryString["OrderBy"].ToString(); ViewState["SelectedOrder"] = OrderBy; }
            if (!string.IsNullOrEmpty(Request.QueryString["OrderType"]))
            {
                OrderByType = Request.QueryString["OrderType"];
                ViewState["SelectedOrderType"] = OrderByType;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["DisposedAssets"]) && Gen.isSpecificType(Request.QueryString["DisposedAssets"], "bool"))
            {
                thisDisposedRecords = Convert.ToBoolean(Request.QueryString["DisposedAssets"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["LostRecords"]) && Gen.isSpecificType(Request.QueryString["LostRecords"], "bool"))
            {
                LostRecords = Convert.ToBoolean(Request.QueryString["LostRecords"]);
            }


            if (!string.IsNullOrEmpty(Request.QueryString["mustbeActive"]) && Gen.isSpecificType(Request.QueryString["mustbeActive"], "bool"))
            {
                mustbeActive = Convert.ToBoolean(Request.QueryString["mustbeActive"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["StoreRoom"]) && Gen.isSpecificType(Request.QueryString["StoreRoom"], "bool"))
            {
                InStoreRoom = Convert.ToBoolean(Request.QueryString["StoreRoom"]);
            }


            //if (!string.IsNullOrEmpty(Request.QueryString["selCategories"]))
            //{
            //    selCategories = Request.QueryString["selCategories"].ToString();
            //}

            //if (!string.IsNullOrEmpty(Request.QueryString["selConditions"]))
            //{
            //    selConditions = Request.QueryString["selConditions"].ToString();
            //}

            //selDCSAssNo

            if (!string.IsNullOrEmpty(Request.QueryString["selDCSAssNo"]))
            {
                selDCSAssNo = Request.QueryString["selDCSAssNo"];
            }

            if (!Page.IsPostBack)
            {
               
                f_fromDate.Attributes.Add("readonly", "readonly");
                f_ToDate.Attributes.Add("readonly", "readonly");
                f_mov_from.Attributes.Add("readonly", "readonly");
                f_mov_To.Attributes.Add("readonly", "readonly");

                PopulateDropDownOptions();
                PopulateSelectedFilterOptions();
                buildAssetsList(Navigation.None);
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            buildAssetsList(Navigation.Previous);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            buildAssetsList(Navigation.Next);
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            buildAssetsList(Navigation.Last);
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            buildAssetsList(Navigation.First);
        }

            
        private void buildAssetsList(Navigation navigation)
        {
            try
            {
                thisList = new AssetsList(SearchBy, SearchText, mustbeActive, selDCSAssNo, selConditions, selCategories, OrderBy, OrderByType.ToString(), thisDisposedRecords, LostRecords, InStoreRoom, awaitVerify, awaitApproval, purchaseStartDate, purchaseEndDate, movStartDate, movEndDate,SelAssetStatus);
                PagedDataSource pageData = new PagedDataSource();
                pageData.DataSource = thisList.Listing;
                pageData.AllowPaging = true;
                pageData.PageSize = pageSize;

                //sbtnFilterOptions.Attributes.Add("onclick", "OpenPopupDialog('divPopupDialog', 'ifOptions', 150, 650, 'AssetFilterOptions.aspx?awaitApproval=" + awaitApproval + "&awaitVerify=" + awaitVerify + "&Capex=" + CapexRecords + "&DisposedAssets=" + DisposedRecords + "','PopUpBase PopPrimary'); return false;");

               
                switch (navigation)
                {
                    case Navigation.First:
                        NowViewing = 0;
                        break;
                    case Navigation.Next:
                        NowViewing++;
                        break;
                    case Navigation.Previous:
                        NowViewing--;
                        break;
                    case Navigation.Last:
                        NowViewing = pageData.PageCount - 1;
                        break;
                    case Navigation.Pager:
                        if (int.Parse(txtPage.Text) >= pageData.PageCount)
                            NowViewing = pageData.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = PageCurr;
                        break;
                }

                ViewState["SelectedPage"] = NowViewing;
                pageData.CurrentPageIndex = NowViewing;
                PageCurr = (int)ViewState["SelectedPage"];

                rptList.DataSource = pageData;
                rptList.DataBind();

                if (pageData.Count == 0)
                {
                    lblTotalPages.Text = "0";
                    txtPage.Text = "0";

                    btnNext.CssClass = "btn btn-primary btn-sm disabled";
                    btnPrev.CssClass = "btn btn-primary btn-sm disabled";
                    btnFirst.CssClass = "btn btn-primary btn-sm disabled";
                    btnLast.CssClass = "btn btn-primary btn-sm disabled";

                    dvRecords.Visible = false;
                    dvNoRecords.Visible = true;
                    ofNoRec.Visible = false;
                    //dvNavigation.Visible = false;
                }
                else
                {
                    lblTotalPages.Text = pageData.PageCount.ToString();
                    txtPage.Text = (pageData.CurrentPageIndex + 1).ToString();
                    lblBalance.Text = String.Format(cultureInfo, "{0:C}", CalculateTotalValue(thisList, CapexRecords, DisposedRecords));

                    pnlExports.Visible = true;
                    dvRecords.Visible = true;
                    dvNoRecords.Visible = false;
                    ofNoRec.Visible = true;

                    if (pageData.IsLastPage)
                    {
                        btnNext.Enabled = false;
                        btnNext.CssClass = "btn btn-primary btn-sm disabled";

                        btnLast.Enabled = false;
                        btnLast.CssClass = "btn btn-primary btn-sm disabled";
                    }
                    else
                    {
                        btnNext.Enabled = true;
                        btnNext.CssClass = "btn btn-primary btn-sm";

                        btnLast.Enabled = true;
                        btnLast.CssClass = "btn btn-primary btn-sm";
                    }

                    if (pageData.IsFirstPage)
                    {
                        btnFirst.Enabled = false;
                        btnFirst.CssClass = "btn btn-primary btn-sm disabled";

                        btnPrev.Enabled = false;
                        btnPrev.CssClass = "btn btn-primary btn-sm disabled";
                    }
                    else
                    {
                        btnPrev.Enabled = true;
                        btnPrev.CssClass = "btn btn-primary btn-sm";

                        btnFirst.Enabled = true;
                        btnFirst.CssClass = "btn btn-primary btn-sm";
                    }

                }
            }
            catch (Exception io)
            {
                Response.Write(io.Message.ToString());
            }
        }

        public int NowViewing
        {
            get
            {
                object obj = ViewState["_NowViewing"];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }
            set
            {
                this.ViewState["_NowViewing"] = value;
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            bool Cordinator = Convert.ToBoolean(Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["role5_Cordinator"]);
            bool AssetMover = Convert.ToBoolean(Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["role17_Approver"]);
            bool ReportAdmin = Convert.ToBoolean(Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["roleID_ReportAdmin_147"]);
            

            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Assets item = (Assets)e.Item.DataItem;
                HtmlGenericControl wrapList = new HtmlGenericControl("div");
                StringBuilder outStr = new StringBuilder();

                DateDifference(item.Id);

                String rowClass = "";

                rowClass = "txt_blue_cgothic_13";

                if (item.Active == 0 || item.Disposed == 1)
                {
                    rowClass = "txt_red_cgothic_13";
                }

                String delName = string.Empty, delTitle = string.Empty;
                if (!Convert.ToBoolean(item.Disposed))
                {

                    delParam = "ActionDispose.aspx?cmd=d&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate  + "[TTTTT]mStartDate=" + movStartDate + "[TTTTT]mEndDate=" + movEndDate;
                    delName = "D";
                    delTitle = "Dispose Asset";
                    
                }
                //else
                //{
                //    delParam = "ActionDispose.aspx?cmd=u&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]pg=" + ViewState["SelectedPage"] + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "[TTTTT]mStartDate=" + movStartDate + "[TTTTT]mEndDate=" + movEndDate;
                //    delName = "U";
                //    delTitle = "Un-Dispose Asset";
                //}



                String lostParam = string.Empty, lostName = string.Empty, lostTitle = string.Empty;
               
                if (!Convert.ToBoolean(item.Lost))
                {
                    lostParam = "ActionLost.aspx?cmd=l&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "[TTTTT]mStartDate=" + movStartDate + "[TTTTT]mEndDate=" + movEndDate;
                    lostName = "L";
                    lostTitle = "Mark Asset As Lost / Stolen";
                }
                else
                {
                    lostParam = "ActionLost.aspx?cmd=u&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]pg=" + ViewState["SelectedPage"] + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "[TTTTT]mStartDate=" + movStartDate + "[TTTTT]mEndDate=" + movEndDate;
                    lostName = "U";
                    lostTitle = "Un-Lose / Find Asset";
                }
               

                outStr.Append("  <tr class=\"" + rowClass + "\">");
                outStr.Append("     <td align=\"center\" valign=\"middle\"  class=\"datagridRow\" bgcolor=\"#FFFFFF\">");
                outStr.Append("         <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                outStr.Append("	            <tr>");


                if (Cordinator || AssetMover || ReportAdmin)
                {
                    if (Cordinator)
                    {
                        if (!Convert.ToBoolean(item.Disposed) && !Convert.ToBoolean(item.Lost) && !Convert.ToBoolean(InStoreRoom))
                        {

                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit Asset\" class=\"btn btn-primary btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 550, 950,'Asset_edit.aspx?edit=true&id=" + item.Id + "&mustbeActive=true  &page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">E</div></td>");
                            
                            if (item.CurrentUserId > 0)
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Cannot Dispose Currently Assigned Asset\" class=\"btn btn-danger btn-sm disabled\">D</div></td>");
                            }
                            else
                            {
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Dispose Asset\" class=\"btn btn-danger btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 550, 950,'Asset_edit.aspx?dispose=true&new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">" + delName + "</div></td>");
                            }
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle + "\" class=\"btn btn-info btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 950,'Asset_edit.aspx?lost=true&new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">" + lostName + "</div></td>");

                            if (item.Condition != "repairs" || item.Condition != "damaged")
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Move Asset\" class=\"btn btn-success btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 550, 1200,'AssetMovement.aspx?moveAssigned=true&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">M</div></td>");
                            }
                            else
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Cannot Move Damaged Asset or Assets that are still being repaired\" class=\"btn btn-success btn-sm disabled\">M</div></td>");
                            }
                           
                        }
                        else if (Convert.ToBoolean(item.Disposed) && !Convert.ToBoolean(InStoreRoom))
                        {
                            
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Cannot Edit Disposed Asset\" class=\"btn btn-primary  btn-sm disabled\">E</div></td>");
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Asset is already Disposed\" class=\"btn btn-danger btn-sm disabled\">D</div></td>");
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"Cannot Lose Disposed Asset\"  class=\"btn btn-info btn-sm disabled\">L</div></td>");
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Cannot Move Disposed Asset\"  class=\"btn btn-success btn-sm disabled\">M</div></td>");
                        }
                        else if (Convert.ToBoolean(item.Lost) && !Convert.ToBoolean(InStoreRoom))
                        {
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Cannot Edit Lost Asset\" class=\"btn btn-primary btn-sm disabled\">&nbspE</div></td>");
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle + "\" class=\"btn btn-info btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 900,'Asset_edit.aspx?lost=true&lostcondition=true&id=" + item.Id + "','PopUpBase PopPrimary');\">" + lostName + "</div></td>");
                            
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Cannot Dispose a Lost Asset\" class=\"btn btn-danger btn-sm disabled\">D</div></td>");
                            
                            if(item.Condition == "good" || item.Condition == "fair")
                            {
                                outStr.Append("         <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Cannot Move Lost Asset\" class=\"btn btn-success btn-sm disabled\">M</div></td>");
                            }
                            
                        }
                        else if (Convert.ToBoolean(InStoreRoom && (item.Condition == "repairs" || item.Condition == "damaged" || item.Condition=="old")))
                        {
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit Asset\" class=\"btn btn-primary btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 900,'Asset_edit.aspx?edit=true&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">E</div></td>");

                            if (item.CurrentUserId > 0)
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Cannot Dispose Currently Assigned Asset\" class=\"btn btn-danger btn-sm disabled\">D</div></td>");
                            }
                            else if (item.Condition == "good" || item.Condition == "fair" || item.Condition == "repairs")
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Cannot Dispose Asset with the current condition\" class=\"btn btn-danger btn-sm disabled\">D</div></td>");
                            }
                            else
                            {
                                outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Dispose Asset\" class=\"btn btn-danger btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 900,'Asset_edit.aspx?dispose=true&new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">" + delName + "</div></td>");
                            }
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle + "\" class=\"btn btn-info btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 900,'Asset_edit.aspx?lost=true&new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">" + lostName + "</div></td>");

                            if (item.Condition != "repairs" && item.Condition != "damaged")
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Move Asset\" class=\"btn btn-success btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 420, 1200,'AssetMovement.aspx?id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">M</div></td>");
                            }
                            
                        }
                        else if ((Convert.ToBoolean(InStoreRoom && (item.Condition == "good" || item.Condition == "fair" || item.Condition == "repairs"))))
                        {

                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit Asset\" class=\"btn btn-primary btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 900,'Asset_edit.aspx?edit=true&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">E</div></td>");

                            if (item.CurrentUserId > 0 )
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Cannot Dispose Currently Assigned Asset\" class=\"btn btn-danger btn-sm disabled\">D</div></td>");
                            }
                            else if (item.Condition == "good" || item.Condition == "fair" || item.Condition == "repairs")
                            {
                                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Cannot Dispose Asset with the current condition\" class=\"btn btn-danger btn-sm disabled\">D</div></td>");
                            }
                            else
                            {
                                outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDisposeAsset_" + item.Id + "\" title=\"Dispose Asset\" class=\"btn btn-danger btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 900,'Asset_edit.aspx?dispose=true&new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">" + delName + "</div></td>");
                            }
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle + "\" class=\"btn btn-info btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 510, 900,'Asset_edit.aspx?lost=true&new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">" + lostName + "</div></td>");
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Move Asset\" class=\"btn btn-success btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 420, 1200,'AssetMovement.aspx?id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">M</div></td>");


                        }
                        else 
                        {
                            outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDelete_" + item.Id + "\" title\"" + delTitle + "\" class=\"btn btn-danger btn-sm\" onclick=\"OpenPopupDialog('divYesNo''" + delParam + "');\">" + delName + " </div></td>");
                            
                        }
                    }
                    else
                    {
                        outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit Asset\" class=\"btn btn-primary  btn-sm disabled\">E</div></td>");
                        outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDelete_" + item.Id + "\" title\"" + delTitle + "\" class=\"btn btn-warning btn-sm disabled\"</div></td>");
                        outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle + "\" class=\"btn btn-info btn-sm disabled\" </div></td>");
                    }


                    if (!Convert.ToBoolean(item.Disposed))
                    {
                        if(item.Condition == "repairs" || item.Condition == "damaged" )
                        {
                     
                        outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Cannot Move Damaged Asset or Assets that are still being repaired\" class=\"btn btn-success btn-sm disabled\">M</div></td>");
                        }
                    }
                    


                    if (item.AssetMovementHistory.Count > 0 || item.Lost == 1 || item.Disposed == 1)
                    {
                        outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMove_" + item.Id + "\" title=\"View Asset Movement History\" class=\"btn btn-warning btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 420, 1200,'AssetMovementHistory.aspx?id=" + item.Id + "','PopUpBase PopPrimary');\">H</div></td>");
                    }
                    else
                    {
                        outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMove_" + item.Id + "\" title=\"No History\" class=\"btn btn-warning  btn-sm disabled\">H</div></td>");

                    }
                }
                else
                {
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\"class=\"BtnOff\">E</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDelete_" + item.Id + "\"class=\"BtnOff\">D</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0\"><div id=\"btnLost_" + item.Id + "\"class=\"BtnOff\">L</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\"class=\"BtnOff\">M</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnMove_" + item.Id + "\"class=\"BtnOff\">H</div></td>");
                }

                outStr.Append("             <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding-left:3px;\"><div style=\"width:35px;\" id=\"btnInfo_" + item.Id + "\" title=\"Information\" class=\"btn btn-default btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 350, 700, 'AssetDetails.aspx?id=" + item.Id + "','PopUpBase PopPrimary');\">I</div></td>");

                outStr.Append("             </tr>");
                outStr.Append("         </table>");
                outStr.Append("     </td>");

                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\"><strong>" + item.AssetNo + "</strong></td>");
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.Category.ToUpper() + "</td>");
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.Description.ToUpper() + "</td>");
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.Condition.ToUpper() + "</td>");
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.SerialNo.ToUpper() + "</td>");

                

                 if (item.CurrentUserId == 0 && item.Lost == 0 && item.Disposed == 0 && item.StoreRoom ==1)
                {
                    outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\"><strong>" + "ASSET IN STORE ROOM" + "</strong></td>");
                }
                 else if (item.CurrentUserId >= 0 && item.Lost == 1 && item.Disposed == 0)
                {
                    outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + "ASSET IS LOST" + "</td>");
                }
                 else if (item.Disposed == 1)
                 {
                     outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + "ASSET IS DISPOSED" + "</td>");
                 }
                else if (item.CurrentUserId > 0 && item.Lost ==0)
                {
                    User thisOfficial = new User(item.CurrentUserId.ToString(), false, false);

                    if (thisOfficial.Id > 0)
                    {
                        outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">");

                        outStr.Append("         <div id=\"DIV_EXT_" + item.Id + "\" style=\"position:relative; padding-botton:2px; padding-top:2px;\" onmouseover=\"javascript:ShowHideDiv('DIV_INT_" + item.Id + "','block')\" onmouseout=\"javascript:ShowHideDiv('DIV_INT_" + item.Id + "','none')\"><strong>" + thisOfficial.FullName.ToUpper() + "</strong>"); 
                        outStr.Append("            <div id=\"DIV_INT_" + item.Id + "\" class=\"PopUpDate txt_black_12\" style=\"position:absolute; display:none; padding-left:2px; padding-top:2px;\">");

                        string Inner_Html = "        <span class=\"txt_blue_12\"><strong>" + thisOfficial.FullName.ToUpper() + "</strong></span> <br/>";

                        if (!string.IsNullOrEmpty(thisOfficial.Floor))
                        {
                            Inner_Html += "        <span class=\"txt_blue_11\"><strong>Floor : </strong>" + thisOfficial.Floor + "</span> <br/>";
                        }

                        if (!string.IsNullOrEmpty(thisOfficial.OfficeNo))
                        {
                            Inner_Html += "        <span class=\"txt_blue_11\"><strong>Office : </strong>" + thisOfficial.OfficeNo + "</span> <br/>";
                        }

                        if (!string.IsNullOrEmpty(thisOfficial.Telephone) && (thisOfficial.Mobile_Phone != "0"))
                        {
                            Inner_Html += "        <span class=\"txt_blue_11\">" + thisOfficial.Telephone + "</span> <br/>";
                        }

                        if (!string.IsNullOrEmpty(thisOfficial.Mobile_Phone) && (thisOfficial.Mobile_Phone != "0"))
                        {
                            Inner_Html += "        <span class=\"txt_blue_11\">" + thisOfficial.Mobile_Phone + "</span> <br/>";
                        }

                        //Region
                        if (thisOfficial.RegionId > 0)
                        {
                            Region thisORegion = new Region(thisOfficial.RegionId);
                            Inner_Html += "        <span class=\"txt_blue_11\"><strong>Region : </strong>" + thisORegion.RegionName + "</span> <br/>";
                        }

                        outStr.Append(Inner_Html);

                        outStr.Append("            </div>");
                        outStr.Append("         <div>");
                    }
                    else
                    {
                        outStr.Append("         <div id=\"DIV_EXT_" + item.Id + "\" style=\"position:relative; padding-botton:2px; padding-top:2px;\" onmouseover=\"javascript:ShowHideDiv('DIV_INT_" + item.Id + "','block')\" onmouseout=\"javascript:ShowHideDiv('DIV_INT_" + item.Id + "','none')\">" + thisOfficial.FullName);
                        outStr.Append("            <div id=\"DIV_INT_" + item.Id + "\" class=\"PopUpDate txt_black_12\" style=\"position:absolute; display:none; padding-left:2px; padding-top:2px;\">");

                        string Inner_Html = "        <span class=\"txt_red_10\"><strong>No records found for the employee</strong></span> <br/>";

                        outStr.Append(Inner_Html);

                        outStr.Append("            </div>");
                        outStr.Append("         <div>");
                    }
                }


                outStr.Append("      </td>");

                if (item.PurchasedValue > 0)
                {
                    outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.PurchasedValue.ToString("R #,##0.00;#,##0.00'- ';0 ").Replace(",", ".") + "</td>");
                }
                else
                {
                    outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\"></td>");
                }
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + Duration + "</td>");

                outStr.Append("  </tr>");

                wrapList.InnerHtml = outStr.ToString();
                e.Item.Controls.Add(wrapList);

            }
        }

        protected void btnExportToPDF_Click(object sender, EventArgs e)
        {
            buildAssetsList(Navigation.None);

            if (thisList.Listing.Count > 0)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        

                        StringBuilder sb = new StringBuilder();

                        //Header.
                        sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='1'>");
                        sb.Append("     <tr><td align='center'><img src=\"" + Server.MapPath("~/Common/Graphics/logo_gaut.png") + "\" width=\"160px\" height=\"60px\" alt=\"DCS\"/></td></tr>");
                        sb.Append("</table>");
                        sb.Append("<br />");

                        sb.Append("<table width='100%' bordercolor='#CD5C5C' border='0.5' cellspacing='0' cellpadding='2'>");
                        sb.Append("   <tr>");
                        sb.Append("     <td height='50px' bgcolor='#4682B4' align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 10px; color: #FFF; text-decoration: none;'><strong>ASSET NUMBER</strong></span></td>");
                        sb.Append("     <td height='50px' bgcolor='#4682B4' align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 10px; color: #FFF; text-decoration: none;'><strong>CATEGORY</strong></span></td>");
                        sb.Append("     <td height='50px' bgcolor='#4682B4' align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 10px; color: #FFF; text-decoration: none;'><strong>DESCRIPTION</strong></span></td>");
                        sb.Append("     <td height='50px' bgcolor='#4682B4' align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 10px; color: #FFF; text-decoration: none;'><strong>SERIAL NUMBER</strong></span></td>");
                        sb.Append("     <td height='50px' bgcolor='#4682B4' align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 10px; color: #FFF; text-decoration: none;'><strong> CURRENT USER</strong></span></td>");
                        sb.Append("     <td height='50px' bgcolor='#4682B4' align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 10px; color: #FFF; text-decoration: none;'><strong> VALUE </strong></span></td>");
                        sb.Append("   </tr>");

                        int count = 0;

                        foreach (var item in thisList.Listing)
                        {
                            User thisOfficial = new User(item.CurrentUserId.ToString(), false, false);
                            count++;
                            sb.Append("   <tr>");

                            sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'>" + item.AssetNo + "</span></td>");
                            sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'>" + item.Category + "</span></td>");
                            sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'>" + item.Description + "</span></td>");
                            sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'>" + item.SerialNo + "</span></td>");
                            if (item.CurrentUserId == 0)
                            {
                                sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'>" + "ICT STORE ROOM" + "</span></td>");
                            }
                            else if (!String.IsNullOrEmpty(thisOfficial.FullName))
                            {
                                sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'>" + thisOfficial.FullName.ToUpper() + "</span></td>");
                            }
                            else
                            {
                                sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'></span></td>");
                            }
                                sb.Append("     <td align='left'><span style='font-family: Arial, Helvetica, sans-serif; font-size: 8px; color: #000000; text-decoration: none;'>" + item.PurchasedValue + "</span></td>");

                            sb.Append("   </tr>");
                        }

                        sb.Append("</table>");

                        StringReader sr = new StringReader(sb.ToString());


                        Document pdfDoc = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
                        pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();

                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentType = "assetlist/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=assetlist_Report" + DateTime.Now + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();
                    }
                }
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            buildAssetsList(Navigation.None);

            if (thisList.Listing.Count > 0)
            {
                String fileName = "AssetList_Report" + DateTime.Now.ToString(("dd-MM-yyyy")) + ".doc";
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName + ".xls"));
                HttpContext.Current.Response.ContentType = "assetlist/ms-excel";

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();
                        TableRow row = new TableRow();

                        TableHeaderCell hcel0 = new TableHeaderCell();
                        hcel0.Text = "ASSET NUMBER";
                        row.Cells.Add(hcel0);

                        TableHeaderCell hceld = new TableHeaderCell();
                        hceld.Text = "CATEGORY";
                        row.Cells.Add(hceld);

                        TableHeaderCell hcell1 = new TableHeaderCell();
                        hcell1.Text = "DESCRIPTION";
                        row.Cells.Add(hcell1);

                        TableHeaderCell hcel2 = new TableHeaderCell();
                        hcel2.Text = "SERIAL NUMBER";
                        row.Cells.Add(hcel2);

                        TableHeaderCell hcel3 = new TableHeaderCell();
                        hcel3.Text = "CURRENT USER";
                        row.Cells.Add(hcel3);

                        TableHeaderCell hcel4 = new TableHeaderCell();
                        hcel4.Text = "VALUE";
                        row.Cells.Add(hcel4);

                        table.Rows.Add(row);

                        foreach (var item in thisList.Listing)
                        {
                            User thisOfficial = new User(item.CurrentUserId.ToString(), false, false);

                            TableRow row1 = new TableRow();
                            TableCell cellAssetNo = new TableCell();
                            cellAssetNo.Text = "" + item.AssetNo;

                            TableCell cellCategory = new TableCell();
                            cellCategory.Text = "" + item.Category;

                            TableCell cellDescription = new TableCell();
                            cellDescription.Text = "" + item.Description;

                            TableCell cellSerialNo = new TableCell();
                            cellSerialNo.Text = "" + item.SerialNo;


                            TableCell cellCurrentUserId = new TableCell();
                           
                            if (item.CurrentUserId > 0)
                            {
                                if (!string.IsNullOrEmpty(thisOfficial.FullName))
                                {
                                    cellCurrentUserId.Text = "" + thisOfficial.FullName.ToUpper();
                                }
                                cellCurrentUserId.Text = "";
                            }
                            else if (item.CurrentUserId == 0)
                            {
                                
                                cellCurrentUserId.Text = "" + "ICT STOREROOM";
                            }
                            
                            

                            TableCell cellPurchasedValue = new TableCell();
                            cellPurchasedValue.Text = "" + item.PurchasedValue;

                            row1.Cells.Add(cellAssetNo);
                            row1.Cells.Add(cellCategory);
                            row1.Cells.Add(cellDescription);
                            row1.Cells.Add(cellSerialNo);
                            row1.Cells.Add(cellCurrentUserId);
                            row1.Cells.Add(cellPurchasedValue);



                            table.Rows.Add(row1);
                        }

                        table.RenderControl(hw);

                        HttpContext.Current.Response.Write(sw.ToString());
                        HttpContext.Current.Response.End();
                    }
                }
            }
        }

        double CalculateTotalValue(AssetsList thisList, bool? Capex, bool? DisposedAsset)
        {
            double TotalAssetValue = 0;

            if (Capex.HasValue)
            {
                switch (Capex)
                {
                    case true:
                        TotalAssetValue = thisList.Listing.Where(x => x.PurchasedValue > 4999).Select(x => x.PurchasedValue).Sum();
                        break;

                    case false:
                        TotalAssetValue = thisList.Listing.Where(x => x.PurchasedValue <= 4999).Select(x => x.PurchasedValue).Sum();
                        break;
                }
            }
            else
            {
                TotalAssetValue = thisList.Listing.Select(x => x.PurchasedValue).Sum();
            }

            if (DisposedAsset.HasValue)
            {
                if (DisposedAsset.Value == true)
                {
                    TotalAssetValue = thisList.Listing.Where(x => x.Disposed == 1).Select(x => x.PurchasedValue).Sum();

                }

            }
            return TotalAssetValue;
        }

        public void DateDifference(int AssetId)
        {
            Assets thisAsset = new Assets(AssetId);

            if (thisAsset.DatePurchased < thisAsset.CreatedDate)
            {
                this.purchasedDate = thisAsset.DatePurchased;
                this.CurrentDate = DateTime.Now;
            }

            increment = 0;
            if (this.purchasedDate.Day > this.CurrentDate.Day)
            {
                increment = this.monthDay[this.purchasedDate.Month - 1];
            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(this.CurrentDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }

            if (increment != 0)
            {
                day = (this.CurrentDate.Day + increment) - this.purchasedDate.Day;
                increment = 1;
            }
            else
            {
                day = this.CurrentDate.Day - this.purchasedDate.Day;
            }

            if ((this.purchasedDate.Month + increment) > this.CurrentDate.Month)
            {
                this.month = (this.CurrentDate.Month + 12) - (this.purchasedDate.Month + increment);
                increment = 1;
            }
            else
            {
                this.month = (this.CurrentDate.Month) - (this.purchasedDate.Month + increment);
                increment = 0;
            }

            this.year = this.CurrentDate.Year - (this.purchasedDate.Year + increment);

            Duration = Convert.ToString(this.year + " Year(s), " + this.month + " month(s), " + this.day + " day(s)");

        }

        protected void f_sections_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = drpSearchBy.SelectedValue;

            switch (filter)
            {
                case "":
                    pnlSearchBy.Visible = false;
                    pnlPurchased.Visible = false;
                    pnlSearchBy.Visible = false;
                    pnlMoveDate.Visible = false;
                    break;

                case "persal":
                case "asset_no":
                case "serial":
                    pnlSearchBy.Visible = false;
                    pnlPurchased.Visible = false;
                    pnlMoveDate.Visible = false;
                    pnlSearchBy.Visible = true;
                    pnlSearchText.Visible = false;
                    f_searchby.Attributes.Remove("readonly");
                    break;

                case "employee":
                    pnlSearchBy.Visible = false;
                    pnlPurchased.Visible = false;
                    pnlMoveDate.Visible = false;
                    pnlSearchText.Visible = true;
                    f_SearchText.Attributes.Remove("readonly");
                    break;

                case "pur_date":
                    pnlPurchased.Visible = true;
                    pnlMoveDate.Visible = false;
                    pnlSearchBy.Visible = false;
                    pnlSearchText.Visible = false;
                    break;

                case "mov_date":
                    pnlMoveDate.Visible = true;
                    pnlPurchased.Visible = false;
                    pnlSearchBy.Visible = false;
                    pnlSearchText.Visible = false;
                    break;
            }
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
                buildResultFilterOptions();
           
                StringBuilder searchParameters = new StringBuilder();


                if (mustbeActive)
                {
                    searchParameters.Append("StartUp.htm?page=Asset_list.aspx?pg=" + PageCurr + "&OrderBy=" + drpSortBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories + "&SelAssetStatus=" + SelAssetStatus);
                }
                else if (LostRecords)
                {
                    searchParameters.Append("StartUp.htm?page=Asset_list.aspx?pg=" + PageCurr + "&OrderBy=" + drpSortBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories + "&SelAssetStatus=" + SelAssetStatus);
                }
                else if (thisDisposedRecords)
                {
                    searchParameters.Append("StartUp.htm?page=Asset_list.aspx?pg=" + PageCurr + "&OrderBy=" + drpSortBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories + "&SelAssetStatus=" + SelAssetStatus);
                }
                else if (InStoreRoom)
                {
                    searchParameters.Append("StartUp.htm?page=Asset_list.aspx?pg=" + PageCurr + "&OrderBy=" + drpSortBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories + "&SelAssetStatus=" + SelAssetStatus);
                }
                else
                {
                    searchParameters.Append("StartUp.htm?page=Asset_list.aspx?pg=" + PageCurr + "&OrderBy=" + drpSortBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories + "&SelAssetStatus=" + SelAssetStatus);
                }

                DateTime? pstartDate = null, pendDate = null, mStartDate = null, mEndDate = null;

                if ((!string.IsNullOrEmpty(drpSearchBy.SelectedValue)) && (!string.IsNullOrEmpty(f_searchby.Text.Trim())))
                {
                    searchParameters.Append("&SearchBy=" + drpSearchBy.SelectedValue);
                    searchParameters.Append("&SearchText=" + f_searchby.Text.Trim().Replace("&", "[TTTTT]"));
                }

                if ((!string.IsNullOrEmpty(drpSearchBy.SelectedValue)) && (!string.IsNullOrEmpty(this.f_SearchText.Text.Trim())))
                {

                    searchParameters.Append("&SearchBy=" + drpSearchBy.SelectedValue);
                    searchParameters.Append("&SearchText=" + f_SearchText.Text.Trim().Replace("&", "[TTTTT]"));                
                }
                if (!string.IsNullOrEmpty(this.f_fromDate.Text))
                {
                    searchParameters.Append("startDate=" + f_fromDate.Text);
                    pstartDate = Convert.ToDateTime(f_fromDate.Text);
                }

                if (!string.IsNullOrEmpty(this.f_ToDate.Text))
                {
                    searchParameters.Append("&endDate=" + f_ToDate.Text);
                    pendDate = Convert.ToDateTime(f_ToDate.Text);
                }
                if (!string.IsNullOrEmpty(this.f_mov_from.Text))
                {
                    searchParameters.Append("&mStartDate=" + f_mov_from.Text);
                    mStartDate = Convert.ToDateTime(f_mov_from.Text);
                }

                if (!string.IsNullOrEmpty(this.f_mov_To.Text))
                {
                    searchParameters.Append("&mEndDate=" + f_mov_To.Text);
                    mEndDate = Convert.ToDateTime(f_mov_To.Text);
                }

                //if (!string.IsNullOrEmpty(chkStatus.SelectedValue))
                //{
                //    searchParameters.Append("&SelAssetStatus=" + chkStatus.SelectedValue);
                  
                //}
            
                Response.Redirect(searchParameters.ToString());
           
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            StringBuilder searchParameters = new StringBuilder();

            if (mustbeActive)
            {
                searchParameters.Append("StartUp.htm?page=Asset_list.aspx?mustbeActive=" + mustbeActive);
            }
            else if (LostRecords)
            {
                searchParameters.Append("StartUp.htm?page=Asset_list.aspx?LostRecords=" + LostRecords);
            }
            else if (thisDisposedRecords)
            {
                searchParameters.Append("StartUp.htm?page=Asset_list.aspx?DisposedAssets=" + thisDisposedRecords);
            }
            else if (InStoreRoom)
            {
                searchParameters.Append("StartUp.htm?page=Asset_list.aspx?StoreRoom=" + InStoreRoom);
            }

            //searchParameters.Append("Asset_list.aspx?mustbeActive=true");
            Response.Redirect(searchParameters.ToString());
        }

        void PopulateDropDownOptions()
        {
            CategoryList categoryData = new CategoryList(true);
            chkCategories.DataSource = categoryData.Listing;
            chkCategories.DataTextField = "Description";
            chkCategories.DataValueField = "Description";
            chkCategories.DataBind();

            chkAssetCondition.DataSource = Gen.getConditionList();
            chkAssetCondition.DataTextField = "Key";
            chkAssetCondition.DataValueField = "Value";
            chkAssetCondition.DataBind();



            chkStatus.DataSource = Gen.getAssetStatusList();
            chkStatus.DataTextField = "Key";
            chkStatus.DataValueField = "Value";
            chkStatus.DataBind();
         
        }

        protected void buildResultFilterOptions()
        {
            selCategories = string.Empty;
            selConditions = string.Empty;
            SelAssetStatus = string.Empty;
            //Selected  Asset Condition
            foreach (System.Web.UI.WebControls.ListItem item in chkAssetCondition.Items)
            {
                if (item.Selected)
                {
                    selConditions += "" + item.Value + ",";
                }
            }

            if (selConditions.Length > 0) { selConditions = selConditions.Remove(selConditions.Length - 1, 1); }

            //Selected Asset Categories
            foreach (System.Web.UI.WebControls.ListItem item in chkCategories.Items)
            {
                if (item.Selected)
                {
                    selCategories += "" + item.Value + ",";
                }
            }

            if (selCategories.Length > 0) { selCategories = selCategories.Remove(selCategories.Length - 1, 1); }

            foreach (System.Web.UI.WebControls.ListItem item in chkStatus.Items)
            {
                if (item.Selected)
                {
                    SelAssetStatus += "" + item.Value + ",";
                }
            }

            if (SelAssetStatus.Length > 0) { SelAssetStatus = SelAssetStatus.Remove(SelAssetStatus.Length - 1, 1); }
        }

        void PopulateSelectedFilterOptions()
        {
          
            //Pupulate Categories
            if (!string.IsNullOrEmpty(selCategories))
            {
                string[] CategoryList = selCategories.Split(',');

                foreach (var itemValue in CategoryList)
                {
                    System.Web.UI.WebControls.ListItem listItem = chkCategories.Items.FindByValue(itemValue.Replace("'", ""));
                    if (listItem != null) listItem.Selected = true;
                }
            }

            //Pupulate Conditions
            if (!string.IsNullOrEmpty(selConditions))
            {
                string[] ConditionsList = selConditions.Split(',');

                foreach (var itemValue in ConditionsList)
                {
                    System.Web.UI.WebControls.ListItem listItem = chkAssetCondition.Items.FindByValue(itemValue.Replace("'", ""));
                    if (listItem != null) listItem.Selected = true;
                }
            }

            if (!string.IsNullOrEmpty(SelAssetStatus))
            {
                string[] AssetStatusList = SelAssetStatus.Split(',');

                foreach (var itemValue in AssetStatusList)
                {
                    System.Web.UI.WebControls.ListItem listItem = chkStatus.Items.FindByValue(itemValue.Replace("'", ""));
                    if (listItem != null) listItem.Selected = true;
                }
            }

          
        }

    
    }
}
