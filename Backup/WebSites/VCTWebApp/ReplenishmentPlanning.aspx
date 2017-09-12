<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true"
    CodeBehind="ReplenishmentPlanning.aspx.cs" Inherits="VCTWebApp.ReplenishmentPlanning" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function pageLoad() {
            InitGridEvent('<%= gridKitTable.ClientID %>');


        }

        $j = $.noConflict();

        $j(".ExpandRow").live("click", function () {

            if ($j(this).attr("src").toLowerCase() == "images/plus.png") {
                $j(this).next().show();
                $j(this).closest("tr").after("<td style='width:5%'></td><td colspan = '999'>" + $j(this).next().html() + "</td>");
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
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Replenishment Planning "></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <table>
                                        <tr>
                                            <td colspan="2">
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
                            <tr>
                                <td valign="top" align="left">
                                    <br />
                                    <asp:Panel CssClass="pnlGrid" ID="pnlKitTableGrid" runat="server">
                                        <table cellspacing="0" cellpadding="0" width="99%">
                                            <tr>
                                                <td align="left">
                                                    <%--  <asp:Label ID="lblInventoryDetail" runat="server" Text="Replenishment Planning" CssClass="SectionHeaderText"></asp:Label>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Panel ID="pnlKitGrid" Height="390px" Width="100%" ScrollBars="Auto" runat="server">
                                                        <asp:GridView ID="gridKitTable" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" OnRowDataBound="gridKitTable_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle Width="5%" />
                                                                    <ItemStyle Width="5%" />
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnCaseKitId" runat="server" Value='<%#Eval("CaseKitId") %>' />
                                                                        <asp:HiddenField ID="hdnBuildKitId" runat="server" Value='<%#Eval("BuildKitId") %>' />
                                                                        <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                            ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                        <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                            <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView">
                                                                                <Columns>
                                                                                    <asp:BoundField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataField="PartNum"
                                                                                        HeaderText="Part #" />
                                                                                    <asp:BoundField ItemStyle-Width="20%" DataField="Description" HeaderText="Description" />
                                                                                    <asp:BoundField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                                                        HeaderText="Lot #" />
                                                                                    <asp:BoundField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate"
                                                                                        HeaderText="Expiry Date" DataFormatString="{0:d}" />
                                                                                    <asp:BoundField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataField="UsageMarkedBy"
                                                                                        HeaderText="Marked By" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitNumberHeader" runat="server" Text="Assign Kit Number" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitNumber" runat="server" Text='<%#Eval("KitNumber") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="35%" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitFamilyHeader" runat="server" Text="Kit Family"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitFamily" runat="server" Text='<%#Eval("KitFamilyName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="15%" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblCaseNumberHeader" runat="server" Text="Case Number" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbllblCaseNumber" runat="server" Text='<%#Eval("CaseNumber") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
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
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
       
    </asp:UpdatePanel>
</asp:Content>
