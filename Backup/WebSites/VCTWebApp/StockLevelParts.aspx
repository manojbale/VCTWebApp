<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockLevelParts.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.StockLevelParts" Title="Stock Level (Parts)"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Src="~/Controls/PartDetail.ascx" TagName="PartDetailPopUp" TagPrefix="ucPartDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function SetPositionOfPopUp() {
            var x = $(window).width();
            var y = $(window).height();
            $("#pnlPartDetail").css({ top: y / 2, left: x / 2 });
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
    </script>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <br />
                            <br />
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Inventory Stock Level (Parts)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLocation" runat="server" Text="Location:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLocation" runat="server" Width="300px" CssClass="ListBox"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <asp:Panel ID="pnlProductAttributes" CssClass="pnlGrid" runat="server" Width="95%"
                                        ScrollBars="Auto" Height="370px">
                                        <asp:GridView ID="gdvStockLevel" runat="server" ShowHeaderWhenEmpty="true" Width="99%"
                                            AutoGenerateColumns="false" OnRowDataBound="gdvStockLevel_RowDataBound" OnRowCommand="gdvStockLevel_RowCommand"
                                            SkinID="GridView">
                                            <Columns>
                                                <asp:BoundField HeaderText="Location" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                    ItemStyle-HorizontalAlign="Center" DataField="LocationName" />
                                                <asp:BoundField HeaderText="Location Type" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                    ItemStyle-HorizontalAlign="Center" DataField="LocationType" />
                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Ref #" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPartNum" runat="server" CausesValidation="false" Font-Underline="true"
                                                            ForeColor="Blue" CommandArgument='<%#Eval("LocationId")+","+Eval("PartNumber")+","+Eval("LocationName")%>'
                                                            CommandName="PartNumberClick" Text='<%#Eval("PartNumber") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Description" ItemStyle-Width="30%" DataField="Description" />
                                                <asp:TemplateField HeaderText="Least Expiry Date" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLeastExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("LeastExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Available" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center" DataField="AvailableQuantity" />
                                                <asp:BoundField HeaderText="Assigned To Case" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                    ItemStyle-HorizontalAlign="Center" DataField="AssignedToCaseQuantity" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                    <br />
                                    <asp:Label ID="Label1" Text="* Items in red denotes Parts containing Expired / Near Expiry Items"
                                        ForeColor="Red" runat="server" />
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" OnClientClick="javascript:SetPositionOfPopUp();" />
            <ajaxtk:ModalPopupExtender ID="mpePartDetail" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlPartDetail" TargetControlID="btnShowPopUp" />
            <asp:Panel ID="pnlPartDetail" runat="server">
                <ucPartDetail:PartDetailPopUp id="ucPartDetailPopUp" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
