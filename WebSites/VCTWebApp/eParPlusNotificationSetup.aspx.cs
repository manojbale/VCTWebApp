using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using VCTWeb.Core.Domain;
using VCTWebApp.Resources;
using VCTWebApp.Shell.Views;
using VCTWebApp.Web;
using AjaxControlToolkit;

namespace VCTWebApp
{
    public partial class EParPlusNotificationSetup : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusNotificationSetup
    {
        #region Instance Variables
        private eParPlusNotificationSetupPresenter _presenter;
        private readonly VCTWebAppResource _vctResource = new VCTWebAppResource();
        private Security _security;
        #endregion

        #region Init/Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblError.Text = string.Empty;
                AuthorizedPage();
                _presenter.OnViewInitialized();
                pnlEmailDetails.Enabled = false;
                ResetControl();
            }
            LocalizePage();
        }

        #endregion Init/Page Load

        #region Create New Presenter
        [CreateNew]
        public eParPlusNotificationSetupPresenter Presenter
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

        #region Private Methods

        private void AuthorizedPage()
        {
            _security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (_security.HasAccess("ePar+.NotificationSetup"))
            {
                //CanCancel = security.HasPermission("PARLevel");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            Page.Title = lblHeader.Text = _vctResource.GetString("mnueParNotificationSetup");
        }

        private void ResetControl()
        {
            SelectedNotificationDetailId = 0;
            ddlReceiverType.SelectedIndex = 0;
            txtlblConfigurationSetting.Text = "7";
            txtReceiverEmailId.Text = string.Empty;
            txtReceiverName.Text = string.Empty;
            txtReceiverEmailId.Enabled = false;
            txtReceiverName.Enabled = false;
            pnlNotificationSelection.Enabled = false;
            ddlNotificationType.SelectedIndex = 0;
            gdvNotification.EditIndex = -1;
            gdvNotification.DataSource = null;
            gdvNotification.DataBind();
            //pnlDays.Enabled = false;
            foreach (ListItem item in chkDays.Items)
            {
                item.Selected = false;
            }

        }

        private void RefreshControl()
        {
            ResetControl();
            lstExistingCustomers.SelectedIndex = -1;
        }

        private bool ValidateControls()
        {
            var bretVal = true;
            lblError.Text = string.Empty;

            if (lstExistingCustomers.SelectedIndex < 0)
            {
                lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valSelectCustomer"), lblHeader.Text) + "</font>";
                return false;
            }

            if (ddlNotificationType.SelectedIndex <= 0)
            {
                lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valSelectNotificationType"), lblHeader.Text) + "</font>";
                return false;
            }

            if (ddlReceiverType.SelectedIndex <= 0)
            {
                lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valSelectReceiverType"), lblHeader.Text) + "</font>";
                return false;
            }

            if (string.IsNullOrEmpty(txtReceiverName.Text.Trim()))
            {
                lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valEnterReceiverName"), lblHeader.Text) + "</font>";
                return false;
            }

            if (string.IsNullOrEmpty(txtReceiverEmailId.Text.Trim()))
            {
                lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valEnterReceiverEmailId"), lblHeader.Text) + "</font>";
                return false;
            }

            if (!string.IsNullOrEmpty(txtReceiverEmailId.Text.Trim()))
            {
                const string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                var match = Regex.Match(txtReceiverEmailId.Text.Trim(), pattern, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("validEmailId"), lblHeader.Text) + "</font>";
                    return false;
                }
            }

            if (txtlblConfigurationSetting.Visible)
            {
                if (string.IsNullOrEmpty(txtlblConfigurationSetting.Text.Trim()))
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valEnterBelowParLevelDays"), lblHeader.Text) + "</font>";
                    return false;
                }

                if (!string.IsNullOrEmpty(txtlblConfigurationSetting.Text.Trim()))
                {
                    if (Convert.ToInt32(ConfigurationSetting) <= 0)
                    {
                        lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valBelowParLevelDaysZero"), lblHeader.Text) + "</font>";
                        return false;
                    }
                }
            }
            else if (chkDays.Visible)
            {
                List<string> selectedValueList = GetSelectedDaysFromCheckBoxList(chkDays);
                if (selectedValueList.Count <= 0)
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valSelectNotificationDays"), lblHeader.Text) + "</font>";
                    return false;
                }
            }

            var emailid = txtReceiverEmailId.Text.Trim();
            if (NotificationDetailList.Count > 0)
            {
                if (NotificationDetailList.FindIndex(x => x.ReceiverEmailID.Trim().ToUpper() == emailid.Trim().ToUpper()) >= 0)
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valEmailAlreadyExists"), lblHeader.Text) + "</font>";
                    bretVal = false;
                }
            }
            return bretVal;
        }

        private List<string> GetSelectedDaysFromCheckBoxList(CheckBoxList checkBoxList)
        {
            List<string> selectedValueList = new List<string>();

            if (checkBoxList != null)
            {
                foreach (ListItem item in checkBoxList.Items)
                {
                    if (item.Selected)
                    {
                        selectedValueList.Add(item.Value);
                    }
                }
            }
            return selectedValueList;
        }

        private void SetSelectedDaysToCheckBoxList(CheckBoxList checkBoxList, string[] arrSelectedValues)
        {
            if (checkBoxList != null)
            {
                for (int i = 0; i < checkBoxList.Items.Count; i++)
                {
                    checkBoxList.Items[i].Selected = false;
                }

                if (arrSelectedValues != null)
                {
                    foreach (string value in arrSelectedValues)
                    {
                        string dayNo = GetDayNumber(value);

                        if (checkBoxList.Items.FindByValue(dayNo) != null)
                        {
                            checkBoxList.Items.FindByValue(dayNo).Selected = true;
                        }
                    }
                }
            }
        }

        private string GetDayNumber(string day)
        {
            string dayNumber = string.Empty;
            switch (day.Trim().ToUpper())
            {
                case "SUN":
                    dayNumber = "1";
                    break;
                case "MON":
                    dayNumber = "2";
                    break;
                case "TUE":
                    dayNumber = "3";
                    break;
                case "WED":
                    dayNumber = "4";
                    break;
                case "THU":
                    dayNumber = "5";
                    break;
                case "FRI":
                    dayNumber = "6";
                    break;
                case "SAT":
                    dayNumber = "7";
                    break;
            }
            return dayNumber;
        }

        private bool ValidateItemsOnEdit(string configurationValues, bool isCheckBox)
        {
            if (string.IsNullOrEmpty(configurationValues))
            {
                if (isCheckBox)
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valConfigurationSettingDays"), lblHeader.Text);
                else
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valConfigurationSetting"), lblHeader.Text);
                return false;
            }

            if (!isCheckBox)
            {
                if (Convert.ToInt32(configurationValues) < 1)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("valQuantityValue"), lblHeader.Text);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region IeParPlusNotificationSetup Members

        List<VCTWeb.Core.Domain.Customer> IeParPlusNotificationSetup.CustomerList
        {
            set
            {
                lstExistingCustomers.DataSource = value;
                lstExistingCustomers.DataTextField = "NameAccount";
                lstExistingCustomers.DataValueField = "AccountNumber";
                lstExistingCustomers.DataBind();
            }
        }

        public List<NotificationDetail> NotificationDetailList
        {
            set
            {
                string configurationSettingHeader = string.Empty;

                gdvNotification.PageSize = PageSize;
                List<NotificationDetail> notificationlist = value;

                if (notificationlist != null && notificationlist.Count > 0)
                {
                    Notification notification = ListNotification.Find(x => x.NotificationType == SelectedNotificationType);

                    foreach (NotificationDetail objNotificationDetail in notificationlist)
                    {
                        if (notification != null)
                        {
                            configurationSettingHeader = notification.ConfigurationSettingHeader;
                            if (!notification.IsTextBoxEditable)
                            {
                                if (notification.NotificationType.Trim().ToUpper() == "NOCYCLECOUNTNOTIFICATION")
                                {
                                    string keyValue = Presenter.FetchDictionaryKeyValue("NoCycleCountNotificationInHours");
                                    if (!string.IsNullOrEmpty(keyValue.Trim()))
                                        objNotificationDetail.ConfigurationSetting = keyValue;
                                }
                            }
                            if (notification.ShowDays)
                            {
                                string[] arrDays = objNotificationDetail.ConfigurationSetting.Split(',');
                                if (arrDays.Length > 0)
                                {
                                    List<string> dayNameList = new List<string>();
                                    foreach (string day in arrDays)
                                    {
                                        switch (day.Trim().ToUpper())
                                        {
                                            case "1":
                                                dayNameList.Add("Sun");
                                                break;
                                            case "2":
                                                dayNameList.Add("Mon");
                                                break;
                                            case "3":
                                                dayNameList.Add("Tue");
                                                break;
                                            case "4":
                                                dayNameList.Add("Wed");
                                                break;
                                            case "5":
                                                dayNameList.Add("Thu");
                                                break;
                                            case "6":
                                                dayNameList.Add("Fri");
                                                break;
                                            case "7":
                                                dayNameList.Add("Sat");
                                                break;
                                        }
                                    }
                                    if (dayNameList.Count > 0)
                                    {
                                        var daynames = string.Join(",", dayNameList);
                                        objNotificationDetail.ConfigurationSetting = daynames;
                                    }
                                }

                            }

                            if (!notification.IsTimerEditable)
                            {
                                objNotificationDetail.TimeToSendEmail = "00:00";
                            }
                        }
                    }
                }
                gdvNotification.DataSource = notificationlist;
                gdvNotification.DataBind();

                if (!string.IsNullOrEmpty(configurationSettingHeader))
                    gdvNotification.HeaderRow.Cells[4].Text = configurationSettingHeader;


                ViewState["NotificationDetailList"] = notificationlist;
            }
            get
            {
                return ViewState["NotificationDetailList"] as List<NotificationDetail>;
            }
        }

        public List<Notification> ListNotification
        {
            set
            {
                ddlNotificationType.DataSource = null;
                ddlNotificationType.DataSource = value;
                Session["NotificationType"] = value;
                ddlNotificationType.DataTextField = "Description";
                ddlNotificationType.DataValueField = "NotificationType";
                ddlNotificationType.DataBind();
                ddlNotificationType.Items.Insert(0, new ListItem(_vctResource.GetString("listItemSelect"), "0"));
            }
            get
            {
                return Session["NotificationType"] as List<Notification>;
            }
        }

        public string ReceiverType
        {
            get
            {
                if (ddlReceiverType.SelectedIndex > 0)
                    return ddlReceiverType.Text.Trim();
                return string.Empty;
            }
            set
            {
                if (value != null)
                    ddlReceiverType.SelectedValue = value;
            }
        }

        public string ReceiverName
        {
            get
            {
                return txtReceiverName.Text.Trim();
            }
            set
            {
                if (value != null)
                    txtReceiverName.Text = value;
            }
        }

        public string ReceiverEmailId
        {
            get
            {
                return txtReceiverEmailId.Text.Trim();
            }
            set
            {
                if (value != null)
                    txtReceiverEmailId.Text = value;
            }
        }

        public string Time
        {
            get
            {
                var localDateTime = ctlTime.Date;
                var uTcDateTime = ctlTime.Date;
                if ((System.Web.HttpContext.Current.Session != null) && (System.Web.HttpContext.Current.Session["TimeZoneOffset"] != null))
                {
                    double timeZoneOffset = double.Parse(System.Web.HttpContext.Current.Session["TimeZoneOffset"].ToString());
                    uTcDateTime = localDateTime.AddMinutes(timeZoneOffset);
                }
                return uTcDateTime.ToString("HH:mm");
            }
            set
            {
                if (value != null)
                {
                    var str = value.Split(':');
                    if (str.Length > 1)
                    {
                        var dtTimeToSendEmail = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), 0);
                        ctlTime.Date = dtTimeToSendEmail;
                    }
                }
            }
        }

        public string ConfigurationSetting
        {
            get
            {
                string configurationSetting = string.Empty;
                if (txtlblConfigurationSetting.Visible)
                {
                    configurationSetting = txtlblConfigurationSetting.Text.Trim();
                }
                else
                {
                    List<string> selectedValueList = GetSelectedDaysFromCheckBoxList(chkDays);
                    if (selectedValueList.Count > 0)
                    {
                        configurationSetting = string.Join(",", selectedValueList);
                    }
                }
                return configurationSetting;
            }
            set
            {
                txtlblConfigurationSetting.Text = Convert.ToString(value);
            }
        }

        public long SelectedNotificationDetailId { get; set; }

        string IeParPlusNotificationSetup.SelectedAccountNumber
        {
            get
            {
                return lstExistingCustomers.SelectedValue;
            }
        }

        public int PageSize
        {
            set
            {
                ViewState["PageSize"] = value;
            }
            get
            {
                return (int)ViewState["PageSize"];
            }
        }

        public string SelectedNotificationType
        {
            get
            {
                return ddlNotificationType.SelectedIndex > 0 ? Convert.ToString(ddlNotificationType.SelectedValue) : string.Empty;
            }
            set
            {
                try
                {
                    ddlNotificationType.Text = value;
                }
                catch { }
            }
        }

        #endregion

        #region Events

        protected void gdvNotification_OnPaging(object sender, GridViewPageEventArgs e)
        {
            gdvNotification.EditIndex = -1;
            gdvNotification.PageIndex = e.NewPageIndex;
            NotificationDetailList = NotificationDetailList;
        }

        protected void gdvNotification_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gdvNotification.EditIndex = e.NewEditIndex;
                NotificationDetailList = NotificationDetailList;
                var lblNotificationDetailId = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("lblNotificationDetailId") as Label;
                if (lblNotificationDetailId != null)
                {
                    var index = NotificationDetailList.FindIndex(x => x.NotificationDetailId == Convert.ToInt64(lblNotificationDetailId.Text.Trim()));
                    if (index >= 0)
                    {
                        var configurationSetting = NotificationDetailList[index].ConfigurationSetting;
                        var timeToSendEmail = NotificationDetailList[index].TimeToSendEmail;
                        var ctlTimeEdit = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("ctlTimeEdit") as MKB.TimePicker.TimeSelector;
                        var txtTimeEdit = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("txtTimeEdit") as TextBox;
                        var txtConfigurationSettingEdit = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("txtConfigurationSettingEdit") as TextBox;
                        var chkConfigurationSettingEdit = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("chkConfigurationSettingEdit") as CheckBoxList;
                        var filteredTextBoxExtender = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("txtFilteredTextBoxExtender") as FilteredTextBoxExtender;



                        if (txtConfigurationSettingEdit != null)
                            txtConfigurationSettingEdit.Visible = false;
                        if (chkConfigurationSettingEdit != null)
                            chkConfigurationSettingEdit.Visible = false;
                        if (filteredTextBoxExtender != null)
                            filteredTextBoxExtender.Enabled = false;

                        if (ctlTimeEdit != null && !string.IsNullOrEmpty(timeToSendEmail))
                        {
                            var str = timeToSendEmail.Split(':');
                            if (str.Length > 1)
                            {
                                var dtTimeToSendEmail = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(str[0]), Convert.ToInt32(str[1]), 0);
                                ctlTimeEdit.Date = dtTimeToSendEmail;
                            }
                        }
                        var notification = ListNotification.Find(x => x.NotificationType == SelectedNotificationType);
                        if (notification != null)
                        {
                            if (!string.IsNullOrEmpty(notification.ConfigurationSettingHeader))
                                gdvNotification.HeaderRow.Cells[4].Text = notification.ConfigurationSettingHeader;

                            if (notification.ShowDays)
                            {
                                if (txtConfigurationSettingEdit != null)
                                {
                                    txtConfigurationSettingEdit.Visible = false;
                                    if (filteredTextBoxExtender != null) filteredTextBoxExtender.Enabled = false;
                                }
                                if (chkConfigurationSettingEdit != null) chkConfigurationSettingEdit.Visible = true;
                                var selectedValues = configurationSetting.Trim();
                                var arrValues = selectedValues.Split(',');
                                SetSelectedDaysToCheckBoxList(chkConfigurationSettingEdit, arrValues.Length > 0 ? arrValues : null);
                            }
                            else
                            {
                                if (txtConfigurationSettingEdit != null)
                                {
                                    txtConfigurationSettingEdit.Visible = true;
                                    if (filteredTextBoxExtender != null) filteredTextBoxExtender.Enabled = true;
                                }
                                if (chkConfigurationSettingEdit != null)
                                {
                                    chkConfigurationSettingEdit.Visible = false;
                                    if (filteredTextBoxExtender != null) filteredTextBoxExtender.Enabled = false;
                                }
                            }

                            if (notification.IsTextBoxEditable)
                            {
                                txtlblConfigurationSetting.ReadOnly = false;
                            }
                            else
                            {
                                if (txtConfigurationSettingEdit != null)
                                {
                                    txtConfigurationSettingEdit.ReadOnly = true;
                                    if (notification.NotificationType.Trim().ToUpper() == "NOCYCLECOUNTNOTIFICATION")
                                    {
                                        string keyValue = Presenter.FetchDictionaryKeyValue("NoCycleCountNotificationInHours");
                                        if (!string.IsNullOrEmpty(keyValue.Trim()))
                                            txtConfigurationSettingEdit.Text = keyValue;
                                    }
                                }
                            }

                            if (notification.IsTimerEditable)
                            {
                                if (ctlTimeEdit != null) ctlTimeEdit.Visible = true;
                                if (txtTimeEdit != null) txtTimeEdit.Visible = false;
                            }
                            else
                            {
                                if (ctlTimeEdit != null) ctlTimeEdit.Visible = false;
                                if (txtTimeEdit != null) txtTimeEdit.Visible = true;
                            }
                        }

                        if (!string.IsNullOrEmpty(configurationSetting) && txtConfigurationSettingEdit != null)
                            txtConfigurationSettingEdit.Text = Convert.ToString(configurationSetting);

                        if (txtConfigurationSettingEdit != null)
                            txtConfigurationSettingEdit.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void gdvNotification_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gdvNotification.EditIndex = -1;
                NotificationDetailList = NotificationDetailList;
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void gdvNotification_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("DeleteRec"))
                {
                    var notificationDetailId = Convert.ToInt32(e.CommandArgument);
                    if (notificationDetailId <= 0) return;
                    Presenter.Delete(notificationDetailId);
                    gdvNotification.DataSource = null;
                    gdvNotification.DataBind();
                    Presenter.FetchAllNotificationDetailByAccountNumber();
                }
                else if (e.CommandName.Equals("UpdateRec"))
                {
                    var lblNotificationDetailId = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("lblNotificationDetailId") as Label;
                    var txtConfigurationSettingEdit = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("txtConfigurationSettingEdit") as TextBox;
                    var ctlTimeEdit = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("ctlTimeEdit") as MKB.TimePicker.TimeSelector;
                    var chkConfigurationSettingEdit = gdvNotification.Rows[gdvNotification.EditIndex].FindControl("chkConfigurationSettingEdit") as CheckBoxList;


                    if (lblNotificationDetailId != null && txtConfigurationSettingEdit != null && ctlTimeEdit != null)
                    {
                        var localDateTime = ctlTimeEdit.Date;
                        var uTcDateTime = ctlTime.Date;
                        if ((System.Web.HttpContext.Current.Session != null) && (System.Web.HttpContext.Current.Session["TimeZoneOffset"] != null))
                        {
                            var timeZoneOffset = double.Parse(System.Web.HttpContext.Current.Session["TimeZoneOffset"].ToString());
                            uTcDateTime = localDateTime.AddMinutes(timeZoneOffset);
                        }

                        var configurationSetting = string.Empty;

                        var notification = ListNotification.Find(x => x.NotificationType == SelectedNotificationType);
                        if (notification != null)
                        {
                            if (!string.IsNullOrEmpty(notification.ConfigurationSettingHeader))
                                gdvNotification.HeaderRow.Cells[4].Text = notification.ConfigurationSettingHeader;

                            if (notification.ShowDays)
                            {
                                if (chkConfigurationSettingEdit != null)
                                {
                                    configurationSetting = "";
                                    List<string> selectedValueList = GetSelectedDaysFromCheckBoxList(chkConfigurationSettingEdit);
                                    if (selectedValueList.Count > 0)
                                        configurationSetting = string.Join(",", selectedValueList);
                                }
                            }
                            else
                            {
                                configurationSetting = txtConfigurationSettingEdit.Text.Trim();
                            }
                        }


                        if (notification != null && ValidateItemsOnEdit(configurationSetting, notification.ShowDays))
                        {
                            if (Presenter.Update(Convert.ToInt64(lblNotificationDetailId.Text), configurationSetting, uTcDateTime.ToString("HH:mm")))
                            {
                                lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("msgNotificationDetailUpdate"), lblHeader.Text) + "</font>";
                                NotificationDetailList[gdvNotification.EditIndex].ConfigurationSetting = Convert.ToString(configurationSetting);
                                NotificationDetailList[gdvNotification.EditIndex].TimeToSendEmail = Convert.ToString(ctlTimeEdit.Hour) + ":" + Convert.ToString(ctlTimeEdit.Minute);
                                gdvNotification.EditIndex = -1;
                                Presenter.FetchAllNotificationDetailByAccountNumber();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
            {
                if (Presenter.Save())
                {
                    var selectedNotification = SelectedNotificationDetailId;
                    Presenter.FetchAllNotificationDetailByAccountNumber();
                    RefreshControl();
                    if (selectedNotification > 0)
                    {
                        lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("msgUpdated"), lblHeader.Text) + "</font>";
                    }
                    else
                    {
                        lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("msgCreated"), lblHeader.Text) + "</font>";
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Presenter.OnViewInitialized();
            RefreshControl();
            lblError.Text = string.Empty;
            pnlEmailDetails.Enabled = false;
        }

        protected void lstExistingCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ResetControl();
                lblError.Text = string.Empty;
                pnlNotificationSelection.Enabled = true;
                //Presenter.FetchAllNotificationDetailByAccountNumber();
                //pnlEmailDetails.Enabled = true;
            }
            catch (SqlException ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlReceiverType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtReceiverEmailId.Text = string.Empty;
                txtReceiverName.Text = string.Empty;
                lblError.Text = string.Empty;
                if (ddlReceiverType.SelectedIndex > 0)
                {
                    if (ddlReceiverType.SelectedValue.Trim().ToUpper() == "OTHER")
                    {
                        txtReceiverEmailId.Enabled = true;
                        txtReceiverName.Enabled = true;
                    }
                    else if (ddlReceiverType.SelectedValue.Trim().ToUpper() == "LOCAL REP")
                    {
                        txtReceiverEmailId.Enabled = false;
                        txtReceiverName.Enabled = false;
                        Presenter.FetchSalesRepresentativeForCustomer();
                        txtReceiverEmailId.Enabled = (txtReceiverEmailId.Text.Trim() == string.Empty);
                        txtReceiverName.Enabled = (txtReceiverName.Text.Trim() == string.Empty);

                    }
                    else if (ddlReceiverType.SelectedValue.Trim().ToUpper() == "REGIONAL REP")
                    {
                        txtReceiverEmailId.Enabled = false;
                        txtReceiverName.Enabled = false;
                        Presenter.FetchManagerForCustomer();
                        txtReceiverEmailId.Enabled = (txtReceiverEmailId.Text.Trim() == string.Empty);
                        txtReceiverName.Enabled = (txtReceiverName.Text.Trim() == string.Empty);
                    }
                    else if (ddlReceiverType.SelectedValue.Trim().ToUpper() == "SPECIALIST REP")
                    {
                        txtReceiverEmailId.Enabled = false;
                        txtReceiverName.Enabled = false;
                        Presenter.FetchSpecialistRepForCustomer();
                        txtReceiverEmailId.Enabled = (txtReceiverEmailId.Text.Trim() == string.Empty);
                        txtReceiverName.Enabled = (txtReceiverName.Text.Trim() == string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlNotificationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                string selectedNotification = ddlNotificationType.SelectedValue;

                pnlEmailDetails.Enabled = false;
                lblConfigurationSetting1.Text = string.Empty;
                lblConfigurationSetting2.Text = string.Empty;
                txtlblConfigurationSetting.Text = string.Empty;

                lblConfigurationSetting1.Visible = false;
                lblConfigurationSetting2.Visible = false;
                txtlblConfigurationSetting.Visible = false;
                chkDays.Visible = false;

                Notification notification = ListNotification.Find(x => x.NotificationType == selectedNotification);
                Presenter.FetchAllNotificationDetailByAccountNumber();

                if (notification != null)
                {
                    if (!string.IsNullOrEmpty(notification.ConfigurationSettingHeader))
                        gdvNotification.HeaderRow.Cells[4].Text = notification.ConfigurationSettingHeader;

                    pnlEmailDetails.Enabled = true;

                    bool isConfigurationSettingVisible = false;

                    if (!string.IsNullOrEmpty(notification.LabelText.Trim()))
                    {
                        var arrList = Regex.Split(notification.LabelText.Trim(), "{TEXTBOX}");
                        if (arrList.Length >= 2)
                        {
                            lblConfigurationSetting1.Text = arrList[0];
                            lblConfigurationSetting2.Text = arrList[1];
                            txtlblConfigurationSetting.Text = notification.TextBoxDefaultValue;
                            txtlblConfigurationSetting.Visible = true;
                            isConfigurationSettingVisible = true;
                        }
                        else if (arrList.Length == 1)
                        {
                            isConfigurationSettingVisible = false;
                            lblConfigurationSetting1.Text = arrList[0];
                            txtlblConfigurationSetting.Visible = false;
                        }
                    }

                    if (notification.ShowDays)
                    {
                        lblConfigurationSetting1.Visible = false;
                        lblConfigurationSetting2.Visible = false;
                        txtlblConfigurationSetting.Visible = false;
                        //pnlDays.Enabled = chkDays.Visible = true;
                        chkDays.Visible = true;
                    }
                    else
                    {
                        lblConfigurationSetting1.Visible = true;
                        lblConfigurationSetting2.Visible = true;
                        txtlblConfigurationSetting.Visible = true;
                        //pnlDays.Enabled = chkDays.Visible = false;
                        chkDays.Visible = false;
                    }
                    
                    txtlblConfigurationSetting.Visible = isConfigurationSettingVisible;

                    if (notification.IsTimerEditable)
                    {
                        txtTime.Visible = false;
                        ctlTime.Visible = true;
                    }
                    else
                    {
                        txtTime.Visible = true;
                        ctlTime.Visible = false;
                    }

                    if (notification.IsTextBoxEditable)
                        txtlblConfigurationSetting.ReadOnly = false;
                    else
                    {
                        txtlblConfigurationSetting.ReadOnly = true;
                        if (notification.NotificationType.Trim().ToUpper() == "NOCYCLECOUNTNOTIFICATION")
                        {
                            string keyValue = Presenter.FetchDictionaryKeyValue("NoCycleCountNotificationInHours");
                            if (!string.IsNullOrEmpty(keyValue.Trim()))
                                txtlblConfigurationSetting.Text = keyValue;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        #endregion

    }
}