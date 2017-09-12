<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NearExpiryItems.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.NearExpiryItems" Title="Near Expiry Items" MasterPageFile="~/Site1.master" %>

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
                InitGridEvent('<%= gdvNearExpiryItems.ClientID %>');
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
                                        <asp:Label ID="lblHeader" runat="server" Text="Expired / Near Expiry Items"></asp:Label>
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
                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="ListBox" Width="300px"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" />
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
                                                        <asp:GridView ID="gdvNearExpiryItems" runat="server" AutoGenerateColumns="false"
                                                            ShowHeaderWhenEmpty="true" SkinID="GridView" OnRowDataBound="gdvNearExpiryItems_RowDataBound"
                                                            OnRowCommand="gdvNearExpiryItems_RowCommand">
                                                            <Columns>
                                                                <asp:BoundField DataField="LocationName" HeaderStyle-Width="15%" HeaderText="Location"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                                <asp:BoundField DataField="LocationType" HeaderStyle-Width="15%" HeaderText="Location Type"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                                <asp:BoundField DataField="PartNum" HeaderStyle-Width="10%" HeaderText="Ref #"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="25%" />
                                                                <asp:BoundField DataField="LotNum" HeaderStyle-Width="10%" HeaderText="Lot #" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="10%" />
                                                                <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeastExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PartStatus" HeaderStyle-Width="10%" HeaderText="Status"
                                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" />
                                                                <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Trash" ItemStyle-HorizontalAlign="Center"
                                                                    ItemStyle-Width="5%">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton Visible="false" ID="lnkDelete" runat="server" CausesValidation="false"
                                                                            CommandArgument='<%#DataBinder.Eval(Container, "DataItem.LocationPartDetailId")%>'
                                                                            CommandName="DeleteRec">
                                                                            <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Images/Delete.gif" BorderStyle="None"
                                                                                ToolTip="Delete" AlternateText="Delete" /></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br />
                                        <asp:Label ID="Label1" Text="* Items in red denotes Expired Items" ForeColor="Red"
                                            runat="server" />
                                        <br />
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
