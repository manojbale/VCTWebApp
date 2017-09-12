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
    public partial class PartDetail : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IPartDetailView
    {
        #region Instance Variables
        private PartDetailPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        #endregion

        #region Create New Presenter

        [CreateNew]
        public PartDetailPresenter Presenter
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

        public List<VCTWeb.Core.Domain.PartDetailStockLevel> PartDetailStockLevelList
        {
            set
            {
                this.gdvPartDeatil.DataSource = value;
                this.gdvPartDeatil.DataBind();
            }
        }

        public string PartNum
        {
            set
            {
                lblPartNum.Text = value;
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

        #endregion

        #region Event Handlers

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (OnCloseClicked != null)
                OnCloseClicked(false);
        }

        protected void gdvPartDeatil_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PartDetailStockLevel partDetailStockLevel = (PartDetailStockLevel)e.Row.DataItem;
                if (partDetailStockLevel.IsNearExpiry)
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
        public void PopulateData(int locationId, string partNum)
        {
            try
            {
                Presenter.PopulateData(locationId, partNum);
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }
        #endregion

    }
}