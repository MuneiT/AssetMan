using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using AssetManagement.Proc;
using AssetManagement.Proc.DAL;

namespace AssetManagement
{
    public partial class AssetDetails : System.Web.UI.Page
    {

        public int AssetID = 0;
        public CultureInfo cultureInfo = new CultureInfo("en-ZA");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Gen.isSpecificType(Request.QueryString["id"], "int"))
            {
                AssetID = Convert.ToInt32(Request.QueryString["id"]);
            }


            Assets thisAsset = new Assets(AssetID);

            if (thisAsset.Id !=0)
            {
                if (thisAsset.CurrentUserId > 0 && thisAsset.Lost == 0)
                {
                    lblEmployee.Text = thisAsset.EmpName.ToString() + "   " + thisAsset.EmpSurname.ToString();
                }
                else if (thisAsset.StoreRoom == 1 && thisAsset.Disposed==0)
                {
                    lblEmployee.Text = "ASSET IN STORE";
                }
                else if (thisAsset.Disposed == 1)
                {
                    lblEmployee.Text = "ASSET IS DISPOSED";
                }
                else if (thisAsset.Lost == 1)
                {
                    lblEmployee.Text = "ASSET IS LOST";
                }

                

                lblAssetNumber.Text = thisAsset.AssetNo;

                if (thisAsset.DatePurchased.Year != 1900)
                {
                    lblDatePurcased.Text = thisAsset.DatePurchased.ToString("yyyy-MM-dd");
                }


                lblOrderNumber.Text = thisAsset.PurchasedOrderNo;
                lblAssetDescription.Text = thisAsset.Description;
                lblSerialNumber.Text = thisAsset.SerialNo;
                lblAssetCategory.Text = thisAsset.Category;
                lblCondition.Text = thisAsset.Condition.ToUpper();
                
                
                lblValue.Text = ((decimal)thisAsset.PurchasedValue).ToString("R #,##0.00;#,##0.00'-  R'; R 0.00");
                
            }
        }
    }
}