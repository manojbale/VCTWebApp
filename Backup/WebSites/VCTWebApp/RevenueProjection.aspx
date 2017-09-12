<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RevenueProjection.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.RevenueProjection" Title="Revenue Projection"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
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
                InitGridEvent('<%= gdvRevenueProjection.ClientID %>');
            }

        });

        $j = $.noConflict();

        $j(".ExpandRow").live("click", function () {

            if ($j(this).attr("src").toLowerCase() == "images/plus.png") {
                $j(this).next().show();
                $j(this).closest("tr").after("<td style='width:3%'></td><td colspan = '999'>" + $j(this).next().html() + "</td>");
                $j(this).next().hide();
                $j(this).attr("src", "images/minus.png");
            }
            else {
                $j(this).attr("src", "images/plus.png");
                $j(this).closest("tr").next().next().hide();
                $j(this).closest("tr").next().hide();
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
                                        <asp:Label ID="lblHeader" runat="server" Text="Revenue Projection"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Panel ID="pnlFilters" runat="server" CssClass="pnlDetail" Width="95%">
                                            <table>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLocation" runat="server" Text="Location:&nbsp;" />
                                                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="ListBox" Width="200px" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButtonList runat="server" ID="rblPeriod" AutoPostBack="True" OnSelectedIndexChanged="rblPeriod_SelectedIndexChanged"
                                                                        RepeatDirection="Horizontal">
                                                                        <asp:ListItem Text="Next Week" Selected="True" />
                                                                        <asp:ListItem Text="Current Month" />
                                                                        <asp:ListItem Text="Date Range" />
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblStartDate" Text="Start Date:&nbsp;" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="100" Enabled="false" Text=""
                                                                        ClientIDMode="Static" />
                                                                    <asp:Image ID="imgCalenderFrom" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtenderFrom1" runat="server" PopupButtonID="imgCalenderFrom"
                                                                        TargetControlID="txtStartDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblEndDate" Text="End Date:&nbsp;" runat="server" />&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="100" Text="" Enabled="false" ClientIDMode="Static" />
                                                                    <asp:Image ID="Image1" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif" />
                                                                    <ajaxtk:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="Image1"
                                                                        TargetControlID="txtEndDate">
                                                                    </ajaxtk:CalendarExtender>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSearch" runat="server" ValidationGroup="search" OnClick="btnSearch_Click"
                                                                        CssClass="smallviewbutton" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top" colspan="2">
                                        <asp:Panel ID="pnlGrid" runat="server" CssClass="pnlGrid" Height="370px" ScrollBars="Auto"
                                            Width="99%">
                                            <asp:GridView ID="gdvRevenueProjection" runat="server" AutoGenerateColumns="false"
                                                OnRowDataBound="gdvRevenueProjection_RowDataBound" ShowHeaderWhenEmpty="true"
                                                SkinID="GridView" Width="99%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                Style="cursor: pointer; vertical-align: top;" />
                                                            <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Party" HeaderStyle-Width="36.5%" HeaderStyle-HorizontalAlign="Left"
                                                                            HeaderText="Hospital" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="36.5%" />
                                                                        <asp:BoundField DataField="KitRentalAmount" HeaderStyle-Width="16%" HeaderText="Kit Rental Amount"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%" />
                                                                        <asp:BoundField DataField="KitPartAmount" HeaderStyle-Width="16%" HeaderText="Kit Part Amount"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="16%" />
                                                                        <asp:BoundField DataField="PartAmount" HeaderStyle-Width="15.75%" HeaderText="Part Amount"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15.75%" />
                                                                        <asp:BoundField DataField="TotalAmount" HeaderStyle-Width="15.75%" HeaderText="Total Amount"
                                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15.75%" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LocationName" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderText="Location" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%" />
                                                    <asp:BoundField DataField="LocationType" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderText="Location Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%" />
                                                    <asp:BoundField DataField="KitRentalAmount" HeaderStyle-Width="15%" HeaderText="Kit Rental Amount"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                    <asp:BoundField DataField="KitPartAmount" HeaderStyle-Width="15%" HeaderText="Kit Part Amount"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                    <asp:BoundField DataField="PartAmount" HeaderStyle-Width="15%" HeaderText="Part Amount"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                    <asp:BoundField DataField="TotalAmount" HeaderStyle-Width="15%" HeaderText="Total Amount"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="98%" id="tblTotalBar" runat="server" visible="false">
                                            <tr class="header">
                                                <td width="40%" align="right">
                                                    <asp:Label ID="Label1" Text="Total: " runat="server" />&nbsp;
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="lblKitRentalTotal" Text="$ 0.00" runat="server" />
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="lblKitPartTotal" Text="$ 0.00" runat="server" />
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="lblPartTotal" Text="$ 0.00" runat="server" />
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="lblGrandTotal" Text="$ 0.00" runat="server" />
                                                </td>
                                            </tr>
                                           
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                 <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="70%">
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
</asp:Content>
