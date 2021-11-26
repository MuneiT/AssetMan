using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssetManagement.Proc;
using AssetManagement.Proc.BL;
using AssetManagement.Proc.DAL;
using System.Configuration;
using System.Text;
using System.Globalization;

namespace AssetManagement
{
    public partial class Asset_edit : System.Web.UI.Page
    {
        public bool IsNew = false, showLostAsset = false, showDisposedAsset = false;
        public bool  mustbeActive = false;
        public int assetID = 0;
        public Assets assetEnrty;
        public string thisUrl = string.Empty;
        public int PageCurr = 0;
        public String nextPage;
        public bool IsToBeDisposed = false;
        public bool IsToBeLost = false;
        public bool IsToBeEdited = true;
        public bool LostCondition = false;
        //calculating asset duration.
        private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private DateTime purchasedDate;
        private DateTime CurrentDate;
        private int year;
        private int month;
        private int day;
        private int increment;
        Data dataObj = new Data();

        protected void Page_Load(object sender, EventArgs e)
        {
            thisUrl = Gen.GetCurrentPageName(Request.Url.Query, false);

            if (!string.IsNullOrEmpty(Request.QueryString["page"])) { nextPage = Request.QueryString["page"].Replace("[TTTTT]", "&"); }

            if (!string.IsNullOrEmpty(Request.QueryString["id"]) && Gen.isSpecificType(Request.QueryString["id"], "int"))
            {
                assetID = Convert.ToInt32(Request.QueryString["id"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["new"]) && Gen.isSpecificType(Request.QueryString["new"], "bool"))
            {
                IsNew = Convert.ToBoolean(Request.QueryString["new"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["dispose"]) && Gen.isSpecificType(Request.QueryString["dispose"], "bool"))
            {
                IsToBeDisposed = Convert.ToBoolean(Request.QueryString["dispose"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["lost"]) && Gen.isSpecificType(Request.QueryString["lost"], "bool"))
            {
                IsToBeLost = Convert.ToBoolean(Request.QueryString["lost"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["edit"]) && Gen.isSpecificType(Request.QueryString["edit"], "bool"))
            {
                IsToBeEdited = Convert.ToBoolean(Request.QueryString["edit"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["lostcondition"]) && Gen.isSpecificType(Request.QueryString["lostcondition"], "bool"))
            {
                LostCondition = Convert.ToBoolean(Request.QueryString["lostcondition"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["mustbeActive"]) && Gen.isSpecificType(Request.QueryString["mustbeActive"], "bool"))
            {
                mustbeActive = Convert.ToBoolean(Request.QueryString["mustbeActive"]);
            }


            


            if (!Page.IsPostBack)
            {
                PopulateDropDownOptions();
                assetEnrty = new Assets(assetID);
                Condition ConEntry = new Condition(assetID);
                if (!IsNew)
                {
                    //f_assetNo.Attributes.Add("readonly", "readonly");
                    if (assetEnrty.Id > 0)
                    {
                        if (assetEnrty.DatePurchased.Year > 1900)
                        {
                            f_date.Text = Convert.ToString(assetEnrty.DatePurchased.ToString("yyyy-MM-dd"));
                        }

                        f_assetNo.Text = assetEnrty.AssetNo;
                        f_description.Text = assetEnrty.Description;
                        f_assetSerial.Text = assetEnrty.SerialNo;
                        f_category.SelectedValue = assetEnrty.Category;

                        f_value.Text = ((double)assetEnrty.PurchasedValue).ToString("#,##0.00;#,##0.00'-';0 ");
                       

                        ddl_Condition.SelectedValue = assetEnrty.Condition;
                        //f_active.Checked = Convert.ToBoolean(assetEnrty.Active);
                        f_purchasedOrderNo.Text = assetEnrty.PurchasedOrderNo;

                        if (assetEnrty.LostDate.Year > 1900)
                        {
                            f_lostDate.Text = Convert.ToString(assetEnrty.LostDate.ToString("yyyy-MM-dd"));
                        }


                        divYesNo.Visible = false;
                        btnCommit.Visible = true;

                        DateDifference();

                    }
                }
                else
                {
                    divYesNo.Visible = false;
                    btnCommit.Visible = true;
                }

                if (IsToBeDisposed == true)
                {
                    divYesNo.Visible = true;
                    btnCommit.Visible = false;
                    btnDisposeYes.Visible = true;
                    btnDisposeNo.Visible = true;

                }
                else if (IsToBeLost == true)
                {
                    divLostYesNo.Visible = true;
                    btnCommit.Visible = false;
                    btnLoseYes.Visible = true;
                    btnLoseNo.Visible = true;
                }


                if (IsToBeEdited == false)
                {
                    f_assetNo.Attributes.Add("readonly", "readonly");
                    f_description.Attributes.Add("readonly", "readonly");
                    f_assetSerial.Attributes.Add("readonly", "readonly");
                    f_category.Enabled = false;
                    f_value.Attributes.Add("readonly", "readonly");
                    ddl_Condition.Enabled = false;
                    cldPurchaseDate.Enabled = false;
                    f_purchasedOrderNo.Attributes.Add("readonly", "readonly");
                }
                

                f_date.Attributes.Add("readonly", "readonly");
                f_Disposal.Attributes.Add("readonly", "readonly");

                if (IsToBeLost)
                {
                    

                    f_assetNo.Attributes.Add("readonly", "readonly");
                    cldPurchaseDate.Enabled = false;
                    f_purchasedOrderNo.Attributes.Add("readonly", "readonly");
                    f_description.Attributes.Add("readonly", "readonly");
                    f_assetSerial.Attributes.Add("readonly", "readonly");
                    f_category.Enabled = false;
                    f_value.Attributes.Add("readonly", "readonly");
                    
                    if (LostCondition)
                    {
                        ddl_Condition.Enabled = true;
                        pnlReason.Visible = true;
                        pnlLost.Visible = false;
                        divLostYesNo.Visible = false;
                        divUnlose.Visible = true;
                    }
                    else
                    {

                        ddl_Condition.Enabled = false;
                        pnlReason.Visible = false;
                        pnlLost.Visible = true;
                       
                    }

                        f_lostDate.Attributes.Add("readonly", "readonly");                

                }
                else if (IsToBeDisposed)
                {
                    pnlDisposal.Visible = true;

                    f_assetNo.Attributes.Add("readonly", "readonly");
                    cldPurchaseDate.Enabled = false;
                    f_purchasedOrderNo.Attributes.Add("readonly", "readonly");
                    f_description.Attributes.Add("readonly", "readonly");
                    f_assetSerial.Attributes.Add("readonly", "readonly");
                    f_category.Enabled = false;
                    f_value.Attributes.Add("readonly", "readonly");
                    //ddl_Condition.Enabled = false;
                    f_Disposal.Attributes.Add("readonly", "readonly");

                    if (ddl_Condition.SelectedValue == "good" || ddl_Condition.SelectedValue == "fair" || ddl_Condition.SelectedValue == "repairs")
                    {
                        btnDisposeYes.Enabled = false;
                   
                        btnDisposeYes.ToolTip = "Cannot Dispose this Asset";
                        btnDisposeYes.CssClass = "button button-red disabled";
                    }
                    else if (ddl_Condition.SelectedValue == "old" || ddl_Condition.SelectedValue == "damaged")
                    {
                        btnDisposeYes.Enabled = true;
                        //btnDisposeYes.ToolTip.Remove(0);
                        btnDisposeYes.CssClass = "button button-red";
                    }
                }

                
                //else
                //{
                //    divLostYesNo.Visible = false;
                //}
            }
        }

        public void DateDifference()
        {
            if (assetEnrty.DatePurchased < assetEnrty.CreatedDate)
            {
                this.purchasedDate = assetEnrty.DatePurchased;
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

            lblDuration.Text = Convert.ToString(this.year + " Year(s), " + this.month + " month(s), " + this.day + " day(s)");

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

        protected void btnDisposeYes_Click(object sender, EventArgs e)
        {
            try
            {
                DisposeData(true);
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

        protected void btnDisposeNo_Click(object sender, EventArgs e)
        {
            try
            {
                DisposeData(false);
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

        protected void btnLoseYes_Click(object sender, EventArgs e)
        {
            try
            {
                LoseData(true);
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

        protected void btnLoseNo_Click(object sender, EventArgs e)
        {
            try
            {
                LoseData(false);
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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                approveData(true);
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
            Assets existingAsset = new Assets(assetID);
            String prevAssignedOfficial = existingAsset.CurrentUserId.ToString();
            StringBuilder outStr = new StringBuilder();

           List<string> ErrorMessage = new List<string>();

           if (EntriesBL.ValidateCreateUpdateAsset(this.f_assetNo.Text, this.f_description.Text, this.f_assetSerial.Text, this.f_category.SelectedValue, this.f_date.Text, this.f_value.Text, this.ddl_Condition.SelectedValue, f_purchasedOrderNo.Text, IsNew)) 
           {
                String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];
                String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Assets thisAsset = new Assets();

                if (!string.IsNullOrEmpty(f_date.Text.Trim()))
                {
                    thisAsset.DatePurchased = Convert.ToDateTime(this.f_date.Text.Replace("'", "''"));
                }
                else
                {
                    thisAsset.DatePurchased = Convert.ToDateTime("1900-01-01 00:00:00");
                }




                if (!string.IsNullOrEmpty(this.f_value.Text) && Gen.isSpecificType(this.f_value.Text, "double"))
                {



                    string userInput = f_value.Text.Replace(",", "").Replace("R", "").Replace("r", "").Replace(".", "").Replace("','", "''").TrimStart('0');

                    double assetRec;

                    if (double.TryParse(userInput, out assetRec))
                    {
                        assetRec /= 100;
                        f_value.TextChanged -= f_value_TextChanged;
                        f_value.Text = string.Format(CultureInfo.CreateSpecificCulture("en-ZA"), "{0:C2}", assetRec);
                        f_value.TextChanged += f_value_TextChanged;



                        thisAsset.PurchasedValue = Convert.ToDouble(assetRec);
                     }


                }
                else
                {
                    thisAsset.PurchasedValue = 0.00;
                }

                thisAsset.AssetNo = Gen.RemoveWhitespace(this.f_assetNo.Text.Replace("'", "''"));
               
                thisAsset.PurchasedOrderNo = this.f_purchasedOrderNo.Text.Replace("'", "''");
                thisAsset.SerialNo = this.f_assetSerial.Text.Replace("'", "''");
                thisAsset.Category = this.f_category.SelectedValue.Replace("'", "''");
                thisAsset.Condition = this.ddl_Condition.SelectedValue.Replace("'", "''");
                thisAsset.CurrentUserId = existingAsset.CurrentUserId;

                if (thisAsset.CurrentUserId == 0)
                {
                    thisAsset.StoreRoom = Convert.ToInt32(1);
                }
                
               
               thisAsset.Description = this.f_description.Text.Replace("'", "''");
                thisAsset.Active = Convert.ToInt32(1);

              
               

                if (!IsNew)
                {
                    thisAsset.Id = assetID;
                    thisAsset.ModifiedBy = curUser;
                    thisAsset.ModifiedDate = Convert.ToDateTime(curDateTime);
                }
                else
                {
                    thisAsset.Guid = Gen.generateGUID();
                    thisAsset.CreatedBy = curUser;
                    thisAsset.CreatedDate = Convert.ToDateTime(curDateTime);
                    thisAsset.ModifiedBy = curUser;
                    thisAsset.ModifiedDate = Convert.ToDateTime(curDateTime);
                }

                String curRecID = string.Empty;

                String  CurrAsset = thisAsset.AssetNo;
                    if (IsNew)
                    {
                        if (!thisAsset.CheckIfExist(true))
                        {
                            curRecID = thisAsset.CreateUpdate(true);
                            btnCommit.Visible = false;
                        }
                    }
                    else
                    {
                        if (!thisAsset.CheckIfExistEdit(true, assetID, CurrAsset))
                        {
                            curRecID = thisAsset.CreateUpdate(false);
                            btnCommit.Visible = false;
                        }
                    }

                    if (!mustbeActive)
                    {
                        Gen.refreshParent(this, "Asset_List.aspx?StoreRoom=true");
                    }
                    else
                    {
                        Gen.refreshParent(this, "Asset_List.aspx?mustbeActive=true");
                    }
                   
                
            }
        }

        protected void DisposeData(bool reload)
        {
            List<string> ErrorMessage = new List<string>();


            if (EntriesBL.ValidateCreateUpdateAsset(this.f_assetNo.Text, this.f_description.Text, this.f_assetSerial.Text, this.f_category.SelectedValue, this.f_date.Text, this.f_value.Text, this.ddl_Condition.SelectedValue, f_purchasedOrderNo.Text, IsNew))
                {
                    Assets thisEntry = new Assets(assetID);

                    if (string.IsNullOrEmpty(f_Disposal.Text) && reload == true)
                    {

                        ErrorMessage.Add("* Disposal date cannot be empty.");
                        AppExceptions custExeption = new AppExceptions();
                        custExeption.ErrorMessage = ErrorMessage;
                        throw custExeption;
                    }
                    else if (!string.IsNullOrEmpty(f_Disposal.Text) && DateTime.Parse(f_Disposal.Text) < thisEntry.DatePurchased && reload == true)
                    {
                        ErrorMessage.Add("* Disposal Date cannot be before the Purchase Date....");
                        AppExceptions custExeption = new AppExceptions();
                        custExeption.ErrorMessage = ErrorMessage;
                        throw custExeption;
                    }
                    else if (!string.IsNullOrEmpty(f_Disposal.Text) && DateTime.Parse(f_Disposal.Text) > DateTime.Now && reload == true)
                    {
                        ErrorMessage.Add("* Dispose Date cannot be after current Date....");
                        AppExceptions custExeption = new AppExceptions();
                        custExeption.ErrorMessage = ErrorMessage;
                        throw custExeption;
                    }
                    //else if(ddl_Condition.SelectedValue == )
                    //{

                    //}
                    else
                    {
                        String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];
                        String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        Assets thisAsset = new Assets();
                        AssetMovementItem thisMove = new AssetMovementItem();

                        int curRec = 0;

                        if (reload )
                        {
                            if (!string.IsNullOrEmpty(f_date.Text.Trim()))
                            {
                                thisAsset.DatePurchased = Convert.ToDateTime(this.f_date.Text.Replace("'", "''"));
                            }
                            else
                            {
                                thisAsset.DatePurchased = Convert.ToDateTime("1900-01-01 00:00:00");
                            }

                            if (!string.IsNullOrEmpty(this.f_value.Text) && Gen.isSpecificType(this.f_value.Text, "double"))
                            {
                                thisAsset.PurchasedValue = Convert.ToDouble(this.f_value.Text.Replace("'", "''"));
                            }
                            else
                            {
                                thisAsset.PurchasedValue = 0.00;
                            }

                            thisAsset.AssetNo = this.f_assetNo.Text.Replace("'", "''");
                            thisAsset.SerialNo = this.f_assetSerial.Text.Replace("'", "''");
                            thisAsset.Category = this.f_category.SelectedValue.Replace("'", "''");
                            thisAsset.Condition = this.ddl_Condition.SelectedValue.Replace("'", "''");
                            thisAsset.Description = this.f_description.Text.Replace("'", "''");

                            thisAsset.PurchasedOrderNo = this.f_purchasedOrderNo.Text.Replace("'", "''");

                            if (!string.IsNullOrEmpty(this.f_Disposal.Text.Trim()))
                            {
                                thisAsset.DisposedDate = Convert.ToDateTime(this.f_Disposal.Text.Replace("'", "''"));
                            }
                            else
                            {
                                thisAsset.DisposedDate = Convert.ToDateTime("1900-01-01 00:00:00");
                            }
                            thisAsset.Id = assetID;
                            thisAsset.ModifiedBy = curUser;
                            thisAsset.ModifiedDate = Convert.ToDateTime(curDateTime);

                            thisAsset.Disposed = 1;
                            thisAsset.Active = 0;
                            thisAsset.Lost = 0;
                            thisAsset.StoreRoom = 0;

                            curRec = thisAsset.Lost;

                            String curRecID = thisAsset.CreateUpdate(IsNew);

                            thisMove.AssetId = assetID;
                            Assets prevRec = new Assets(assetID);

                            int PrevID;
                            PrevID = prevRec.CurrentUserId;

                            thisMove.LostAsset = 0;
                            thisMove.movedDate = Convert.ToDateTime(curDateTime);
                            thisMove.CreatedBy = curUser;
                            thisMove.CreatedDate = Convert.ToDateTime(curDateTime);
                            thisMove.ModifiedBy = curUser;
                            thisMove.ModifiedDate = Convert.ToDateTime(curDateTime);
                            curRecID = thisMove.CreateUpdate(PrevID, curRec);
                            Gen.refreshParent(this, "Asset_List.aspx?DisposedAssets=true");
                        }
                        else
                        {
                            thisAsset.Disposed = 0;
                            thisAsset.Active = 1;
                            thisAsset.Lost = 0;

                            Gen.refreshParent(this, "Asset_List.aspx?DisposedAssets=true");
                        }


                    }
               
            }
        }

        protected void f_value_TextChanged(object sender, EventArgs e)
        {

        }

        protected void LoseData(bool reload)
        {
            List<string> ErrorMessage = new List<string>();

            if (EntriesBL.ValidateCreateUpdateAsset(this.f_assetNo.Text, this.f_description.Text, this.f_assetSerial.Text, this.f_category.SelectedValue, this.f_date.Text, this.f_value.Text, this.ddl_Condition.SelectedValue, f_purchasedOrderNo.Text, IsNew))
            {
                Assets thisEntry = new Assets(assetID);

                //if (IsToBeLost)
                //{

                //}

                if (string.IsNullOrEmpty(this.f_lostDate.Text) && reload == true && thisEntry.Active == 1 && reload == true)
                {

                    ErrorMessage.Add("* Lost date cannot be empty.");
                    AppExceptions custExeption = new AppExceptions();
                    custExeption.ErrorMessage = ErrorMessage;
                    throw custExeption;
                }
                else if (!string.IsNullOrEmpty(f_lostDate.Text) && DateTime.Parse(f_lostDate.Text) < thisEntry.DatePurchased && reload == true)
                {
                    ErrorMessage.Add("* Lost Date cannot be before the Purchase Date....");
                    AppExceptions custExeption = new AppExceptions();
                    custExeption.ErrorMessage = ErrorMessage;
                    throw custExeption;
                }
                else if (!string.IsNullOrEmpty(f_lostDate.Text) && DateTime.Parse(f_lostDate.Text) > DateTime.Now && reload == true)
                {
                    ErrorMessage.Add("* Lost Date cannot be after current Date....");
                    AppExceptions custExeption = new AppExceptions();
                    custExeption.ErrorMessage = ErrorMessage;
                    throw custExeption;
                }
                else if(string.IsNullOrEmpty(this.f_Reason.Text) && LostCondition)
                {
                    ErrorMessage.Add("* Reason field cannot be empty....");
                    AppExceptions custExeption = new AppExceptions();
                    custExeption.ErrorMessage = ErrorMessage;
                    throw custExeption;
                }
                else
                {
                    String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];
                    String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    Assets thisAsset = new Assets();
                    Assets thisRec = new Assets(assetID);
                    AssetMovementItem thisMove = new AssetMovementItem();

                    StringBuilder outStr = new StringBuilder();
                    int curRec = 0;
                    String curRecID = string.Empty;

                    if (reload && thisRec.Active == 1)
                    {
                        if (!string.IsNullOrEmpty(f_date.Text.Trim()))
                        {
                            thisAsset.DatePurchased = Convert.ToDateTime(this.f_date.Text.Replace("'", "''"));
                        }
                        else
                        {
                            thisAsset.DatePurchased = Convert.ToDateTime("1900-01-01 00:00:00");
                        }

                        if (!string.IsNullOrEmpty(this.f_value.Text) && Gen.isSpecificType(this.f_value.Text, "double"))
                        {
                            thisAsset.PurchasedValue = Convert.ToDouble(this.f_value.Text.Replace("'", "''"));
                        }
                        else
                        {
                            thisAsset.PurchasedValue = 0.00;
                        }

                        thisAsset.AssetNo = this.f_assetNo.Text.Replace("'", "''");
                        thisAsset.SerialNo = this.f_assetSerial.Text.Replace("'", "''");
                        thisAsset.Category = this.f_category.SelectedValue.Replace("'", "''");
                        thisAsset.Condition = this.ddl_Condition.SelectedValue.Replace("'", "''");
                        thisAsset.Description = this.f_description.Text.Replace("'", "''");
                        thisAsset.CurrentUserId = thisRec.CurrentUserId;
                        thisAsset.PurchasedOrderNo = this.f_purchasedOrderNo.Text.Replace("'", "''");

                        //thisAsset.CapturedBy = Convert.ToInt32(Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["id"]);

                        thisAsset.Active = Convert.ToInt32(1);
                        thisAsset.Id = assetID;
                        thisAsset.ModifiedBy = curUser;
                        thisAsset.ModifiedDate = Convert.ToDateTime(curDateTime);

                        thisAsset.Disposed = 0;
                        thisAsset.Active = 0;
                        thisAsset.Lost = 1;



                        curRecID = thisAsset.CreateUpdate(IsNew);

                        thisMove.AssetId = assetID;
                        Assets prevRec = new Assets(assetID);

                        int PrevID;
                        PrevID = prevRec.CurrentUserId;

                        if (!string.IsNullOrEmpty(this.f_lostDate.Text.Trim()))
                        {
                            thisMove.LostDate = Convert.ToDateTime(this.f_lostDate.Text.Replace("'", "''"));
                        }
                        else
                        {
                            thisMove.LostDate = Convert.ToDateTime("1900-01-01 00:00:00");
                        }

                        curRec = thisAsset.Lost;
                        thisMove.movedDate = Convert.ToDateTime(curDateTime);
                        thisMove.CreatedBy = curUser;
                        thisMove.CreatedDate = Convert.ToDateTime(curDateTime);
                        thisMove.ModifiedBy = curUser;
                        thisMove.ModifiedDate = Convert.ToDateTime(curDateTime);
                        curRecID = thisMove.CreateUpdate(PrevID, curRec);

                        Gen.refreshParent(this, "Asset_List.aspx?mustbeActive=true");
                    }


                    else if(reload == true) 
                    {
                        //update table assets
                        thisAsset.AssetNo = this.f_assetNo.Text.Replace("'", "''");
                        thisAsset.SerialNo = this.f_assetSerial.Text.Replace("'", "''");
                        thisAsset.Category = this.f_category.SelectedValue.Replace("'", "''");
                        if (!string.IsNullOrEmpty(f_date.Text.Trim()))
                        {
                            thisAsset.DatePurchased = Convert.ToDateTime(this.f_date.Text.Replace("'", "''"));
                        }
                        else
                        {
                            thisAsset.DatePurchased = Convert.ToDateTime("1900-01-01 00:00:00");
                        }

                        thisAsset.Condition = this.ddl_Condition.SelectedValue.Replace("'", "''");
                        thisAsset.Description = this.f_description.Text.Replace("'", "''");
                        thisAsset.PurchasedOrderNo = this.f_purchasedOrderNo.Text.Replace("'", "''");
                        thisAsset.PurchasedValue = Convert.ToDouble(this.f_value.Text.Replace("'", "''"));
                        
                        thisAsset.Disposed = 0;
                        thisAsset.Active = 1;
                        thisAsset.Lost = 0;
                        thisAsset.StoreRoom = 1;
                        thisAsset.EmpName = "";
                        thisAsset.EmpSurname = "";
                        thisAsset.PersalNo = "";
                        //thisAsset.CurrentUserId = 0;
                        thisAsset.ModifiedBy = curUser;
                        thisAsset.ModifiedDate = Convert.ToDateTime(curDateTime);
                        thisAsset.Id = assetID;
                        curRecID = thisAsset.CreateUpdate(IsNew);

                        //insert into table movement history
                        thisMove.movementReason = this.f_Reason.Text.Replace("'", "''");

                        outStr.Clear();
                        outStr.Append("Insert Into tbl_asset_movement_history (AssetId, FoundAsset, MovementReason, movedDate,CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, Guid) ");
                        outStr.Append(" values (");
                        outStr.Append("'" + assetID + "',");
                        outStr.Append("'" + 1 + "', ");
                        outStr.Append("'" + thisMove.movementReason + "',");
                        outStr.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        outStr.Append("'" + curUser.Replace("'", "''") + "',");
                        outStr.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        outStr.Append("'" + curUser.Replace("'", "''") + "',");
                        outStr.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ");
                        outStr.Append("'" + Gen.generateGUID() + "' ");
                        outStr.Append(")");

                        dataObj.SqlQuery.CommandText = outStr.ToString();
                        dataObj.SetMySqlDbConn(1, dataObj.dbConnAssetMan);
                        dataObj.SqlQuery.ExecuteNonQuery();
                        dataObj.SetMySqlDbConn(0, dataObj.dbConnAssetMan);

                        
                    }

                    if(LostCondition && mustbeActive)
                    {
                        Gen.refreshParent(this, "Asset_List.aspx?mustbeActive=true");
                    }
                    else if (!LostCondition && !mustbeActive)
                    {
                        Gen.refreshParent(this, "Asset_List.aspx?StoreRoom=true");
                    }
                    else
                    {
                        Gen.refreshParent(this, "Asset_List.aspx?LostRecords=true"); 
                    }

                }
            }
        }

        protected void approveData(bool reload)
        {
            List<string> ErrorMessage = new List<string>();

            if (EntriesBL.ValidateCreateUpdateAsset(this.f_assetNo.Text, this.f_description.Text, this.f_assetSerial.Text, this.f_category.SelectedValue, this.f_date.Text, this.f_value.Text, this.ddl_Condition.SelectedValue, this.f_purchasedOrderNo.Text,IsNew))
            {
                String curUser = Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["name"] + " " + Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["surname"];
                String curDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Assets thisAsset = new Assets();

                if (!string.IsNullOrEmpty(f_date.Text.Trim()))
                {
                    thisAsset.DatePurchased = Convert.ToDateTime(this.f_date.Text.Replace("'", "''"));
                }
                else
                {
                    thisAsset.DatePurchased = Convert.ToDateTime("1900-01-01 00:00:00");
                }

                if (!string.IsNullOrEmpty(this.f_value.Text) && Gen.isSpecificType(this.f_value.Text, "double"))
                {
                    thisAsset.PurchasedValue = Convert.ToDouble(this.f_value.Text.Replace("'", "''"));
                }
                else
                {
                    thisAsset.PurchasedValue = 0.00;
                }

                thisAsset.AssetNo = this.f_assetNo.Text.Replace("'", "''");
                thisAsset.SerialNo = this.f_assetSerial.Text.Replace("'", "''");
                thisAsset.Category = this.f_category.SelectedValue.Replace("'", "''");
                thisAsset.Condition = this.ddl_Condition.SelectedValue.Replace("'", "''");
                thisAsset.Description = this.f_description.Text.Replace("'", "''");
                thisAsset.Active = Convert.ToInt32(1);
                thisAsset.PurchasedOrderNo = this.f_purchasedOrderNo.Text.Replace("'", "''");

                //thisAsset.CapturedBy = Convert.ToInt32(Request.Cookies[ConfigurationManager.AppSettings["CookieUser"]]["id"]);

                
                thisAsset.Id = assetID;
                thisAsset.ModifiedBy = curUser;
                thisAsset.ModifiedDate = Convert.ToDateTime(curDateTime);

                thisAsset.ToBeVerified = 0;
                thisAsset.ToBeApproved = 0;

                String curRecID = thisAsset.CreateUpdate(IsNew);
                Gen.refreshParent(this, "Asset_list.aspx?awaitApproval=true");
            }
        }

        void PopulateDropDownOptions()
        {
            CategoryList categoryData = new CategoryList(true);
            f_category.DataSource = categoryData.Listing;
            f_category.DataTextField = "Description";
            f_category.DataValueField = "Description";
            f_category.DataBind();
            f_category.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Category...", ""));

            ConditionsList ConditionData = new ConditionsList(true);
            ddl_Condition.DataSource = ConditionData.Listing;
            ddl_Condition.DataTextField = "Description";
            ddl_Condition.DataValueField = "Description";
            ddl_Condition.DataBind();
            ddl_Condition.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Condition...", ""));


        }

        protected void ddl_Condition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Condition.SelectedValue == "" || ddl_Condition.SelectedValue == "good" || ddl_Condition.SelectedValue == "fair" || ddl_Condition.SelectedValue == "repairs")
            {
                btnDisposeYes.Enabled = false;
                btnDisposeYes.ToolTip = "Cannot Dispose this Asset";
                btnDisposeYes.CssClass = "button button-red disabled";
            }
            else if (ddl_Condition.SelectedValue == "old" || ddl_Condition.SelectedValue == "damaged")
            {
                btnDisposeYes.Enabled = true;
                //btnDisposeNo.Visible = true;
                //btnCommit.Visible = false;
                //pnlDisposal.Visible = true;
                //divYesNo.Visible = true;

                btnDisposeYes.CssClass = "button button-red";
            }
        }

         
        
    }
}
