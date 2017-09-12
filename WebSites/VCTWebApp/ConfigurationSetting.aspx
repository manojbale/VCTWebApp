<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationSetting.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.ConfigurationSetting" Title="ConfigurationSetting"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTK" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center" valign="middle">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="85%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Configuration Settings"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 20%">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblListConfigSetting" runat="server" CssClass="listboxheading" Text="Existing Configuration Setting(s)"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingConfigSetting" Height="400px" CssClass="leftlistboxlong" AutoPostBack="true"
                                                    runat="server" OnSelectedIndexChanged="lstExistingConfigSetting_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left" width="80%">
                                    <asp:Panel ID="pnlConfigSettings" class="pnlGrid" runat="server">
                                        <table cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:GridView ID="gdvConfigSetting" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        DataKeyNames="KeyName,DataType" OnPageIndexChanging="gdvConfigSetting_PageIndexChanging"
                                                        OnRowCancelingEdit="gdvConfigSetting_RowCancelingEdit" OnRowDataBound="gdvConfigSetting_RowDataBound"
                                                        OnRowEditing="gdvConfigSetting_RowEditing" OnRowCommand="gdvConfigSetting_RowCommand"
                                                        OnRowUpdating="gdvConfigSetting_RowUpdating" PageSize="15" SkinID="GridView"
                                                         Width="630px" Height="34px">
                                                        <Columns>
                                                            <asp:TemplateField ControlStyle-Width="300" HeaderText="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ControlStyle Width="300px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Key" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblKey" runat="server" Text='<%# Bind("KeyName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <%--   <ControlStyle Width="125px" />--%>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Value" ControlStyle-Width="250">
                                                                <EditItemTemplate>
                                                                    <%--<%   
                                                                        if (isEDI)
                                                                        { %>
                                                                            <asp:DropDownList ID="ddlEdi" CssClass="ListBox" runat="server">
                                                                                <asp:ListItem Value="false">Local</asp:ListItem>
                                                                                <asp:ListItem Value="true">FTP</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                          <%}
                                                                        else
                                                                        { %>--%>
                                                                    <asp:TextBox ID="txtValue" runat="server" CssClass="textbox" MaxLength="255" Text='<%# Bind("KeyValue") %>'
                                                                        Width="220px"></asp:TextBox>
                                                                    <asp:TextBox ID="txtPropertyValueDate" Width="150Px" MaxLength="10" CssClass="TextBox"
                                                                        runat="server" Text='<%# Bind("KeyValue") %>' Enabled="false"></asp:TextBox>
                                                                    <asp:TextBox ID="txtIntValue" runat="server" CssClass="textbox" MaxLength="64" Text='<%# Bind("KeyValue") %>'
                                                                        Width="220px"></asp:TextBox>
                                                                    <ajaxTK:FilteredTextBoxExtender ID="ftxtIntValue" runat="server" Enabled="True" FilterType="Custom,Numbers"
                                                                        TargetControlID="txtIntValue" ValidChars="0123456789">
                                                                    </ajaxTK:FilteredTextBoxExtender>
                                                                    <asp:Image ID="imgCalender" Height="20" Width="30" ImageUrl="~/Images/calbtn.gif"
                                                                        runat="server" />
                                                                    <ajaxTK:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgCalender"
                                                                        TargetControlID="txtPropertyValueDate">
                                                                    </ajaxTK:CalendarExtender>
                                                                    <asp:DropDownList ID="ddlConfig" Width="150px" CssClass="ListBox" runat="server">
                                                                    </asp:DropDownList>
                                                                    <%--     <% }
                                                                    %>--%>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblValue" runat="server" Text='<%# Bind("KeyValue") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <%--   <ControlStyle Width="300px" />--%>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action" ControlStyle-Width="100px">
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="False" CommandName="Update"
                                                                        CssClass="link" Text="Update"></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                        CssClass="link" Text="Cancel"></asp:LinkButton>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                        CssClass="link" CommandArgument='<%# Eval("KeyName") %>' Text="Edit"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Width="100px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server" Height="25">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="70%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="30%" valign="top">
                                                    <%--<asp:Button ID="btnPublish" runat="server" Text="" CssClass="publishbutton" CausesValidation="False" OnClick="btnPublish_Click" />
                                                        <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" CausesValidation="False" OnClick="btnNew_Click" />
                                                        <asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click" />--%>
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
