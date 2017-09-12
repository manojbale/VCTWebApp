using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWebApp.Shell.Views;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using Microsoft.Practices.ObjectBuilder;
using System.Text;
using System.Globalization;
using System.Data;

namespace VCTWebApp.Controls
{
    public partial class KitDetail : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IKitDetailView
    {
        #region Instance Variables
        private KitDetailPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        #endregion

        #region Create New Presenter

        [CreateNew]
        public KitDetailPresenter Presenter
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

        #region Delegate

        public delegate void CloseClickedHandler(bool flagBindGrid);
        public delegate void ShowCaseHandler();
        public event CloseClickedHandler OnCloseClicked;
        public event ShowCaseHandler OnShowPopup;

        #endregion

        #region ICaseView Implementations

        public List<VCTWeb.Core.Domain.KitDetailStockLevel> KitDetailStockLevelList
        {
            set
            {
                this.gdvKitDeatil.DataSource = value;
                this.gdvKitDeatil.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> VirtualCheckOutList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.VirtualCheckOut>)ViewState["VirtualCheckOutList"];
            }
            set
            {
                ViewState["VirtualCheckOutList"] = value;
            }
        }

        public string KitFamily
        {
            set
            {
                lblKitFamily.Text = value;
            }
        }

        public string Location
        {
            set
            {
                lblLocation.Text = value;
            }
        }

        #endregion

        #region private methods

        private void LocalizePage()
        {
            try
            {
                //this.lblLocation.Text = vctResource.GetString("lblLocation");
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        //public List<VCTWeb.Core.Domain.VirtualCheckOut> PopulateBuildKitById(Int64 BuildKitId)
        //{
        //    return(new InventoryStockRepository().PopulateBuildKitById(BuildKitId));
        //}

        #endregion

        #region Event Handlers

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //if (OnCloseClicked != null)
            //    OnCloseClicked(false);
           

        }



        protected void gdvKitDeatil_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KitDetailStockLevel kitDetailStockLevel = (KitDetailStockLevel)e.Row.DataItem;

               // Label lblStatus =e.Row.FindControl("lblStatus") as Label;
                if (kitDetailStockLevel.IsNearExpiry)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                   // kitDetailStockLevel.Status = "Expired";
                }
                //lblStatus.Text = kitDetailStockLevel.Status;

                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                List<VCTWeb.Core.Domain.VirtualCheckOut> lst = this.VirtualCheckOutList.FindAll(v => v.BuildKitId == kitDetailStockLevel.BuildKitId);
                grdChild.DataSource = this.VirtualCheckOutList.FindAll(v => v.BuildKitId == kitDetailStockLevel.BuildKitId);
                //grdChild.DataSource = PopulateBuildKitById(kitDetailStockLevel.BuildKitId);
                grdChild.DataBind();
            }
        }

        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VCTWeb.Core.Domain.VirtualCheckOut virtualCheckOut = (VCTWeb.Core.Domain.VirtualCheckOut)e.Row.DataItem;
                if (virtualCheckOut.IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this._presenter.OnViewInitialized();
                    this.LocalizePage();
                }
                Presenter.OnViewLoaded();
                //btnCaseCancel.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgCancelCaseConfirm") + "')");
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        #endregion

        #region public methods
        public void PopulateData(int locationId, long kitFamilyId)
        {
            try
            {
                Presenter.PopulateData(locationId, kitFamilyId);
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }
        #endregion

    }
}