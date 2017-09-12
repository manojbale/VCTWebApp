<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.ForgotPassword" Title="Forgot Password" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="0" cellspacing="0"
                            width="40%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Forgot Password"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="top" align="center">
                                                <asp:Panel ID="pnlUserDetail" runat="server" class="pnlDetail">
                                                    <input style="display: none" type="text" name="fakeusernameremembered" />
                                                    <input style="display: none" type="password" name="fakepasswordremembered" />
                                                    <table width="100%" cellspacing="0" cellpadding="2">
                                                        <tr>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblUserName" runat="server" Text="User Name" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 50%" />
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" Width="200px" MaxLength="64" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblNewPassword" runat="server" Text="New Password" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblSecurityQuestion" runat="server" Text="Security Question" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="textbox" Width="200px"
                                                                    MaxLength="50" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
                                                                <cc1:PasswordStrength ID="passwordStrengthExtender" runat="server" MinimumLowerCaseCharacters="1"
                                                                    MinimumUpperCaseCharacters="1" MinimumNumericCharacters="1" PreferredPasswordLength="8"
                                                                    RequiresUpperAndLowerCaseCharacters="true" Enabled="True" TargetControlID="txtNewPassword"
                                                                    TextCssClass="label">
                                                                </cc1:PasswordStrength>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtSecurityQuestion" runat="server" CssClass="textbox" Width="400px"
                                                                    MaxLength="250"  AutoCompleteType="Disabled" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm New Password" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblSecurityAnswer" runat="server" Text="Security Answer" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="textbox" Width="200px"
                                                                    MaxLength="50" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="textbox" Width="200px"
                                                                    MaxLength="50"  AutoCompleteType="Disabled"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="50%" valign="top">
                                                    <asp:Button ID="btnReset" runat="server" Text="" CssClass="resetbutton" OnClick="btnReset_Click"
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
