<%@ Page Language="C#" AutoEventWireup="true" Inherits="VCTWebApp.Web.ErrorPage"
    Title="Error" MasterPageFile="~/Site1.master" CodeBehind="ErrorPage.aspx.cs" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div style="height: 100px">
    </div>
    <div align="center">
        <asp:Panel ID="pnlError" runat="server" CssClass="loginbox">
            <table border="0" width="85%" cellpadding="20" cellspacing="0">
                <tr>
                    <td height="50px">
                    </td>
                </tr>
                <tr>
                    <td valign="top" width="15%">
                        <img src="Images/icon_error.png" />
                    </td>
                    <td align="left" width="85%" valign="top">
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="ErrorPageErrorText"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
