<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryCountReconcile.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.InventoryCountReconcile" Title="Inventory Count Reconcile"
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
                            <caption>
                                <br />
                                <br />
                                <tr class="header">
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblHeader" runat="server" Text="Inventory Count Reconcile"></asp:Label>
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
                                                    <asp:DropDownList ID="ddlHospital" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                        OnSelectedIndexChanged="ddlHospital_SelectedIndexChanged" Width="300px" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Panel ID="pnlProductAttributes" runat="server" CssClass="pnlGrid" Height="320px"
                                            ScrollBars="Auto" Width="95%">
                                            <table cellpadding="0" cellspacing="0" width="99%">
                                                <tr>
                                                    <td align="center" valign="top">
                                                        <asp:GridView ID="gdvInventoryCount" runat="server" AutoGenerateColumns="false" OnRowDataBound="gdvInventoryCount_RowDataBound"
                                                            ShowHeaderWhenEmpty="true" SkinID="GridView">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                            Style="cursor: pointer; vertical-align: top;" />
                                                                        <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                            <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" OnRowDataBound="grdChild_RowDataBound"
                                                                                SkinID="GridView">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Ref #" ItemStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPartNum" runat="server" Text='<%# Bind("PartNum") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Lot #" ItemStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLotNum" runat="server" Text='<%# Bind("LotNum") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Quantity" ItemStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-Width="10%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Disposition" ItemStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-Width="20%">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnPartyCycleCountId" runat="server" Value='<%# Bind("PartyCycleCountId") %>' />
                                                                                            <asp:HiddenField ID="hdnIsNegativeVariance" runat="server" Value='<%# Bind("IsNegativeVariance") %>' />
                                                                                            <asp:DropDownList ID="ddlDispositionType" runat="server" CssClass="ListBox" Width="90%" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Ref #" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="15%">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnPartyCycleCountId" runat="server" Value='<%# Bind("PartyCycleCountId") %>' />
                                                                        <asp:Label ID="lblPartNum" runat="server" Text='<%# Bind("PartNum") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Lot #" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="15%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%# Bind("LotNum") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PartDescription" HeaderText="Description" ItemStyle-Width="30%" />
                                                                <asp:BoundField DataField="CycleCountDate" HeaderStyle-Width="15%" HeaderText="Cycle Count Date"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Expected Qty" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblExpectedQty" runat="server" Text='<%# Bind("ExpectedQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Cycle Count Qty" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="10%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCycleCountQty" runat="server" Text='<%# Bind("CycleCountQty") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="Action" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:RadioButtonList ID="rblstAction" runat="server" RepeatDirection="Vertical">
                                                                        <asp:ListItem Text="Accept" Selected="True" />
                                                                        <asp:ListItem Text="Reject" />
                                                                    </asp:RadioButtonList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Disposition" HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                                                ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField runat="server" ID="hdnPartyCycleCountId" Value='<%# Bind("PartyCycleCountId") %>' />
                                                                    <asp:DropDownList runat="server" ID="ddlDispositionType" CssClass="ListBox" Width="90%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
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
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlButton" runat="server" CssClass="ActionPanel" Width="95%">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="50%">
                                                        <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                    </td>
                                                    <td align="right" valign="top" width="50%">
                                                        <asp:Button ID="btnNew" runat="server" CausesValidation="False" CssClass="resetbutton"
                                                            OnClick="btnNew_Click" />
                                                        <asp:Button ID="btnSave" runat="server" CssClass="savebutton" OnClick="btnSave_Click" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxtk:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdatePnlProgress2"
        PopupControlID="UpdatePnlProgress2" BackgroundCssClass="modalPopup" />
</asp:Content>
