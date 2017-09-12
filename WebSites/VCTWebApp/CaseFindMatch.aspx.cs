using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWeb.Core.Domain;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using VCTWebApp.Resources;
using VCTWebApp.Web;
using System.Web.UI.WebControls;
using System.Globalization;

namespace VCTWebApp.Shell.Views
{
    public partial class CaseFindMatch : Microsoft.Practices.CompositeWeb.Web.UI.Page, ICaseFindMatchView
    {
        #region Instance Variables

        private CaseFindMatchPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private VCTWeb.Core.Domain.Helper helper = new VCTWeb.Core.Domain.Helper();
        private Security security = null;

        #endregion

        #region Private Properties

        private bool CanView
        {
            get
            {
                return ViewState[Common.CAN_VIEW] != null ? (bool)ViewState[Common.CAN_VIEW] : false;
            }
            set
            {
                ViewState[Common.CAN_VIEW] = value;
            }
        }

        #endregion

        #region Init/Page Load



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!this.IsPostBack)
                {
                    //this.GetPendingRequestsData();
                    this.AuthorizedPage();
                    this.LocalizePage();
                    //this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    this.DisplayMessageForMissingMasters();
                    if (Session["CaseIdToSearch"] != null)
                    {
                        this.Presenter.OnViewInitialized();
                    }
                }
            }
            catch (SqlException ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Create New Presenter

        [CreateNew]
        public CaseFindMatchPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }

        #endregion

        #region ICaseFindMatchView Members

        public List<KitSearchResult> KitSearchResultList
        {
            set
            {
                this.rptMain.DataSource = value;
                this.rptMain.DataBind();

                ViewState["KitSearchResultData"] = value;
            }
            get
            {
                return ViewState["KitSearchResultData"] as List<KitSearchResult>;
            }
        }

        public long SelectedCaseId
        {
            get
            {
                if (Session["CaseIdToSearch"] != null)
                {
                    return Convert.ToInt64(Session["CaseIdToSearch"]);
                }
                else
                    return 0;
            }
        }

        public int SearchQuantity
        {
            get
            {
                if (Session["QuantityToSearch"] != null)
                {
                    return Convert.ToInt32(Session["QuantityToSearch"]);
                }
                else
                    return 0;
            }
        }

        public int SelectedLocationId { get; set; }
        public int RequestedQuantity { get; set; }

        public KitSearchResult kitSearchResultForRequestedLocation
        {
            set
            {
                ViewState["kitSearchResultForRequestedLocation"] = value;
            }

            get
            {
                return ViewState["kitSearchResultForRequestedLocation"] as KitSearchResult;
            }
        }

        public KitSearchResult kitSearchResultForShipToLocation
        {
            set
            {
                ViewState["kitSearchResultForShipToLocation"] = value;
            }

            get
            {
                return ViewState["kitSearchResultForShipToLocation"] as KitSearchResult;
            }
        }

        #endregion

        #region Event Handlers

        protected void SlideShowImage1_Click(object sender, EventArgs e)
        {
            //Response.Redirect("~/LocateKit.aspx");
            if (Session["CaseIdToSearch"] != null)
            {
                string sKitMapInfo = PrepareKitInfoForMap();
                Session["KitMapInfo"] = sKitMapInfo;

                // Response.Redirect("LocateKit.aspx");
                string str = "<script type='text/javascript'>window.open('" + "LocateKit.aspx" + "','_blank','top=0,left=0,width=1000,height=700');</script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Locate Kit", str);
            }
        }

        protected void rptMain_ItemCommand(Object Sender, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "SendRequest")
                {
                    if (String.IsNullOrEmpty(hdnStatus.Value))
                    {
                        int itemIndex = int.Parse(e.CommandArgument.ToString());
                        KitSearchResult objKSR = KitSearchResultList[itemIndex];
                        this.SelectedLocationId = objKSR.BranchId;
                        //int excess = Convert.ToInt32(objKSR.Excess);
                        //int quantity = objKSR.Quantity;
                        this.RequestedQuantity = (Convert.ToInt32(objKSR.Excess) >= objKSR.Quantity) ? objKSR.Quantity : Convert.ToInt32(objKSR.Excess);
                        string caseNumberCreated = string.Empty;
                        Constants.ResultStatus resultStatus = this.Presenter.SendRequest(out caseNumberCreated);
                        if (resultStatus == Constants.ResultStatus.RequestSent)
                        {
                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgNewInternalRequestCreated")) + caseNumberCreated + "</font>";
                            //if (Session["LoggedInRole"] != null)
                            //{
                            //    if (Session["LoggedInRole"].ToString().Equals("LIM", StringComparison.OrdinalIgnoreCase))
                            //    {
                            //        //Session["LoggedInSalesOffice"] = ddlSalesOffice.SelectedItem.Text;
                            //        Response.Redirect("~/CreateRequest.aspx");
                            //    }
                            //    if (Session["LoggedInRole"].ToString().Equals("RIM", StringComparison.OrdinalIgnoreCase))
                            //        Response.Redirect("~/RegionalOfficeRequest.aspx");
                            //    if (Session["LoggedInRole"].ToString().Equals("SP", StringComparison.OrdinalIgnoreCase))
                            //        Response.Redirect("~/SalesOfficeRequest.aspx");
                            //    else
                            //        Response.Redirect("~/Default.aspx");
                            //}
                            //SlideShowImage1.Enabled = false;
                            btnViewMap.Visible = false;
                            foreach (RepeaterItem item in rptMain.Items)
                            {
                                Button btnSendRequest = item.FindControl("btnSendRequest") as Button;
                                if (btnSendRequest != null)
                                {
                                    btnSendRequest.Visible = false;
                                }
                            }
                            Image imgCheck = rptMain.Items[itemIndex].FindControl("imgCheck") as Image;
                            if (imgCheck != null)
                            {
                                imgCheck.Visible = true;
                            }
                            this.hdnStatus.Value = "RequestSent";
                        }
                        else
                        {
                            this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgErrorSendingRequest"));
                        }
                    }
                    else
                    {
                        lblError.Text = vctResource.GetString("msgAlreadySentForConfirmation");
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.Compare(Common.MSG_VAL_EXISTS, ex.Message.Trim(), true, CultureInfo.InvariantCulture) == 0 || string.Compare(Common.MSG_BADGE_EXISTS, ex.Message.Trim(), true, CultureInfo.InvariantCulture) == 0)
                {
                    this.lblError.Text = string.Format(CultureInfo.InvariantCulture, this.vctResource.GetString(ex.Message), this.vctResource.GetString("mnuCreateRequest"));
                }
                else
                {
                    throw ex;
                }
            }
        }

        protected void rptMain_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Button btnSendRequest = (Button)e.Item.FindControl("btnSendRequest");
                    KitSearchResult objKSR = (KitSearchResult)e.Item.DataItem;
                    if (objKSR.Excess >= objKSR.Quantity)
                    {
                        btnSendRequest.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgSendRequestConfirm") + "')");
                    }
                    else
                    {
                        btnSendRequest.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgSendRequestConfirmExcessQuantity") + "')");
                    }
                    Image imgShipToInfo = (Image)e.Item.FindControl("imgShipToInfo");
                    imgShipToInfo.ToolTip = objKSR.ShipToCustomerAdd1.Trim() + Environment.NewLine + objKSR.ShipToCustomerAdd2.Trim();
                    Image imgExcessInfo = (Image)e.Item.FindControl("imgExcessInfo");
                    imgExcessInfo.ToolTip =
                        "Perfect Excess Count: " + objKSR.PerfectExcess.ToString() + Environment.NewLine +
                        "Partial Excess Count: " + objKSR.PartialExcess.ToString();
                    Image imgBranchInfo = (Image)e.Item.FindControl("imgBranchInfo");
                    imgBranchInfo.ToolTip = objKSR.BranchAddress + Environment.NewLine + objKSR.Address2;
                    lblHeader.Text = vctResource.GetString("mnuSearchResult") + ": " + objKSR.KitName;
                }
            }
            catch (Exception ex)
            {
                this.helper.LogError(ex);
            }

        }

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("InventoryAssignment"))
            {
                CanView = security.HasPermission("InventoryAssignment");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuSearchResult");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.btnViewMap.Text = vctResource.GetString("btnViewMap");
                //this.lbPendingRequests.Text = vctResource.GetString("lbPendingRequests");

                //this.btnNew.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisplayMessageForMissingMasters()
        {
            //if (this.TempRoleList.Count <= 0)
            //{
            //    this.lblError.Text = vctResource.GetString("msgDefineMaster");
            //    this.lblError.Text += "<br />" + vctResource.GetString("mnuRole");
            //}
        }

        private string PrepareKitInfoForMap()
        {
            string mapInfo = string.Empty;
            //if (rptMain.DataSource != null)
            {
                //List<KitSearchResult> lstSearchResults = this.KitSearchResultList;
                //lstSearchResults.Insert(0, this.kitSearchResult);
                StringBuilder sbr = new StringBuilder();
                string singleKit = string.Empty;
                if (this.kitSearchResultForRequestedLocation != null)
                {
                    singleKit = string.Empty;
                    singleKit += string.Format("\"lat\": \"{0}\",", this.kitSearchResultForRequestedLocation.Latitude.ToString());
                    singleKit += string.Format("\"long\": \"{0}\",", this.kitSearchResultForRequestedLocation.Longitude.ToString());
                    singleKit += string.Format("\"title\": \"{0}\",", this.kitSearchResultForRequestedLocation.BranchName);
                    singleKit += string.Format("\"BranchName\": \"{0}\",", this.kitSearchResultForRequestedLocation.BranchName);
                    singleKit += string.Format("\"BranchAddress\": \"{0}\",", this.kitSearchResultForRequestedLocation.BranchAddress);
                    singleKit += string.Format("\"Address2\": \"{0}\",", this.kitSearchResultForRequestedLocation.Address2);
                    singleKit += string.Format("\"CatalogNumber\": \"{0}\",", this.kitSearchResultForRequestedLocation.CatalogNumber);
                    singleKit += string.Format("\"Quantity\": \"{0}\",", this.kitSearchResultForRequestedLocation.Quantity.ToString());
                    singleKit += string.Format("\"Excess\": \"{0}\",", this.kitSearchResultForRequestedLocation.Excess.ToString());
                    singleKit += string.Format("\"TotalKitBuilt\": \"{0}\",", this.kitSearchResultForRequestedLocation.TotalKitBuilt.ToString());
                    singleKit += string.Format("\"TotalKitShipped\": \"{0}\",", this.kitSearchResultForRequestedLocation.TotalKitShipped.ToString());
                    singleKit += string.Format("\"ReservedQty\": \"{0}\",", this.kitSearchResultForRequestedLocation.ReservedQty.ToString());
                    singleKit += string.Format("\"BOMItemCount\": \"{0}\",", this.kitSearchResultForRequestedLocation.BOMItemCount.ToString());
                    singleKit += string.Format("\"BuiltItemCount\": \"{0}\",", this.kitSearchResultForRequestedLocation.BuiltItemCount.ToString());
                    singleKit += string.Format("\"MatchingPtage\": \"{0}\",", this.kitSearchResultForRequestedLocation.MatchingPtage.ToString());
                    singleKit += string.Format("\"KitName\": \"{0}\",", this.kitSearchResultForRequestedLocation.KitName);
                    singleKit += string.Format("\"KitNumber\": \"{0}\",", this.kitSearchResultForRequestedLocation.KitNumber);
                    singleKit += string.Format("\"PreviousCheckInDate\": \"{0}\",", Convert.ToDateTime(this.kitSearchResultForRequestedLocation.PreviousCheckInDate).ToString("MM/dd/yyyy"));
                    singleKit += string.Format("\"RequestedBy\": \"{0}\",", this.kitSearchResultForRequestedLocation.RequestedBy);
                    singleKit += string.Format("\"RequestedLocation\": \"{0}\",", this.kitSearchResultForRequestedLocation.RequestedLocation);
                    singleKit += string.Format("\"ShipToCustomer\": \"{0}\",", this.kitSearchResultForRequestedLocation.ShipToCustomer);
                    singleKit += string.Format("\"ContactPersonName\": \"{0}\",", this.kitSearchResultForRequestedLocation.ContactPersonName);
                    singleKit += string.Format("\"ContactPersonEmail\": \"{0}\"", this.kitSearchResultForRequestedLocation.ContactPersonEmail);
                    sbr.Append("{" + singleKit + "},");
                }

                singleKit = string.Empty;
                if (this.kitSearchResultForShipToLocation != null)
                {
                    singleKit = string.Empty;
                    singleKit += string.Format("\"lat\": \"{0}\",", this.kitSearchResultForShipToLocation.Latitude.ToString());
                    singleKit += string.Format("\"long\": \"{0}\",", this.kitSearchResultForShipToLocation.Longitude.ToString());
                    singleKit += string.Format("\"title\": \"{0}\",", this.kitSearchResultForShipToLocation.BranchName);
                    singleKit += string.Format("\"BranchName\": \"{0}\",", this.kitSearchResultForShipToLocation.BranchName);
                    singleKit += string.Format("\"BranchAddress\": \"{0}\",", this.kitSearchResultForShipToLocation.BranchAddress);
                    singleKit += string.Format("\"Address2\": \"{0}\",", this.kitSearchResultForShipToLocation.Address2);
                    singleKit += string.Format("\"CatalogNumber\": \"{0}\",", this.kitSearchResultForShipToLocation.CatalogNumber);
                    singleKit += string.Format("\"Quantity\": \"{0}\",", this.kitSearchResultForShipToLocation.Quantity.ToString());
                    singleKit += string.Format("\"Excess\": \"{0}\",", this.kitSearchResultForShipToLocation.Excess.ToString());
                    singleKit += string.Format("\"TotalKitBuilt\": \"{0}\",", this.kitSearchResultForShipToLocation.TotalKitBuilt.ToString());
                    singleKit += string.Format("\"TotalKitShipped\": \"{0}\",", this.kitSearchResultForShipToLocation.TotalKitShipped.ToString());
                    singleKit += string.Format("\"ReservedQty\": \"{0}\",", this.kitSearchResultForShipToLocation.ReservedQty.ToString());
                    singleKit += string.Format("\"BOMItemCount\": \"{0}\",", this.kitSearchResultForShipToLocation.BOMItemCount.ToString());
                    singleKit += string.Format("\"BuiltItemCount\": \"{0}\",", this.kitSearchResultForShipToLocation.BuiltItemCount.ToString());
                    singleKit += string.Format("\"MatchingPtage\": \"{0}\",", this.kitSearchResultForShipToLocation.MatchingPtage.ToString());
                    singleKit += string.Format("\"KitName\": \"{0}\",", this.kitSearchResultForShipToLocation.KitName);
                    singleKit += string.Format("\"KitNumber\": \"{0}\",", this.kitSearchResultForShipToLocation.KitNumber);
                    singleKit += string.Format("\"PreviousCheckInDate\": \"{0}\",", Convert.ToDateTime(this.kitSearchResultForShipToLocation.PreviousCheckInDate).ToString("MM/dd/yyyy"));
                    singleKit += string.Format("\"RequestedBy\": \"{0}\",", this.kitSearchResultForShipToLocation.RequestedBy);
                    singleKit += string.Format("\"RequestedLocation\": \"{0}\",", this.kitSearchResultForShipToLocation.RequestedLocation);
                    singleKit += string.Format("\"ShipToCustomer\": \"{0}\",", this.kitSearchResultForShipToLocation.ShipToCustomer);
                    singleKit += string.Format("\"ContactPersonName\": \"{0}\",", this.kitSearchResultForShipToLocation.ContactPersonName);
                    singleKit += string.Format("\"ContactPersonEmail\": \"{0}\"", this.kitSearchResultForShipToLocation.ContactPersonEmail);
                    sbr.Append("{" + singleKit + "},");
                }

                if (this.KitSearchResultList.Count > 0)
                {
                    foreach (KitSearchResult searchedKit in this.KitSearchResultList)
                    {
                        singleKit = string.Empty;
                        singleKit += string.Format("\"lat\": \"{0}\",", searchedKit.Latitude.ToString());
                        singleKit += string.Format("\"long\": \"{0}\",", searchedKit.Longitude.ToString());
                        singleKit += string.Format("\"title\": \"{0}\",", searchedKit.BranchName);
                        singleKit += string.Format("\"BranchName\": \"{0}\",", searchedKit.BranchName);
                        singleKit += string.Format("\"BranchAddress\": \"{0}\",", searchedKit.BranchAddress);
                        singleKit += string.Format("\"Address2\": \"{0}\",", searchedKit.Address2);
                        singleKit += string.Format("\"CatalogNumber\": \"{0}\",", searchedKit.CatalogNumber);
                        singleKit += string.Format("\"Quantity\": \"{0}\",", searchedKit.Quantity.ToString());
                        singleKit += string.Format("\"Excess\": \"{0}\",", searchedKit.Excess.ToString());
                        singleKit += string.Format("\"TotalKitBuilt\": \"{0}\",", searchedKit.TotalKitBuilt.ToString());
                        singleKit += string.Format("\"TotalKitShipped\": \"{0}\",", searchedKit.TotalKitShipped.ToString());
                        singleKit += string.Format("\"TotalKitHold\": \"{0}\",", searchedKit.TotalKitHold.ToString());
                        singleKit += string.Format("\"ReservedQty\": \"{0}\",", searchedKit.ReservedQty.ToString());
                        singleKit += string.Format("\"BOMItemCount\": \"{0}\",", searchedKit.BOMItemCount.ToString());
                        singleKit += string.Format("\"BuiltItemCount\": \"{0}\",", searchedKit.BuiltItemCount.ToString());
                        singleKit += string.Format("\"MatchingPtage\": \"{0}\",", searchedKit.MatchingPtage.ToString());
                        singleKit += string.Format("\"KitName\": \"{0}\",", searchedKit.KitName);
                        singleKit += string.Format("\"KitNumber\": \"{0}\",", searchedKit.KitNumber);
                        singleKit += string.Format("\"PreviousCheckInDate\": \"{0}\",", Convert.ToDateTime(searchedKit.PreviousCheckInDate).ToString("MM/dd/yyyy"));
                        singleKit += string.Format("\"RequestedBy\": \"{0}\",", searchedKit.RequestedBy);
                        singleKit += string.Format("\"RequestedLocation\": \"{0}\",", searchedKit.RequestedLocation);
                        singleKit += string.Format("\"ShipToCustomer\": \"{0}\",", searchedKit.ShipToCustomer);
                        singleKit += string.Format("\"ContactPersonName\": \"{0}\",", searchedKit.ContactPersonName);
                        singleKit += string.Format("\"ContactPersonEmail\": \"{0}\"", searchedKit.ContactPersonEmail);
                        sbr.Append("{" + singleKit + "},");
                    }
                }
                mapInfo = sbr.ToString();
                mapInfo = mapInfo.Substring(0, mapInfo.LastIndexOf(","));
            }
            return mapInfo;
        }

        #endregion

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (Session["KitNumberToSearch"] != null)
        //    {
        //        PopulateSliderColumn1(GetSearchRequestsByKitName(Session["KitNumberToSearch"].ToString()));
        //    }
        //}


        //private void PopulateSliderColumn1(List<KitSearchResult> lstSource)
        //{
        //    if (lstSource.Count > 0)
        //    {
        //        this.rptMain.DataSource = lstSource;
        //        this.rptMain.DataBind();
        //    }
        //}


        //private List<KitSearchResult> GetSearchRequestsByKitName(string kitNumber)
        //{
        //    List<KitSearchResult> lstRequests = new List<KitSearchResult>();

        //    string filename = Path.Combine(Server.MapPath("~"), "KitSerachResult.xml");
        //    if (!File.Exists(filename))
        //        return lstRequests;
        //    DataSet ds = new DataSet();
        //    ds.ReadXml(filename);
        //    if (ds.Tables.Count == 0)
        //        return lstRequests;
        //    DataTable dtPendingRequests = ds.Tables[0];
        //    DataRow[] drMatchingRows = dtPendingRequests.Select("KitNumber = '" + kitNumber + "'");
        //    if (drMatchingRows.Length > 0)
        //    {
        //        foreach (DataRow row in drMatchingRows)
        //        {
        //            {
        //                KitSearchResult newSearchResult = new KitSearchResult();

        //                newSearchResult.KitNumber = row["KitNumber"].ToString();
        //                newSearchResult.KitName = row["KitName"].ToString();
        //                newSearchResult.BranchName = row["BranchName"].ToString();
        //                newSearchResult.BranchAddress = row["BranchAddress"].ToString();
        //                newSearchResult.Address2 = row["Address2"].ToString();
        //                newSearchResult.MatchingPtage = row["MatchingPtage"].ToString();
        //                newSearchResult.Excess = row["Excess"].ToString();
        //                newSearchResult.KitItemsCount = row["KitItemsCount"].ToString();
        //                newSearchResult.PreviousCheckInDate = row["PreviousCheckInDate"].ToString();
        //                newSearchResult.RequestedBy = row["RequestedBy"].ToString();
        //                newSearchResult.RequestedLocation = row["RequestedLocation"].ToString();
        //                newSearchResult.CatalogNumber = row["CatalogNumber"].ToString();
        //                newSearchResult.ShipToCustomer = row["ShipToCustomer"].ToString();
        //                if (!row.Table.Columns.Contains("Longitude"))
        //                    newSearchResult.Longitude = string.Empty;
        //                else
        //                    newSearchResult.Longitude = row["Longitude"].ToString();

        //                if (!row.Table.Columns.Contains("Lattitude"))
        //                    newSearchResult.Lattitude = string.Empty;
        //                else
        //                    newSearchResult.Lattitude = row["Lattitude"].ToString();

        //                if (!row.Table.Columns.Contains("ContactPersonEmail"))
        //                    newSearchResult.ContactPersonEmail = string.Empty;
        //                else
        //                    newSearchResult.ContactPersonEmail = row["ContactPersonEmail"].ToString();

        //                if (!row.Table.Columns.Contains("ContactPersonName"))
        //                    newSearchResult.ContactPersonName = string.Empty;
        //                else
        //                    newSearchResult.ContactPersonName = row["ContactPersonName"].ToString();

        //                lstRequests.Add(newSearchResult);
        //            }
        //        }
        //    }
        //    return lstRequests;
        //}
    }
}

