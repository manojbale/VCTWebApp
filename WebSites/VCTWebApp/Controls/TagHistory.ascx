<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagHistory.ascx.cs"
    Inherits="VCTWebApp.Controls.TagHistory" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table align="left" width="1000px">
            <tr>
                <td align="center">
                    <table class="maintablePopUp" align="center" width="100%">
                        <tr class="header">
                            <td align="center" width="100%">
                                <asp:Label ID="lblHeader" runat="server" Text="Tag History"></asp:Label>
                            </td>
                            <td align="right" valign="top">
                                <asp:Button ID="btnClose" runat="server" CssClass="closebutton" CausesValidation="False"
                                    OnClick="btnClose_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <br />
                                <asp:Panel ID="Panel1" CssClass="pnlGrid" runat="server" Width="95%" ScrollBars="Auto"
                                    BorderWidth="0px">
                                    <table cellpadding="3" width="100%">
                                        <tr>
                                            <td align="center" style="width: 33%">
                                                Ref #:&nbsp;
                                                <asp:Label Text="Ref Num" runat="server" ID="lblRefNum" CssClass="listboxheading" />
                                            </td>
                                            <td align="center" style="width: 33%">
                                                Lot #:&nbsp;
                                                <asp:Label Text="Lot Num" runat="server" ID="lblLotNum" CssClass="listboxheading" />
                                            </td>
                                            <td align="center" style="width: 33%">
                                                Tag Id:&nbsp;
                                                <asp:Label Text="TagId" runat="server" ID="lblTagId" CssClass="listboxheading" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <table cellpadding="3" width="100%">
                                    <tr>
                                        <td align="center" width="100%">
                                            <asp:Panel ID="pnlKitDeatil" CssClass="pnlGrid" runat="server" Width="95%" ScrollBars="Auto"
                                                Height="500px" BorderWidth="1px">
                                                <asp:GridView ID="gdvKitDeatil" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                    Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="gdvKitDeatil_RowDataBound">
                                                    <Columns>
                                                        <%--  <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="RefNum"
                                                            HeaderText="Ref #" />
                                                        <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                            HeaderText="Lot #" />
                                                        <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="TagId"
                                                            HeaderText="Tag Id" />--%>
                                                        <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="StatusDescription"
                                                            HeaderText="Status" />
                                                        <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="UpdatedOn"
                                                            HeaderText="Updated On" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
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
