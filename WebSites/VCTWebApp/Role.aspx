<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role.aspx.cs" Inherits="VCTWebApp.Shell.Views.Role"
    Title="Role" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript">
//                $(function () {
//                    $(window).load(function () {
//                        fixedGrid();
//                    });

//                    var updm1 = Sys.WebForms.PageRequestManager.getInstance();

//                    updm1.add_endRequest(function () {
//                        fixedGrid();
//                    });

//                    function fixedGrid() {
//                        InitGridEvent('<%= gdvPermissionList.ClientID %>');
//                    }
//                });
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
                                    <asp:Label ID="lblHeader" runat="server" Text="Role"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingRoles" runat="server" Text="Existing Role(s)" CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingRoles" Height="400px" CssClass="leftlistboxlong" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstExistingRoles_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <asp:Panel ID="pnlRoleDetail" runat="server" class="pnlDetail">
                                        <table width="100%" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td align="left" style="width: 50%">
                                                    <asp:Label ID="lblRoleName" runat="server" Text="Role Name" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" style="width: 50%">
                                                    <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRoleName" runat="server" CssClass="textbox" Width="200px" MaxLength="24"></asp:TextBox>
                                                    <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator1" runat="server" ControlToValidate="txtRoleName"
                                                        PropertyName="RoleName" SourceTypeName="VCTWeb.Core.Domain.Role" RulesetName="Role"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" Width="200px"
                                                        MaxLength="64"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel CssClass="pnlGrid" ID="pnlPermission" runat="server">
                                        <table cellspacing="0" cellpadding="0" width="80%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblPermissions" runat="server" Text="Permission(s)" CssClass="SectionHeaderText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Panel ID="pnlProductAttributes" runat="server" CssClass="pnlGrid" Height="320px"
                                                        ScrollBars="Auto" Width="95%">
                                                        <asp:GridView ID="gdvPermissionList" runat="server" SkinID="GridView"
                                                            AutoGenerateColumns="False" OnRowDataBound="gdvPermissionList_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Entity">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblEntity" runat="server" Text="Entity"></asp:Label><br />
                                                                        <asp:DropDownList ID="ddlSelectEntity" runat="server" OnSelectedIndexChanged="ddlSelectEntity_SelectedIndexChanged"
                                                                            CssClass="ListBox" Width="100%" AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEntity" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle VerticalAlign="Top" Width="40%" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Permission">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPermissionCode" runat="server" Text='<%# Bind("PermissionCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="40%" />
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblSelect" runat="server" Text="Select"></asp:Label><br />
                                                                        <asp:LinkButton ID="lnkSelectAll" runat="server" CssClass="link" CausesValidation="False"
                                                                            OnClick="lnkSelectAll_Click">All</asp:LinkButton>
                                                                        &nbsp;<asp:LinkButton ID="lnkSelectNone" runat="server" CssClass="link" CausesValidation="False"
                                                                            OnClick="lnkSelectNone_Click">None</asp:LinkButton>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("GrantPermission") %>'
                                                                            CssClass="CheckBox" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle VerticalAlign="Top" Width="20%" />
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
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="50%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click"
                                                        CausesValidation="False" />
                                                    <asp:Button ID="btnDelete" runat="server" Text="" OnClick="btnDelete_Click" CssClass="deletebutton"
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
