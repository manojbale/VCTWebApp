﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusManualConsumptionReport.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.eParPlusManualConsumptionReport" Title="Manual Consumption Report"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $(window).load(function () {
                fixedGrid();
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                fixedGrid();
            });

            function fixedGrid() {
                InitGridEvent('<%= gdvManualConsumption.ClientID %>');
            }

        });

        $j = $.noConflict();

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
    </script>
    <script type="text/javascript">
        function ConfirmConsumed() {
            var IsDeelete = confirm("Are you sure you want to revert the item as On-Cart ?");
            if (IsDeelete == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function AlreadyConsumed() {
            alert("This item is already marked as consumed.");
        }
      
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <caption>
                                <br />
                                <br />
                                <tr class="header">
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblHeader" runat="server" Text="Manual Consumption Report"></asp:Label>
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
                                        <asp:Panel ID="pnlProductFilterSection" runat="server" GroupingText="Product" HorizontalAlign="Left"
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
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                        <asp:Panel ID="Panel2" runat="server" GroupingText="Date Range" HorizontalAlign="Left"
                                            Width="100%">
                                            <table>
                                                <tr>
                                                    <td colspan="8">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:RadioButtonList runat="server" ID="rblPeriod" AutoPostBack="True" OnSelectedIndexChanged="rblPeriod_SelectedIndexChanged"
                                                                        RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Last One Week" Selected="True" />
                                                                        <asp:ListItem Text="Last One Month" />
                                                                        <asp:ListItem Text="Date Range" />
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblStartDate" Text="Start Date:&nbsp;" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="100" Enabled="false" Text=""
                                                                        ClientIDMode="Static" />
                                                                    <asp:Image ID="imgCalenderFrom" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtenderFrom1" runat="server" PopupButtonID="imgCalenderFrom"
                                                                        TargetControlID="txtStartDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblEndDate" Text="End Date:&nbsp;" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="100" Text="" Enabled="false" ClientIDMode="Static" />
                                                                    <asp:Image ID="Image1" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="Image1"
                                                                        TargetControlID="txtEndDate">
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
                                                                        <asp:Image ID="imgExport" runat="server" ImageUrl="~/Images/download.png" BorderStyle="None"
                                                                            ToolTip="Export to Excel" /></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                        <%--<asp:Panel ID="Panel2" runat="server" GroupingText="Date Range" HorizontalAlign="Left"
                                            Width="100%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 320px">
                                                                    <asp:RadioButtonList runat="server" ID="rblPeriod" AutoPostBack="True" OnSelectedIndexChanged="rblPeriod_SelectedIndexChanged"
                                                                        RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Last One Week" Selected="True" />
                                                                        <asp:ListItem Text="Last One Month" />
                                                                        <asp:ListItem Text="Date Range" />
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" style="width: 70px">
                                                                    <asp:Label ID="lblStartDate" Text="Start Date:&nbsp;" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 135px">
                                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="100" Enabled="false" Text=""
                                                                        ClientIDMode="Static" />
                                                                    <asp:Image ID="imgCalenderFrom" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtenderFrom1" runat="server" PopupButtonID="imgCalenderFrom"
                                                                        TargetControlID="txtStartDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" style="width: 70px">
                                                                    <asp:Label ID="lblEndDate" Text="End Date:&nbsp;" runat="server" />&nbsp;
                                                                </td>
                                                                <td align="left" style="width: 135px">
                                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="100" Text="" Enabled="false" ClientIDMode="Static" />
                                                                    <asp:Image ID="Image1" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif"
                                                                        Visible="false" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="Image1"
                                                                        TargetControlID="txtEndDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:LinkButton ID="lnkFilterData" runat="server" OnClick="lnkFilterCustomerListData_Click"
                                                                        CausesValidation="false">
                                                                        <asp:Image ID="imgFilterData" runat="server" ImageUrl="~/Images/view_small.png" BorderStyle="None"
                                                                            ToolTip="View" /></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="lnkRefresh_Click" CausesValidation="false">
                                                                        <asp:Image ID="imgRefresh" runat="server" ImageUrl="~/Images/refresh_small.png" BorderStyle="None"
                                                                            ToolTip="Reset" /></asp:LinkButton>
                                                                    <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CausesValidation="false">
                                                                        <asp:Image ID="imgExport" runat="server" ImageUrl="~/Images/download.png" BorderStyle="None"
                                                                            ToolTip="Export to Excel" /></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>--%>



                                            
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top" colspan="2">
                                        <asp:Panel ID="Panel1" runat="server" CssClass="pnlGrid" Height="290px" ScrollBars="Auto"
                                            Width="99%">
                                            <asp:GridView ID="gdvManualConsumption" runat="server" AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="false" SkinID="GridView" Width="99%" OnRowDataBound="gdvManualConsumption_RowDataBound"
                                                OnSorting="gdvManualConsumption_Sorting" AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                Style="cursor: pointer; vertical-align: top;" />
                                                            <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                    OnRowDataBound="grdChild_RowDataBound" OnSorting="grdChild_Sorting" AllowSorting="false"
                                                                    OnRowCommand="grdChild_RowCommand">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Account Number" HeaderStyle-Width="1%" Visible="false"
                                                                            ItemStyle-Width="1%" ItemStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAccountNumber" runat="server" Text='<%# Eval("AccountNumber") %>'> </asp:Label>
                                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'> </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ProductLine" SortExpression="ProductLine" HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center" HeaderText="Product Line" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="ProductLineDesc" SortExpression="ProductLineDesc" HeaderStyle-Width="14%" ItemStyle-Width="14%" HeaderStyle-HorizontalAlign="Center" HeaderText="Product Line Desc" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="RefNum" SortExpression="RefNum" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center" HeaderText="Ref #" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="PartDesc" SortExpression="PartDesc" HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="center" HeaderText="Description" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="LotNum" SortExpression="LotNum" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderText="Lot Num" ItemStyle-HorizontalAlign="Center" /> 
                                                                        <asp:BoundField DataField="TagId" SortExpression="TagId" HeaderStyle-Width="17%" ItemStyle-Width="17%" HeaderText="RFID Tag Id" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="UpdatedOn" SortExpression="UpdatedOn" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderText="Manual Consumed On" ItemStyle-HorizontalAlign="Center" />
                                                                        <asp:BoundField DataField="UpdatedBy" SortExpression="UpdatedBy" HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderText="Manual Consumed By" ItemStyle-HorizontalAlign="Center" />
                                                                        
                                                                        <asp:TemplateField HeaderText="Consumed Date" HeaderStyle-Width="8%" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" SortExpression="ConsumedDate">
                                                                            <ItemTemplate><asp:Label ID="lblConsumedDate" runat="server" Text='<%#  GetDate(Eval("ConsumedDate"))%>'></asp:Label></ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkRevertConsumed" CausesValidation="false" CommandName="RevertConsumed"
                                                                                    CommandArgument='<%#DataBinder.Eval(Container, "DataItem.TagId")%>' runat="server"
                                                                                    OnClientClick="Javascript:return ConfirmConsumed();">
                                                                                    <asp:Image ID="imgRevertConsume" runat="server" ImageUrl="~/Images/icon_undo.png"
                                                                                        BorderStyle="None" ToolTip="Revert Manually Consumed" AlternateText="Revert Manually Consumed" /></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="Label2" runat="server" Text="Revert to On-Cart" />
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CustomerName" HeaderStyle-Width="50%" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%"
                                                        SortExpression="CustomerName" />
                                                    <asp:BoundField DataField="AccountNumber" HeaderStyle-Width="50%" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderText="Account Number" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%"
                                                        SortExpression="AccountNumber" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <br />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:Panel ID="Panel3" CssClass="ActionPanel" runat="server">
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
