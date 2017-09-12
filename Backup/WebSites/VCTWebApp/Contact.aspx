<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="VCTWebApp.Shell.Views.Contact"
    Title="Contact" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript">

        function keyPress(e) {
   
            var key = (e.keyCode ? e.keyCode : e.which);
            var valid = (key >= 48 && key <= 57) || (key == 45) ;
            if (!valid) {
                if (window.event) {
                    window.event.returnValue = false;
                }
                else {
                    e.preventDefault();
                }
            }
        }
         


    </script>
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="0" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Contact"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingContatcts" runat="server" Text="Existing Contact(s)" CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingContacts" Height="400px" CssClass="leftlistboxlong" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstExistingContacts_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <asp:Panel ID="pnlContactDetail" runat="server" class="pnlDetail">
                                        <table width="100%" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblFirstName" runat="server" Text="First Name" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblLastName" runat="server" Text="Last Name" CssClass="labelbold"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="textbox" Width="200px" MaxLength="50"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator2" runat="server" ControlToValidate="txtFirstName"
                                                        PropertyName="FirstName" SourceTypeName="VCTWeb.Core.Domain.Contact" RulesetName="Contact"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="textbox" Width="200px" MaxLength="50"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator5" runat="server" ControlToValidate="txtLastName"
                                                        PropertyName="LastName" SourceTypeName="VCTWeb.Core.Domain.Contact" RulesetName="Contact"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblEmailId" runat="server" Text="Email ID"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="textbox" Width="200px" MaxLength="64"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator3" runat="server" ControlToValidate="txtEmailId"
                                                        PropertyName="Email" SourceTypeName="VCTWeb.Core.Domain.Contact" RulesetName="Contact"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox" Width="200px" MaxLength="12"  onkeypress="keyPress(event);" ></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblCell" runat="server" Text="Cell"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtCell" runat="server" CssClass="textbox" Width="200px" MaxLength="12"  onkeypress="keyPress(event);" ></asp:TextBox>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="textbox" Width="200px" MaxLength="12"  onkeypress="keyPress(event);" ></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="pnlLocationContact" runat="server" CssClass="pnlGrid" Height="170px"
                                                        ScrollBars="Auto" Width="95%">
                                                        <asp:GridView ID="gdvLocationContact" runat="server" SkinID="GridView" Width="100%"
                                                            AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Location">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField runat="server" ID="hdnLocationId" Value='<%# Bind("LocationId") %>' />
                                                                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Bind("LocationName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="250px" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("Selected") %>' CssClass="CheckBox" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="75px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                                <%--<td align="left" width="50%">
                                                    <asp:Label ID="lblSalesOffice" runat="server" Text="Sales Office"></asp:Label>
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:CheckBox ID="chkActive" runat="server" Text="Active" CssClass="CheckBox" />
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td align="left" width="50%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:DropDownList ID="ddlSalesOfficeLocation" runat="server" CssClass="ListBox" Width="200px" />
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="70%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText" ></asp:Label>
                                                </td>
                                                <td align="right" width="30%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click"
                                                        CausesValidation="False" />
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click"  />
                                                    <%--<asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click"
                                                        OnClientClick="javascript:return Validate();" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:HiddenField ID="hdnEmailId" runat="server" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
