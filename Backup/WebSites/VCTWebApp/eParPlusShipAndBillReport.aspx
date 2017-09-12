<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusShipAndBillReport.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.eParPlusShipAndBillReport" Title="Ship & Bill Report"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var updm1 = Sys.WebForms.PageRequestManager.getInstance();
        updm1.add_endRequest(function () 
        {});

        $j = $.noConflict();
        $j(function () 
        {
            $j(window).load(function () 
            {});
            var btnCancelId = "", remarks = "", Qty = 0, DispositionType = 0, RemainingQty = 0, saveFlag = false;
            $j('.ValidateCancel').live("click", function (e) {

                debugger;
                RemainingQty = this.nextElementSibling.value;
                ShowPopup();

                btnCancelId = $j(this).attr('id');

                remarks = $j('#txtRemarks').val();
                DispositionType = $j('#ddlDispositionType').val();
                Qty = $j('#txtQty').val();

                $j('#txtRemainingQty').val(RemainingQty);

                if ($j('#ddlDispositionType').val() == 0) {
                    if (saveFlag) {
                        alert("Please select Disposition Type.");
                        $j('#ddlDispositionType').focus();
                    }
                    $j('#ContainerBox').fadeIn('slow');
                    return false;
                }


                if ($j('#txtQty').val() == 0) {
                    if (saveFlag) {
                        alert("Please enter Adjusted quantity.");
                        $j('#txtQty').focus();
                    }
                    $j('#ContainerBox').fadeIn('slow');
                    return false;
                }

                var qtyTemp = parseInt($j('#txtQty').val());
                var remQtyTemp = parseInt($j('#txtRemainingQty').val());

                if (qtyTemp > remQtyTemp) {
                    if (saveFlag) {
                        alert("Adjusted Qty should be less than Remaining quantity.");
                        $j('#txtQty').focus();
                        return false;
                    }
                    $j('#ContainerBox').fadeIn('slow');
                    return false;
                }

                $j('#hdnRemarks').val(remarks);
                $j('#hdnDispositionTypeId').val(DispositionType);
                $j('#hndQty').val(Qty);

                saveFlag = false;
                $j('#ddlDispositionType').val('0');
                $j('#txtRemarks').val('');
                //$j('#txtQty').val('0');

                $j('#ContainerBox').fadeOut('slow');
            });

            function ShowPopup() {
                $j('#ContainerBox').fadeIn('slow');
                var totWidth = $j(window).width();
                var totHeight = $j(window).height();
                var boxWidth = $j('#ConfirmBox').width() / 2;
                var boxHeight = $j('#ConfirmBox').height() / 2;
                var netWidth = totWidth / 2 - boxWidth;
                var netHeight = totHeight / 2 - (boxHeight + 80);
                $j('#ContainerBox').css({ 'width': totWidth + 'px', 'height': totHeight - 2 + 'px' });
                $j('#ConfirmBox').css('left', '' + netWidth + 'px').css('top', '' + netHeight + 'px');
            }

            $j('#btnSaveRemarks').click(function () {
                if ($('#ddlDispositionType option:selected').text() == "Others" && $('#txtRemarks').val() == "") {
                    alert("Remarks required for Other Disposition type.");
                    $j('#txtRemarks').focus();
                }
                else {
                    saveFlag = true;
                    $j('#' + btnCancelId).click();
                }
                return false;
            });


            $j('#btnCancelRemarks').click(function () {
                saveFlag = false;
                $j('#txtRemarks').val('');
                $j('#ddlDispositionType').val('0');
                //$j('#txtQty').val('0');
                $j('#ContainerBox').fadeOut('slow');
                return false;
            });


            $j(".ExpandRow").live("click", function () {

                if ($j(this).attr("src").toLowerCase() == "images/plus.png") {
                    $j(this).next().show();
                    $j(this).closest("tr").after("<td style='width:3%'></td><td colspan = '999'>" + $j(this).next().html() + "</td>");
                    $j(this).next().hide();
                    $j(this).attr("src", "images/minus.png");
                }
                else {
                    $j(this).attr("src", "images/plus.png");
                    $j(this).closest("tr").next().next().hide();
                    $j(this).closest("tr").next().hide();
                }
            });
        });
      
    </script>
    <div id="ContainerBox">
        <div id="ConfirmBox" class="view-confirm-box" style="height: auto;">
            <table width="450px" cellpadding="0" cellspacing="5" style="padding: 10px 0 10px 0;">
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td colspan="2" style="color: Black; font-size: 11pt; text-decoration: underline;"
                        align="center">
                        Order Adjustment
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%;">
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="left" style="width: 35%;">
                        Disposition Type :
                    </td>
                    <td align="left" style="width: 55%;">
                        <asp:DropDownList ID="ddlDispositionType" ClientIDMode="Static" runat="server" Width="313px">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="asfas" Value="1"></asp:ListItem>
                            <asp:ListItem Text="asfasdsfsd" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Others" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%;">
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="left" style="width: 35%;">
                        Remarks :
                    </td>
                    <td style="width: 55%">
                        <asp:TextBox ID="txtRemarks" ClientIDMode="Static" TextMode="MultiLine" Width="308px"
                            Rows="3" runat="server" Height="45px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%;">
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="left" style="width: 35%;">
                        Remaining Qty :
                    </td>
                    <td style="width: 55%">
                        <asp:TextBox ID="txtRemainingQty" Enabled="false" ClientIDMode="Static" Width="302px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%;">
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="left" style="width: 35%;">
                        Qty :
                    </td>
                    <td align="left" style="width: 55%;">
                        <asp:TextBox ID="txtQty" ClientIDMode="Static" Width="302px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:Button ID="btnSaveRemarks" ClientIDMode="Static" runat="server" Text="Save" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancelRemarks" ClientIDMode="Static" runat="server" Text="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%;">
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnDispositionTypeId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnRemarks" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hndQty" ClientIDMode="Static" runat="server" />
        </div>
    </div>
   
   
   
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <caption>
                                <tr class="header">
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblHeader" runat="server" Text="Consumption Rate Report"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="8">
                                        <asp:Panel ID="pnlCustomerFilterSection" runat="server" GroupingText="Customer" HorizontalAlign="Left"
                                            Width="100%">
                                            <table>
                                                <tr>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblCustomerNameFilter" runat="server" Text="Customer Name" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="imgHelp1" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Customer Name" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblBranchAgencyFilter" runat="server" Text="Branch/ Agency" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Branch/ Agency" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblManagerFilter" runat="server" Text="Regional Rep" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Regional Rep" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblSalesRepresentativeFilter" runat="server" Text="Local Rep"
                                                            CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="Image9" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Local Rep" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblStateFilter" runat="server" Text="State" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Customer State" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblOwnershipStructureFilter" runat="server" Text="Ownership Structure"
                                                            CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Ownership Structure" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblManagementStructureFilter" runat="server" Text="Management Structure"
                                                            CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Management Structure" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlCustomerNameFilter" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlCustomerNameFilter_SelectedIndexChanged" Width="150px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlBranchAgencyFilter" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlBranchAgencyFilter_SelectedIndexChanged" Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlManagerFilter" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlManagerFilter_SelectedIndexChanged" Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlSalesRepresentativeFilter" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlSalesRepresentativeFilter_SelectedIndexChanged" CssClass="ListBox"
                                                            Width="100px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlStateFilter" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlStateFilter_SelectedIndexChanged" Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlOwnershipStructureFilter" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlOwnershipStructureFilter_SelectedIndexChanged" CssClass="ListBox"
                                                            Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlManagementStructureFilter" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="ddlManagementStructureFilter_SelectedIndexChanged" CssClass="ListBox"
                                                            Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlProduct" runat="server" GroupingText="Products" HorizontalAlign="Left"
                                            Width="100%">
                                            <table>
                                                <tr>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblProductLine" runat="server" Text="Product Line" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="imgProductLine" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Product Line" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblCategory" runat="server" Text="Category" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="imgCategory" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Product Category" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblSubCategory1" runat="server" Text="Sub Category 1" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="imgSubCategory1" runat="server" ImageUrl="~/Images/Help-icon.png"
                                                            BorderStyle="None" ToolTip="Select Product Sub Category 1" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblSubCategory2" runat="server" Text="Sub Category 2" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="imgSubCategory2" runat="server" ImageUrl="~/Images/Help-icon.png"
                                                            BorderStyle="None" ToolTip="Select Product Sub Category 2" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:Label ID="lblSubCategory3" runat="server" Text="Sub Category 3" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="imgSubCategory3" runat="server" ImageUrl="~/Images/Help-icon.png"
                                                            BorderStyle="None" ToolTip="Select Product Sub Category 3" />
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlProductLine" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlProductLine_SelectedIndexChanged" Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlSubCategory1" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlSubCategory1_SelectedIndexChanged" Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlSubCategory2" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            OnSelectedIndexChanged="ddlSubCategory2_SelectedIndexChanged" Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlSubCategory3" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                            Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                    </td>
                                                    <td align="left" style="width: 110px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlOrderDates" runat="server" GroupingText="Date Range" HorizontalAlign="Left"
                                            Width="100%">
                                            <table>
                                                <tr>
                                                    <td colspan="8">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label4" runat="server" Text="Order Date" CssClass="listboxheading"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rbOrderDateLastOneWeek" AutoPostBack="true" Checked="true" Text="Last One Week" GroupName="GrpOrderDateShippedDate" OnCheckedChanged="rbOrderDateLastOneWeek_OnCheckedChanged" />
                                                                    <asp:RadioButton runat="server" ID="rbOrderDateLastOneMonth" AutoPostBack="true" Checked="false" Text="Last One Month" GroupName="GrpOrderDateShippedDate" OnCheckedChanged="rbOrderDateLastOneMonth_OnCheckedChanged" />
                                                                    <asp:RadioButton runat="server" ID="rbOrderDateRange" AutoPostBack="true" Checked="false" Text="Date Range" GroupName="GrpOrderDateShippedDate" OnCheckedChanged="rbOrderDateRange_OnCheckedChanged" />
                                                                    <%--<asp:RadioButtonList runat="server" ID="rblPeriodOrder" AutoPostBack="True" OnSelectedIndexChanged="rblPeriodOrder_SelectedIndexChanged"
                                                                        RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Last One Week" Selected="True" />
                                                                        <asp:ListItem Text="Last One Month" />
                                                                        <asp:ListItem Text="Date Range" />
                                                                    </asp:RadioButtonList>--%>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblOrderStartDate" Text="Start Date:&nbsp;" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOrderStartDate" runat="server" Width="100" Enabled="false" Text=""
                                                                        ClientIDMode="Static" />
                                                                    <asp:Image ID="imgOrderCalenderFrom" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtenderFrom1" runat="server" PopupButtonID="imgOrderCalenderFrom"
                                                                        TargetControlID="txtOrderStartDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblOrderEndDate" Text="End Date:&nbsp;" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtOrderEndDate" runat="server" Width="100" Text="" Enabled="false"
                                                                        ClientIDMode="Static" />
                                                                    <asp:Image ID="imgOrderCalenderTo" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgOrderCalenderTo"
                                                                        TargetControlID="txtOrderEndDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label5" runat="server" Text="Shipped Date" CssClass="listboxheading"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton runat="server" ID="rbShippedDateLastOneWeek" AutoPostBack="true" Checked="false" Text="Last One Week" GroupName="GrpOrderDateShippedDate" OnCheckedChanged="rbShippedDateLastOneWeek_OnCheckedChanged" />
                                                                    <asp:RadioButton runat="server" ID="rbShippedDateLastOneMonth" AutoPostBack="true" Checked="false" Text="Last One Month" GroupName="GrpOrderDateShippedDate" OnCheckedChanged="rbShippedDateLastOneMonth_OnCheckedChanged" />
                                                                    <asp:RadioButton runat="server" ID="rbShippedDateRange" AutoPostBack="true" Checked="false" Text="Date Range" GroupName="GrpOrderDateShippedDate" OnCheckedChanged="rbShippedDateRange_OnCheckedChanged" />
                                                                    <%--<asp:RadioButtonList runat="server" ID="rblPeriodShipped" AutoPostBack="True" OnSelectedIndexChanged="rblPeriodShipped_SelectedIndexChanged"
                                                                        RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Last One Week" Selected="True" />
                                                                        <asp:ListItem Text="Last One Month" />
                                                                        <asp:ListItem Text="Date Range" />
                                                                    </asp:RadioButtonList>--%>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblShippedStartDate" Text="Start Date:&nbsp;" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtShippedStartDate" runat="server" Width="100" Enabled="false"
                                                                        Text="" ClientIDMode="Static" />
                                                                    <asp:Image ID="imgShippedCalenderFrom" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgShippedCalenderFrom"
                                                                        TargetControlID="txtShippedStartDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblShippedEndDate" Text="End Date:&nbsp;" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtShippedEndDate" runat="server" Width="100" Text="" Enabled="false"
                                                                        ClientIDMode="Static" />
                                                                    <asp:Image ID="imgShippedCalenderTo" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgShippedCalenderTo"
                                                                        TargetControlID="txtShippedEndDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkFilterData" runat="server" OnClick="lnkFilterCustomerListData_Click"
                                                                        CausesValidation="false">
                                                                        <asp:Image ID="ImageView" runat="server" ImageUrl="~/Images/view_small.png" BorderStyle="None"
                                                                            ToolTip="View" /></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="lnkRefresh_Click" CausesValidation="false">
                                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/refresh_small.png" BorderStyle="None"
                                                                            ToolTip="Reset" /></asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CausesValidation="false">
                                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/download.png" BorderStyle="None"
                                                                            ToolTip="Export to Excel" /></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top" colspan="0">
                                        <asp:Panel ID="pnlGrid" runat="server" CssClass="pnlGrid" Height="300px" ScrollBars="Auto"
                                            ClientIDMode="Static" Width="99.5%">
                                            <asp:GridView ID="gdvShipandBill" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false"
                                                SkinID="GridView" Width="99%" OnRowDataBound="gdvShipandBill_RowDataBound" OnSorting="gdvShipandBill_Sorting"
                                                OnRowCommand="gdvShipandBill_RowCommand" AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                Style="cursor: pointer; vertical-align: top;" />
                                                            <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Account Number" HeaderStyle-Width="1%" Visible="false"
                                                                            ItemStyle-Width="1%" ItemStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("OrderId") %>'> </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="DispositionType" SortExpression="DispositionType" HeaderStyle-Width="20%"
                                                                            ItemStyle-Width="20%" HeaderText="Disposition Type" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="Remarks" SortExpression="Remarks" HeaderStyle-Width="40%"
                                                                            ItemStyle-Width="40%" HeaderText="Remarks" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="AdjustQty" SortExpression="Qty" HeaderStyle-Width="10%"
                                                                            ItemStyle-Width="10%" HeaderText="Qty" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="UpdatedBy" SortExpression="UpdatedBy" HeaderStyle-Width="15%"
                                                                            ItemStyle-Width="15%" HeaderText="Adjusted By" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:TemplateField HeaderText="Adjusted On" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                                            ItemStyle-HorizontalAlign="Center" SortExpression="ExpiryDt">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUpdatedOn" runat="server" Text='<%# Convert.ToDateTime(Eval("UpdatedOn"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Account Number" Visible="false" ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAccountNumber" runat="server" Text='<%# Eval("AccountNumber") %>'> </asp:Label>
                                                            <asp:Label ID="lblOrderId" runat="server" Text='<%# Eval("OrderId") %>'> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CustomerName" SortExpression="CustomerName" HeaderStyle-Width="11%" ItemStyle-Width="11%" HeaderStyle-HorizontalAlign="Center" HeaderText="Customer Name" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="OrderNumber" SortExpression="OrderNumber" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Order #" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="LineNumber" SortExpression="LineNumber" HeaderStyle-Width="4%" ItemStyle-Width="4%" HeaderStyle-HorizontalAlign="Center" HeaderText="Line #" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="RefNum" SortExpression="RefNum" HeaderStyle-Width="9%" ItemStyle-Width="9%" HeaderStyle-HorizontalAlign="Center" HeaderText="Ref #" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="OrderedQty" SortExpression="OrderedQty" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Order Qty" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ShippedQty" SortExpression="ShippedQty" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Shipped Qty" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="CancelledQty" SortExpression="CancelledQty" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Cancelled Qty" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="OrderStatus" SortExpression="OrderStatus" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" HeaderText="Order Status" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="OrderDate" DataFormatString="{0:d}" SortExpression="OrderDate" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" HeaderText="Ordered Date" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ShippedDate" DataFormatString="{0:d}" SortExpression="ShippedDate" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" HeaderText="Shipped Date" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="ReceivedQty" SortExpression="ReceivedQty" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Recd. Qty" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="AdjustQty" SortExpression="AdjustQty" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Adjusted Qty" ItemStyle-HorizontalAlign="Center" />
                                                    
                                                    <asp:BoundField DataField="RemainingQty" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Rem. Qty" ItemStyle-HorizontalAlign="Center" HeaderStyle-BackColor="#244061" />
                                                    
                                                    
                                                    <asp:TemplateField HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center">
                                                        <HeaderTemplate>
                                                                <asp:Label ID="lblActionHeader" runat="server" Text="Adjust" Font-Bold="true"/>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkAdjust" runat="server" Height="25px" ImageUrl="~/images/Adjust_Icon.png"
                                                                CommandName="SaveAdjustment" Width="25px" CommandArgument='<%# Eval("OrderId") %>'
                                                                AlternateText='<%# Eval("RemainingQty") %>' CssClass="ValidateCancel" ToolTip="Adjust remaining quantity." />
                                                            
                                                            <asp:HiddenField ID="hndRemainingQty" Value='<%# Eval("RemainingQty") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                               
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <%-- <br />
                                        <asp:Label ID="Label2" Text="* Remaining Qty = Shipped Qty - Received Qty - Adjusted Qty"
                                            ForeColor="Red" runat="server" />--%>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="50%">
                                                        <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr id="trExportData" runat="server" visible="false">
                                    <td>
                                        <asp:GridView ID="grdViewExport" runat="server" SkinID="GridViewDownload">
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </caption>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
