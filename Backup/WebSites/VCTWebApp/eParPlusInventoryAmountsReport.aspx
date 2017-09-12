<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusInventoryAmountsReport.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.eParPlusInventoryAmountsReport" Title="Inventory Audit"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Src="~/Controls/TagHistory.ascx" TagName="TagHistoryPopUp" TagPrefix="ucTagHistory" %>
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
                InitGridEvent('<%= gdvInventoryAmount.ClientID %>');
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
            var IsDeelete = confirm("Are you sure, you want to mark the item as manually consumed ?");
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
    <asp:Timer runat="server" ID="tmrUpdateTimer" Enabled="false" Interval="10000" OnTick="UpdateTimer_Tick" />
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
                                        <asp:Label ID="lblHeader" runat="server" Text="Inventory Audit"></asp:Label>
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
                                                        <asp:Label ID="lblSalesRepresentativeFilter" runat="server" Text="Local Rep" CssClass="listboxheading"></asp:Label>
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
                                                    <td align="center" style="width: 140px">
                                                        <asp:CheckBox Text="Near Expiry Filter" AutoPostBack="true" CssClass="CheckBox" Font-Bold="true"
                                                            runat="server" ID="chkNearExpiryDayFilter" Checked="false" OnCheckedChanged="chkNearExpiryDayFilter_CheckedChanged" />
                                                    </td>
                                                    <td align="center" style="width: 120px">
                                                        <asp:Label ID="lblTransaction" runat="server" Text="Transaction" CssClass="listboxheading"></asp:Label>
                                                        <asp:Image ID="imgTransaction" runat="server" ImageUrl="~/Images/Help-icon.png" BorderStyle="None"
                                                            ToolTip="Select Transaction" />
                                                    </td>
                                                    <td align="center" style="width: 15px">
                                                    </td>
                                                    <td align="center" style="width: 15px">
                                                    </td>
                                                    <td align="center" style="width: 15px">
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
                                                        <asp:TextBox ID="txtNearExpiryDay" runat="server" MaxLength="5" ReadOnly="false"
                                                            Width="110px" Font-Size="8pt" Enabled="False"></asp:TextBox>
                                                        <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                            FilterType="Custom" TargetControlID="txtNearExpiryDay" ValidChars="0123456789">
                                                        </ajaxtk:FilteredTextBoxExtender>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                        <asp:DropDownList ID="ddlTransaction" runat="server" AutoPostBack="false" CssClass="ListBox"
                                                            Width="110px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td align="left" style="width: 550px">
                                                        <asp:LinkButton ID="btnFilterData" runat="server" OnClick="btnFilterData_Click" CausesValidation="false">
                                                            <asp:Image ID="imgFilterData" runat="server" ImageUrl="~/Images/view_small.png" BorderStyle="None"
                                                                ToolTip="View" /></asp:LinkButton>
                                                        <asp:LinkButton ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" CausesValidation="false">
                                                            <asp:Image ID="imgRefresh" runat="server" ImageUrl="~/Images/refresh_small.png" BorderStyle="None"
                                                                ToolTip="Reset" /></asp:LinkButton>
                                                        <asp:LinkButton ID="btnInitiateManualScan" runat="server" OnClick="btnInitiateManualScan_Click"
                                                            CausesValidation="false">
                                                            <asp:Image ID="imgInitiateManualScan" runat="server" ImageUrl="~/Images/RFID.png"
                                                                BorderStyle="None" ToolTip="Initiate Manual Scan" /></asp:LinkButton>
                                                        <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CausesValidation="false">
                                                            <asp:Image ID="imgExport" runat="server" ImageUrl="~/Images/download.png" BorderStyle="None"
                                                                ToolTip="Export to Excel" /></asp:LinkButton>
                                                        <asp:LinkButton ID="btnSendEmail" runat="server" OnClick="btnSendEmail_Click" CausesValidation="false">
                                                            <asp:Image ID="imgSendEmail" runat="server" ImageUrl="~/Images/email.png" BorderStyle="None"
                                                                ToolTip="Send Notification Email" /></asp:LinkButton>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top" colspan="2">
                                        <asp:Panel ID="Panel1" runat="server" CssClass="pnlGrid" Height="315px" ScrollBars="Auto"
                                            Width="99%">
                                            <asp:GridView ID="gdvInventoryAmount" runat="server" AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="false" SkinID="GridView" Width="99%" OnRowDataBound="gdvInventoryAmount_RowDataBound"
                                                OnSorting="gdvInventoryAmount_Sorting" AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                Style="cursor: pointer; vertical-align: top;" />
                                                            <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                    OnRowDataBound="grdChild_RowDataBound" OnSorting="grdChild_Sorting" AllowSorting="false">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Account Number" HeaderStyle-Width="1%" Visible="false"
                                                                            ItemStyle-Width="1%" ItemStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAccountNumber" runat="server" Text='<%# Eval("AccountNumber") %>'> </asp:Label>
                                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CustomerName") %>'> </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="imgChildKit2" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                                    Style="cursor: pointer; vertical-align: top;" />
                                                                                <asp:Panel ID="pnlChild2" runat="server" Style="display: none">
                                                                                    <asp:GridView ID="grdChild2" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                        OnRowDataBound="grdChild2_RowDataBound" OnRowCommand="grdChild2_RowCommand" OnSorting="grdChild2_Sorting"
                                                                                        AllowSorting="false">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Account Number" HeaderStyle-Width="20%" Visible="false"
                                                                                                ItemStyle-Width="20%" ItemStyle-HorizontalAlign="center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblAccountNumber" runat="server" Text='<%# Eval("AccountNumber") %>'> </asp:Label>
                                                                                                    <asp:Label ID="lblRefNum" runat="server" Text='<%# Eval("RefNum") %>'> </asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="LotNum" HeaderStyle-Width="15%" HeaderText="Lot Num" ItemStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-Width="15%" SortExpression="LotNum" />
                                                                                            <%--
                                                                                            <asp:BoundField DataField="TagId" HeaderStyle-Width="30%" HeaderText="RFID Tag Id"
                                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%" SortExpression="TagId" />
                                                                                            --%>
                                                                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Tag Id" ItemStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-Width="30%" SortExpression="TagId">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkTagId" runat="server" CausesValidation="false" Font-Underline="true"
                                                                                                        ForeColor="Blue" CommandArgument='<%#Eval("AccountNumber")+","+Eval("TagId")%>'
                                                                                                        CommandName="TagHistoryClick" Text='<%#Eval("TagId") %>' />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="ItemStatusDescription" HeaderStyle-Width="20%" HeaderText="Status"
                                                                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%" SortExpression="ItemStatus" />
                                                                                            <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="25%" ItemStyle-Width="25%"
                                                                                                ItemStyle-HorizontalAlign="Center" SortExpression="ExpiryDt">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDt"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'>
                                                                                                    </asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkConsumed" CausesValidation="false" CommandName="ConsumedRec"
                                                                                                        CommandArgument='<%#DataBinder.Eval(Container, "DataItem.TagId")%>' runat="server"
                                                                                                        OnClientClick="Javascript:return ConfirmConsumed();">
                                                                                                        <asp:Image ID="imgConsumed" runat="server" ImageUrl="~/Images/Consumed.png" BorderStyle="None"
                                                                                                            ToolTip="Mark Manually Consumed" AlternateText="Mark Manually Consumed" /></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <HeaderTemplate>
                                                                                                    <asp:Label ID="Label2" runat="server" Text="Mark Consume" />
                                                                                                </HeaderTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="RefNum" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Ref #" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" SortExpression="RefNum" />
                                                                        <asp:BoundField DataField="PartDesc" HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="center"
                                                                            HeaderText="Description" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="35%"
                                                                            SortExpression="PartDesc" />
                                                                        <asp:BoundField DataField="LastScanned" HeaderStyle-Width="18%" HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Last Scanned" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="18%"
                                                                            SortExpression="LastScanned" />
                                                                        <asp:BoundField DataField="OffCartQty" HeaderStyle-Width="8%" HeaderText="Off Cart"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%" />
                                                                        <asp:BoundField DataField="Qty" HeaderStyle-Width="8%" HeaderText="Current Inventory"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%"
                                                                            SortExpression="Qty" HeaderStyle-BackColor="#244061" />
                                                                        <asp:BoundField DataField="PARLevelQty" HeaderStyle-Width="12%" HeaderText="PAR Level"
                                                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%"
                                                                            SortExpression="PARLevelQty" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CustomerName" HeaderStyle-Width="50%" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%"
                                                        SortExpression="CustomerName" />
                                                    <asp:BoundField DataField="AccountNumber" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderText="Account Number" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45%"
                                                        SortExpression="AccountNumber" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <br />
                                        <asp:Label ID="Label1" Text="* Items in red denotes Expired / Near Expiry Items"
                                            ForeColor="Red" runat="server" />
                                        <br />
                                    </td>
                                </tr>
                                <tr id="trExportData" runat="server" visible="false">
                                    <td>
                                        <asp:GridView ID="grdViewExport" runat="server" SkinID="GridViewDownload">
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="70%">
                                                        <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </caption>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" OnClientClick="javascript:SetPositionOfPopUp();" />
            <ajaxtk:ModalPopupExtender ID="mpeKitDetail" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlTagHistory" TargetControlID="btnShowPopUp" />
            <asp:Panel ID="pnlTagHistory" runat="server">
                <ucTagHistory:TagHistoryPopUp id="ucTagHistoryPopUp" runat="server" />
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:AsyncPostBackTrigger ControlID="tmrUpdateTimer" EventName="Tick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
