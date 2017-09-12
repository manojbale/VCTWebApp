<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryCountExpectedKits.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.InventoryCountExpectedKits" Title="Expected Inventory Count (Kits)"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $(".ExpandRow").live("click", function () {

                if ($(this).attr("src").toLowerCase() == "images/plus.png") {
                    $(this).next().show();
                    $(this).closest("tr").after("<td style='width:5%'></td><td colspan = '999'>" + $(this).next().html() + "</td>");
                    $(this).next().hide();
                    $(this).attr("src", "images/minus.png");
                }
                else {
                    $(this).attr("src", "images/plus.png");
                    $(this).closest("tr").next().next().hide();
                    $(this).closest("tr").next().hide();
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
                            <br />
                            <br />
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Expected Inventory Count"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblHospital" runat="server" Text="Hospital:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlHospital" runat="server" Width="300px" CssClass="ListBox"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlHospital_SelectedIndexChanged" />
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
                                        <table cellspacing="0" cellpadding="0" width="99%">
                                            <tr>
                                                <td align="center" valign="top">
                                                    <asp:GridView ID="gdvInventoryCount" runat="server" SkinID="GridView" ShowHeaderWhenEmpty="true" OnRowDataBound="gdvInventoryCount_RowDataBound"
                                                        AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                        Style="cursor: pointer; vertical-align: top;" />
                                                                    <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                        <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" OnRowDataBound="grdChild_RowDataBound"
                                                                            SkinID="GridView">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Ref #" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPartNum" runat="server" Text='<%# Bind("PartNum") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="45%" HeaderText="Lot #" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Lot #" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%# Bind("LotNum") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Expiry Date" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="Kit #" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center"
                                                                DataField="KitNumber" />
                                                            <asp:BoundField HeaderText="Description" ItemStyle-Width="55%" DataField="Description" />
                                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Least Expiry Date" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLeastExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("LeastExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxtk:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdatePnlProgress2"
        PopupControlID="UpdatePnlProgress2" BackgroundCssClass="modalPopup" />
</asp:Content>
