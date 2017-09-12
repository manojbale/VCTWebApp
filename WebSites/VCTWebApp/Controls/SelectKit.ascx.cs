using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class SelectKit : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, ISelectKitView
    {
        #region Delegate

        public delegate void CloseClickedHandler(string KitNumber, string KitName);
        public event CloseClickedHandler OnCloseClicked;

        #endregion

        #region Instance Variables

        private SelectKitPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();

        #endregion


        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.LocalizePage();
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        #endregion

        #region Create New Presenter

        [CreateNew]
        public SelectKitPresenter Presenter
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

        #region ISelectKitView Members

        public List<VCTWeb.Core.Domain.KitListing> KitListingList
        {
            set 
            {
                lstKit.DataSource = value;
                lstKit.DataTextField = "KitName";
                lstKit.DataValueField = "KitNumber";
                lstKit.DataBind();
                if (lstKit.Items.Count > 0)
                {
                    lstKit.SelectedIndex = 0;
                    btnSelect.Enabled = true;
                }
            }
        }

        public string ProcedureName { get; set; }

        public string CatalogNumber { get; set; }

        #endregion

        #region Event Handlers

        protected void btnClose_Click(object sender, EventArgs e)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SelectKit page", "Closing Popup");
            
            btnSelect.Enabled = false;
            lblError.Text = string.Empty;
            if (OnCloseClicked != null)
                OnCloseClicked(string.Empty, string.Empty);
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (lstKit.SelectedIndex >= 0)
            {
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "SelectKit page", "Selecting Kit");

                btnSelect.Enabled = false;
                lblError.Text = string.Empty;
                if (OnCloseClicked != null)
                    OnCloseClicked(lstKit.SelectedValue, lstKit.SelectedItem.Text);
            }
        }

        #endregion

        #region Private Methods

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("uctSelectKit");
                lblHeader.Text = heading;
                //Page.Title = heading;
                lblMessage.Text = vctResource.GetString("msgSelectKit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Public Methods

        public void PopulateData()
        {
            this.Presenter.PopulateKitListingList();
        }

        #endregion
    }
}

