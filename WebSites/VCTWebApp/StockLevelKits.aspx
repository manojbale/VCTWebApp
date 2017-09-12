<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockLevelKits.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.StockLevelKits" Title="Stock Level (Kits)" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Src="~/Controls/KitDetail.ascx" TagName="KitDetailPopUp" TagPrefix="ucKitDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        function pageLoad() {
            SearchKitFamilyByNumber('txtKitFamily', 'sKitFamily', 'GetKitFamilyByNumber', 'hdnKitFamilyId');
        }

        function SetPositionOfPopUp() {
            var x = $(window).width();
            var y = $(window).height();
            $("#pnlKitDetail").css({ top: y / 2, left: x / 2 });
        }

        $(function () {
            $(window).load(function () {
                fixedGrid();
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                fixedGrid();
            });

            function fixedGrid() {
                InitGridEvent('<%= gdvStockLevel.ClientID %>');
            }


        });
      

        function KitFamilyKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnKitFamilyId.ClientID %>');
                if (myHidden) {
                    myHidden.value = '0';
                }
            }
        }

        $j = $.noConflict();
        $j(function () {

            $j(".ExpandRow").live("click", function () {

                if ($j(this).attr("src").toLowerCase() == "images/plus.png") {
                    $j(this).next().show();
                    $j(this).closest("tr").after("<td style='width:5%'></td><td colspan = '999'>" + $j(this).next().html() + "</td>");
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
                                        <asp:Label ID="lblHeader" runat="server" Text="Inventory Stock Level (Kits)"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <br />
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <span style="color: Red">&nbsp;&nbsp;*</span><span>Location :&nbsp;</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="ListBox" Width="300px">
                                                    </asp:DropDownList>
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Kit Family :&nbsp;" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtKitFamily" runat="server" ClientIDMode="Static" Width="220px"
                                                        AutoCompleteType="None" onKeyUp="javascript:KitFamilyKeyUp(this, event);"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnKitFamilyId" runat="server" ClientIDMode="Static" Value="0" />
                                                    &nbsp;&nbsp;
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkFilterData" runat="server" OnClick="lnkFilterCustomerListData_Click"
                                                        CausesValidation="false">
                                                        <asp:Image ID="ImageView" runat="server" ImageUrl="~/Images/view_small.png" BorderStyle="None"
                                                            ToolTip="View" /></asp:LinkButton>
                                                </td>
                                                <%--<td>
                                                 <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="lnkRefresh_Click" CausesValidation="false">
                                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/refresh_small.png" BorderStyle="None"
                                                                            ToolTip="Reset" /></asp:LinkButton>
                                                </td>--%>
                                                <td>
                                                    <div style="float: left; padding-left: 10px;">
                                                        <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CausesValidation="false">
                                                            <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/download.png" BorderStyle="None"
                                                                ToolTip="Export to Excel" /></asp:LinkButton>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Panel ID="pnlProductAttributes" runat="server" CssClass="pnlGrid" Height="370px"
                                            ScrollBars="Auto" Width="95%">
                                            <table cellpadding="0" cellspacing="0" width="99%">
                                                <tr>
                                                    <td align="center" valign="top">
                                                        <asp:GridView ID="gdvStockLevel" runat="server" AutoGenerateColumns="false" OnRowCommand="gdvStockLevel_RowCommand"
                                                            OnRowDataBound="gdvStockLevel_RowDataBound" ShowHeaderWhenEmpty="true" SkinID="GridView"
                                                            OnSorting="gdvStockLevel_Sorting" AllowSorting="true">
                                                            <Columns>
                                                                <asp:BoundField DataField="LocationName" HeaderStyle-Width="8%" HeaderText="Location"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%"  SortExpression="LocationName" />
                                                                <asp:BoundField DataField="LocationType" HeaderStyle-Width="13%" HeaderText="Location Type"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="13%" SortExpression="LocationType"  />
                                                                <asp:TemplateField HeaderStyle-Width="9%" HeaderText="Kit Family" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="9%" SortExpression="KitFamilyName" >
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkKitFamily" runat="server" CausesValidation="false" Font-Underline="true"
                                                                            ForeColor="Blue" CommandArgument='<%#Eval("LocationId")+","+Eval("KitFamilyId")+","+Eval("LocationName")+","+Eval("KitFamilyName")%>'
                                                                            CommandName="KitFamilyClick" Text='<%#Eval("KitFamilyName") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="KitFamilyDescription" HeaderText="Description" ItemStyle-Width="22%"   SortExpression="KitFamilyDescription" />
                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Least Expiry Date" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="10%"  SortExpression="LeastExpiryDate" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeastExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("LeastExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="AvailableQuantity" HeaderStyle-Width="10%" HeaderText="Available"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" SortExpression="AvailableQuantity" />
                                                                <asp:BoundField DataField="AssignedToCaseQuantity" HeaderStyle-Width="7%" HeaderText="Assigned To Case"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%"  SortExpression="AssignedToCaseQuantity"/>
                                                                <asp:BoundField DataField="ShippedQuantity" HeaderStyle-Width="7%" HeaderText="Shipped"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%"  SortExpression="ShippedQuantity" />
                                                                <asp:BoundField DataField="ReceivedQuantity" HeaderStyle-Width="7%" HeaderText="Received"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" SortExpression="ReceivedQuantity" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        <asp:Label runat="server" ForeColor="Red" Text="* Items in red denotes Kits containing Expired / Near Expiry Items" />
                                        <br />
                                        <br />
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
                                                    <td align="right" width="50%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" CausesValidation="False"
                                                        OnClick="btnNew_Click" />
                                                
                                                </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr id="trExportData" runat="server" visible="false">
                                    <td>
                                        <asp:GridView ID="grdViewExport" runat="server">
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </caption>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" OnClientClick="javascript:SetPositionOfPopUp();" />
            <ajaxtk:ModalPopupExtender ID="mpeKitDetail" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlKitDetail" TargetControlID="btnShowPopUp" />
            <asp:Panel ID="pnlKitDetail" runat="server">
                <ucKitDetail:KitDetailPopUp ID="ucKitDetailPopUp" runat="server" />
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
