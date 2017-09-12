using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWeb.Core.Domain;
using System.Data;
using System.ComponentModel;
using System.IO;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWebApp.Web;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace VCTWebApp.Shell.Views
{
    public partial class InventoryCountReconcile : Microsoft.Practices.CompositeWeb.Web.UI.Page//, IRegionalOfficeRequestView
    {
        #region Instance Variables

        //private RegionalOfficeRequestPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        //private VCTWeb.Core.Domain.Helper helper = new VCTWeb.Core.Domain.Helper();
        private Security security = null;

        #endregion

        #region Private Properties

        //private bool CanView
        //{
        //    get
        //    {
        //        return ViewState[Common.CAN_VIEW] != null ? (bool)ViewState[Common.CAN_VIEW] : false;
        //    }
        //    set
        //    {
        //        ViewState[Common.CAN_VIEW] = value;
        //    }
        //}

        #endregion

        #region Init/Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    this.AuthorizedPage();
                    PopulateHospital();
                    PopulateDispositionType();
                    ////this.GetPendingRequestsData();
                    //this.LocalizePage();
                    //this.Presenter.OnViewInitialized();
                    ////this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    //this.DisplayMessageForMissingMasters();
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

        //[CreateNew]
        //public RegionalOfficeRequestPresenter Presenter
        //{
        //    get
        //    {
        //        return this._presenter;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            throw new ArgumentNullException("value");

        //        this._presenter = value;
        //        this._presenter.View = this;
        //    }
        //}

        #endregion

        #region IRegionalOfficeRequestView Members

        public List<VCTWeb.Core.Domain.DispositionType> NegativeVarianceDispositionTypeList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.DispositionType>)ViewState["NegativeVarianceDispositionTypeList"];
            }
            set
            {
                ViewState["NegativeVarianceDispositionTypeList"] = value;
            }
        }

        public List<VCTWeb.Core.Domain.DispositionType> PositiveVarianceDispositionTypeList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.DispositionType>)ViewState["PositiveVarianceDispositionTypeList"];
            }
            set
            {
                ViewState["PositiveVarianceDispositionTypeList"] = value;
            }
        }

        #endregion

        #region Event Handlers

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                gdvInventoryCount.DataSource = null;
                gdvInventoryCount.DataBind();
                ddlHospital.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gdvInventoryCount.Rows.Count > 0)
                {
                    bool validInput = true;
                    StringBuilder consumptionDetailXML = new StringBuilder();
                    StringBuilder reconcileDetailXML = new StringBuilder();
                    foreach (GridViewRow row in gdvInventoryCount.Rows)
                    {
                        //DropDownList ddlDispositionType = row.FindControl("ddlDispositionType") as DropDownList;
                        //if (ddlDispositionType.SelectedIndex <= 0)
                        //{
                        //    lblError.Text = "Please select Disposition for all the records";
                        //    validInput = false;
                        //    break;
                        //}
                        //RadioButtonList rblstAction = row.FindControl("rblstAction") as RadioButtonList;

                        //PartyCycleCount partyCycleCount = row.DataItem as PartyCycleCount;
                        HiddenField hdnPartyCycleCountId = row.FindControl("hdnPartyCycleCountId") as HiddenField;
                        Label lblPartNum = row.FindControl("lblPartNum") as Label;
                        Label lblLotNum = row.FindControl("lblLotNum") as Label;
                        Label lblExpectedQty = row.FindControl("lblExpectedQty") as Label;
                        Label lblCycleCountQty = row.FindControl("lblCycleCountQty") as Label;
                        bool isNegativeVariance = Convert.ToInt32(lblExpectedQty.Text) > Convert.ToInt32(lblCycleCountQty.Text) ? false : true;

                        if (Convert.ToInt64(hdnPartyCycleCountId.Value) > 0)
                        {
                            if (string.IsNullOrEmpty(reconcileDetailXML.ToString()))
                                reconcileDetailXML.Append("<root>");
                            reconcileDetailXML.Append("<ReconcileDetail>");
                            reconcileDetailXML.Append("<PartyCycleCountId>" + hdnPartyCycleCountId.Value + "</PartyCycleCountId>");
                            reconcileDetailXML.Append("<IsAccepted>" + (!isNegativeVariance ? "1" : "0") + "</IsAccepted>");
                            reconcileDetailXML.Append("<DispositionTypeId>" + (isNegativeVariance ? NegativeVarianceDispositionTypeList[0].DispositionTypeId.ToString() : PositiveVarianceDispositionTypeList[0].DispositionTypeId.ToString()) + "</DispositionTypeId>");
                            reconcileDetailXML.Append("</ReconcileDetail>");
                        }
                        if (!isNegativeVariance)
                        {
                            if (string.IsNullOrEmpty(consumptionDetailXML.ToString()))
                                consumptionDetailXML.Append("<root>");
                            consumptionDetailXML.Append("<ConsumptionDetail>");
                            consumptionDetailXML.Append("<PartNum>" + lblPartNum.Text + "</PartNum>");
                            consumptionDetailXML.Append("<LotNum>" + lblLotNum.Text + "</LotNum>");
                            consumptionDetailXML.Append("<ConsumedQty>" + (Convert.ToInt32(lblExpectedQty.Text) - Convert.ToInt32(lblCycleCountQty.Text)).ToString() + "</ConsumedQty>");
                            consumptionDetailXML.Append("</ConsumptionDetail>");
                        }
                    }
                    if (validInput)
                    {
                        if (!string.IsNullOrEmpty(consumptionDetailXML.ToString()))
                            consumptionDetailXML.Append("</root>");
                        if (!string.IsNullOrEmpty(reconcileDetailXML.ToString()))
                            reconcileDetailXML.Append("</root>");
                        PartyRepository repository = new PartyRepository(HttpContext.Current.User.Identity.Name);
                        repository.SaveInventoryCountReconcile(Convert.ToInt64(ddlHospital.SelectedValue), consumptionDetailXML.ToString(), reconcileDetailXML.ToString());
                        lblError.Text = "<font color='blue'>" + "Inventory Count Reconcile Saved Successfully" + "</font>";
                        PopulatePartyCycleCount();
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void ddlHospital_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                PopulatePartyCycleCount();
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvInventoryCount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PartyCycleCount partyCycleCount = e.Row.DataItem as PartyCycleCount;
                if (partyCycleCount.ExpectedQty > partyCycleCount.CycleCountQty)
                {
                    List<PartyCycleCountChild> lstPartyCycleCountChild = new List<PartyCycleCountChild>();
                    for (int i = 0; i < partyCycleCount.ExpectedQty - partyCycleCount.CycleCountQty; i++)
                    {
                        lstPartyCycleCountChild.Add(new PartyCycleCountChild()
                            {
                                IsNegativeVariance = false,
                                LotNum = partyCycleCount.LotNum,
                                PartNum = partyCycleCount.PartNum,
                                PartyCycleCountId = partyCycleCount.PartyCycleCountId,
                                Quantity = 1
                            });
                    }
                    GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                    grdChild.DataSource = lstPartyCycleCountChild;
                    grdChild.DataBind();
                }
                else if (partyCycleCount.ExpectedQty < partyCycleCount.CycleCountQty)
                {
                    List<PartyCycleCountChild> lstPartyCycleCountChild = new List<PartyCycleCountChild>();
                    for (int i = 0; i < partyCycleCount.CycleCountQty - partyCycleCount.ExpectedQty; i++)
                    {
                        lstPartyCycleCountChild.Add(new PartyCycleCountChild()
                        {
                            IsNegativeVariance = true,
                            LotNum = partyCycleCount.LotNum,
                            PartNum = partyCycleCount.PartNum,
                            PartyCycleCountId = partyCycleCount.PartyCycleCountId,
                            Quantity = 1
                        });
                    }
                    GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                    grdChild.DataSource = lstPartyCycleCountChild;
                    grdChild.DataBind();
                }




                //DropDownList ddlDispositionType = e.Row.FindControl("ddlDispositionType") as DropDownList;
                //if (ddlDispositionType != null)
                //{
                //    PartyCycleCount partyCycleCount = e.Row.DataItem as PartyCycleCount;
                //    if (partyCycleCount.ExpectedQty > partyCycleCount.CycleCountQty)
                //    {
                //        ddlDispositionType.DataSource = this.NegativeVarianceDispositionTypeList;
                //        ddlDispositionType.DataTextField = "Disposition";
                //        ddlDispositionType.DataValueField = "DispositionTypeId";
                //        ddlDispositionType.DataBind();
                //    }
                //    else if (partyCycleCount.ExpectedQty < partyCycleCount.CycleCountQty)
                //    {
                //        ddlDispositionType.DataSource = this.PositiveVarianceDispositionTypeList;
                //        ddlDispositionType.DataTextField = "Disposition";
                //        ddlDispositionType.DataValueField = "DispositionTypeId";
                //        ddlDispositionType.DataBind();
                //        //RadioButtonList rblstAction = e.Row.FindControl("rblstAction") as RadioButtonList;
                //        //ddlDispositionType.SelectedIndex = 2;
                //        //rblstAction.SelectedIndex = 1;
                //    }

                //    //ddlDispositionType.SelectedIndex = 1;
                //    //ddlDispositionType.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
                //}
            }
        }

        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlDispositionType = e.Row.FindControl("ddlDispositionType") as DropDownList;
                if (ddlDispositionType != null)
                {
                    PartyCycleCountChild partyCycleCountChild = e.Row.DataItem as PartyCycleCountChild;
                    if (partyCycleCountChild.IsNegativeVariance)
                    {
                        ddlDispositionType.DataSource = this.NegativeVarianceDispositionTypeList;
                        ddlDispositionType.DataTextField = "Disposition";
                        ddlDispositionType.DataValueField = "DispositionTypeId";
                        ddlDispositionType.DataBind();
                    }
                    else
                    {
                        ddlDispositionType.DataSource = this.PositiveVarianceDispositionTypeList;
                        ddlDispositionType.DataTextField = "Disposition";
                        ddlDispositionType.DataValueField = "DispositionTypeId";
                        ddlDispositionType.DataBind();
                        //RadioButtonList rblstAction = e.Row.FindControl("rblstAction") as RadioButtonList;
                        //ddlDispositionType.SelectedIndex = 2;
                        //rblstAction.SelectedIndex = 1;
                    }

                    //ddlDispositionType.SelectedIndex = 1;
                    //ddlDispositionType.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
                }
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
            else if (security.HasAccess("InventoryCountReconcile"))
            {
                //CanCancel = security.HasPermission("InventoryCountReconcile");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void PopulatePartyCycleCount()
        {
            gdvInventoryCount.DataSource = null;
            if (ddlHospital.SelectedIndex != 0)
            {
                gdvInventoryCount.DataSource = new PartyRepository().GetPartyCycleCountMatchByPartyId(Convert.ToInt64(ddlHospital.SelectedValue));
            }
            gdvInventoryCount.DataBind();
        }

        private void PopulateHospital()
        {
            ddlHospital.DataSource = new PartyRepository().FetchParties(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
            ddlHospital.DataTextField = "Name";
            ddlHospital.DataValueField = "PartyId";
            ddlHospital.DataBind();

            ddlHospital.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
        }

        private void PopulateDispositionType()
        {
            this.NegativeVarianceDispositionTypeList = new PartyRepository().GetDispositionTypesByCategory(Constants.DispositionCategory.InventoryCountReconcileNegativeVariance.ToString());
            this.PositiveVarianceDispositionTypeList = new PartyRepository().GetDispositionTypesByCategory(Constants.DispositionCategory.InventoryCountReconcilePositiveVariance.ToString());
        }

        #endregion

    }
}

