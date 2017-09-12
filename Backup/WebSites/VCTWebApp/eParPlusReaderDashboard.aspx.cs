using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using VCTWebApp.Resources;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Generic;
using VCTWebApp.Web;
using System.Web.UI;
using System.Drawing;
using System.Text;

namespace VCTWebApp.Shell.Views
{
    public partial class eParPlusReaderDashboard : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusReaderDashboard
    {
        #region Instance Variables
        private eParPlusReaderDashboardPresenter _presenter;
        private VCTWebAppResource _vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //lblError.Text = string.Empty;
            if (!this.IsPostBack)
            {
                //btnReaderdetailShow.Click += new EventHandler(btnReaderdetail_Click);
                //this.AuthorizedPage();
                divDashboard.Controls.Clear();
                this._presenter.OnViewInitialized();
                //this.LocalizePage();
                _presenter.FetchColumnPerRowInDashboard();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateDynamicTable();
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
            else if (!security.HasAccess("ePar+.ReaderDashboard"))
            {
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
            }
        }

        private void LocalizePage()
        {
            try
            {
                Page.Title = lblHeader.Text = _vctResource.GetString("mnueParReaderDashboard");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateDynamicTable()
        {
            _presenter.FetchColumnPerRowInDashboard();

            divDashboard.Controls.Clear();

            if (ListCustomerShelf == null)
                return;

            if (ListCustomerShelf.Count <= 0)
                return;

            int totalItems = ListCustomerShelf.Count;

            var myTable = new Table();
            myTable.Controls.Clear();
            myTable.Style.Add(HtmlTextWriterStyle.Height, "100%");
            myTable.Style.Add(HtmlTextWriterStyle.Width, "100%");
            myTable.CellPadding = 0;
            myTable.CellSpacing = 0;

            int cellWidth = 100;
            int cellHeight = 100;
            int maxCells = ColumnPerRowInDashboard;


            

            int cells = maxCells;

            if (totalItems == 1)
                cells = 1;
            else if (totalItems == 2)
                cells = 2;
            else if (totalItems == 3)
                cells = 3;
            else if (totalItems == 4)
                cells = 4;

            if (totalItems >= 50 && totalItems < 80)
            {
                cells = maxCells = 8;
            }
            else if (totalItems >= 80 && totalItems < 100)
            {
                cells = maxCells = 10;
            }
            else if (totalItems >= 100 )
            {
                cells = maxCells = 15;
            }
            


            decimal rowCount = Convert.ToDecimal(totalItems) / Convert.ToDecimal(maxCells);
            rowCount = Math.Ceiling(rowCount);
            cellWidth = 100 / cells;
            int counter = 0;

            cellHeight = 550 / Convert.ToInt16(rowCount);

            bool isFontBold = true;

            if (totalItems <= 40)
                isFontBold = true;
            else
                isFontBold = false;


            for (int rowCtr = 1; rowCtr <= rowCount; rowCtr++)
            {
                TableRow rowNew = new TableRow();
                rowNew.Style.Add(HtmlTextWriterStyle.Height, Unit.Percentage(cellHeight).ToString());
                rowNew.Style.Add(HtmlTextWriterStyle.Width, "100%");
                myTable.Rows.Add(rowNew);

                for (int cellCtr = 1; cellCtr <= cells; cellCtr++)
                {
                    TableCell cellNew = new TableCell();
                    cellNew.Style.Add(HtmlTextWriterStyle.Height, Unit.Pixel(cellHeight).ToString());
                    cellNew.Style.Add(HtmlTextWriterStyle.Width, Unit.Percentage(cellWidth).ToString());
                    if (counter < ListCustomerShelf.Count)
                    {
                        string buttonText = String.Empty;
                        var btnReaderdetail = new Button();
                        btnReaderdetail.Attributes.Add("runat", "server");
                        btnReaderdetail.CausesValidation = true;
                        btnReaderdetail.UseSubmitBehavior = true;
                        btnReaderdetail.ID = Convert.ToString(ListCustomerShelf[counter].CustomerShelfId);

                        string buttonToolTipText = String.Empty;

                        if (isFontBold)
                        {
                            buttonText = string.Concat(ListCustomerShelf[counter].AccountNumber, "\r\n", ListCustomerShelf[counter].ShelfCode, "\r\n", ListCustomerShelf[counter].ReaderHealthLastUpdatedOn);
                            btnReaderdetail.Font.Size = FontUnit.Point(12);
                            btnReaderdetail.Font.Bold = true;
                        }
                        else
                        {
                            buttonText = string.Concat(ListCustomerShelf[counter].AccountNumber, ",", ListCustomerShelf[counter].ShelfCode);
                            btnReaderdetail.Font.Size = FontUnit.Point(8);
                            btnReaderdetail.Font.Bold = false;
                        }

                        buttonToolTipText = string.Concat(
                                                          "Customer : ", ListCustomerShelf[counter].AccountNumber, "-", ListCustomerShelf[counter].CustomerName
                                                          , "\r\n",
                                                          "Shelf : ", ListCustomerShelf[counter].ShelfCode, "-", ListCustomerShelf[counter].ShelfName
                                                          , "\r\n",
                                                          "Reader : ", ListCustomerShelf[counter].ReaderIP, "-", ListCustomerShelf[counter].ReaderName
                                                          , "\r\n",
                                                          "Status : ", ListCustomerShelf[counter].ReaderStatus, "-", ListCustomerShelf[counter].ReaderHealthLastUpdatedOn
                                                          );
                        btnReaderdetail.ToolTip = buttonToolTipText;                        

                        btnReaderdetail.Text = buttonText;
                        btnReaderdetail.Click += btnReaderdetail_Click;
                        if (ListCustomerShelf[counter].ReaderStatus.Trim().ToUpper() == "UP")
                            btnReaderdetail.BackColor = Color.LightGreen;
                        else if (ListCustomerShelf[counter].ReaderStatus.Trim().ToUpper() == "DOWN")
                            btnReaderdetail.BackColor = Color.OrangeRed;
                        else
                            btnReaderdetail.BackColor = Color.NavajoWhite;

                        btnReaderdetail.Height = Unit.Percentage(100);
                        btnReaderdetail.Width = Unit.Percentage(100);
                        cellNew.Controls.Add(btnReaderdetail);
                    }
                    counter++;
                    cellNew.BorderStyle = BorderStyle.Solid;
                    cellNew.BorderWidth = Unit.Pixel(1);
                    rowNew.Controls.Add(cellNew);
                }
            }
            divDashboard.Controls.Add(myTable);
        }

        void btnReaderdetail_Click(object sender, EventArgs e)
        {
            CustomerShelfId = 0;
            Button button = (Button)sender;
            if (button != null)
            {
                CustomerShelfId = Convert.ToInt16(button.ID);
                Session["CustomerShelfId"] = CustomerShelfId;
                Response.Redirect("~/eParPlusReaderDetail.aspx");
            }
        }

        #endregion

        #region Create New Presenter
        [CreateNew]
        public eParPlusReaderDashboardPresenter Presenter
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

        #region IeParPlusReaderDashboard Implementation

        public int ColumnPerRowInDashboard { get; set; }

        public int CustomerShelfId { get; set; }

        public List<VCTWeb.Core.Domain.CustomerShelf> ListCustomerShelf
        {
            get
            {
                return (List<VCTWeb.Core.Domain.CustomerShelf>)Session["CustomerShelfList"];
            }
            set
            {
                Session["CustomerShelfList"] = value;
                CreateDynamicTable();
            }
        }

        public List<VCTWeb.Core.Domain.CustomerShelfProperty> ListCustomerShelfProperty
        {
            get
            {
                return (List<VCTWeb.Core.Domain.CustomerShelfProperty>)ViewState["CustomerShelfPropertyList"];
            }
            set
            {
                ViewState["CustomerShelfPropertyList"] = value;
            }
        }

        public List<VCTWeb.Core.Domain.CustomerShelfAntennaProperty> ListCustomerShelfAntennaProperty
        {
            get
            {
                return (List<VCTWeb.Core.Domain.CustomerShelfAntennaProperty>)ViewState["CustomerShelfAntennaPropertyList"];
            }
            set
            {
                ViewState["CustomerShelfAntennaPropertyList"] = value;
            }
        }

        #endregion
    }
}


