using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc.DAL;
using AssetManagement.Proc;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data;
using System.Globalization;
using System.Configuration;


namespace AssetManagement
{
    public partial class Asset_list : System.Web.UI.Page
    {

        public string thisURL = string.Empty;
        public string OrderBy = string.Empty;

        public bool? CapexRecords = null;
        public bool? DisposedRecords = null;
        public bool? LostRecords = null;

        public bool? awaitVerify = null;
        public bool? awaitApproval = null;

        public bool? passedCapex { get { return CapexRecords; } }
        public bool? passedDisposed { get { return DisposedRecords; } }
        public bool? passedLostAsset { get { return LostRecords; } }

        public string selDCSAssNo = string.Empty;
        public string selConditions = string.Empty;
        public string selCategories = string.Empty;
        public bool mustbeActive = false;
        
        Data dataObj = new Data();

        public int pageSize = 25;
        public int PageCurr = 0;
        public string OrderByType = string.Empty;
        public string selOrderBy = string.Empty, dateRangeType = "ld";
        public string purchaseStartDate = string.Empty, purchaseEndDate = string.Empty;

        public CultureInfo cultureInfo = new CultureInfo(ConfigurationManager.AppSettings["GlobalLocation"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.BodyTag.Attributes.Add("OnResize", "CenterDiv('divPopupDialog')");
            thisURL = Gen.GetCurrentPageName(Request.Url.Query, false);

            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]) && Gen.isSpecificType(Request.QueryString["startDate"], "date"))
            {
                purchaseStartDate = Convert.ToDateTime(Request.QueryString["startDate"]).ToString("yyyy-MM-dd");
            }

            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]) && Gen.isSpecificType(Request.QueryString["endDate"], "date"))
            {
                purchaseEndDate = Convert.ToDateTime(Request.QueryString["endDate"]).ToString("yyyy-MM-dd");
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

            if (!string.IsNullOrEmpty(Request.QueryString["OrderBy"])) { OrderBy = Request.QueryString["OrderBy"].ToString(); ViewState["SelectedOrder"] = OrderBy; }
            if (!string.IsNullOrEmpty(Request.QueryString["OrderType"]))
            {
                OrderByType = Request.QueryString["OrderType"];
                ViewState["SelectedOrderType"] = OrderByType;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["DisposedAssets"]) && Gen.isSpecificType(Request.QueryString["DisposedAssets"], "bool"))
            {
                DisposedRecords = Convert.ToBoolean(Request.QueryString["DisposedAssets"].ToString());
            }

            if (!string.IsNullOrEmpty(Request.QueryString["LostRecords"]) && Gen.isSpecificType(Request.QueryString["LostRecords"], "bool"))
            {
                LostRecords = Convert.ToBoolean(Request.QueryString["LostRecords"].ToString());
            }

            //if (!string.IsNullOrEmpty(Request.QueryString["awaitVerify"]) && Gen.isSpecificType(Request.QueryString["awaitVerify"], "bool"))
            //{
            //    awaitVerify = Convert.ToBoolean(Request.QueryString["awaitVerify"].ToString());
            //}

            //if (!string.IsNullOrEmpty(Request.QueryString["awaitApproval"]) && Gen.isSpecificType(Request.QueryString["awaitApproval"], "bool"))
            //{
            //    awaitApproval = Convert.ToBoolean(Request.QueryString["awaitApproval"].ToString());
            //}

            if (!string.IsNullOrEmpty(Request.QueryString["mustbeActive"]) && Gen.isSpecificType(Request.QueryString["mustbeActive"], "bool"))
            {
                mustbeActive = Convert.ToBoolean(Request.QueryString["mustbeActive"]);
            }

            //selDCSAssNo

            if (!string.IsNullOrEmpty(Request.QueryString["selDCSAssNo"]))
            {
                selDCSAssNo = Request.QueryString["selDCSAssNo"];
            }

            if (!Page.IsPostBack)
            {
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
                AssetsList thisList = new AssetsList(mustbeActive, selDCSAssNo, selConditions, selCategories, OrderBy, OrderByType.ToString(), DisposedRecords, LostRecords, awaitVerify, awaitApproval, purchaseStartDate, purchaseEndDate);
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

                    btnNext.Attributes.Remove("OnMouseOver");
                    btnNext.Attributes.Remove("OnMouseOut");
                    btnNext.Enabled = false;
                    btnNext.CssClass = "NAGIGATION_ITEM_DISABLE NoUnderLine";

                    btnPrev.Attributes.Remove("OnMouseOver");
                    btnPrev.Attributes.Remove("OnMouseOut");
                    btnPrev.Enabled = false;
                    btnPrev.CssClass = "NAGIGATION_ITEM_DISABLE NoUnderLine";

                    btnFirst.Attributes.Remove("OnMouseOver");
                    btnFirst.Attributes.Remove("OnMouseOut");
                    btnFirst.Enabled = false;
                    btnFirst.CssClass = "NAGIGATION_ITEM_DISABLE NoUnderLine";

                    btnLast.Attributes.Remove("OnMouseOver");
                    btnLast.Attributes.Remove("OnMouseOut");
                    btnLast.Enabled = false;
                    btnLast.CssClass = "NAGIGATION_ITEM_DISABLE NoUnderLine";

                    dvRecords.Visible = false;
                    dvNoRecords.Visible = true;
                    
                }
                else
                {
                    lblTotalPages.Text = pageData.PageCount.ToString();
                    txtPage.Text = (pageData.CurrentPageIndex + 1).ToString();
                    lblBalance.Text = String.Format(cultureInfo, "{0:C}", CalculateTotalValue(thisList, CapexRecords, DisposedRecords));
  
                    dvRecords.Visible = true;
                    dvNoRecords.Visible = false;

                    if (pageData.IsLastPage)
                    {
                        btnNext.Enabled = false;
                        btnNext.CssClass = "NAGIGATION_ITEM_DISABLE txt_black_12";

                        btnLast.Enabled = false;
                        btnLast.CssClass = "NAGIGATION_ITEM_DISABLE txt_black_12";
                    }
                    else
                    {
                        btnNext.Enabled = true;
                        btnNext.CssClass = "NAGIGATION_ITEM txt_white_12";

                        btnLast.Enabled = true;
                        btnLast.CssClass = "NAGIGATION_ITEM txt_white_12";
                    }

                    if (pageData.IsFirstPage)
                    {
                        btnFirst.Enabled = false;
                        btnFirst.CssClass = "NAGIGATION_ITEM_DISABLE txt_black_12";

                        btnPrev.Enabled = false;
                        btnPrev.CssClass = "NAGIGATION_ITEM_DISABLE txt_black_12";
                    }
                    else
                    {
                        btnPrev.Enabled = true;
                        btnPrev.CssClass = "NAGIGATION_ITEM txt_white_12";

                        btnFirst.Enabled = true;
                        btnFirst.CssClass = "NAGIGATION_ITEM txt_white_12";
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

            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Assets item = (Assets)e.Item.DataItem;       
                HtmlGenericControl wrapList = new HtmlGenericControl("div");
                StringBuilder outStr = new StringBuilder();

                String rowClass = "";

                rowClass = "txt_blue_cgothic_13";

                if (item.Active == 0|| item.Disposed == 1)
                {
                    rowClass = "txt_red_cgothic_13";                    
                }

                String delParam = string.Empty, delName = string.Empty, delTitle = string.Empty;
                if (!Convert.ToBoolean(item.Disposed))
                {
                    delParam = "ActionDispose.aspx?cmd=d&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate;
                    delName = "D";
                    delTitle = "Dispose Asset";
                }
                else
                {
                    delParam = "ActionDispose.aspx?cmd=u&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]pg=" + ViewState["SelectedPage"] + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate; 
                    delName = "U";
                    delTitle = "Un-Dispose Asset";
                }



                String lostParam = string.Empty, lostName = string.Empty, lostTitle = string.Empty;
                if (!Convert.ToBoolean(item.Lost))
                {
                    lostParam = "ActionLost.aspx?cmd=l&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate;
                    lostName = "L";
                    lostTitle = "Mark Asset As Lost";
                }
                else
                {
                    lostParam = "ActionLost.aspx?cmd=u&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]pg=" + ViewState["SelectedPage"] + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate;
                    lostName = "U";
                    lostTitle = "Un-Lost Asset";
                }

                outStr.Append("  <tr class=\"" + rowClass + "\">");
                outStr.Append("     <td align=\"center\" valign=\"middle\"  class=\"datagridRow\" bgcolor=\"#FFFFFF\">");
                outStr.Append("         <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                outStr.Append("	            <tr>");


                if (Cordinator || AssetMover)
                {
                    if (Cordinator)
                    {
                        if (!Convert.ToBoolean(item.Disposed) && !Convert.ToBoolean(item.Lost))
                        {
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit Asset\" class=\"BtnEditOff\" onmouseover=\"ChangeClass(this.id, 'BtnEdit')\" onmouseout=\"ChangeClass(this.id, 'BtnEditOff')\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 415, 900,'Asset_edit.aspx?id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">E</div></td>");
                            //outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit\" class=\"BtnEditOff\" onmouseover=\"ChangeClass(this.id, 'BtnEdit')\" onmouseout=\"ChangeClass(this.id, 'BtnEditOff')\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 380, 900,'Asset_edit.aspx?new=false&mustbeActive=" + mustbeActive + "&id=" + item.Id + "&showLost=" + LostRecords + "&tobeDispose=" + item.Disposed + "&pg=" + PageCurr + "&OrderBy=" + drpSoryBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories + "&startDate=" + purchaseStartDate + "&endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">E</div></td>");
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDelete_" + item.Id + "\" title=\"" + delTitle + "\" class=\"BtnDeleteOff\" onmouseover=\"ChangeClass(this.id, 'BtnDelete')\" onmouseout=\"ChangeClass(this.id, 'BtnDeleteOff')\" onclick=\"gotoURL('" + delParam + "');\">" + delName + " </div></td>");
                            //outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Move Asset\" class=\"BtnYellowOff\" onmouseover=\"ChangeClass(this.id, 'BtnYellow')\" onmouseout=\"ChangeClass(this.id, 'BtnYellowOff')\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 420, 1200,'AssetMovement.aspx?verify=true&new=false&id=" + item.Id + "&tobeDispose=" + item.Disposed + "','PopUpBase PopPrimary');\">M</div></td>");
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle + "\" class=\"BtnBlueOff\" onmouseover=\"ChangeClass(this.id, 'BtnBlue')\" onmouseout=\"ChangeClass(this.id, 'BtnBlueOff')\" onclick=\"gotoURL('" + lostParam + "');\">" + lostName + "</div></td>");
                        }
                        else
                        {
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\"class=\"BtnOff\">E</div></td>");
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnDelete_" + item.Id + "\" title=\"" + delTitle + "\" class=\"BtnDeleteOff\" onmouseover=\"ChangeClass(this.id, 'BtnDelete')\" onmouseout=\"ChangeClass(this.id, 'BtnDeleteOff')\" onclick=\"gotoURL('" + delParam + "');\">" + delName + " </div></td>");
                            outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle + "\" class=\"BtnBlueOff\" onmouseover=\"ChangeClass(this.id, 'BtnBlue')\" onmouseout=\"ChangeClass(this.id, 'BtnBlueOff')\" onclick=\"gotoURL('" + lostParam + "');\">" + lostName + "</div></td>");
                            //outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\"class=\"BtnOff\">M</div></td>");
                        }


                        //if (!Convert.ToBoolean(item.Lost))
                        //{
                            //outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnLost_" + item.Id + "\" title=\"" + lostTitle+ "\" class=\"BtnBlueOff\" onmouseover=\"ChangeClass(this.id, 'BtnBlue')\" onmouseout=\"ChangeClass(this.id, 'BtnBlueOff')\" onclick=\"gotoURL('" + lostParam + "');\">" + lostName + "</div></td>");
                        //}
                        //else
                        //{
                        //    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnLost_" + item.Id + "\"class=\"BtnOff\">L</div></td>");
                        //}
                    }
                    else
                    {
                        outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\"class=\"BtnOff\">E</div></td>");
                        outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnDelete_" + item.Id + "\"class=\"BtnOff\">D</div></td>");
                        outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnLost_" + item.Id + "\"class=\"BtnOff\">L</div></td>");
                    }


                    if (!Convert.ToBoolean(item.Disposed) && !Convert.ToBoolean(item.Lost))
                    {
                        //lostParam = "ActionLost.aspx?cmd=l&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + drpSoryBy.SelectedValue + "[TTTTT]OrderType=" + rblOrderType.SelectedValue + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate;

                        outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Move Asset\" class=\"BtnYellowOff\" onmouseover=\"ChangeClass(this.id, 'BtnYellow')\" onmouseout=\"ChangeClass(this.id, 'BtnYellowOff')\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 420, 1200,'AssetMovement.aspx?id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]pg=" + PageCurr + "[TTTTT]DisposedAssets=" + DisposedRecords + "[TTTTT]LostRecords=" + LostRecords + "[TTTTT]mustbeActive=" + mustbeActive + "[TTTTT]OrderBy=" + OrderBy + "[TTTTT]OrderType=" + OrderByType + "[TTTTT]selConditions=" + selConditions + "[TTTTT]selCategories=" + selCategories + "[TTTTT]startDate=" + purchaseStartDate + "[TTTTT]endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">M</div></td>");
                        //outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\" title=\"Move Asset\" class=\"BtnYellowOff\" onmouseover=\"ChangeClass(this.id, 'BtnYellow')\" onmouseout=\"ChangeClass(this.id, 'BtnYellowOff')\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 420, 1200,'AssetMovement.aspx?verify=true&new=false&mustbeActive=" + mustbeActive + "&id=" + item.Id + "&showLost=" + LostRecords + "&tobeDispose=" + item.Disposed + "&pg=" + PageCurr + "&OrderBy=" + drpSoryBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories + "&startDate=" + purchaseStartDate + "&endDate=" + purchaseEndDate + "','PopUpBase PopPrimary');\">M</div></td>");
                    }
                    else
                    {
                        outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\"class=\"BtnOff\">M</div></td>");
                    }



                    if (item.AssetMovementHistory.Count > 0)
                    {
                        outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMove_" + item.Id + "\" title=\"View Asset Movement History\" class=\"BtnEditOff\" onmouseover=\"ChangeClass(this.id, 'BtnEdit')\" onmouseout=\"ChangeClass(this.id, 'BtnEditOff')\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 420, 1200,'AssetMovementHistory.aspx?id=" + item.Id + "','PopUpBase PopPrimary');\">H</div></td>");
                    }
                    else
                    {
                        outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnMove_" + item.Id + "\"class=\"BtnOff\">H</div></td>");
                    }
                }
                else
                {
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\"class=\"BtnOff\">E</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnDelete_" + item.Id + "\"class=\"BtnOff\">D</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnLost_" + item.Id + "\"class=\"BtnOff\">L</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnMovement_" + item.Id + "\"class=\"BtnOff\">M</div></td>");
                    outStr.Append("<td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnMove_" + item.Id + "\"class=\"BtnOff\">H</div></td>");
                }

                outStr.Append("<td width=\"21\"  bgcolor=\"#FFFFFF\" align=\"left\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnInfo_" + item.Id + "\" style=\"text-align:center;\" title=\"Information\" class=\"BtnCloneOff\" onmouseover=\"ChangeClass(this.id, 'BtnClone')\" onmouseout=\"ChangeClass(this.id, 'BtnCloneOff')\"  onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 350, 700, 'AssetDetails.aspx?id=" + item.Id + "','PopUpBase PopPrimary');\">I</div></td>");                
                
                outStr.Append("             </tr>"); 
                outStr.Append("         </table>");
                outStr.Append("     </td>");

                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\"><strong>" + item.AssetNo + "</strong></td>");
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.Category + "</td>");
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.Description + "</td>");
                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.SerialNo + "</td>");                

                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">");

                if (item.CurrentUserId> 0)
                {
                    User thisOfficial = new User(item.CurrentUserId.ToString(), false, false);

                    if (thisOfficial.Id > 0)
                    {
                        //outStr.Append("      <span valign=\"top\">" + thisOfficial.FullName + "</span>");

                        outStr.Append("         <div id=\"DIV_EXT_" + item.Id + "\" style=\"position:relative; padding-botton:2px; padding-top:2px;\" onmouseover=\"javascript:ShowHideDiv('DIV_INT_" + item.Id + "','block')\" onmouseout=\"javascript:ShowHideDiv('DIV_INT_" + item.Id + "','none')\">" + thisOfficial.FullName);
                        outStr.Append("            <div id=\"DIV_INT_" + item.Id + "\" class=\"PopUpDate txt_black_12\" style=\"position:absolute; display:none; padding-left:2px; padding-top:2px;\">");

                        string Inner_Html = "        <span class=\"txt_blue_12\"><strong>" + thisOfficial.FullName + "</strong></span> <br/>";

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
                    outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">R" + item.PurchasedValue + "</td>");
                }
                else
                {
                    outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\"></td>");
                }

                outStr.Append("  </tr>");
                
                wrapList.InnerHtml = outStr.ToString();
                e.Item.Controls.Add(wrapList);
                                   
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

        protected void chkSelectAllCategory_CheckedChanged(object sender, EventArgs e)
        {
            foreach (System.Web.UI.WebControls.ListItem checkBox in chkCategories.Items)
            {
                checkBox.Selected = chkSelectAllCategories.Checked;
            }
        }

        protected void chkSelectAllCondition_CheckedChanged(object sender, EventArgs e)
        {
            foreach (System.Web.UI.WebControls.ListItem checkBox in chkAssetCondition.Items)
            {
                checkBox.Selected = chSelectAllConditions.Checked;
            }
        }

        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            buildResultFilterOptions();

            List<string> ErrorMessage = new List<string>();

            try
            {
                StringBuilder searchParameters = new StringBuilder();
                searchParameters.Append("Asset_list.aspx?mustbeActive=" + mustbeActive + "&LostRecords=" + LostRecords + "&DisposedAssets=" + DisposedRecords + "&pg=" + PageCurr + "&OrderBy=" + drpSoryBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue + "&selConditions=" + selConditions + "&selCategories=" + selCategories);

                if (!string.IsNullOrEmpty(f_trackNo.Text))
                {
                    searchParameters.Append("&selDCSAssNo=" + f_trackNo.Text);
                }

                DateTime? pstartDate = null, pendDate = null;

                if (!string.IsNullOrEmpty(this.f_loadedStartDate.Text))
                {
                    searchParameters.Append("&startDate=" + f_loadedStartDate.Text);
                    pstartDate = Convert.ToDateTime(f_loadedStartDate.Text);
                }

                if (!string.IsNullOrEmpty(this.f_loadedEndDate.Text))
                {
                    searchParameters.Append("&endDate=" + f_loadedEndDate.Text);
                    pendDate = Convert.ToDateTime(f_loadedEndDate.Text);
                }
                
                Response.Redirect(searchParameters.ToString());
            }
            finally { }
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            StringBuilder searchParameters = new StringBuilder();
            searchParameters.Append("Asset_list.aspx?mustbeActive=true");
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
        }

        protected void buildResultFilterOptions()
        {
            selCategories = string.Empty;
            selConditions = string.Empty;

            //Selected Service Asset Condition
            foreach (System.Web.UI.WebControls.ListItem item in chkAssetCondition.Items)
            {
                if (item.Selected)
                {
                    selConditions += "'" + item.Value + "',";
                }
            }

            if (selConditions.Length > 0) { selConditions = selConditions.Remove(selConditions.Length - 1, 1); }

            //Selected Asset Categories
            foreach (System.Web.UI.WebControls.ListItem item in chkCategories.Items)
            {
                if (item.Selected)
                {
                    selCategories += "'" + item.Value + "',";
                }
            }

            if (selCategories.Length > 0) { selCategories = selCategories.Remove(selCategories.Length - 1, 1); }
        }

        void PopulateSelectedFilterOptions()
        {
            //Pupulate Order Column
            if (!string.IsNullOrEmpty(selOrderBy))
            {
                drpSoryBy.SelectedValue = selOrderBy;
            }

            //Pupulate Order Type
            if (!string.IsNullOrEmpty(OrderByType.ToString()))
            {
                rblOrderType.SelectedValue = OrderByType.ToString();
            }

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

            //Populate Dates
            if (!string.IsNullOrEmpty(purchaseStartDate))
            {
                f_loadedStartDate.Text = purchaseStartDate;
            }

            if (!string.IsNullOrEmpty(purchaseEndDate))
            {
                f_loadedEndDate.Text = purchaseEndDate;
            }
            //f_loadedStartDate.Text
                //f_loadedEndDate.Text
        }

        //[System.Web.Script.Services.ScriptMethod()]
        //[System.Web.Services.WebMethod]
        //public static List<string> AssetNumbersAutoCompletionList(string prefixText, int count)
        //{
        //    List<string> RLS01DCSNumbers = AutoCompleteList.GetAssetsCompletionList(prefixText, count);
        //    return RLS01DCSNumbers;
        //}
    }
}