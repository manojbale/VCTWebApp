using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using CustomControls;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Shell.Views;
using VCTWebApp.Web;

namespace VCTWebApp
{
    public partial class CaseSchedule : Microsoft.Practices.CompositeWeb.Web.UI.Page, ICaseScheduleView
    {

        #region Instance Variables

        private CaseSchedulePresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Create New Presenter

        [CreateNew]
        public CaseSchedulePresenter Presenter
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

        #region Private Properties
        DateTime StartDate
        {
            get
            {
                return (DateTime)ViewState["StartDate"];
            }
            set
            {
                ViewState["StartDate"] = value;
            }
        }

        DateTime EndDate
        {
            get
            {
                return (DateTime)ViewState["EndDate"];
            }
            set
            {
                ViewState["EndDate"] = value;
            }
        }
        #endregion

        #region Private Methods
        private string GetHeaderText()
        {
            DateTime dttmStart = this.StartDate;
            return dttmStart.Day.ToString() + " - " + dttmStart.DayOfWeek.ToString();
        }

        private string GetHeaderText(int index)
        {
            DateTime dttmStart = this.StartDate;
            dttmStart = dttmStart.AddDays(index);
            return dttmStart.Day.ToString() + " - " + dttmStart.DayOfWeek.ToString();
        }

        private int ConvertMonthFromStringToInt(string strMonth)
        {
            switch (strMonth.ToUpper())
            {
                case "JAN":
                case "JANUARY":
                    return 1;
                case "FEB":
                case "FEBRUARY":
                    return 2;
                case "MAR":
                case "MARCH":
                    return 3;
                case "APR":
                case "APRIL":
                    return 4;
                case "MAY":
                    return 5;
                case "JUN":
                case "JUNE":
                    return 6;
                case "JUL":
                case "JULY":
                    return 7;
                case "AUG":
                case "AUGUST":
                    return 8;
                case "SEP":
                case "SEPTEMBER":
                    return 9;
                case "OCT":
                case "OCTOBER":
                    return 10;
                case "NOV":
                case "NOVEMBER":
                    return 11;
                case "DEC":
                case "DECEMBER":
                    return 12;
                default:
                    return 0;
            }
        }

        private string ConvertMonthFromIntToString(int iMonth)
        {
            switch (iMonth)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return string.Empty;
            }
        }

        private System.Drawing.Color GetForeColorByStatus(string status)
        {
            switch (status)
            {
                //case "Cancelled":
                //    return Color.White;
                default:
                    return Color.Black;
            }
        }

        private System.Drawing.Color GetBackColorByStatus(string status)
        {
            switch (status)
            {
                case "New":
                    return Color.FromArgb(196,189, 151);

                case "InternallyRequested":
                    return Color.FromArgb(184, 204, 233);

                case "InventoryAssigned":
                case "PartiallyInventoryAssigned":
                    return Color.FromArgb(252, 213, 180);

                case "Shipped":
                case "PartiallyShipped":
                    return Color.FromArgb(204, 192,218);

                case "Delivered":
                case "PartiallyDelivered":
                    return Color.FromArgb(140, 206,16);

                case "CheckedIn":
                case "PartiallyCheckedIn":
                    return Color.FromArgb(184, 204, 228);

                case "Cancelled":
                    return Color.FromArgb(217, 217, 217);

                case "Closed":
                    return Color.FromArgb(183, 222, 232);
                default:
                    return Color.White;
            }
        }

        private System.Drawing.Color GetBorderColorByStatus(string status)
        {
            switch (status)
            {
                case "New":
                case "InternallyRequested":
                    return Color.FromArgb(148, 138, 84);
                case "InventoryAssigned":
                case "PartiallyInventoryAssigned":
                    return Color.FromArgb(250, 191, 143);
                case "Shipped":
                case "PartiallyShipped":
                    return Color.FromArgb(177, 160, 199);
                case "CheckedIn":
                case "PartiallyCheckedIn":
                    return Color.FromArgb(149, 179, 215);
                case "Cancelled":
                    return Color.FromArgb(191, 191, 191);
                case "Closed":
                    return Color.FromArgb(147, 205, 221);
                default:
                    return Color.White;
            }
        }

        private void ucCasePopup_OnCloseClicked(bool flagBindGrid)
        {
            mpeSelectKit.Hide();
            if (flagBindGrid)
            {
                //if (Session["CaseCreated"] != null)
                //{

                //    DateTime SavedSurgeryDate = Convert.ToDateTime(Session["CaseCreated"].ToString());
                //    this.StartDate = new DateTime(SavedSurgeryDate.Year, SavedSurgeryDate.Month, 1);
                //    this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                //    lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
                //    if (Session["SelectedType"] != null)
                //    {
                //        rdo.Items.FindByText(Session["SelectedType"].ToString()).Selected = true;
                //        rdo_SelectedIndexChanged(null, null);
                //        Session["SelectedType"] = null;
                //    }
                //    if (SavedSurgeryDate.Month != DateTime.Now.Month)
                //    {
                //        calMain.SelectedDate = SavedSurgeryDate;
                //        calMain.VisibleDate = SavedSurgeryDate;
                //    }
                //    Session["CaseCreated"] = null;
                //}
                BindGrid();
            }
        }

        private void ucCasePopup_ShowCasePopup()
        {
            mpeSelectKit.Show();
        }

