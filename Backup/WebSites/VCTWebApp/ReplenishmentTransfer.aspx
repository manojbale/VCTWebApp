<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReplenishmentTransfer.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.ReplenishmentTransfer" Title="Default" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script type="text/javascript">
        var selectedDate = null;
        function checkDate(sender, args) {            
            var CurrDate = new Date();            
            CurrDate = CurrDate.setDate(CurrDate.getDate() - 1);           

            if (sender._selectedDate < CurrDate) {
                alert("Please select future date.");
                sender._textbox.set_Value(selectedDate);
            }

        }

        $(function () {
            $(window).load(function () {
                selectedDate = document.getElementById('<%= txtRequiredOn.ClientID %>').value;
                fixedGrid();
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                fixedGrid();
            });

            function fixedGrid() {
                InitGridEvent('<%= gdvPartDetails.ClientID %>');
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
                                    <asp:Label ID="lblHeader" runat="server" Text="Replenishment Transfer"></asp:Label>
                                </td>
                            </tr>
                             <tr class="blank-table-row">
                                <td>                                                    
                                </td>
                            </tr>
                            <tr>
                                <td align="center">                                    
                                    <asp:RadioButtonList runat="server" AutoPostBack="true" ID="rblstLocationType" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rblstLocationType_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Region" Selected="True" />
                                        <asp:ListItem Text="Branch" />
                                        <asp:ListItem Text="Hospital" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">                                    
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRequiredOn" runat="server" Text="Required On:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRequiredOn" MaxLength="10" CssClass="textbox" runat="server"
                                                    Width="100px" Enabled="false" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Image ID="imgCalenderFrom" runat="server" ImageUrl="~/Images/calbtn.gif" />
                                            </td>
                                            <td>
                                                <ajaxtk:CalendarExtender ID="CalendarExtenderFrom" runat="server" PopupButtonID="imgCalenderFrom"
                                                    TargetControlID="txtRequiredOn" OnClientDateSelectionChanged="checkDate">
                                                </ajaxtk:CalendarExtender>
                                            </td>
                                            <td width="50px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLocation" runat="server" Text="Region:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlLocation" runat="server" Width="300px" CssClass="ListBox" AutoPostBack="True" 
                                                    onselectedindexchanged="ddlLocation_SelectedIndexChanged"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <br />
                                    <asp:Panel ID="pnlGrid" CssClass="pnlGrid" runat="server" Width="95%" ScrollBars="Auto"
                                        Height="330px">
                                        <asp:GridView ID="gdvPartDetails" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                            Width="100%" ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="7%" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ref #" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Bind("PartNum") %>' runat="server" ID="lblPartNum" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Description" HeaderStyle-Width="33%" ItemStyle-Width="33%" DataField="Description" />
                                                <asp:BoundField HeaderText="PAR Level Qty" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                    ItemStyle-HorizontalAlign="Center" DataField="PARLevelQty" />
                                                <asp:BoundField HeaderText="Available Qty" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                    ItemStyle-HorizontalAlign="Center" DataField="AvailableQty" />
                                                <asp:TemplateField HeaderText="Replenish Qty" HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtReplenishQty" runat="server" Text='<%# Bind("ReplenishQty") %>'
                                                            Width="90%" CssClass="textBox" MaxLength="3" />
                                                        <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                            FilterType="Custom" TargetControlID="txtReplenishQty" ValidChars="0123456789">
                                                        </ajaxtk:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server" Width="95%">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="50%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" CssClass="resetbutton" CausesValidation="False"
                                                        OnClick="btnNew_Click" />
                                                    <asp:Button ID="btnSave" runat="server" CssClass="savebutton" OnClick="btnSave_Click" />
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
