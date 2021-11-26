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

namespace AssetManagement
{
    public partial class AssetCategoryList : System.Web.UI.Page
    {
        public string thisURL = string.Empty;
        public string OrderBy = string.Empty;
        public string SearchBy = string.Empty;
        public string SearchText = string.Empty;

        public int pageSize = 25;
        public int PageCurr = 0;
        public int OrderByType = 0;

        public CategoryList thisList;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.BodyTag.Attributes.Add("OnResize", "CenterDiv('divPopupDialog')");
            thisURL = Gen.GetCurrentPageName(Request.Url.Query, false);

            if (!string.IsNullOrEmpty(Request.QueryString["pg"]) && Gen.isSpecificType(Request.QueryString["pg"], "int"))
            {
                PageCurr = Convert.ToInt32(Request.QueryString["pg"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["SearchBy"])) { SearchBy = Request.QueryString["SearchBy"].ToString(); ViewState["selectedSearchColumn"] = SearchBy; }
            if (!string.IsNullOrEmpty(Request.QueryString["SearchText"])) { SearchText = Request.QueryString["SearchText"].ToString(); ViewState["selectedSearchText"] = SearchText; }
            if (!string.IsNullOrEmpty(Request.QueryString["OrderBy"])) { OrderBy = Request.QueryString["OrderBy"].ToString(); ViewState["SelectedOrder"] = OrderBy; }
            if (!string.IsNullOrEmpty(Request.QueryString["OrderType"]) && Gen.isSpecificType(Request.QueryString["OrderType"], "int"))
            {
                OrderByType = Convert.ToInt32(Request.QueryString["OrderType"]);
                ViewState["SelectedOrderType"] = OrderByType;
            }


            if (!Page.IsPostBack)
            {
                buildAssetCategoryList(Navigation.None);
            }


        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            buildAssetCategoryList(Navigation.Previous);

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            buildAssetCategoryList(Navigation.Next);

        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            buildAssetCategoryList(Navigation.Last);

        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            buildAssetCategoryList(Navigation.First);
        }

