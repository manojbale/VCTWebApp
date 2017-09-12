using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Web;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace VCTWebApp.Shell.Views
{
    public partial class ProductLinePartDetail : Microsoft.Practices.CompositeWeb.Web.UI.Page, IProductLinePartDetail
    {
        #region Instance Variables

        private ProductLinePartDetailPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            //security = new Security();
            //if (Session["LoggedInUser"] == null)
            //{
            //    Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            //}
            //else if (security.HasAccess("SalesOfficeLocation"))
            //{
            //    CanView = security.HasPermission("SalesOfficeLocation.Manage");
            //    CanAdd = security.HasPermission("SalesOfficeLocation.Manage");
            //    CanEdit = security.HasPermission("SalesOfficeLocation.Manage");
            //    CanDelete = security.HasPermission("SalesOfficeLocation.Manage");
            //}
            //else
            //    Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ProductLinePartDetail page", "LocalizePage() is invoked.");
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuProductLinePartDetail");
                //lblHeader.Text = heading;
                Page.Title = heading;
            }
            catch
            {
                throw;
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
                    this.AuthorizedPage();
                    this.presenter.OnViewInitialized();
                    //this.lblError.Text = string.Empty;
                    this.LocalizePage();
                    //this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.
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
        public ProductLinePartDetailPresenter Presenter
        {
            get
            {
                return this.presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.presenter = value;
                this.presenter.View = this;
            }
        }

        #endregion

        #region Event Handlers

        protected void tvwSelectPartType_SelectedNodeChanged(object sender, EventArgs e)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ProductLinePartDetail page", "tvwSelectPartType_SelectedNodeChanged() is invoked.");
            gdvPartDetails.EditIndex = -1;
            ResetSelectedValuedofNodes();
            SelectedNodeLevel = tvwSelectPartType.SelectedNode.Depth;

            switch (SelectedNodeLevel)
            {
                case 0:
                    SelectedProductLine = tvwSelectPartType.SelectedNode.Value.Trim();
                    break;

                case 1:
                    SelectedCategory = tvwSelectPartType.SelectedNode.Value.Trim();
                    SelectedProductLine = tvwSelectPartType.SelectedNode.Parent.Value.Trim();
                    break;

                case 2:
                    SelectedSubCategory1 = tvwSelectPartType.SelectedNode.Value.Trim();
                    SelectedCategory = tvwSelectPartType.SelectedNode.Parent.Value.Trim();
                    SelectedProductLine = tvwSelectPartType.SelectedNode.Parent.Parent.Value.Trim();
                    break;

                case 3:
                    SelectedSubCategory2 = tvwSelectPartType.SelectedNode.Value.Trim();
                    SelectedSubCategory1 = tvwSelectPartType.SelectedNode.Parent.Value.Trim();
                    SelectedCategory = tvwSelectPartType.SelectedNode.Parent.Parent.Value.Trim();
                    SelectedProductLine = tvwSelectPartType.SelectedNode.Parent.Parent.Parent.Value.Trim();
                    break;

                case 4:
                    SelectedSubCategory3 = tvwSelectPartType.SelectedNode.Value.Trim();
                    SelectedSubCategory2 = tvwSelectPartType.SelectedNode.Parent.Value.Trim();
                    SelectedSubCategory1 = tvwSelectPartType.SelectedNode.Parent.Parent.Value.Trim();
                    SelectedCategory = tvwSelectPartType.SelectedNode.Parent.Parent.Parent.Value.Trim();
                    SelectedProductLine = tvwSelectPartType.SelectedNode.Parent.Parent.Parent.Parent.Value.Trim();
                    break;
            }
            presenter.FetchFilteredDataForGrid();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            //helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocation page", "btnReset_Click() is invoked.");
            //presenter.OnViewInitialized();
        }

        protected void gdvPartDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("UpdateRec"))
                {
                    TextBox txtPARLevelQty = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("txtPARLevelQty") as TextBox;
                    HiddenField hndRefNum = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("hndRefNum") as HiddenField;
                    if (hndRefNum != null && txtPARLevelQty != null)
                    {
                        string RefNum = hndRefNum.Value;

                        if (string.IsNullOrEmpty(txtPARLevelQty.Text.Trim()))
                        {
                            this.lblError.Text = string.Format(CultureInfo.InvariantCulture, "Please enter PAR Level quantity.", this.lblHeader.Text);
                            return;
                        }

                        int PARLevelQty = Convert.ToInt32(txtPARLevelQty.Text.Trim());

                        if (PARLevelQty <= 0)
                        {
                            this.lblError.Text = string.Format(CultureInfo.InvariantCulture, "PAR Level quantity should be greater than Zero.", this.lblHeader.Text);                            
                            return;
                        }

                        if (presenter.UpdateParLevelQuantityForRefNum(RefNum, PARLevelQty))
                        {
                            this.ListProductLinePartDetail[gdvPartDetails.EditIndex].DefaultPARLevel = Convert.ToInt16(txtPARLevelQty.Text);
                            lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelUpdate"), this.lblHeader.Text) + "</font>";
                            gdvPartDetails.EditIndex = -1;
                            this.ListProductLinePartDetail = this.ListProductLinePartDetail;
                        }                      
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }          
        }

        protected void gdvPartDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gdvPartDetails.EditIndex = e.NewEditIndex;
                this.ListProductLinePartDetail = this.ListProductLinePartDetail;
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvPartDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gdvPartDetails.EditIndex = -1;
                this.ListProductLinePartDetail = this.ListProductLinePartDetail;
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        #endregion

        #region IProductLinePartDetail Members

        public TreeNodeCollection ProductLinePartDetailNodeList
        {
            get
            {
                return this.tvwSelectPartType.Nodes;
            }
            set
            {
                this.tvwSelectPartType.Nodes.Clear();
                foreach (TreeNode node in value)
                {
                    this.tvwSelectPartType.Nodes.Add(node);
                }
                this.tvwSelectPartType.CollapseAll();
            }
        }

        public List<VCTWeb.Core.Domain.ProductLinePartDetail> ListProductLinePartDetail
        {
            get
            {
                return ViewState["ListProductLinePartDetail"] as List<VCTWeb.Core.Domain.ProductLinePartDetail>;
            }
            set
            {
                gdvPartDetails.DataSource = null;
                gdvPartDetails.DataBind();

                gdvPartDetails.DataSource = value;
                gdvPartDetails.DataBind();

                ViewState["ListProductLinePartDetail"] = value;
            }
        }

        public string SelectedProductLine { get; set; }

        public string SelectedCategory { get; set; }

        public string SelectedSubCategory1 { get; set; }

        public string SelectedSubCategory2 { get; set; }

        public string SelectedSubCategory3 { get; set; }

        public int SelectedNodeLevel { get; set; }

        #endregion

        #region Member Methods

        private void ResetSelectedValuedofNodes()
        {
            SelectedNodeLevel = -1;
            SelectedProductLine = string.Empty;
            SelectedCategory = string.Empty;
            SelectedSubCategory1 = string.Empty;
            SelectedSubCategory2 = string.Empty;
            SelectedSubCategory3 = string.Empty;
        }

        #endregion Member Methods
    }
}

