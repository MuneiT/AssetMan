using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc;
using System.Text;
using AssetManagement.Proc.DAL;

namespace AssetManagement
{
    public partial class AssetFilterOptions : System.Web.UI.Page
    {
        string filterPage = string.Empty;
        public bool? CapexRecords = null;
        public bool? DisposedRecords = null;
        public bool? awaitVerify = null;
        public bool? awaitApproval = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Capex"]) && Gen.isSpecificType(Request.QueryString["Capex"], "bool"))
            {
                CapexRecords = Convert.ToBoolean(Request.QueryString["Capex"].ToString());
            }

            if (!string.IsNullOrEmpty(Request.QueryString["DisposedAssets"]) && Gen.isSpecificType(Request.QueryString["DisposedAssets"], "bool"))
            {
                DisposedRecords = Convert.ToBoolean(Request.QueryString["DisposedAssets"].ToString());
            }

            if (!string.IsNullOrEmpty(Request.QueryString["awaitVerify"]) && Gen.isSpecificType(Request.QueryString["awaitVerify"], "bool"))
            {
                awaitVerify = Convert.ToBoolean(Request.QueryString["awaitVerify"].ToString());
            }

            if (!string.IsNullOrEmpty(Request.QueryString["awaitApproval"]) && Gen.isSpecificType(Request.QueryString["awaitApproval"], "bool"))
            {
                awaitApproval = Convert.ToBoolean(Request.QueryString["awaitApproval"].ToString());
            }


            if (!Page.IsPostBack)
            {
                PopulateDropDownOptions();
            }
        }

        protected void f_sections_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = drpSearchBy.SelectedValue;

            switch (filter)
            {
                case "":
                    pnlSearchBy.Visible = false;
                    //pnlPurchaseDate.Visible = false;
                    pnlDivision.Visible = false;
                    pnlRegions.Visible = false;
                    break;

                case "date_pur":
                    pnlSearchBy.Visible = false;
                    //pnlPurchaseDate.Visible = true;
                    pnlDivision.Visible = false;
                    pnlRegions.Visible = false;
                    break;

                case "region":
                    pnlSearchBy.Visible = false;
                    //pnlPurchaseDate.Visible = false;
                    pnlDivision.Visible = false;
                    pnlRegions.Visible = true;
                    break;

                case "div":
                    pnlSearchBy.Visible = false;
                    //pnlPurchaseDate.Visible = false;
                    pnlDivision.Visible = true;
                    pnlRegions.Visible = false;
                    break;

                case "asset_no":
                case "asset_v":             
                case "supplier":                
                case "persal":
                case "cat":
                case "type":
                    pnlSearchBy.Visible = true;
                    f_searchby.Attributes.Remove("readonly");
                    //pnlPurchaseDate.Visible = false;
                    pnlDivision.Visible = false;
                    pnlRegions.Visible = false;
                    break;
            }
        }

        protected void btnApplyFilters_Click(object sender, EventArgs e)
        {
            StringBuilder searchParameters = new StringBuilder();
            searchParameters.Append("Asset_list.aspx?OrderBy=" + drpSortBy.SelectedValue + "&OrderType=" + rblOrderType.SelectedValue);

            if (CapexRecords.HasValue)
            {
                searchParameters.Append("&Capex=" + CapexRecords);
            }

            if (DisposedRecords.HasValue)
            {
                searchParameters.Append("&DisposedAssets=" + DisposedRecords);
            }


            if (awaitVerify.HasValue)
            {
                searchParameters.Append("&awaitVerify=" + awaitVerify);
            }

            if (awaitApproval.HasValue)
            {
                searchParameters.Append("&awaitApproval=" + awaitApproval);
            }
            

            if ((!string.IsNullOrEmpty(drpSearchBy.SelectedValue)) && (!string.IsNullOrEmpty(f_searchby.Text.Trim())))
            {
                searchParameters.Append("&SearchBy=" + drpSearchBy.SelectedValue);
                searchParameters.Append("&SearchText=" + f_searchby.Text.Trim().Replace("&", "[TTTTT]"));
            }
            else if (pnlRegions.Visible == true)
            {
                searchParameters.Append("&SearchBy=" + drpSearchBy.SelectedValue);
                searchParameters.Append("&SearchText=" + this.drpRegion.SelectedItem.Text.Replace("&", "[TTTTT]"));
            }
            else if (this.pnlDivision.Visible == true)
            {
                searchParameters.Append("&SearchBy=" + drpSearchBy.SelectedValue);
                searchParameters.Append("&SearchText=" + this.drpDivision.SelectedItem.Text.Replace("&", "[TTTTT]"));
            }
            

                Gen.refreshParent(this, searchParameters.ToString());
        }


        protected void btnClearFilters_Click(object sender, EventArgs e)
        {
            StringBuilder searchParameters = new StringBuilder();
            searchParameters.Append("Asset_list.aspx?dummy=true");

            if (CapexRecords.HasValue)
            {
                if (CapexRecords.Value == true)
                {
                    searchParameters.Append("&Capex=" + CapexRecords);
                }
                else
                {
                    searchParameters.Append("&Capex=" + CapexRecords);
                }
               
            }
            


            if (DisposedRecords == true)
            {
                searchParameters.Append("&DisposedAssets=" + DisposedRecords);
            }
            


            if (awaitVerify == true)
            {
                searchParameters.Append("&awaitVerify=" + awaitVerify);
            }
           


            if (awaitApproval == true)
            {
                searchParameters.Append("&awaitApproval=" + awaitApproval);
            }
           
            Gen.refreshParent(this, searchParameters.ToString());
          

        }

        void PopulateDropDownOptions()
        {
            RegionsList regionData = new RegionsList("", "", "", 0, true);
            drpRegion.DataSource = regionData.Listing;
            drpRegion.DataTextField = "RegionName";
            drpRegion.DataValueField = "RegionName";
            drpRegion.DataBind();

            DivisionList divisionData = new DivisionList(0, true);
            drpDivision.DataSource = divisionData.Listing;
            drpDivision.DataTextField = "DivisionName";
            drpDivision.DataValueField = "DivisionName";
            drpDivision.DataBind();
        }

    }
}