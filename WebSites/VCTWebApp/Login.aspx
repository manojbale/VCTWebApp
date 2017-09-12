<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VCTWebApp.Shell.Views.Login"
    MasterPageFile="~/Site1.master" Title="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            var dateSpn = $get('hdnTimeZoneOffset');
            var tzo = (new Date()).getTimezoneOffset();
            dateSpn.value = tzo;
        }
    </script>
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <div>
                <div class="loginbox" id="login-box">
                    <h2>
                        <asp:HiddenField ID="hdnTimeZoneOffset" runat="server" ClientIDMode="Static" />
                        <asp:Label ID="lblHeader" runat="server">Login</asp:Label></h2>
                    <asp:Label ID="Label2" runat="server" CssClass="ErrorText"></asp:Label>
                    <br>
                    <br>
                    <input style="display: none" type="text" name="fakeusernameremembered" />
                    <input style="display: none" type="password" name="fakepasswordremembered" />
                    <div class="login-box-name">
                        <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label>
                    </div>
                    <div class="login-box-field">
                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="32" CssClass="textbox" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>
                    <div class="login-box-name">
                        <asp:Label ID="lblPassword" runat="server" Text="Password" ></asp:Label>
                    </div>
                    <div class="login-box-field">
                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" CssClass="textbox" TextMode="Password" AutoCompleteType="Disabled"></asp:TextBox>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="" CssClass="loginButton" />
                            </td>
                            <td>
                                <asp:LinkButton ID="btnForgotPassword" runat="server" Font-Bold="true" CssClass="linkStyle"
                                    OnClick="btnForgotPassword_click" Text="Forgot Password" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div style="padding-left: 100px;">
                        <asp:Label ID="lblError" runat="server" CssClass="redlink" SkinID="labelboldred"></asp:Label>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
