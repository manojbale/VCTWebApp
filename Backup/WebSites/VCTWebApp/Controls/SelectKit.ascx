<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectKit.ascx.cs" Inherits="VCTWebApp.Shell.Views.SelectKit" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <table align="left" border="0" width="300px">
            <tr>
                <td>
                    <table class="maintablePopUp" border="0" align="center" cellpadding="3" cellspacing="0"
                        width="100%">
                        <tr class="header">
                            <td width="100%" align="center">
                                <asp:Label ID="lblHeader" runat="server" />
                            </td>
                            <td align="right" valign="top">
                                <asp:Button ID="btnClose" runat="server" CssClass="closebutton" OnClick="btnClose_Click"
                                    CausesValidation="False" />
                            </td>
                        </tr>
                        <tr align="left">
                            <td colspan="2">
                                <asp:Label runat="server" ID="lblMessage" Text="ITS has found the following matches for the indicative Catalog Numbers. Please select one Kit to proceed." />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2">
                                <asp:ListBox Height="200px" runat="server" Width="100%" ID="lstKit" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2">
                                <%--<asp:Button ID="btnSelect" Text="Select" runat="server" OnClick="btnSelect_Click" />--%>
                                <%--<asp:Panel padding-left="5px" ID="pnlButton" CssClass="ActionPanel" runat="server">--%>
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" width="100%">
                                            <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                        </td>
                                        <td align="right" width="20%">
                                            <asp:Button ID="btnSelect" runat="server" Enabled="false" CssClass="okbutton" OnClick="btnSelect_Click"
                                                ToolTip="Select" />
                                        </td>
                                    </tr>
                                </table>
                                <%--</asp:Panel>--%>
                            </td>
                        </tr>
                        <%--<tr align="left">
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                    </td>
                </tr>--%>
                    </table>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
