<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaseFindMatch.aspx.cs" Inherits="VCTWebApp.Shell.Views.CaseFindMatch"
    Title="CaseFindMatch" MasterPageFile="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <div class="maintable">
        <div class="header">
            <asp:Label ID="lblHeader" runat="server" Text="SEARCH RESULTS"></asp:Label>
        </div>
        <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
            <table width="100%">
                <tr>
                    <td width="70%">
                        <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                    </td>
                    <td width="30%">
                        <div class="viewMap">
                            <a id="link1" runat="Server">
                                <asp:Button ID="btnViewMap" CssClass="viewbuttonnew" Text="View Map" runat="server"
                                    OnClick="SlideShowImage1_Click" ToolTip="View Map" />
                            </a>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div class="container">
            <div>
                <div class="pnlDetail" style="min-height: 350px;">
                    <asp:HiddenField ID="hdnStatus" runat="server" />
                    <asp:Repeater ID="rptMain" runat="server" OnItemCommand="rptMain_ItemCommand" OnItemDataBound="rptMain_ItemDataBound">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Panel runat="server" BorderWidth="1px" Style="margin-top: 5px" Width="100%">
                                <table style="font-size: 13px;" class="ca-table-item" width="70%">
                                    <tr>
                                        <td align="left">
                                            <table style="margin-left:10px;">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Image ID="imgCheck" runat="server" ImageUrl="~/Images/check.png" Visible="false" /> &nbsp;
                                                    </td>
                                                    
                                                    <td align="left">
                                                        <span style="font-weight: bold; font-size: 17px; color: #4495de">
                                                            <%#DataBinder.Eval(Container, "DataItem.BranchName")%>
                                                        </span>                                                        
                                                        <asp:Image ID="imgBranchInfo" runat="server" ImageUrl="~/Images/MenuIcons/icon_about.png" />
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="left">
                                                <asp:Button ID="btnSendRequest" ToolTip="Send Request" CommandName="SendRequest"
                                                    CommandArgument="<%# ((RepeaterItem) Container).ItemIndex %>" runat="server"
                                                    CssClass="sendrequestbutton" Text="Send Request" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="Label1">Excess:&nbsp;</span>
                                            &nbsp;<span class="Label2"><%#DataBinder.Eval(Container, "DataItem.Excess")%>&nbsp;<asp:Image ID="imgExcessInfo" runat="server" ImageUrl="~/Images/MenuIcons/icon_about.png" /></span>
                                        </td>
                                        <td>
                                            <span class="Label1">Total Kit Built: &nbsp;</span><span class="Label2"><%#DataBinder.Eval(Container, "DataItem.TotalKitBuilt")%></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span class="Label1">Requested Quantity: &nbsp;</span><span class="Label2"><%#DataBinder.Eval(Container, "DataItem.Quantity")%></span>
                                        </td>
                                        <td>
                                            <span class="Label1">Total Kit Shipped: &nbsp;</span><span class="Label2"><%#DataBinder.Eval(Container, "DataItem.TotalKitShipped")%></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="Label1">Ship To: &nbsp;</span><span class="Label2"><%#DataBinder.Eval(Container, "DataItem.ShipToCustomer")%>&nbsp;<asp:Image
                                                ID="imgShipToInfo" runat="server" ImageUrl="~/Images/MenuIcons/icon_about.png" /></span>
                                        </td>
                                        <td>
                                            <span class="Label1">Total Kit Hold: &nbsp;</span><span class="Label2"><%#DataBinder.Eval(Container, "DataItem.TotalKitHold")%></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="Label1">Previous Check In: &nbsp;</span><span class="Label2"><%#DataBinder.Eval(Container, "DataItem.PreviousCheckInDate")%></span>
                                        </td>
                                        <td>
                                            <span class="Label1">Reserved Quantity: &nbsp;</span><span class="Label2"><%#DataBinder.Eval(Container, "DataItem.ReservedQty")%></span>
                                        </td>
                                    </tr>                                    
                                </table>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
