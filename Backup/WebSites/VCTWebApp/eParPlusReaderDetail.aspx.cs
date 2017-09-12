using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Microsoft.Practices.CompositeWeb.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public partial class eParPlusReaderDetail : Page, IeParPlusReaderDetailView
    {
        #region Instance Variables
        private eParPlusReaderDetailPresenter _presenter;
        #endregion

        #region Create New Presenter

        [CreateNew]
        public eParPlusReaderDetailPresenter Presenter
        {
            get
            {
                return _presenter;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["CustomerShelfId"] != null)
                {
                    var customerShelfId = Session["CustomerShelfId"];
                    if (customerShelfId != null)
                    {
                        CustomerShelfId = Convert.ToInt16(customerShelfId);
                        _presenter.OnViewInitialized();
                    }
                }
            }
        }

        protected void btnReStart_Click(object sender, EventArgs e)
        {
            //Update Re-Start Flag Here.
        }

        protected void gdvReaderDeatil_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var objCustomerShelfProperty = (CustomerShelfProperty)e.Row.DataItem;
                if (objCustomerShelfProperty.IsEditable)
                {
                    var rdoTrue = e.Row.FindControl("rdoTrue") as RadioButton;
                    var rdofalse = e.Row.FindControl("rdofalse") as RadioButton;
                    var txtModifiedPropertyValue = e.Row.FindControl("txtModifiedPropertyValue") as TextBox;
                    var ddlModifiedPropertyValue = e.Row.FindControl("ddlModifiedPropertyValue") as DropDownList;
                    var txtFilteredTextBoxExtender = e.Row.FindControl("txtFilteredTextBoxExtender") as FilteredTextBoxExtender;
                    var imgInformation = e.Row.FindControl("imgInformation") as Image;

                    if (txtModifiedPropertyValue != null) txtModifiedPropertyValue.Visible = false;
                    if (ddlModifiedPropertyValue != null) ddlModifiedPropertyValue.Visible = false;
                    if (rdoTrue != null) rdoTrue.Visible = false;
                    if (rdofalse != null) rdofalse.Visible = false;
                    if (txtFilteredTextBoxExtender != null) txtFilteredTextBoxExtender.Enabled = false;

                    if (imgInformation != null)
                    {
                        imgInformation.ToolTip = objCustomerShelfProperty.DataType + " : " + objCustomerShelfProperty.ListValues;
                        imgInformation.Visible = true;
                    }

                    switch (objCustomerShelfProperty.DataType.ToUpper().Trim())
                    {
                        case "INT":
                            if (txtModifiedPropertyValue != null)
                            {
                                txtModifiedPropertyValue.Visible = true;
                                if (txtFilteredTextBoxExtender != null)
                                {
                                    txtFilteredTextBoxExtender.FilterMode = FilterModes.ValidChars;
                                    txtFilteredTextBoxExtender.FilterType = FilterTypes.Custom;
                                    txtFilteredTextBoxExtender.ValidChars = "0123456789-";
                                    txtFilteredTextBoxExtender.Enabled = true;
                                }

                                if (objCustomerShelfProperty.MaximumLength > 0)
                                    txtModifiedPropertyValue.MaxLength = objCustomerShelfProperty.MaximumLength;

                                if (objCustomerShelfProperty.ModifiedPropertyValue != null && !string.IsNullOrEmpty(objCustomerShelfProperty.ModifiedPropertyValue.Trim()))
                                    txtModifiedPropertyValue.Text = objCustomerShelfProperty.ModifiedPropertyValue.Trim();
                                else
                                    txtModifiedPropertyValue.Text = objCustomerShelfProperty.PropertyValue.Trim();
                            }
                            break;

                        case "STRING":
                            if (txtModifiedPropertyValue != null)
                            {
                                txtModifiedPropertyValue.Visible = true;
                                if (objCustomerShelfProperty.MaximumLength > 0)
                                    txtModifiedPropertyValue.MaxLength = objCustomerShelfProperty.MaximumLength;

                                if (txtFilteredTextBoxExtender != null)
                                {
                                    txtFilteredTextBoxExtender.FilterMode = FilterModes.ValidChars;
                                    txtFilteredTextBoxExtender.FilterType = FilterTypes.Custom;
                                    txtFilteredTextBoxExtender.ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
                                    txtFilteredTextBoxExtender.Enabled = true;
                                }

                                if (objCustomerShelfProperty.ModifiedPropertyValue != null && !string.IsNullOrEmpty(objCustomerShelfProperty.ModifiedPropertyValue.Trim()))
                                    txtModifiedPropertyValue.Text = objCustomerShelfProperty.ModifiedPropertyValue.Trim();
                                else
                                    txtModifiedPropertyValue.Text = objCustomerShelfProperty.PropertyValue.Trim();
                            }
                            break;

                        case "BOOLEAN":
                            if (rdoTrue != null && rdofalse != null)
                            {
                                rdoTrue.Visible = true;
                                rdofalse.Visible = true;

                                if (objCustomerShelfProperty.ModifiedPropertyValue != null && !string.IsNullOrEmpty(objCustomerShelfProperty.ModifiedPropertyValue.Trim()))
                                {
                                    if (objCustomerShelfProperty.ModifiedPropertyValue.Trim().ToUpper() == "TRUE")
                                        rdoTrue.Checked = true;
                                    else
                                        rdofalse.Checked = true;
                                }
                                else
                                {
                                    if (objCustomerShelfProperty.PropertyValue.Trim().ToUpper() == "TRUE")
                                        rdoTrue.Checked = true;
                                    else
                                        rdofalse.Checked = true;
                                }
                            }
                            break;

                        case "LIST":
                            if (ddlModifiedPropertyValue != null)
                            {
                                string[] arrList = null;
                                ddlModifiedPropertyValue.Visible = true;
                                string listValue = objCustomerShelfProperty.ListValues;
                                if (!string.IsNullOrEmpty(listValue))
                                {
                                    arrList = listValue.Split(',');
                                    if (arrList.Length > 0)
                                    {
                                        foreach (string itemValue in arrList)
                                            ddlModifiedPropertyValue.Items.Add(itemValue);
                                    }
                                }

                                if (objCustomerShelfProperty.ModifiedPropertyValue != null && !string.IsNullOrEmpty(objCustomerShelfProperty.ModifiedPropertyValue.Trim()))
                                {
                                    if (arrList != null && arrList.Length > 0)
                                        ddlModifiedPropertyValue.Text = objCustomerShelfProperty.ModifiedPropertyValue;
                                }
                                else
                                {
                                    if (arrList != null && arrList.Length > 0)
                                        ddlModifiedPropertyValue.Text = objCustomerShelfProperty.PropertyValue;
                                }
                            }
                            break;
                    }
                }
            }
        }

        protected void grdAntennaDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CustomerShelfAntennaProperty objCustomerShelfAntennaProperty = (CustomerShelfAntennaProperty)e.Row.DataItem;

                if (objCustomerShelfAntennaProperty.IsEditableAntenna)
                {

                    TextBox txtPropertyValue = e.Row.FindControl("txtModifiedPropertyValueAntenna") as TextBox;
                    DropDownList ddlPropertyValue = e.Row.FindControl("ddlModifiedPropertyValueAntenna") as DropDownList;
                    RadioButton rdoTrue = e.Row.FindControl("rdoTrueAntenna") as RadioButton;
                    RadioButton rdofalse = e.Row.FindControl("rdofalseAntenna") as RadioButton;
                    FilteredTextBoxExtender txtFilteredTextBoxExtender = e.Row.FindControl("txtFilteredTextBoxExtenderAntenna") as FilteredTextBoxExtender;
                    Image imgInformation = e.Row.FindControl("imgInformationAntenna") as Image;

                    if (txtPropertyValue != null)
                        txtPropertyValue.Visible = false;

                    if (ddlPropertyValue != null)
                        ddlPropertyValue.Visible = false;

                    if (rdoTrue != null)
                        rdoTrue.Visible = false;

                    if (rdofalse != null)
                        rdofalse.Visible = false;

                    if (txtFilteredTextBoxExtender != null)
                        txtFilteredTextBoxExtender.Enabled = false;

                    if (imgInformation != null)
                    {
                        imgInformation.ToolTip = objCustomerShelfAntennaProperty.DataTypeAntenna + " : " + objCustomerShelfAntennaProperty.ListValuesAntenna;
                        imgInformation.Visible = true;
                    }

                    switch (objCustomerShelfAntennaProperty.DataTypeAntenna.ToUpper().Trim())
                    {

                        case "INT":
                            if (txtPropertyValue != null)
                            {
                                txtPropertyValue.Visible = true;
                                if (txtFilteredTextBoxExtender != null)
                                {
                                    txtFilteredTextBoxExtender.FilterMode = FilterModes.ValidChars;
                                    txtFilteredTextBoxExtender.FilterType = FilterTypes.Custom;
                                    txtFilteredTextBoxExtender.ValidChars = "0123456789-";
                                    txtFilteredTextBoxExtender.Enabled = true;
                                }

                                if (objCustomerShelfAntennaProperty.MaximumLengthAntenna > 0)
                                    txtPropertyValue.MaxLength = objCustomerShelfAntennaProperty.MaximumLengthAntenna;

                                if (objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna != null && string.IsNullOrEmpty(objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim()))
                                    txtPropertyValue.Text = objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim();
                                else
                                    txtPropertyValue.Text = objCustomerShelfAntennaProperty.PropertyValueAntenna.Trim();
                            }
                            break;

                        case "STRING":
                            if (txtPropertyValue != null)
                            {
                                txtPropertyValue.Visible = true;
                                if (objCustomerShelfAntennaProperty.MaximumLengthAntenna > 0)
                                    txtPropertyValue.MaxLength = objCustomerShelfAntennaProperty.MaximumLengthAntenna;

                                if (txtFilteredTextBoxExtender != null)
                                {
                                    txtFilteredTextBoxExtender.FilterMode = FilterModes.ValidChars;
                                    txtFilteredTextBoxExtender.FilterType = FilterTypes.Custom;
                                    txtFilteredTextBoxExtender.ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
                                    txtFilteredTextBoxExtender.Enabled = true;
                                }


                                if (objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna != null && string.IsNullOrEmpty(objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim()))
                                    txtPropertyValue.Text = objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim();
                                else
                                    txtPropertyValue.Text = objCustomerShelfAntennaProperty.PropertyValueAntenna.Trim();
                            }
                            break;

                        case "BOOLEAN":
                            if (rdoTrue != null && rdofalse != null)
                            {
                                rdoTrue.Visible = true;
                                rdofalse.Visible = true;

                                if (objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna != null && string.IsNullOrEmpty(objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim()))
                                {
                                    if (objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim().ToUpper() == "TRUE")
                                        rdoTrue.Checked = true;
                                    else
                                        rdofalse.Checked = true;
                                }
                                else
                                {
                                    if (objCustomerShelfAntennaProperty.PropertyValueAntenna.Trim().ToUpper() == "TRUE")
                                        rdoTrue.Checked = true;
                                    else
                                        rdofalse.Checked = true;
                                }
                            }
                            break;

                        case "LIST":
                            if (ddlPropertyValue != null)
                            {
                                string[] arrList = null;
                                ddlPropertyValue.Visible = true;
                                string listValue = objCustomerShelfAntennaProperty.ListValuesAntenna;
                                if (!string.IsNullOrEmpty(listValue))
                                {
                                    arrList = listValue.Split(',');
                                    if (arrList.Length > 0)
                                    {
                                        foreach (string itemValue in arrList)
                                            ddlPropertyValue.Items.Add(itemValue);
                                    }
                                }

                                if (objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna != null && string.IsNullOrEmpty(objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim()))
                                {
                                    if (arrList != null && arrList.Length > 0)
                                        ddlPropertyValue.Text = objCustomerShelfAntennaProperty.ModifiedPropertyValueAntenna;
                                }
                                else
                                {
                                    if (arrList != null && arrList.Length > 0)
                                        ddlPropertyValue.Text = objCustomerShelfAntennaProperty.PropertyValueAntenna;
                                }
                            }
                            break;
                    }


                    //switch (objCustomerShelfAntennaProperty.DataTypeAntenna.ToUpper().Trim())
                    //{
                    //    case "PERCENTAGE":
                    //        txtPropertyValue.Visible = true;
                    //        txtPropertyValue.MaxLength = 3;
                    //        txtFilteredTextBoxExtender.FilterMode = FilterModes.ValidChars;
                    //        txtFilteredTextBoxExtender.FilterType = FilterTypes.Custom;
                    //        txtFilteredTextBoxExtender.ValidChars = "0123456789";
                    //        txtFilteredTextBoxExtender.Enabled = true;
                    //        break;

                    //    case "INT":
                    //        txtPropertyValue.Visible = true;
                    //        txtFilteredTextBoxExtender.FilterMode = FilterModes.ValidChars;
                    //        txtFilteredTextBoxExtender.FilterType = FilterTypes.Custom;
                    //        txtFilteredTextBoxExtender.ValidChars = "0123456789-";
                    //        txtFilteredTextBoxExtender.Enabled = true;
                    //        string listValues = objCustomerShelfAntennaProperty.ListValuesAntenna;
                    //        if (!string.IsNullOrEmpty(listValues))
                    //        {
                    //            //Range -127 To 127
                    //            listValues = listValues.Replace("Range ", "");
                    //            string[] arrList = Regex.Split(listValues, "To");
                    //            if (arrList.Length > 0)
                    //            {

                    //            }
                    //        }
                    //        break;

                    //    case "STRING":
                    //        txtPropertyValue.Visible = true;
                    //        txtFilteredTextBoxExtender.FilterMode = FilterModes.ValidChars;
                    //        txtFilteredTextBoxExtender.FilterType = FilterTypes.Custom;
                    //        txtFilteredTextBoxExtender.ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
                    //        txtFilteredTextBoxExtender.Enabled = true;
                    //        break;

                    //    case "BOOLEAN":
                    //        rdoTrue.Visible = true;
                    //        rdofalse.Visible = true;
                    //        if (objCustomerShelfAntennaProperty.PropertyValueAntenna.Trim().ToUpper() == "TRUE")
                    //            rdoTrue.Checked = true;
                    //        else
                    //            rdofalse.Checked = true;
                    //        break;

                    //    case "LIST":
                    //        ddlPropertyValue.Visible = true;
                    //        string listValue = objCustomerShelfAntennaProperty.ListValuesAntenna;
                    //        if (!string.IsNullOrEmpty(listValue))
                    //        {
                    //            string[] arrList = listValue.Split(',');
                    //            if (arrList.Length > 0)
                    //            {
                    //                foreach (string itemValue in arrList)
                    //                    ddlPropertyValue.Items.Add(itemValue);
                    //                //ddlPropertyValue.DataSource = arrList;
                    //                //ddlPropertyValue.DataBind();
                    //            }
                    //        }
                    //        break;
                    //}

                }
            }
        }

        protected void ddlAntenna_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAntenna.SelectedIndex > 0)
            {
                SelectedCustomerShelfAntennaId = Convert.ToInt16(ddlAntenna.SelectedValue);
                _presenter.GetAntennaProperties();

                int index = ListOfCustomerShelfAntennaProperty.FindIndex(x => x.PropertyNameAntenna.Trim().ToUpper() == "ANTENNASTATUS");
                if (index >= 0)
                {
                    imgAntennaStatus.Visible = true;
                    if (ListOfCustomerShelfAntennaProperty[index].PropertyValueAntenna.Trim().ToUpper() == "CONNECTED")
                    {
                        imgAntennaStatus.ImageUrl = "~/Images/LedGreen.png";
                        imgAntennaStatus.ToolTip = "Antenna Connected";
                    }
                    else
                    {
                        imgAntennaStatus.ImageUrl = "~/Images/LedRed.png";
                        imgAntennaStatus.ToolTip = "Antenna Disconnected";
                    }
                }
                else
                {
                    imgAntennaStatus.Visible = false;
                }
            }
            else
            {
                imgAntennaStatus.Visible = false;
                ListOfCustomerShelfAntennaProperty = new List<CustomerShelfAntennaProperty>();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateControls())
                {
                    Presenter.SaveModifiedReaderAntennaValues();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SetReaderModifiedValuesToList()
        {
            for (int rowIndex = 0; rowIndex < gdvReaderDeatil.Rows.Count; rowIndex++)
            {
                var rdoTrue = gdvReaderDeatil.Rows[rowIndex].FindControl("rdoTrue") as RadioButton;
                var rdofalse = gdvReaderDeatil.Rows[rowIndex].FindControl("rdofalse") as RadioButton;
                var txtModifiedPropertyValue = gdvReaderDeatil.Rows[rowIndex].FindControl("txtModifiedPropertyValue") as TextBox;
                var ddlModifiedPropertyValue = gdvReaderDeatil.Rows[rowIndex].FindControl("ddlModifiedPropertyValue") as DropDownList;
                var hndReaderPropertyId = gdvReaderDeatil.Rows[rowIndex].FindControl("hndReaderPropertyId") as HiddenField;

                if (hndReaderPropertyId != null)
                {
                    int readerPropertyId = Convert.ToInt32(hndReaderPropertyId.Value);
                    int index = ListOfCustomerShelfProperty.FindIndex(x => x.ReaderPropertyId == readerPropertyId);
                    if (index >= 0)
                    {
                        var objCustomerShelfProperty = ListOfCustomerShelfProperty[index];
                        if (objCustomerShelfProperty.IsEditable)
                        {
                            switch (objCustomerShelfProperty.DataType.ToUpper().Trim())
                            {
                                case "STRING":
                                case "INT":
                                    if (txtModifiedPropertyValue != null)
                                        ListOfCustomerShelfProperty[index].ModifiedPropertyValue = txtModifiedPropertyValue.Text.Trim();
                                    break;

                                case "BOOLEAN":
                                    if (rdoTrue != null && rdofalse != null)
                                        ListOfCustomerShelfProperty[index].ModifiedPropertyValue = rdoTrue.Checked ? "True" : "False";
                                    break;

                                case "LIST":
                                    if (ddlModifiedPropertyValue != null)
                                        ListOfCustomerShelfProperty[index].ModifiedPropertyValue = ddlModifiedPropertyValue.Text.Trim();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private bool ValidateControls()
        {
            bool isReaderValidated = true;
            bool isValueEdited = false;
            try
            {
                lblError.Text = string.Empty;
                SetReaderModifiedValuesToList();

                if (ListOfCustomerShelfProperty.Any())
                {
                    foreach (var customerShelfProperty in ListOfCustomerShelfProperty)
                    {
                        if (customerShelfProperty.IsEditable)
                        {
                            if (!string.IsNullOrEmpty(customerShelfProperty.ModifiedPropertyValue.Trim()))
                            {
                                if (customerShelfProperty.PropertyValue.Trim().ToUpper() != customerShelfProperty.ModifiedPropertyValue.Trim().ToUpper())
                                {
                                    isValueEdited = true;
                                }
                            }
                        }
                    }
                    if (!isValueEdited)
                    {
                        lblError.Text = "Please enter modified property value for RFID reader.";
                        isReaderValidated = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return isReaderValidated;
        }



        protected void btnReset_Click(object sender, EventArgs e)
        {
            _presenter.OnViewInitialized();
        }

        #endregion Events

        #region IeParPlusReaderDetailView Implementations

        public int CustomerShelfId
        {
            set
            {
                lblCustomerShelfId.Text = Convert.ToString(value);
            }
            get
            {
                return Convert.ToInt16(lblCustomerShelfId.Text);
            }
        }

        public string AccountNumber
        {
            set { lblAccountNumber.Text = value; }
            get { return lblAccountNumber.Text.Trim(); }
        }

        public string ShelfName
        {
            set { lblShelfName.Text = value; }
            get { return lblShelfName.Text.Trim(); }
        }

        public string ShelfCode
        {
            set { lblShelfCode.Text = value; }
            get { return lblShelfCode.Text.Trim(); }
        }

        public string ReaderHealthLastUpdatedOn
        {
            set
            {
                lblHealthLastUpdatedOn.Text = value;
            }
        }

        public string ReaderIP
        {
            set
            {
                lblReaderIP.Text = value;
            }
        }

        public string TotalAntenna
        {
            set
            {
                lblTotalAntenna.Text = value;
            }
        }

        public List<CustomerShelfProperty> ListOfCustomerShelfProperty
        {
            set
            {
                Session["CustomerShelfProperty"] = value;
                gdvReaderDeatil.DataSource = value;
                gdvReaderDeatil.DataBind();
            }
            get
            {
                return Session["CustomerShelfProperty"] as List<CustomerShelfProperty>;
            }
        }

        public List<CustomerShelfAntennaProperty> ListOfCustomerShelfAntennaProperty
        {
            set
            {
                Session["CustomerShelfAntennaProperty"] = value;
                grdAntennaDetails.DataSource = value;
                grdAntennaDetails.DataBind();
            }
            get
            {
                return Session["CustomerShelfAntennaProperty"] as List<CustomerShelfAntennaProperty>;
            }
        }

        public List<CustomerShelfAntenna> ListOfDistinctAntenna
        {
            get
            {
                return Session["DistinctAntenna"] as List<CustomerShelfAntenna>;
            }
            set
            {
                Session["DistinctAntenna"] = value;
                ddlAntenna.DataSource = value;
                ddlAntenna.DataValueField = "CustomerShelfAntennaId";
                ddlAntenna.DataTextField = "AntennaName";
                ddlAntenna.DataBind();
            }
        }

        public int SelectedCustomerShelfAntennaId { get; set; }

        #endregion IeParPlusReaderDetailView Implementations
    }
}