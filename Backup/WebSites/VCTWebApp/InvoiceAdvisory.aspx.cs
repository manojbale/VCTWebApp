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

namespace VCTWebApp.Shell.Views
{
    public partial class InvoiceAdvisory : Microsoft.Practices.CompositeWeb.Web.UI.Page//, IRegionalOfficeRequestView
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
                if (!this.IsPostBack)
                {
                    this.AuthorizedPage();
                    if (Request.QueryString.HasKeys() && Request.QueryString.AllKeys.Contains("CaseId"))
                    {
                        PopulateCaseDetail(Convert.ToInt64(Request.QueryString["CaseId"]));
                        PopulateInvoiceDetail(Convert.ToInt64(Request.QueryString["CaseId"]));
                    }
                    //if (Session["InvoiceAdvisoryCaseId"] != null && Session["InvoiceAdvisoryCaseId"].ToString() != string.Empty)
                    //{
                    //    PopulateCaseDetail(Convert.ToInt64(Session["InvoiceAdvisoryCaseId"]));
                    //    PopulateInvoiceDetail(Convert.ToInt64(Session["InvoiceAdvisoryCaseId"]));
                    //    Session["InvoiceAdvisoryCaseId"] = null;
                    //}
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

        #endregion

        #region Event Handlers

        protected void gdvInvoiceItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CaseInvoiceAdvisory caseInvoiceAdvisory = (CaseInvoiceAdvisory)e.Row.DataItem;
                if (caseInvoiceAdvisory.InventoryType == Constants.InventoryType.Kit.ToString() || caseInvoiceAdvisory.Status == Constants.PartStatus.Consumed.ToString())
                {
                    e.Row.Font.Bold = true;
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
            else if (security.HasAccess("ViewCancelTransaction"))
            {
                //CanCancel = security.HasPermission("NearExpiryItems");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void PopulateInvoiceDetail(long caseId)
        {
            List<CaseInvoiceAdvisory> lstCaseInvoiceAdvisory = new CaseRepository().GetInvoiceAdvisoryByCaseId(caseId);
            gdvInvoiceItems.DataSource = lstCaseInvoiceAdvisory;
            decimal sum = lstCaseInvoiceAdvisory.Where(p => p.InventoryType == Constants.InventoryType.Kit.ToString() || p.Status == Constants.PartStatus.Consumed.ToString()).Sum(s => s.Amount);
            lblTotal.Text = "$ "+sum.ToString();
            gdvInvoiceItems.DataBind();
        }

        private void PopulateCaseDetail(long caseId)
        {
            Cases caseDetail = new CaseRepository().FetchCaseById(caseId);
            if (caseDetail != null)
            {
                lblHeader.Text = "Invoice Advisory for Case # " + caseDetail.CaseNumber;
                txtSalesRep.Text = caseDetail.SalesRep;
                txtSurgeryDate.Text = string.Format(CultureInfo.CurrentCulture, caseDetail.SurgeryDate.ToString("d"));
                txtHospital.Text = caseDetail.PartyName;
                txtCaseStatus.Text = caseDetail.CaseStatus;
            }
        }

        #endregion

    }
}