        private void BindGrid()
        {
            VCTWeb.Core.Domain.CaseRepository repository = new VCTWeb.Core.Domain.CaseRepository();
            List<VCTWeb.Core.Domain.CaseMerge> lstCaseMerge = null;


            if (!string.IsNullOrEmpty(txtHospital.Text) && string.IsNullOrEmpty(hdnShipToPartyId.Value))
            {
                this.lblError.Text = "<font color='red'>" + "Please select valid Hospital. " + "</font>";
               lstCaseMerge = repository.FetchCasesByFilter(this.StartDate, this.EndDate, Convert.ToInt32(Session["LoggedInLocationId"]), SalesRep, Procedure, 0, CaseStatus, Constants.CaseType.RoutineCase.ToString(), Physician, Convert.ToInt64(hdnKitFamilyId.Value));
            }
            else if (!string.IsNullOrEmpty(txtKitFamily.Text) && Convert.ToInt64(hdnKitFamilyId.Value)==0)
            {
                this.lblError.Text = "<font color='red'>" + "Please select valid Kit Family. " + "</font>";
                lstCaseMerge = repository.FetchCasesByFilter(this.StartDate, this.EndDate, Convert.ToInt32(Session["LoggedInLocationId"]), SalesRep, Procedure, Party, CaseStatus, Constants.CaseType.RoutineCase.ToString(), Physician,-1);

            }
            else if (!string.IsNullOrEmpty(txtProcedureName.Text) && string.IsNullOrEmpty(hdnProcedureName.Value) )
            {
                this.lblError.Text = "<font color='red'>" + "Please select valid Procedure Name. " + "</font>";
                lstCaseMerge = repository.FetchCasesByFilter(this.StartDate, this.EndDate, Convert.ToInt32(Session["LoggedInLocationId"]), SalesRep, Procedure, Party, CaseStatus, Constants.CaseType.RoutineCase.ToString(), Physician,-1);

            }
            else
            {
                this.lblError.Text = string.Empty;
                lstCaseMerge = repository.FetchCasesByFilter(this.StartDate, this.EndDate, Convert.ToInt32(Session["LoggedInLocationId"]), SalesRep, Procedure, Party, CaseStatus, Constants.CaseType.RoutineCase.ToString(), Physician, Convert.ToInt64(hdnKitFamilyId.Value));
            }
         

            if (rdo.SelectedItem.Text == "Monthly")
            {

                DataTable dtMonthly = new DataTable();
                dtMonthly.Columns.Add("Col1", typeof(string));
                dtMonthly.Columns.Add("Col2", typeof(string));
                dtMonthly.Columns.Add("Col3", typeof(string));
                dtMonthly.Columns.Add("Col4", typeof(string));
                dtMonthly.Columns.Add("Col5", typeof(string));
                dtMonthly.Columns.Add("Col6", typeof(string));
                dtMonthly.Columns.Add("Col7", typeof(string));

                DateTime dtStart1 = this.StartDate;
                while (dtStart1.DayOfWeek != DayOfWeek.Sunday)
                {
                    dtStart1 = dtStart1.AddDays(-1);
                }
                DateTime dtStart2 = dtStart1;

                for (int i = 0; i < 10; i++)
                {
                    DataRow dr = dtMonthly.NewRow();
                    for (int j = 0; j < 7; j++)
                    {
                        if (i % 2 == 0 || i == 0)
                        {
                            dr[j] = dtStart1.ToString("MMM dd");
                            dtStart1 = dtStart1.AddDays(1);
                        }
                        else
                        {
                            if (dtStart2 < this.StartDate || dtStart2 > this.EndDate)
                            {
                                dr[j] = "OTHERMONTH";
                            }
                            else
                            {
                                dr[j] = string.Join("|", lstCaseMerge.Where(w => w.SurgeryDate.Date == dtStart2.Date).Select(s => s.CaseValues).ToArray());
                                dr[j] = dr[j].ToString() + "|Date" + dtStart2.ToString("yyyy-MM-dd");
                            }
                            dtStart2 = dtStart2.AddDays(1);
                        }
                    }
                    dtMonthly.Rows.Add(dr);
                }
                gvMonthly.DataSource = dtMonthly;
                gvMonthly.DataBind();

                HighligtTodayDateInCalender(gvMonthly);

            }
            else if (rdo.SelectedItem.Text == "Weekly")
            {

                DataTable dtWeekly = new DataTable();
                dtWeekly.Columns.Add("Col1", typeof(string));
                dtWeekly.Columns.Add("Col2", typeof(string));
                dtWeekly.Columns.Add("Col3", typeof(string));
                dtWeekly.Columns.Add("Col4", typeof(string));
                dtWeekly.Columns.Add("Col5", typeof(string));
                dtWeekly.Columns.Add("Col6", typeof(string));
                dtWeekly.Columns.Add("Col7", typeof(string));

                DataRow dr = dtWeekly.NewRow();
                for (int i = 0; i < 7; i++)
                {
                    dr[i] = string.Join("|", lstCaseMerge.Where(w => w.SurgeryDate.Date == this.StartDate.AddDays(i).Date).Select(s => s.CaseValues).ToArray())
                        + "|Date" + calMain.SelectedDate.AddDays(i).ToString("yyyy-MM-dd");
                }
                dtWeekly.Rows.Add(dr);
                gvWeekly.DataSource = dtWeekly;
                gvWeekly.DataBind();

            }
            else if (rdo.SelectedItem.Text == "Daily")
            {

                DataTable dtDaily = new DataTable();
                dtDaily.Columns.Add("Col1", typeof(string));
                DataRow dr = dtDaily.NewRow();
                dr["Col1"] = string.Join("|", lstCaseMerge.Select(s => s.CaseValues).ToArray());
                dr["Col1"] = dr["Col1"].ToString() + "|Date" + calMain.SelectedDate.ToString("yyyy-MM-dd");
                dtDaily.Rows.Add(dr);
                gvDaily.DataSource = dtDaily;
                gvDaily.DataBind();

            }


        }
        #endregion

        private void HighligtTodayDateInCalender(GridView gridViewName)
        {
            foreach (GridViewRow row in gridViewName.Rows)
            {
                for (int index = 0; index < 7; index++)
                {
                    HiddenField hdnCol = (HiddenField)row.Cells[index].FindControl("hdnCol" + (index + 1).ToString()) as HiddenField;

                    if (hdnCol.Value == DateTime.Now.ToString("MMM dd"))
                    {
                        row.Cells[index].BackColor = Color.FromArgb(249, 216, 126);
                        row.Cells[index].ForeColor = Color.Black;
                        row.Cells[index].BorderColor = Color.Green;
                        row.Cells[index].BorderStyle = BorderStyle.Solid;

                    }

                }
            }
        }

        #region Init/Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.AuthorizedPage();
                //this.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                //lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");


