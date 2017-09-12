﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartyLocation.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.PartyLocation" Title="PartyLocation" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="3">
                                    <asp:Label ID="lblHeader" runat="server" Text="Party Location"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingPartyLocations" runat="server" Text="Existing Party Location(s)"
                                                    CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstPartyLocation" CssClass="leftlistboxlongForKit" runat="server"
                                                    AutoPostBack="True" Height="470px" OnSelectedIndexChanged="lstPartyLocation_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlPartyLocationDetail" runat="server" CssClass="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td align="left" style="width: 66%">
                                                                <asp:Label ID="lblParty" runat="server" Text="Party" CssClass="label"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 34%">
                                                                <asp:Label ID="lblLocationType" runat="server" Text="Location Type" CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtParty" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                                    Width="385px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocationType" Text="SHIPPING PARTY" runat="server" ReadOnly="true"
                                                                    ReadOnly="true" CssClass="readonlytextbox" Width="150px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlLocationAttributesDetail" runat="server" CssClass="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td colspan="3" align="left">
                                                                <asp:Label ID="lblLocationAttributes" runat="server" Text="Location Attributes" CssClass="SectionHeaderText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 33%">
                                                                <asp:Label ID="lblLocationName" runat="server" Text="Location Name" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 33%">
                                                                <asp:Label ID="lblLocationCode" runat="server" Text="Location Code" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 34%">
                                                                <asp:Label ID="lblGLN" runat="server" Text="GLN" CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtLocationName" runat="server" MaxLength="100" CssClass="textbox"
                                                                    Width="150px"></asp:TextBox>
                                                                <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator1" runat="server" ControlToValidate="txtLocationName"
                                                                    PropertyName="LocationName" SourceTypeName="VCTWeb.Core.Domain.PartyLocation"
                                                                    RulesetName="PartyLocation" DisplayMode="SingleParagraph" Display="Dynamic" SetFocusOnError="true" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocationCode" runat="server" MaxLength="10" CssClass="textbox"
                                                                    Width="150px"></asp:TextBox>
                                                                <cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator2" runat="server" ControlToValidate="txtLocationCode"
                                                                    PropertyName="Code" SourceTypeName="VCTWeb.Core.Domain.PartyLocation" RulesetName="PartyLocation"
                                                                    DisplayMode="SingleParagraph" Display="Dynamic" SetFocusOnError="true" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtGLN" runat="server" MaxLength="13" CssClass="textbox" Width="150px"></asp:TextBox>
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
                                                                <ajaxtk:FilteredTextBoxExtender ID="txtLatitude_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Custom" TargetControlID="txtLatitude" ValidChars="0123456789.-">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtLongitude" runat="server" MaxLength="25" Width="150px" CssClass="textbox"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="txtLongitude_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Custom" TargetControlID="txtLongitude" ValidChars="0123456789.-">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" colspan="3">
                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="TextBoxMultiLine" Width="630px"
                                                                    Height="50px" TextMode="MultiLine" MaxLength="255"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <%--<asp:Panel ID="pnlAddress" runat="server" CssClass="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td colspan="3" align="left">
                                                                <asp:Label ID="lblLocationAddress" runat="server" Text="Location Address" CssClass="SectionHeaderText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                        <tr>
                                                            <td style="width: 33%">
                                                                <asp:Label ID="lblAddress1" runat="server" CssClass="label" Text="Address 1"></asp:Label>
                                                            </td>
                                                            <td style="width: 33%">
                                                                <asp:Label ID="lblAddress2" runat="server" Text="Address 2" CssClass="label"></asp:Label>
                                                            </td>
                                                            <td style="width: 34%">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCity" runat="server" CssClass="label" Text="City"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblZipCode" runat="server" Text="Zip Code" CssClass="label"></asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCity" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="textbox" MaxLength="10" Width="150px"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="txtZipCode_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtZipCode" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr><td>&nbsp;</td></tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRegion" runat="server" Text="Region" CssClass="labelbold"></asp:Label>
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
                                                                <asp:TextBox ID="txtRegion" runat="server" CssClass="textbox" MaxLength="20" Width="150px" />
                                                                <asp:DropDownList ID="ddlRegion" runat="server" CssClass="ListBox" Width="150px" Visible="false"/>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtState" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="ListBox" Width="150px" Visible="false"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>--%>
                                                <asp:Panel ID="pnlAddress" runat="server" CssClass="pnlDetail" Height="120">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td colspan="3" align="left">
                                                                <asp:Label ID="lblLocationAddress" runat="server" Text="Location Address" CssClass="SectionHeaderText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 33%">
                                                                <asp:Label ID="lblAddress1" runat="server" CssClass="label" Text="Address 1"></asp:Label>
                                                            </td>
                                                            <td style="width: 33%">
                                                                <asp:Label ID="lblAddress2" runat="server" Text="Address 2" CssClass="label"></asp:Label>
                                                            </td>
                                                            <td style="width: 34%">
                                                                <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                                    Width="150px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                </asp:DropDownList>
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
                                                            <td>
                                                                <asp:Label ID="lblState" runat="server" CssClass="label" Text="State/Province"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCity" runat="server" CssClass="textbox" MaxLength="100" Width="150px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="textbox" MaxLength="10" Width="150px"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="txtZipCode_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="True" FilterType="Numbers" TargetControlID="txtZipCode" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
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
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CheckBox Text="IsActive" CssClass="CheckBox" runat="server" ID="chkIsActive" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="50%">
                                                    <asp:Panel ID="pnlButtonOnly" runat="server" Width="100%" Height="100%">
                                                        <%--<asp:Button ID="btnReset" runat="server" Text="" OnClick="btnReset_Click" CssClass="resetbutton"
                                                            CausesValidation="False" />--%>
                                                        <%--<asp:Button ID="btnDelete" runat="server" Text="" OnClick="btnDelete_Click" CssClass="deletebutton"
                                                            CausesValidation="False" />--%>
                                                        <asp:Button ID="btnSave" runat="server" Text="" OnClick="btnSave_Click" CssClass="savebutton" />
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
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
