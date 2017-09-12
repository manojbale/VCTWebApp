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
using System.Web.UI.HtmlControls;



namespace VCTWebApp
{
    public partial class ReplenishmentPlanning : Microsoft.Practices.CompositeWeb.Web.UI.Page
    {

        #region Instance Variables


        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;

        #endregion

        #region Protected Methods
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.AuthorizedPage();
                    txtStartDate.Text = DateTime.Now.AddMonths(-1).ToString("d");
                    txtEndDate.Text = DateTime.Now.ToString("d");

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {


                DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
                DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

                if (startDate > endDate)
                {
                    lblError.Text = vctResource.GetString("StartDateGreaterEndDate");
                    SetFieldsBlank();
                    return;
                }


                PopulateReport();
            }
            catch
            {

            }
        }


        protected void gridKitTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblKitFamilyHeader") as Label).Text = vctResource.GetString("Common_lblKitFamily");
                (e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("WebCheckOut_Description");
                (e.Row.FindControl("lblKitNumberHeader") as Label).Text = vctResource.GetString("WebCheckOut_AssignKitNumber");

            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                
                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                Int64 BuildKitId = Convert.ToInt64((e.Row.FindControl("hdnBuildKitId") as HiddenField).Value);
                Int64 CaseKitId = Convert.ToInt64((e.Row.FindControl("hdnCaseKitId") as HiddenField).Value);

                if (ViewState["ReplenishmentKits"] != null)
                {
                    List<VirtualCheckOut> lstReplenishmentKits = ViewState["ReplenishmentKits"] as List<VirtualCheckOut>;
                    if (lstReplenishmentKits.Count > 0)
                    {
                        grdChild.DataSource = lstReplenishmentKits.Where(w => w.CaseKitId == CaseKitId).ToList().OrderBy(x => x.PartNum);
                        grdChild.DataBind();
                    }
                    else
                    {
                        grdChild.DataSource = null;
                        grdChild.DataBind();
                    }
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
            else if (security.HasAccess("ReplenishmentPlanning"))
            {              
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void SetFieldsBlank()
        {
            gridKitTable.DataSource = null;
            gridKitTable.DataBind();
        }

        

        private void PopulateReport()
        {
            lblError.Text = string.Empty;
            List<VirtualCheckOut> lstReplenishmentKits = new InventoryStockRepository().GetReplenishmentPlanning(Convert.ToInt32(Session["LoggedInLocationId"]), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));

            if (lstReplenishmentKits != null && lstReplenishmentKits.Count > 0)
            {
                ViewState["ReplenishmentKits"] = lstReplenishmentKits;
                var DistinctItemsBasedOnCaseKitId = lstReplenishmentKits.GroupBy(x => x.CaseKitId).Select(y => y.First());
                gridKitTable.DataSource = DistinctItemsBasedOnCaseKitId;
                gridKitTable.DataBind();

            }
            else
            {
                ViewState["ReplenishmentKits"] = null;
                gridKitTable.DataSource = null;
                gridKitTable.DataBind();
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("noRecordFound"), this.lblHeader.Text);
               
            }

        }


        #endregion
    }
}