<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuNavigation.ascx.cs"
    Inherits="WebApplication.Controls.MenuNavigation" %>
<table border="0" align="left" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div>
                            <asp:Menu ID="mnuNavigation" runat="server" StaticEnableDefaultPopOutImage="false"
                                SkinID="Menu">
                            </asp:Menu>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
