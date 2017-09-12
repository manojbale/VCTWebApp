<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Party.aspx.cs" Inherits="VCTWebApp.Shell.Views.Party"
    Title="Party" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTK" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="0" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Party"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingParties" runat="server" Text="Existing Parties" CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingParties" Height="400px" CssClass="leftlistboxlong" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstExistingParties_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" colspan="2">
                                                <asp:Panel ID="pnlPartyDetail" runat="server" class="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2">
                                                        <tr>
                                                            <td align="left" width="25%">
                                                                <asp:Label ID="lblPartyName" runat="server" Text="Party Name" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td align="left" width="25%">
                                                                <asp:Label ID="lblPartyCode" runat="server" Text="Party Code"></asp:Label>
                                                            </td>
                                                            <td align="left" width="25%">
                                                                <asp:Label ID="lblCompanyPrefix" runat="server" Text="Company Prefix"></asp:Label>
                                                            </td>
                                                            <td align="left" width="25%">
                                                                <asp:Label ID="lblDescription" runat="server" Text="Description" />
                                                                <asp:Label ID="lblLinkedLocation" runat="server" Text="Linked Location" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="150px" MaxLength="100"></asp:TextBox>
                                                                <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator1" runat="server" ControlToValidate="txtName"
                                                                    PropertyName="Name" SourceTypeName="VCTWeb.Core.Domain.Party" RulesetName="Party"
                                                                    DisplayMode="SingleParagraph" Display="Dynamic" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtCode" runat="server" CssClass="textbox" Width="150px" MaxLength="20"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtCompanyPrefix" runat="server" CssClass="textbox" Width="150px"
                                                                    MaxLength="10" />
                                                                <ajaxTK:FilteredTextBoxExtender ID="txtCompanyPrefix_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtCompanyPrefix" ValidChars="0123456789">
                                                                </ajaxTK:FilteredTextBoxExtender>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" Width="150px"
                                                                    MaxLength="255" />
                                                                <asp:DropDownList ID="ddlLinkedLocation" runat="server" Width="150px" CssClass="ListBox"
                                                                    Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblPartyType" runat="server" Text="Party Type" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblShippingDaysGap" runat="server" Text="Shipping Days Gap" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblRetrievalDaysGap" runat="server" Text="Retrieval Days Gap"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlPartyType" runat="server" Width="150px" CssClass="ListBox" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtShippingDaysGap" runat="server" CssClass="textbox" Width="150px"
                                                                    MaxLength="2" />
                                                                <ajaxTK:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                    FilterType="Numbers" TargetControlID="txtShippingDaysGap" ValidChars="0123456789">
                                                                </ajaxTK:FilteredTextBoxExtender>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtRetrievalDaysGap" runat="server" CssClass="textbox" Width="150px"
                                                                    MaxLength="2" />
                                                                <ajaxTK:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                    FilterType="Numbers" TargetControlID="txtRetrievalDaysGap" ValidChars="0123456789">
                                                                </ajaxTK:FilteredTextBoxExtender>
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBox ID="chkActive" runat="server" Text="Active" CssClass="CheckBox" />
                                                                <asp:CheckBox ID="chkOwner" runat="server" Text="Owner" CssClass="CheckBox" Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="50%">
                                                <asp:Panel ID="pnlAddress" runat="server" CssClass="pnlDetail" Height="280px">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td colspan="3" align="left">
                                                                <asp:Label ID="lblLocationAddress" runat="server" Text="Party Address" CssClass="SectionHeaderText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 50%">
                                                                <asp:Label ID="lblAddress1" runat="server" CssClass="label" Text="Address 1"></asp:Label>
                                                            </td>
                                                            <td style="width: 50%">
                                                                <asp:Label ID="lblAddress2" runat="server" Text="Address 2" CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCity" runat="server" CssClass="label" Text="City"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblZipCode" runat="server" Text="Zip Code" CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCity" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="textbox" MaxLength="10" Width="150px"></asp:TextBox>
                                                                <ajaxTK:FilteredTextBoxExtender ID="txtZipCode_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtZipCode" ValidChars="0123456789">
                                                                </ajaxTK:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblState" runat="server" CssClass="label" Text="State/Province"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                                    Width="150px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtState" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="ListBox" Width="150px" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblLatitude" runat="server" Text="Latitude" CssClass="label"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblLongitude" runat="server" Text="Longitude" CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtLatitude" runat="server" MaxLength="25" Width="150px" CssClass="textbox"></asp:TextBox>
                                                                <ajaxTK:FilteredTextBoxExtender ID="txtLatitude_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Custom" TargetControlID="txtLatitude" ValidChars="0123456789.-">
                                                                </ajaxTK:FilteredTextBoxExtender>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtLongitude" runat="server" MaxLength="25" Width="150px" CssClass="textbox"></asp:TextBox>
                                                                <ajaxTK:FilteredTextBoxExtender ID="txtLongitude_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Custom" TargetControlID="txtLongitude" ValidChars="0123456789.-">
                                                                </ajaxTK:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td align="left" width="50%">
                                                <asp:Panel ID="pnlPartyLocation" runat="server" CssClass="pnlDetail" Height="280px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblPartyLocation" runat="server" Text="Party Location" CssClass="SectionHeaderText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="pnlGrid" runat="server" CssClass="pnlGrid" Height="260px" ScrollBars="Auto" 
                                                                    Width="98%">
                                                                    <asp:GridView ID="gdvPartyLocation" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                                        ShowHeaderWhenEmpty="true" ShowHeader="true">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-Width="50%" ItemStyle-Width="50%">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblLocationHeader" runat="server" Text="Location" ForeColor="White" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("LocationName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-Width="40%" ItemStyle-Width="40%">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblLocationTypeHeader" runat="server" Text="Location Type" ForeColor="White" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLocationType" runat="server" Text='<%# Bind("LocationType") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="Center">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblSelect" runat="server" Text="Select" ForeColor="White" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" Text="" runat="server" Checked='<%# Eval("Selected") %>' />
                                                                                    <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%# Eval("LocationId") %>' />
                                                                                </ItemTemplate>
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
                                    </table>
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
                <asp:HiddenField ID="partyName" runat="server" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