        private void buildAssetCategoryList(Navigation navigation)
        {
            try
            {
                 thisList = new CategoryList(false);

                PagedDataSource pageData = new PagedDataSource();
                pageData.DataSource = thisList.Listing;
                pageData.AllowPaging = true;
                pageData.PageSize = pageSize;

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
                PageCurr = (int)ViewState["SelectedPage"];
                pageData.CurrentPageIndex = NowViewing;

                rptList.DataSource = pageData;
                rptList.DataBind();

                if (pageData.Count > 0)
                {

                    lblTotalPages.Text = pageData.PageCount.ToString();
                    txtPage.Text = (pageData.CurrentPageIndex + 1).ToString();

                    if (pageData.IsLastPage)
                    {
                        btnNext.CssClass = "btn btn-primary btn-sm disabled";
                        btnLast.CssClass = "btn btn-primary btn-sm disabled";
                    }
                    else
                    {
                        btnNext.CssClass = "btn btn-primary btn-sm";
                        btnLast.CssClass = "btn btn-primary btn-sm";
                    }

                    if (pageData.IsFirstPage)
                    {
                        btnFirst.CssClass = "btn btn-primary btn-sm disabled";
                        btnPrev.CssClass = "btn btn-primary btn-sm disabled";
                    }
                    else
                    {
                        btnPrev.CssClass = "btn btn-primary btn-sm";
                        btnFirst.CssClass = "btn btn-primary btn-sm";
                    }

                    dvRecords.Visible = true;
                    dvNoRecords.Visible = false;
                }
                else
                {
                    lblTotalPages.Text = "0";
                    txtPage.Text = "0";

                    btnNext.CssClass = "btn btn-primary btn-sm disabled";
                    btnPrev.CssClass = "btn btn-primary btn-sm disabled";
                    btnFirst.CssClass = "btn btn-primary btn-sm disabled";
                    btnLast.CssClass = "btn btn-primary btn-sm disabled";

                    dvRecords.Visible = false;
                    dvNoRecords.Visible = true;
                }
            }
            catch (Exception io)
            {
                //Response.Write(io.Message.ToString());
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
            if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CategoryItem item = (CategoryItem)e.Item.DataItem;

                HtmlGenericControl wrapList = new HtmlGenericControl("div");
                StringBuilder outStr = new StringBuilder();

                String rowClass = "";

                rowClass = "txt_blue_cgothic_13";

                if (!item.Active)
                {
                    rowClass = "txt_red_cgothic_13";
                }


                String delParam = string.Empty, delName = string.Empty, delTitle = string.Empty;

                if (!Convert.ToBoolean(item.Active))
                {
                    delParam = "deleteEntry.aspx?inclD=false&cmd=u&tableName=tbl_category&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]pg=" + ViewState["SelectedPage"];
                    delName = "A";
                    delTitle = "Activate";
                }
                else
                {
                    delParam = "deleteEntry.aspx?inclD=false&cmd=d&tableName=tbl_category&id=" + item.Id + "&page=" + thisURL + "?typ=app[TTTTT]pg=" + ViewState["SelectedPage"];
                    delName = "D";
                    delTitle = "De-Activate";
                }

                outStr.Append("  <tr class=\"" + rowClass + "\">");
                outStr.Append("     <td align=\"center\" class=\"datagridRow\" valign=\"middle\"  bgcolor=\"#FFFFFF\">");
                outStr.Append("         <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                outStr.Append("	            <tr>");
                if (Convert.ToBoolean(item.Active))
                {
                    outStr.Append("		            <td width=\"21\"  bgcolor=\"#FFFFFF\" align=\"left\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit\" class=\"btn btn-success btn-sm\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 150, 550, 'AssetCategoryAddEdit.aspx?new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">E</div></td>");
                }
                else
                {
                    outStr.Append("	                <td width=\"21\"  bgcolor=\"#FFFFFF\" align=\"left\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Record currently disabled\" class=\"btn btn-success btn-sm disabled\">E</div></td>");
                }
                outStr.Append("		            <td width=\"21\"  bgcolor=\"#FFFFFF\" align=\"left\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnDelete_" + item.Id + "\" title=\"" + delTitle + "\" class=\"btn btn-danger btn-sm\" onclick=\"gotoURL('" + delParam + "');\">" + delName + "</div></td>");

                //outStr.Append("		            <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 1px 0 0;\"><div id=\"btnEdit_" + item.Id + "\" title=\"Edit\" class=\"BtnEditOff\" onmouseover=\"ChangeClass(this.id, 'BtnEdit')\" onmouseout=\"ChangeClass(this.id, 'BtnEditOff')\" onclick=\"OpenPopupDialog('divPopupDialog', 'ifOptions', 150, 550, 'AssetCategoryAddEdit.aspx?new=false&id=" + item.Id + "','PopUpBase PopPrimary');\">E</div></td>");
                //outStr.Append("		            <td width=\"21\" align=\"center\" valign=\"middle\" style=\"padding:0 0 0 0;\"><div id=\"btnDiable_" + item.Id + "\" title=\"" + delTitle + "\" class=\"BtnDeleteOff\" onmouseover=\"ChangeClass(this.id, 'BtnDelete')\" onmouseout=\"ChangeClass(this.id, 'BtnDeleteOff')\" onclick=\"gotoURL('" + delParam + "');\">" + delName + " </div></td>");
                outStr.Append("             </tr>");
                outStr.Append("         </table>");
                outStr.Append("     </td>");

                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + item.Description + "</td>");

                StringBuilder auditUser = new StringBuilder();

                if (!string.IsNullOrEmpty(item.CreatedBy))
                {
                    auditUser.Append(item.CreatedBy);
                }

                if (item.CreatedDate.Year != 1900)
                {
                    auditUser.Append(" @ " + item.CreatedDate.ToString("yyyy-MM-dd HH:mm"));
                }

                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + auditUser + "</td>");



                auditUser.Clear();
                if (!string.IsNullOrEmpty(item.ModifiedBy))
                {
                    auditUser.Append(item.ModifiedBy);
                }

                if (item.ModifiedDate.Year != 1900)
                {
                    auditUser.Append(" @ " + item.ModifiedDate.ToString("yyyy-MM-dd HH:mm"));
                }

                outStr.Append("      <td align=\"left\" class=\"datagridRow\" valign=\"top\" bgcolor=\"#FFFFFF\">" + auditUser + "</td>");
                outStr.Append("  </tr>");

                wrapList.InnerHtml = outStr.ToString();
                e.Item.Controls.Add(wrapList);

            }
        }
    }
}