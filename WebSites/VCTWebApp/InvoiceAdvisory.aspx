<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAdvisory.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.InvoiceAdvisory" Title="Invoice Advisory" MasterPageFile="~/Site1.master" %>

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
                InitGridEvent('<%= gdvInvoiceItems.ClientID %>');
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
                            <caption>
                                <br />
                                <br />
                                <tr class="header">
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblHeader" runat="server" Text="Invoice Advisory for Case"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <br />
                                        <table width="95%">
                                            <tr>
                                                <td align="left" width="20%">
                                                    <asp:Label ID="lblSalesRep" runat="server" Text="Sales Representative:&nbsp;" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" width="10%">
                                                    <asp:Label ID="lblSurgeryDate" runat="server" Text="Surgery Date:&nbsp;" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblHospital" runat="server" Text="Hospital:&nbsp;" CssClass="label"></asp:Label>
                                                </td>
                                                <td align="left" width="20%">
                                                    <asp:Label ID="lblCaseStatus" runat="server" Text="Case Status:&nbsp;" CssClass="label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSalesRep" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="90%" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSurgeryDate" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="80%" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtHospital" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="90%" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCaseStatus" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="90%" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Panel ID="pnlProductAttributes" runat="server" CssClass="pnlGrid" Height="280px"
                                            ScrollBars="Auto" Width="95%" >
                                            <table cellpadding="0" cellspacing="0" width="99%">
                                                <tr>
                                                    <td align="center" valign="top" colspan="2">
                                                        <asp:GridView ID="gdvInvoiceItems" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                            SkinID="GridView" OnRowDataBound="gdvInvoiceItems_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="InventoryType" HeaderStyle-Width="10%" HeaderText="Inv. Type"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                <asp:BoundField DataField="Particular" HeaderStyle-Width="30%" HeaderText="Particular"
                                                                    ItemStyle-Width="30%" />
                                                                <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="30%"
                                                                    ItemStyle-Width="30%" />
                                                                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="15%" />
                                                                <asp:BoundField DataField="Amount" HeaderStyle-Width="15%" HeaderText="Amount (in $)" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="15%" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="94%">
                                            <tr class="header">
                                                <td width="83%" align="right">
                                                    <asp:Label ID="Label1" Text="Total: " runat="server" />&nbsp;
                                                </td>
                                                <td width="17%">
                                                    <asp:Label ID="lblTotal" Text="0" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server" Width="98%">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="50%">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxtk:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdatePnlProgress2"
        PopupControlID="UpdatePnlProgress2" BackgroundCssClass="modalPopup" />
</asp:Content>
