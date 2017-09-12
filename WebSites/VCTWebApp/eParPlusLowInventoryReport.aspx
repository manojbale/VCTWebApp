<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusLowInventoryReport.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.eParPlusLowInventoryReport" Title="Low Inventory Report"
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
                InitGridEvent('<%= gdvLowInventory.ClientID %>');
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                                        <asp:Label ID="lblHeader" runat="server" Text="Low Inventory Report"></asp:Label>
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
                                        <asp:Panel ID="pnlProductSection" runat="server" GroupingText="Product" HorizontalAlign="Left"
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
                                                    <td align="left" style="width: 110px">
                                                        <asp:LinkButton ID="lnkFilterData" runat="server" OnClick="lnkFilterCustomerListData_Click"
                                                            CausesValidation="false">
                                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/view_small.png" BorderStyle="None"
                                                                ToolTip="View" /></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="lnkRefresh_Click" CausesValidation="false">
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/refresh_small.png" BorderStyle="None"
                                                                ToolTip="Reset" /></asp:LinkButton>
                                                        <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CausesValidation="false">
                                                            <asp:Image ID="imgdownload" runat="server" ImageUrl="~/Images/download.png" BorderStyle="None"
                                                                ToolTip="Export to Excel" /></asp:LinkButton>
                                                        <asp:LinkButton ID="btnSendEmail" runat="server" OnClick="btnSendEmail_Click" CausesValidation="false">
                                                            <asp:Image ID="imgSendEmail" runat="server" ImageUrl="~/Images/email.png" BorderStyle="None"
                                                                ToolTip="Send Notification Email" /></asp:LinkButton>
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                    </td>
                                                    <td align="center" style="width: 125px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top" colspan="2">
                                        <asp:Panel ID="Panel1" runat="server" CssClass="pnlGrid" Height="315px" ScrollBars="Auto"
                                            Width="99%">
                                            <asp:GridView ID="gdvLowInventory" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false"
                                                SkinID="GridView" Width="99%" OnRowDataBound="gdvLowInventory_RowDataBound" OnSorting="gdvLowInventory_Sorting"
                                                AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                Style="cursor: pointer; vertical-align: top;" />
                                                            <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                    OnRowDataBound="grdChild_RowDataBound" OnSorting="grdChild_Sorting" AllowSorting="false">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Product Line" ItemStyle-HorizontalAlign="Center" SortExpression="ProductLine"
                                                                            HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hndAccountNumber" runat="server" Value='<%# Eval("AccountNumber") %>' />
                                                                                <asp:Label ID="lblProductLine" runat="server" Text='<%# Eval("ProductLine") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="ProductLineDesc" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Product Line Desc" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%"
                                                                            SortExpression="ProductLineDesc" />
                                                                        <asp:BoundField DataField="RefNum" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Ref #" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" SortExpression="RefNum" />
                                                                        <asp:BoundField DataField="PartDesc" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                                            HeaderText="Description" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                                            SortExpression="PartDesc" />
                                                                        <asp:BoundField DataField="Size" HeaderStyle-Width="8%" HeaderText="Size" ItemStyle-HorizontalAlign="Center"
                                                                            ItemStyle-Width="8%" SortExpression="Size" />
                                                                        <asp:TemplateField HeaderText="Required Order" HeaderStyle-BackColor="#244061" ItemStyle-HorizontalAlign="Center"
                                                                            SortExpression="LowInvQty" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLowInvQty" runat="server" Text='<%# Eval("LowInvQty") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <%--<asp:BoundField DataField="LowInvQty" HeaderStyle-Width="8%" HeaderText="Required Order"
                                                                            HeaderStyle-BackColor="#244061" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%"
                                                                            SortExpression="LowInvQty" />--%>
                                                                        <asp:BoundField DataField="OrderedProductQty" HeaderStyle-Width="7%" HeaderText="Already On Order"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" SortExpression="OrderedProductQty" />
                                                                        <asp:BoundField DataField="InvLevelQty" HeaderStyle-Width="8%" HeaderText="Current Inventory"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%" SortExpression="InvLevelQty" />
                                                                        <asp:BoundField DataField="PARLevelQty" HeaderStyle-Width="6%" HeaderText="Par Level"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%" SortExpression="PARLevelQty" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CustomerName" HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="35%"
                                                        SortExpression="CustomerName" />
                                                    <asp:BoundField DataField="AccountNumber" HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderText="Account Number" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="35%"
                                                        SortExpression="AccountNumber" />
                                                    <asp:BoundField DataField="LowInvQty" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center"
                                                        HeaderText="Required Order" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25%"
                                                        SortExpression="LowInvQty" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <br />
                                        <asp:Label ID="Label2" Text="* Quantities in red are on backorder"
                                            ForeColor="Red" runat="server" />
                                        <br />
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
