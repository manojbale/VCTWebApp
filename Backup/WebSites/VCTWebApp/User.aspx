<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="VCTWebApp.Shell.Views.User"
    Title="User" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript">
        //        $(function () {
        //            $(window).load(function () {
        //                fixedGrid();
        //            });

        //            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

        //            updm1.add_endRequest(function () {
        //                fixedGrid();
        //            });

        //            function fixedGrid() {
        //                InitGridEvent('<%= gdvRoleList.ClientID %>');                
        //            }

        //        });
    </script>


     <script type="text/javascript">

         function keyPress(e) {

             var key = (e.keyCode ? e.keyCode : e.which);
             var valid = (key >= 48 && key <= 57) || (key == 45);
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
                                    <asp:Label ID="lblHeader" runat="server" Text="User"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingUsers" runat="server" Text="Existing User(s)" CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingUsers" Height="570px" CssClass="leftlistboxlong" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstExistingUsers_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <asp:Panel ID="pnlUserDetail" runat="server" class="pnlDetail">
                                        <table width="100%" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblUserId" runat="server" Text="User ID" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtUserId" runat="server" CssClass="textbox" Width="200px" MaxLength="32"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator1" runat="server" ControlToValidate="txtUserId"
                                                        PropertyName="UserName" SourceTypeName="VCTWeb.Core.Domain.Users" RulesetName="User"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:DropDownList ID="ddlLocation" runat="server" Width="200px" CssClass="ListBox" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
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
                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="textbox" Width="200px" MaxLength="64"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator2" runat="server" ControlToValidate="txtFirstName"
                                                        PropertyName="FirstName" SourceTypeName="VCTWeb.Core.Domain.Users" RulesetName="User"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="textbox" Width="200px" MaxLength="64"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator5" runat="server" ControlToValidate="txtLastName"
                                                        PropertyName="LastName" SourceTypeName="VCTWeb.Core.Domain.Users" RulesetName="User"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblSecurityQuestion" runat="server" Text="Security Question" CssClass="labelbold"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtSecurityQuestion" runat="server" CssClass="textbox" Width="580"
                                                        MaxLength="250"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator8" runat="server" ControlToValidate="txtSecurityQuestion"
                                                        PropertyName="SecurityQuestion" SourceTypeName="VCTWeb.Core.Domain.Users" RulesetName="User"
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
                                                    <asp:Label ID="lblSecurityAnswer" runat="server" Text="Security Answer" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="textbox" Width="200px"
                                                        MaxLength="50"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator9" runat="server" ControlToValidate="txtSecurityAnswer"
                                                        PropertyName="SecurityAnswer" SourceTypeName="VCTWeb.Core.Domain.Users" RulesetName="User"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="textbox" Width="200px" MaxLength="15" onkeypress="keyPress(event);" ></asp:TextBox>
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
                                                    <asp:Label ID="lblCell" runat="server" Text="Cell" ></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="textbox" Width="200px" MaxLength="64"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator3" runat="server" ControlToValidate="txtEmailId"
                                                        PropertyName="Email" SourceTypeName="VCTWeb.Core.Domain.Users" RulesetName="User"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtCell" runat="server" CssClass="textbox" Width="200px" MaxLength="12" onkeypress="keyPress(event);" ></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:CheckBox ID="chkResetPassword" runat="server" Text="Reset Password" CssClass="CheckBox" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:CheckBox ID="chkActive" runat="server" Text="Active" CssClass="CheckBox" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtFax" runat="server" CssClass="textbox" Width="200px" MaxLength="20" onkeypress="keyPress(event);" ></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rdoSystemUser" runat="server" CssClass="RadioBox" Text="System User" Visible="false"
                                                        GroupName="UserType" Checked="true" AutoPostBack="True" OnCheckedChanged="rdoSystemUser_CheckedChanged" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblDomain" runat="server" Text="Domain" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:RadioButton ID="rdoDomainUser" runat="server" CssClass="RadioBox" Text="Domain User" Visible="false"
                                                        GroupName="UserType" Checked="false" AutoPostBack="True" OnCheckedChanged="rdoDomainUser_CheckedChanged" />
                                                </td>
                                                <td align="left" width="50%">
                                                    <asp:TextBox ID="txtDomain" Enabled="false" runat="server" CssClass="textbox" Width="200px" Visible="false"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlRole" CssClass="pnlDetail" runat="server">
                                        <table cellspacing="0" cellpadding="0" width="50%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblRoles" runat="server" Text="Role(s)" CssClass="SectionHeaderText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr><td>&nbsp;</td></tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="pnlProductAttributes" runat="server" CssClass="pnlGrid" Height="165px"
                                                        ScrollBars="Auto" Width="95%">
                                                        <asp:GridView ID="gdvRoleList" runat="server" SkinID="GridView"
                                                            Width="100%" AutoGenerateColumns="False" 
                                                            OnRowDataBound="gdvRoleList_RowDataBound" PageSize="5">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Role">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="210px" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("GrantRole") %>' CssClass="CheckBox" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="75px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
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
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="70%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="30%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click"
                                                        CausesValidation="False" />
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click" />
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
