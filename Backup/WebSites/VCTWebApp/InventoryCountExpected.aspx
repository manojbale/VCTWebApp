<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryCountExpected.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.InventoryCountExpected" Title="Expected Inventory Count"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
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
                  InitGridEvent('<%= gdvInventoryCount.ClientID %>');
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
                                                    <asp:GridView ID="gdvInventoryCount" runat="server" SkinID="GridView" ShowHeaderWhenEmpty="true"
                                                        AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Ref #" HeaderStyle-Width="15%"
                                                                ItemStyle-HorizontalAlign="Center" DataField="PartNum" />
                                                            <asp:BoundField HeaderText="Lot #" HeaderStyle-Width="15%"
                                                                ItemStyle-HorizontalAlign="Center" DataField="LotNum" />
                                                            <asp:BoundField HeaderText="Description" ItemStyle-Width="55%" DataField="PartDescription" />
                                                            <asp:BoundField HeaderText="Expected Qty" HeaderStyle-Width="15%"
                                                                ItemStyle-HorizontalAlign="Center" DataField="ExpectedQty" />
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
