using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using VCTWebApp.Shell.Views;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using Microsoft.Practices.ObjectBuilder;

namespace VCTWebApp.Controls
{
    public partial class TagHistory : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, ITagHistoryView
    {
        #region Instance Variables
        private TagHistoryPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        #endregion

        #region Create New Presenter

        [CreateNew]
        public TagHistoryPresenter Presenter
        {
            get
            {
                return _presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _presenter = value;
                _presenter.View = this;
            }
        }

        #endregion

        #region Delegate

        public delegate void CloseClickedHandler(bool flagBindGrid);
        public delegate void ShowCaseHandler();
        public event CloseClickedHandler OnCloseClicked;
        public event ShowCaseHandler OnShowPopup;

        #endregion

        #region ITagHistoryView Implementations

        public List<EPPTransaction> TagHistoryList
        {
            set
            {
                gdvKitDeatil.DataSource = value;
                gdvKitDeatil.DataBind();
            }
        }

        public string TagId
        {
            set { lblTagId.Text = value; }
        }

        public string RefNum
        {
            set { lblRefNum.Text = value; }
        }

        public string LotNum
        {
            set { lblLotNum.Text = value; }
        }

        #endregion

        #region private methods

        private void LocalizePage()
        {
            try
            {
                //lblLocation.Text = vctResource.GetString("lblLocation");
            }
            catch (Exception ex)
            {
                //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), lblHeader.Text);
            }
        }

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
                // KitDetailStockLevel kitDetailStockLevel = (KitDetailStockLevel)e.Row.DataItem;

                //// Label lblStatus =e.Row.FindControl("lblStatus") as Label;
                // if (kitDetailStockLevel.IsNearExpiry)
                // {
                //     e.Row.ForeColor = System.Drawing.Color.Red;
                //    // kitDetailStockLevel.Status = "Expired";
                // }
                // //lblStatus.Text = kitDetailStockLevel.Status;

                // GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                // List<VCTWeb.Core.Domain.VirtualCheckOut> lst = VirtualCheckOutList.FindAll(v => v.BuildKitId == kitDetailStockLevel.BuildKitId);
                // grdChild.DataSource = VirtualCheckOutList.FindAll(v => v.BuildKitId == kitDetailStockLevel.BuildKitId);
                // //grdChild.DataSource = PopulateBuildKitById(kitDetailStockLevel.BuildKitId);
                // grdChild.DataBind();
            }
        }

        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //VCTWeb.Core.Domain.VirtualCheckOut virtualCheckOut = (VCTWeb.Core.Domain.VirtualCheckOut)e.Row.DataItem;
                //if (virtualCheckOut.IsNearExpiry)
                //    e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    _presenter.OnViewInitialized();
                    LocalizePage();
                }
                Presenter.OnViewLoaded();
            }
            catch (Exception ex)
            {
                //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), lblHeader.Text);
            }
        }

        #endregion

        #region public methods

        public void PopulateData(string accountNumber, string TagId)
        {
            try
            {
                Presenter.PopulateData(accountNumber,TagId);
            }
            catch (Exception ex)
            {
                //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), lblHeader.Text);
            }
        }
        #endregion





      
    }
}