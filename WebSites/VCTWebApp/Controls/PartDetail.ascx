<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartDetail.ascx.cs" Inherits="VCTWebApp.Controls.PartDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table align="left" width="1000px">
            <tr>
                <td align="center">
                    <table class="maintablePopUp" align="center" width="100%">
                        <tr class="header">
                            <td align="center" width="100%">
                                <asp:Label ID="lblHeader" runat="server" Text="Part Detail"></asp:Label>
                            </td>
                            <td align="right" valign="top">
                                <asp:Button ID="btnClose" runat="server" CssClass="closebutton" CausesValidation="False"
                                    OnClick="btnClose_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <br />
                                <table cellpadding="3" width="100%">
                                    <tr>
                                        <td align="center">
                                            Location::&nbsp;
                                            <asp:Label Text="LocationName" runat="server" ID="lblLocation" />
                                        </td>
                                        <td align="center">
                                            Ref #:&nbsp;
                                            <asp:Label Text="PartNumber" runat="server" ID="lblPartNum" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <table cellpadding="3" width="100%">
                                    <tr>
                                        <td align="center" width="100%">
                                            <asp:Panel ID="pnlPartDeatil" CssClass="pnlGrid" runat="server" Width="95%" ScrollBars="Auto"
                                                Height="500px" BorderWidth="1px">
                                                <asp:GridView ID="gdvPartDeatil" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                    Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="gdvPartDeatil_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Lot #" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"
                                                            DataField="LotNum" />
                                                        <asp:BoundField HeaderText="Description" HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="Left"
                                                            DataField="Description" />
                                                        <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Status" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center"
                                                            DataField="Status" />
                                                        <asp:BoundField HeaderText="Linked Case #" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"
                                                            DataField="LinkedCaseNumber" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <br />
                                            <asp:Label ID="Label1" Text="* Items in red denotes Near Expiry Items"
                                                ForeColor="Red" runat="server" />
                                                <br />
                                                <br />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>