                if (Session["CaseCreated"] == null)
                {
                    this.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                    lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
                    rdo.Items.FindByText("Monthly").Selected = true;

                }
                else
                {
                    string[] strSavedCaseArray = Session["CaseCreated"].ToString().Split('_');
                    if (strSavedCaseArray.Length > 0)
                    {
                        DateTime SavedSurgeryDate = Convert.ToDateTime(strSavedCaseArray[0]);
                        this.StartDate = new DateTime(SavedSurgeryDate.Year, SavedSurgeryDate.Month, SavedSurgeryDate.Day);
                        this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                        lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
                        if (SavedSurgeryDate.Month != DateTime.Now.Month)
                        {
                            calMain.SelectedDate = SavedSurgeryDate;
                            calMain.VisibleDate = SavedSurgeryDate;
                        }
                        if (Session["CaseCreated"] != null)
                        {
                            //string[] strArray1 = Session["CaseCreated"].ToString().Split('_');
                            ChangeGridStatus(strSavedCaseArray[2], strSavedCaseArray[1].ToString());
                            Session["SelectedType"] = null;
                            Session["CaseCreated"] = null;
                            HiddenField hdnSelectedType = ucCasePopup.FindControl("hdnSelectedType") as HiddenField;
                            hdnSelectedType.Value = string.Empty;

                        }
                    }

                }
            }
            BindGrid();


        }
        protected void Page_Init(object sender, EventArgs e)
        {
            ucCasePopup.OnCloseClicked += new VCTWebApp.Controls.Case.CloseClickedHandler(ucCasePopup_OnCloseClicked);
            ucCasePopup.OnShowPopup += new VCTWebApp.Controls.Case.ShowCaseHandler(ucCasePopup_ShowCasePopup);
            Presenter.OnViewInitialized();
        }
        #endregion

        #region Event Handlers

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hdnShipToPartyId.Value))
                {
                    this.lblError.Text = "<font color='red'>" + "Please select valid Hospital. " + "</font>";
                    return;
                }
                if (string.IsNullOrEmpty(hdnKitFamilyId.Value))
                {
                    this.lblError.Text = "<font color='red'>" + "Please select valid Kit Family. " + "</font>";
                    return;
                }
                if (string.IsNullOrEmpty(hdnProcedureName.Value))
                {
                    this.lblError.Text = "<font color='red'>" + "Please select valid Procedure Name. " + "</font>";
                    return;
                }
                BindGrid();
            }
            catch (Exception)
            {

            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtHospital.Text = string.Empty;
                txtKitFamily.Text = string.Empty;
                ddlPhysician.SelectedIndex = 0;
                ddlSalesRep.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;
                txtProcedureName.Text = string.Empty;
                lblError.Text = string.Empty;
                

                VCTWeb.Core.Domain.CaseRepository repository = new VCTWeb.Core.Domain.CaseRepository();
                              
                List<VCTWeb.Core.Domain.CaseMerge> lstCaseMerge = repository.FetchCasesByFilter(this.StartDate, this.EndDate, Convert.ToInt32(Session["LoggedInLocationId"]), SalesRep, Procedure, null, CaseStatus, Constants.CaseType.RoutineCase.ToString(), Physician, Convert.ToInt64(hdnKitFamilyId.Value));
              
                if (rdo.SelectedItem.Text == "Monthly")
                {

                    DataTable dtMonthly = new DataTable();
                    dtMonthly.Columns.Add("Col1", typeof(string));
                    dtMonthly.Columns.Add("Col2", typeof(string));
                    dtMonthly.Columns.Add("Col3", typeof(string));
                    dtMonthly.Columns.Add("Col4", typeof(string));
                    dtMonthly.Columns.Add("Col5", typeof(string));
                    dtMonthly.Columns.Add("Col6", typeof(string));
                    dtMonthly.Columns.Add("Col7", typeof(string));

                    DateTime dtStart1 = this.StartDate;
                    while (dtStart1.DayOfWeek != DayOfWeek.Sunday)
                    {
                        dtStart1 = dtStart1.AddDays(-1);
                    }
                    DateTime dtStart2 = dtStart1;

                    for (int i = 0; i < 10; i++)
                    {
                        DataRow dr = dtMonthly.NewRow();
                        for (int j = 0; j < 7; j++)
                        {
                            if (i % 2 == 0 || i == 0)
                            {
                                dr[j] = dtStart1.ToString("MMM dd");
                                dtStart1 = dtStart1.AddDays(1);
                            }
                            else
                            {
                                if (dtStart2 < this.StartDate || dtStart2 > this.EndDate)
                                {
                                    dr[j] = "OTHERMONTH";
                                }
                                else
                                {
                                    dr[j] = string.Join("|", lstCaseMerge.Where(w => w.SurgeryDate.Date == dtStart2.Date).Select(s => s.CaseValues).ToArray());
                                    dr[j] = dr[j].ToString() + "|Date" + dtStart2.ToString("yyyy-MM-dd");
                                }
                                dtStart2 = dtStart2.AddDays(1);
                            }
                        }
                        dtMonthly.Rows.Add(dr);
                    }
                    gvMonthly.DataSource = dtMonthly;
                    gvMonthly.DataBind();

                    HighligtTodayDateInCalender(gvMonthly);

                }
                else if (rdo.SelectedItem.Text == "Weekly")
                {

                    DataTable dtWeekly = new DataTable();
                    dtWeekly.Columns.Add("Col1", typeof(string));
                    dtWeekly.Columns.Add("Col2", typeof(string));
                    dtWeekly.Columns.Add("Col3", typeof(string));
                    dtWeekly.Columns.Add("Col4", typeof(string));
                    dtWeekly.Columns.Add("Col5", typeof(string));
                    dtWeekly.Columns.Add("Col6", typeof(string));
                    dtWeekly.Columns.Add("Col7", typeof(string));

                    DataRow dr = dtWeekly.NewRow();
                    for (int i = 0; i < 7; i++)
                    {
                        dr[i] = string.Join("|", lstCaseMerge.Where(w => w.SurgeryDate.Date == this.StartDate.AddDays(i).Date).Select(s => s.CaseValues).ToArray())
                            + "|Date" + calMain.SelectedDate.AddDays(i).ToString("yyyy-MM-dd");
                    }
                    dtWeekly.Rows.Add(dr);
                    gvWeekly.DataSource = dtWeekly;
                    gvWeekly.DataBind();

                }
                else if (rdo.SelectedItem.Text == "Daily")
                {

                    DataTable dtDaily = new DataTable();
                    dtDaily.Columns.Add("Col1", typeof(string));
                    DataRow dr = dtDaily.NewRow();
                    dr["Col1"] = string.Join("|", lstCaseMerge.Select(s => s.CaseValues).ToArray());
                    dr["Col1"] = dr["Col1"].ToString() + "|Date" + calMain.SelectedDate.ToString("yyyy-MM-dd");
                    dtDaily.Rows.Add(dr);
                    gvDaily.DataSource = dtDaily;
                    gvDaily.DataBind();

                }

               
            }
            catch (Exception)
            {

            }
        }

        private void ChangeGridStatus(string surgeryDate, string strStatus)
        {
            rdo.Items.FindByText(strStatus).Selected = true;
            if (strStatus == "Monthly")
            {

                Session["SelectedType"] = "Monthly";
                if (!string.IsNullOrEmpty(lblPeriod.Text))
                {
                    if (lblPeriod.Text.Split(' ')[0].ToString() == DateTime.Now.Month.ToString())
                    {
                        this.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                        lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");

                    }
                    else
                    {
                        int iMonthNumber = DateTime.ParseExact(lblPeriod.Text.Split(' ')[0], "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                        this.StartDate = new DateTime(DateTime.Now.Year, iMonthNumber, 1);
                        this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                        lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
                    }
                }


                pnlDetail.Height = 870;
                pnlDetail2.Height = 870;
                pnlCalendar.Height = 800;

                gvMonthly.Visible = true;
                gvWeekly.Visible = false;
                gvDaily.Visible = false;
            }
            else if (strStatus == "Weekly")
            {
                Session["SelectedType"] = "Weekly";
                if (!string.IsNullOrEmpty(lblPeriod.Text))
                {
                    if (lblPeriod.Text != DateTime.Now.ToString("MMMM yyyy"))
                    {
                        string[] strArray = surgeryDate.Split(' ');
                        int iMonthNumber = DateTime.ParseExact(strArray[0], "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                        // DateTime dt = Convert.ToDateTime(lblPeriod.Text);
                        this.StartDate = new DateTime(Convert.ToInt32(strArray[4]), iMonthNumber, Convert.ToInt32(strArray[1]));
                        //this.StartDate = new DateTime(surgeryDate.Year, surgeryDate.Month, dt.Day);

                    }
                    else
                    {
                        this.StartDate = DateTime.Now;

                    }
                    while (this.StartDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        this.StartDate = this.StartDate.AddDays(-1);
                    }
                    this.EndDate = this.StartDate.AddDays(6);

                    lblPeriod.Text = (this.StartDate.Year == this.EndDate.Year ? this.StartDate.ToString("MMMM dd - ") : this.StartDate.ToString("MMMM dd, yyyy - ")) + (this.StartDate.Month == this.EndDate.Month ? this.EndDate.ToString("dd, yyyy") : this.EndDate.ToString("MMMM dd, yyyy"));

                }

                pnlDetail.Height =870;
                pnlDetail2.Height = 870;
                pnlCalendar.Height = 800;

                gvMonthly.Visible = false;
                gvWeekly.Visible = true;
                gvDaily.Visible = false;
            }
            else if (strStatus == "Daily")
            {

                Session["SelectedType"] = "Daily";
                if (!string.IsNullOrEmpty(lblPeriod.Text))
                {
                    if (lblPeriod.Text.Split(' ')[0].ToString() == DateTime.Now.ToString("MMMM"))
                    {
                        this.StartDate = DateTime.Now;
                        this.EndDate = DateTime.Now;
                        lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        int iMonthNumber = DateTime.ParseExact(lblPeriod.Text.Split(' ')[0], "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                        this.StartDate = new DateTime(DateTime.Now.Year, iMonthNumber, DateTime.Now.Day);
                        this.EndDate = StartDate;
                        lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
                    }
                }


                pnlDetail.Height = 870;
                pnlDetail2.Height = 870;
                pnlCalendar.Height = 800;

                gvMonthly.Visible = false;
                gvWeekly.Visible = false;
                gvDaily.Visible = true;

            }

            calMain.SelectedDate = this.StartDate;
            calMain.VisibleDate = this.StartDate;
            // BindGrid();
        }

        protected void rdo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (rdo.SelectedItem.Text == "Monthly")
            {

                Session["SelectedType"] = "Monthly";
                if (!string.IsNullOrEmpty(lblPeriod.Text))
                {

                    if (lblPeriod.Text.Split(' ')[0].ToString() == DateTime.Now.Month.ToString())
                    {
                        this.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                        lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");

                    }
                    else
                    {
                        int iMonthNumber = DateTime.ParseExact(lblPeriod.Text.Split(' ')[0], "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                        this.StartDate = new DateTime(DateTime.Now.Year, iMonthNumber, 1);
                        this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                        lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
                    }
                }

                pnlDetail.Height = 870;
                pnlDetail2.Height = 870;
                pnlCalendar.Height = 800;

                gvMonthly.Visible = true;
                gvWeekly.Visible = false;
                gvDaily.Visible = false;
            }
            else if (rdo.SelectedItem.Text == "Weekly")
            {
                Session["SelectedType"] = "Weekly";
                if (!string.IsNullOrEmpty(lblPeriod.Text))
                {
                    if (lblPeriod.Text != DateTime.Now.ToString("MMMM yyyy"))
                    {
                        DateTime dt = Convert.ToDateTime(lblPeriod.Text);
                        this.StartDate = new DateTime(dt.Year, dt.Month, DateTime.Now.Day);

                    }
                    else
                    {
                        this.StartDate = DateTime.Now;

                    }
                    while (this.StartDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        this.StartDate = this.StartDate.AddDays(-1);
                    }
                    this.EndDate = this.StartDate.AddDays(6);

                    lblPeriod.Text = (this.StartDate.Year == this.EndDate.Year ? this.StartDate.ToString("MMMM dd - ") : this.StartDate.ToString("MMMM dd, yyyy - ")) + (this.StartDate.Month == this.EndDate.Month ? this.EndDate.ToString("dd, yyyy") : this.EndDate.ToString("MMMM dd, yyyy"));
                }


                pnlDetail.Height = 870;
                pnlDetail2.Height = 870;
                pnlCalendar.Height = 800;

                gvMonthly.Visible = false;
                gvWeekly.Visible = true;
                gvDaily.Visible = false;
            }
            else if (rdo.SelectedItem.Text == "Daily")
            {

                Session["SelectedType"] = "Daily";
                if (!string.IsNullOrEmpty(lblPeriod.Text))
                {
                    if (lblPeriod.Text.Split(' ')[0].ToString() == DateTime.Now.ToString("MMMM"))
                    {
                        this.StartDate = DateTime.Now;
                        this.EndDate = DateTime.Now;
                        lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        int iMonthNumber = DateTime.ParseExact(lblPeriod.Text.Split(' ')[0], "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                        this.StartDate = new DateTime(DateTime.Now.Year, iMonthNumber, DateTime.Now.Day);
                        this.EndDate = StartDate;
                        lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
                    }
                }


                pnlDetail.Height = 870;
                pnlDetail2.Height = 870;
                pnlCalendar.Height = 800;

                gvMonthly.Visible = false;
                gvWeekly.Visible = false;
                gvDaily.Visible = true;

            }

            calMain.SelectedDate = this.StartDate;
            calMain.VisibleDate = this.StartDate;

            BindGrid();
        }

        protected void gvWeekly_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ShowCasePopUp(e);
        }

        protected void gvDaily_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ShowCasePopUp(e);
        }

        protected void gvMonthly_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ShowCasePopUp(e);
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("Scheduler"))
            {
                //CanCancel = security.HasPermission("Scheduler");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void ShowCasePopUp(GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "ColumnClick")
            {
                string caseText = Request.Form["__EVENTARGUMENT"];
                long caseID = 0;

                if (caseText.Contains('-'))
                {

                    if ((Convert.ToDateTime(caseText) > DateTime.Now) || Convert.ToDateTime(caseText).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                    {
                        if (!Int64.TryParse(caseText, out caseID))
                        {
                            ucCasePopup.SurgeryDate = Convert.ToDateTime(caseText);
                        }
                        HiddenField hdnSelectedType = ucCasePopup.FindControl("hdnSelectedType") as HiddenField;
                        if (Session["SelectedType"] != null)
                        {
                            hdnSelectedType.Value = Session["SelectedType"].ToString() + "_" + lblPeriod.Text;
                        }
                        else
                        {
                            hdnSelectedType.Value = "Monthly" + "_" + lblPeriod.Text;
                        }
                        ucCasePopup.CaseId = caseID;
                        ucCasePopup.PopulateCase();
                        mpeSelectKit.Show();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), "ShowDateValidationMessage();", true);
                    }

                }
                else
                {
                    if (!Int64.TryParse(caseText, out caseID))
                    {
                        ucCasePopup.SurgeryDate = Convert.ToDateTime(caseText);
                    }
                    HiddenField hdnSelectedType = ucCasePopup.FindControl("hdnSelectedType") as HiddenField;
                    if (Session["SelectedType"] != null)
                    {
                        hdnSelectedType.Value = Session["SelectedType"].ToString() + "_" + lblPeriod.Text;
                    }
                    else
                    {
                        hdnSelectedType.Value = "Monthly" + "_" + lblPeriod.Text;
                    }
                    ucCasePopup.CaseId = caseID;
                    ucCasePopup.PopulateCase();
                    mpeSelectKit.Show();
                }
                
            }
            
        }

        protected override void Render(HtmlTextWriter writer)
        {
            GridViewRowCollection grdViewRows = null;
            if (rdo.SelectedItem.Text == "Monthly")
            {
                grdViewRows = gvMonthly.Rows;
            }
            else if (rdo.SelectedItem.Text == "Weekly")
            {
                grdViewRows = gvWeekly.Rows;
            }
            else if (rdo.SelectedItem.Text == "Daily")
            {
                grdViewRows = gvDaily.Rows;
            }
            foreach (GridViewRow r in grdViewRows)
            {
                if (r.RowType == DataControlRowType.DataRow && r.RowState == DataControlRowState.Alternate)
                {
                    for (int columnIndex = 0; columnIndex < r.Cells.Count; columnIndex++)
                    {
                        //Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$ctl00", columnIndex.ToString());
                        foreach (Control ctrl1 in r.Cells[columnIndex].Controls)
                        {
                            if (ctrl1 is Panel)
                            {
                                Page.ClientScript.RegisterForEventValidation(ctrl1.UniqueID);
                            }
                            if (ctrl1.HasControls() && ctrl1.Controls.Count > 0)
                            {
                                foreach (Control ctrl2 in ctrl1.Controls)
                                {
                                    if (ctrl2 is Panel)
                                    {
                                        Page.ClientScript.RegisterForEventValidation(ctrl2.UniqueID);
                                    }
                                    if (ctrl2.HasControls() && ctrl2.Controls.Count > 0)
                                    {
                                        foreach (Control ctrl3 in ctrl2.Controls)
                                        {
                                            if (ctrl3 is Panel)
                                            {
                                                Page.ClientScript.RegisterForEventValidation(ctrl3.UniqueID);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            base.Render(writer);
        }

        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            if (rdo.SelectedItem.Text == "Monthly")
            {
                this.StartDate = this.StartDate.AddMonths(1);
                this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
                Session["SelectedType"] = "Monthly";
            }
            else if (rdo.SelectedItem.Text == "Weekly")
            {
                this.StartDate = this.StartDate.AddDays(7);
                this.EndDate = this.StartDate.AddDays(6);
                lblPeriod.Text = (this.StartDate.Year == this.EndDate.Year ? this.StartDate.ToString("MMMM dd - ") : this.StartDate.ToString("MMMM dd, yyyy - ")) + (this.StartDate.Month == this.EndDate.Month ? this.EndDate.ToString("dd, yyyy") : this.EndDate.ToString("MMMM dd, yyyy"));
                Session["SelectedType"] = "Weekly";
            }
            else if (rdo.SelectedItem.Text == "Daily")
            {
                this.StartDate = this.StartDate.AddDays(1);
                this.EndDate = this.StartDate;
                lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
                Session["SelectedType"] = "Daily";
            }
            calMain.SelectedDate = this.StartDate;
            calMain.VisibleDate = this.StartDate;
            BindGrid();
        }

        protected void btnPrevious_Click(object sender, ImageClickEventArgs e)
        {
            if (rdo.SelectedItem.Text == "Monthly")
            {
                this.StartDate = this.StartDate.AddMonths(-1);
                this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
            }
            else if (rdo.SelectedItem.Text == "Weekly")
            {
                this.StartDate = this.StartDate.AddDays(-7);
                this.EndDate = this.StartDate.AddDays(6);
                lblPeriod.Text = (this.StartDate.Year == this.EndDate.Year ? this.StartDate.ToString("MMMM dd - ") : this.StartDate.ToString("MMMM dd, yyyy - ")) + (this.StartDate.Month == this.EndDate.Month ? this.EndDate.ToString("dd, yyyy") : this.EndDate.ToString("MMMM dd, yyyy"));
            }
            else if (rdo.SelectedItem.Text == "Daily")
            {
                this.StartDate = this.StartDate.AddDays(-1);
                this.EndDate = this.StartDate;
                lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
            }
            calMain.SelectedDate = this.StartDate;
            calMain.VisibleDate = this.StartDate;
            BindGrid();
        }

        protected void btnToday_Click(object sender, EventArgs e)
        {
            if (rdo.SelectedItem.Text == "Monthly")
            {
                this.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
            }
            else if (rdo.SelectedItem.Text == "Weekly")
            {
                this.StartDate = DateTime.Now;
                while (this.StartDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    this.StartDate = this.StartDate.AddDays(-1);
                }
                this.EndDate = this.StartDate.AddDays(6);
                lblPeriod.Text = (this.StartDate.Year == this.EndDate.Year ? this.StartDate.ToString("MMMM dd - ") : this.StartDate.ToString("MMMM dd, yyyy - ")) + (this.StartDate.Month == this.EndDate.Month ? this.EndDate.ToString("dd, yyyy") : this.EndDate.ToString("MMMM dd, yyyy"));
            }
            else if (rdo.SelectedItem.Text == "Daily")
            {
                this.StartDate = DateTime.Now;
                this.EndDate = DateTime.Now;
                lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
            }
            calMain.SelectedDate = this.StartDate;
            calMain.VisibleDate = this.StartDate;
            BindGrid();
        }

        protected void calMain_SelectionChanged(object sender, EventArgs e)
        {
            if (rdo.SelectedItem.Text == "Monthly")
            {
                this.StartDate = new DateTime(calMain.SelectedDate.Year, calMain.SelectedDate.Month, 1);
                this.EndDate = this.StartDate.AddMonths(1).AddDays(-1);
                lblPeriod.Text = this.StartDate.ToString("MMMM yyyy");
            }
            else if (rdo.SelectedItem.Text == "Weekly")
            {
                this.StartDate = calMain.SelectedDate;
                while (this.StartDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    this.StartDate = this.StartDate.AddDays(-1);
                }
                this.EndDate = this.StartDate.AddDays(6);
                lblPeriod.Text = (this.StartDate.Year == this.EndDate.Year ? this.StartDate.ToString("MMMM dd - ") : this.StartDate.ToString("MMMM dd, yyyy - ")) + (this.StartDate.Month == this.EndDate.Month ? this.EndDate.ToString("dd, yyyy") : this.EndDate.ToString("MMMM dd, yyyy"));
            }
            else if (rdo.SelectedItem.Text == "Daily")
            {
                this.StartDate = calMain.SelectedDate;
                this.EndDate = calMain.SelectedDate;
                lblPeriod.Text = this.StartDate.ToString("MMMM dd, yyyy");
            }
            BindGrid();
        }

        protected void gvMonthly_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Normal)
            {
                for (int index = 0; index < 7; index++)
                {
                    HiddenField hdnCase = (HiddenField)e.Row.Cells[index].FindControl("hdnCol" + (index + 1).ToString()) as HiddenField;

                    Label lblCaseNumber = new Label();
                    lblCaseNumber.Text = hdnCase.Value;
                    e.Row.Cells[index].Controls.Add(lblCaseNumber);
                    //e.Row.Cells[index].BackColor = Color.FromArgb(165, 191, 225);
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Alternate)
            {
                LinkButton newCase = (LinkButton)e.Row.Cells[7].Controls[0];
                string _jsNew = ClientScript.GetPostBackClientHyperlink(newCase, string.Empty);
                string _jsEdit = ClientScript.GetPostBackClientHyperlink(newCase, string.Empty);



                for (int index = 0; index < 7; index++)
                {
                    HiddenField hdnCase = (HiddenField)e.Row.Cells[index].FindControl("hdnCol" + (index + 1).ToString()) as HiddenField;

                    string caseText = hdnCase.Value;

                    if (caseText != "OTHERMONTH")
                    {
                        if (!caseText.StartsWith("|Date"))
                        {
                            string[] caseTextArray = caseText.Split('|');


                            if (caseTextArray.Length > 0)
                            {
                                Panel pnlMain = new Panel();
                                for (int i = 0; i < caseTextArray.Length - 1; i++)
                                {
                                    string[] caseArray = caseTextArray[i].Split(',');

                                    Panel pnlDetail = new Panel();

                                    Label lblCaseNumber = new Label();
                                    lblCaseNumber.Text = "#" + caseArray[0].Trim();
                                    pnlDetail.Controls.Add(lblCaseNumber);
                                    pnlDetail.Controls.Add(new LiteralControl("<br>"));

                                    Label lblHospital = new Label();
                                    lblHospital.Font.Size = FontUnit.Small;
                                    lblHospital.Text = caseArray[1].Trim();
                                    pnlDetail.Controls.Add(lblHospital);

                                    pnlDetail.BorderWidth = 1;
                                    pnlDetail.BorderColor = GetBorderColorByStatus(caseArray[2].Trim());

                                    pnlDetail.BackColor = GetBackColorByStatus(caseArray[2].Trim());
                                    pnlDetail.ForeColor = GetForeColorByStatus(caseArray[2].Trim());

                                    string js = _jsEdit.Insert(_jsEdit.Length - 2, caseArray[5].Trim());
                                    pnlDetail.Attributes["onclick"] = js;
                                    pnlDetail.Attributes["style"] += "cursor:pointer;cursor:hand;";

                                    pnlDetail.ToolTip =
                                        "Location: " + caseArray[3].Trim() + Environment.NewLine +
                                        "Procedure: " + caseArray[4].Trim();

                                    pnlMain.Controls.Add(pnlDetail);
                                }
                                Panel pnlEmpty = new Panel();
                                pnlEmpty.Height = 90 - (caseTextArray.Length > 2 ? 60 : caseTextArray.Length * 30);
                                pnlEmpty.Attributes["ondblclick"] = _jsNew.Insert(_jsNew.Length - 2, caseTextArray[caseTextArray.Length - 1].Replace("Date", string.Empty));
                                //pnlEmpty.Attributes["style"] += "cursor:pointer;cursor:hand;";
                                pnlMain.Controls.Add(pnlEmpty);

                                pnlMain.Height = 130;
                                pnlMain.ScrollBars = ScrollBars.Auto;
                                e.Row.Cells[index].Controls.Add(pnlMain);
                            }
                        }
                        else
                        {
                            Panel pnlMain = new Panel();
                            pnlMain.Height = 130;
                            
                            pnlMain.ScrollBars = ScrollBars.Auto;
                            e.Row.Cells[index].Controls.Add(pnlMain);
                            pnlMain.Attributes["ondblclick"] = _jsNew.Insert(_jsNew.Length - 2, caseText.Replace("|Date", string.Empty));
                            //pnlMain.Attributes["style"] += "cursor:pointer;cursor:hand;";
                        }
                    }
                    else
                    {
                        e.Row.Cells[index].BackColor = Color.FromArgb(183, 202, 202);
                    }
                }
            }
        }

        protected void gvWeekly_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int index = 0; index < 7; index++)
                {
                    Label lblHdr = e.Row.Cells[index].FindControl("lblCol" + (index + 1).ToString() + "Hdr") as Label;
                    lblHdr.Text = GetHeaderText(index);

                    if (DateTime.Now.ToString("MMMM") == lblPeriod.Text.Split(' ')[0])
                    {
                        if (lblHdr.Text == DateTime.Now.Day.ToString() + " - " + DateTime.Now.DayOfWeek.ToString())
                        {
                            e.Row.Cells[0].BorderColor = Color.Green;
                            e.Row.Cells[index].BackColor = Color.FromArgb(249, 216, 126);
                            e.Row.Cells[index].ForeColor = Color.Black;
                            e.Row.Cells[index].BorderStyle = BorderStyle.Solid;

                        }
                    }


                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton newCase = (LinkButton)e.Row.Cells[7].Controls[0];
                string _jsNew = ClientScript.GetPostBackClientHyperlink(newCase, string.Empty);
                string _jsEdit = ClientScript.GetPostBackClientHyperlink(newCase, string.Empty);

                for (int index = 0; index < 7; index++)
                {
                    HiddenField hdnCase = e.Row.Cells[index].FindControl("hdnCol" + (index + 1).ToString()) as HiddenField;

                    string caseText = hdnCase.Value;
                    if (caseText != string.Empty)
                    {

                        if (!caseText.StartsWith("|Date"))
                        {
                            string[] caseTextArray = caseText.Split('|');


                            if (caseTextArray.Length > 0)
                            {
                                Panel pnlMain = new Panel();
                                for (int i = 0; i < caseTextArray.Length - 1; i++)
                                {
                                    string[] caseArray = caseTextArray[i].Split(',');

                                    Panel pnlDetail = new Panel();

                                    Label lblCaseNumber = new Label();
                                    lblCaseNumber.Text = caseArray[0].Trim();
                                    pnlDetail.Controls.Add(lblCaseNumber);
                                    pnlDetail.Controls.Add(new LiteralControl("<br>"));

                                    Label lblHospital = new Label();
                                    lblHospital.Font.Size = FontUnit.Small;
                                    lblHospital.Text = caseArray[1].Trim();
                                    pnlDetail.Controls.Add(lblHospital);

                                    pnlDetail.BorderWidth = 1;
                                    pnlDetail.BorderColor = GetBorderColorByStatus(caseArray[2].Trim());

                                    pnlDetail.BackColor = GetBackColorByStatus(caseArray[2].Trim());
                                    pnlDetail.ForeColor = GetForeColorByStatus(caseArray[2].Trim());
                                    //e.Row.Cells[index].Controls.Add(pnlDetail);

                                    string js = _jsEdit.Insert(_jsEdit.Length - 2, caseArray[5].Trim());
                                    pnlDetail.Attributes["onclick"] = js;
                                    pnlDetail.Attributes["style"] += "cursor:pointer;cursor:hand;";

                                    pnlDetail.ToolTip =
                                        "Location: " + caseArray[3].Trim() + Environment.NewLine +
                                        "Procedure: " + caseArray[4].Trim();

                                    pnlMain.Controls.Add(pnlDetail);
                                }
                                Panel pnlEmpty = new Panel();
                                pnlEmpty.Height = 90 - (caseTextArray.Length > 2 ? 60 : caseTextArray.Length * 30);
                                pnlEmpty.Attributes["ondblclick"] = _jsNew.Insert(_jsNew.Length - 2, caseTextArray[caseTextArray.Length - 1].Replace("Date", string.Empty));
                                //pnlEmpty.Attributes["style"] += "cursor:pointer;cursor:hand;";
                                pnlMain.Controls.Add(pnlEmpty);

                                pnlMain.Height = 130;
                                pnlMain.ScrollBars = ScrollBars.Auto;
                                e.Row.Cells[index].Controls.Add(pnlMain);
                            }
                        }
                        else
                        {
                            Panel pnlMain = new Panel();
                            pnlMain.Height = 130;
                            pnlMain.ScrollBars = ScrollBars.Auto;
                            e.Row.Cells[index].Controls.Add(pnlMain);
                            pnlMain.Attributes["ondblclick"] = _jsNew.Insert(_jsNew.Length - 2, caseText.Replace("|Date", string.Empty));
                            //pnlMain.Attributes["style"] += "cursor:pointer;cursor:hand;";
                        }
                    }
                }
            }
        }

        protected void gvDaily_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Label lblHdr = e.Row.FindControl("lblHdr") as Label;
                lblHdr.Text = GetHeaderText();
                if (DateTime.Now.ToString("MMMM") == lblPeriod.Text.Split(' ')[0])
                {
                    if (lblHdr.Text == DateTime.Now.Day.ToString() + " - " + DateTime.Now.DayOfWeek.ToString())
                    {
                        e.Row.Cells[0].BorderColor = Color.Green;
                        e.Row.Cells[0].BackColor = Color.FromArgb(249, 216, 126);
                        e.Row.Cells[0].ForeColor = Color.Black;
                        e.Row.Cells[0].BorderStyle = BorderStyle.Solid;

                    }
                }


            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton newCase = (LinkButton)e.Row.Cells[1].Controls[0];
                string _jsNew = ClientScript.GetPostBackClientHyperlink(newCase, string.Empty);
                string _jsEdit = ClientScript.GetPostBackClientHyperlink(newCase, string.Empty);

                HiddenField hdnCase = e.Row.Cells[0].FindControl("hdnCol1") as HiddenField;

                string caseText = hdnCase.Value;
                if (caseText != string.Empty)
                {
                    if (!caseText.StartsWith("|Date"))
                    {
                        string[] caseTextArray = caseText.Split('|');



                        if (caseTextArray.Length > 0)
                        {
                            Panel pnlMain = new Panel();
                            for (int i = 0; i < caseTextArray.Length - 1; i++)
                            {
                                string[] caseArray = caseTextArray[i].Split(',');

                                Panel pnlDetail = new Panel();

                                Label lblCaseNumber = new Label();
                                lblCaseNumber.Text = caseArray[0].Trim();
                                pnlDetail.Controls.Add(lblCaseNumber);
                                pnlDetail.Controls.Add(new LiteralControl("<br>"));

                                Label lblHospital = new Label();
                                lblHospital.Font.Size = FontUnit.Small;
                                lblHospital.Text = caseArray[1].Trim();
                                pnlDetail.Controls.Add(lblHospital);

                                pnlDetail.BorderWidth = 1;
                                pnlDetail.BorderColor = GetBorderColorByStatus(caseArray[2].Trim());

                                pnlDetail.BackColor = GetBackColorByStatus(caseArray[2].Trim());
                                pnlDetail.ForeColor = GetForeColorByStatus(caseArray[2].Trim());
                                //e.Row.Cells[0].Controls.Add(pnlDetail);

                                string js = _jsEdit.Insert(_jsEdit.Length - 2, caseArray[5].Trim());
                                pnlDetail.Attributes["onclick"] = js;
                                pnlDetail.Attributes["style"] += "cursor:pointer;cursor:hand;";

                                pnlDetail.ToolTip =
                                    "Location: " + caseArray[3].Trim() + Environment.NewLine +
                                    "Procedure: " + caseArray[4].Trim();

                                pnlMain.Controls.Add(pnlDetail);
                            }
                            Panel pnlEmpty = new Panel();
                            pnlEmpty.Height = 90 - (caseTextArray.Length > 2 ? 60 : caseTextArray.Length * 30);
                            pnlEmpty.Attributes["ondblclick"] = _jsNew.Insert(_jsNew.Length - 2, caseTextArray[caseTextArray.Length - 1].Replace("Date", string.Empty));
                            //pnlEmpty.Attributes["style"] += "cursor:pointer;cursor:hand;";
                            pnlMain.Controls.Add(pnlEmpty);

                            pnlMain.Height = 130;
                            pnlMain.ScrollBars = ScrollBars.Auto;
                            e.Row.Cells[0].Controls.Add(pnlMain);
                        }
                    }
                    else
                    {
                        Panel pnlMain = new Panel();
                        pnlMain.Height = 130;
                        pnlMain.ScrollBars = ScrollBars.Auto;
                        e.Row.Cells[0].Controls.Add(pnlMain);
                        pnlMain.Attributes["ondblclick"] = _jsNew.Insert(_jsNew.Length - 2, caseText.Replace("|Date", string.Empty));
                        //pnlMain.Attributes["style"] += "cursor:pointer;cursor:hand;";
                    }
                }
            }
        }
        #endregion

        #region ICaseScheduleView Implementations
        public List<Users> SalesRepList
        {
            set
            {
                this.ddlSalesRep.DataSource = value;
                this.ddlSalesRep.DataTextField = "FullName";
                this.ddlSalesRep.DataValueField = "UserName";
                this.ddlSalesRep.DataBind();

                this.ddlSalesRep.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll")));
            }
        }

        public List<CaseStatusCls> CaseStatusList
        {
            set
            {
                this.ddlStatus.DataSource = value;
                this.ddlStatus.DataTextField = "CaseStatus";
                this.ddlStatus.DataValueField = "CaseStatus";
                this.ddlStatus.DataBind();

                this.ddlStatus.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll")));
            }
        }

        public List<Physician> PhysicianList
        {
            set
            {
                this.ddlPhysician.DataSource = value;
                this.ddlPhysician.DataTextField = "PhysicianName";
                this.ddlPhysician.DataValueField = "PhysicianName";
                this.ddlPhysician.DataBind();

                this.ddlPhysician.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll")));
            }
        }

        public string SalesRep
        {
            get
            {
                if (ddlSalesRep.SelectedIndex > 0)
                {
                    return ddlSalesRep.SelectedValue;
                }
                return null;
            }
            set
            {
                if (value == null)
                    ddlSalesRep.SelectedIndex = 0;
                else
                    ddlSalesRep.SelectedValue = value.Trim().ToString();
            }
        }

        public string Procedure
        {
            get
            {
                //if (ddlProcedure.SelectedIndex > 0)
                //{
                //    return ddlProcedure.SelectedValue;
                //}
                if (string.IsNullOrEmpty(txtProcedureName.Text))
                {
                    return null;
                }
                return txtProcedureName.Text.Trim();
            }
            set
            {
                //ddlProcedure.SelectedValue = value.Trim().ToString();
                txtProcedureName.Text = value.Trim().ToString();
            }
        }

        public Int64? Party
        {
            //get
            //{
            //    //if (ddlHospital.SelectedIndex > 0)
            //    //{
            //    //    return Convert.ToInt64(ddlHospital.SelectedValue);
            //    //}
            //    //return null;
            //    if (string.IsNullOrEmpty(txtHospital.Text))
            //    {
            //        return null;
            //    }
            //    return txtHospital.Text.Trim();
            //}
            //set
            //{
            //    if (value == 0 || value == null)
            //        ddlHospital.SelectedIndex = 0;
            //    else
            //        ddlHospital.SelectedValue = value.ToString();
            //}
            get
            {
                if (string.IsNullOrEmpty(hdnShipToPartyId.Value))
                    return null;
                else
                    return Convert.ToInt64(hdnShipToPartyId.Value);
            }
            set
            {
                txtHospital.Text = value.ToString();
            }
        }

        public string CaseStatus
        {
            get
            {
                if (ddlStatus.SelectedIndex > 0)
                {
                    return ddlStatus.SelectedValue.ToString();
                }
                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    ddlStatus.SelectedIndex = 0;
                else
                    ddlStatus.SelectedValue = value.ToString();
            }
        }

        public string Physician
        {
            get
            {
                if (ddlPhysician.SelectedIndex > 0)
                {
                    return ddlPhysician.SelectedValue;
                }
                return null;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    ddlPhysician.SelectedIndex = 0;
                else
                    ddlPhysician.SelectedValue = value.Trim().ToString();
            }
        }





        #endregion

    }
}