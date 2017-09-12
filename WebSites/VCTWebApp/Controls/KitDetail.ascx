<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KitDetail.ascx.cs" Inherits="VCTWebApp.Controls.KitDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">

    <ContentTemplate>
  
        <table align="left" width="1000px">
            <tr>
                <td align="center">
                    <table class="maintablePopUp" align="center" width="100%">
                        <tr class="header">
                            <td align="center" width="100%">
                                <asp:Label ID="lblHeader" runat="server" Text="Kit Detail"></asp:Label>
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
                                            Kit Family:&nbsp;
                                            <asp:Label Text="KitFamilyName" runat="server" ID="lblKitFamily" />
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
                                            <asp:Panel ID="pnlKitDeatil" CssClass="pnlGrid" runat="server" Width="95%" ScrollBars="Auto"
                                                Height="500px" BorderWidth="1px">
                                                <asp:GridView ID="gdvKitDeatil" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                    Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="gdvKitDeatil_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                    ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                    <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView" OnRowDataBound="grdChild_RowDataBound">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="PartNum"
                                                                                HeaderText="Ref #" />
                                                                            <asp:BoundField HeaderStyle-Width="55%" DataField="Description" HeaderText="Description" />
                                                                            <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                                                HeaderText="Lot #" />
                                                                            <asp:BoundField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate"
                                                                                HeaderText="Expiry Date" DataFormatString="{0:d}" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Kit #" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"
                                                            DataField="KitNumber" />
                                                        <asp:BoundField HeaderText="Description" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Left"
                                                            DataField="Description" />
                                                       
                                                        <asp:TemplateField HeaderText="Least Expiry Date" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLeastExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("LeastExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField HeaderText="Status" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                                            DataField="Status" />

                                                          <%--    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" ></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>

                                                        <asp:BoundField HeaderText="Linked Case #" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                                            DataField="LinkedCaseNumber" />
                                                        <asp:BoundField HeaderText="Hospital" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center"
                                                            DataField="Hospital" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <br />
                                            <asp:Label ID="Label1" Text="* Items in red denotes Kits containing Near Expiry Items"
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
