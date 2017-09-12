<%@ Page Title="Inventory Transfer - Party" Language="C#" MasterPageFile="~/Site1.master"
    AutoEventWireup="true" CodeBehind="HospitalInventoryTransfer.aspx.cs" Inherits="VCTWebApp.HospitalInventoryTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            SearchTextByCatalogNumberCountForHeader('txtNewPartNum', 'sCatalogNumber', 'GetCatalogCountByCatalogNumber', 'txtNewDescription', 'hdnPartNumNew', 'txtNewLotNum', 'txtNewAvailableQty');
        }


           
    </script>
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnKitFamilyId" runat="server" ClientIDMode="Static" />
            <table align="left" border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="0" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Hospital Inventory Transfer"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table border="0" width="90%" class="header-table" cellpadding="5" cellspacing="0">
                                        <tr>
                                            <td width="12%">
                                                <asp:Label ID="lblLocation" runat="server" Text="From Party :" />
                                            </td>
                                            <td width="45%">
                                                <asp:DropDownList ID="ddlFromParty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFromParty_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:RequiredFieldValidator ID="rfv_RegionBranchLocation" runat="server" ControlToValidate="ddlFromParty"
                                                    InitialValue="0" ValidationGroup="search" CssClass="required" Display="Static"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td width="10%">
                                                <asp:Label ID="lblToLocation" runat="server" Text="To Party :" />
                                            </td>
                                            <td width="55%">
                                                <asp:DropDownList ID="ddlToParty" runat="server">
                                                </asp:DropDownList>
                                               
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlToParty"
                                                    InitialValue="0" ValidationGroup="search" CssClass="required" Display="Static"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <%-- <td width="25%">
                                                <asp:RadioButtonList runat="server" ID="rblstInventoryType" Visible="false">
                                                    <asp:ListItem Text="Kit" />
                                                    <asp:ListItem Text="Part" Selected="True"/>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td width="5%">
                                                <div style="float: left; padding-left: 20px;">
                                                    <asp:Button ID="btnSearch" runat="server" ValidationGroup="search" OnClick="btnSearch_Click"
                                                        CssClass="smallviewbutton" Visible="false"/>
                                                </div>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <div id="controlHead">
                                    </div>
                                    <asp:Panel ID="pnlPart" ClientIDMode="Static" CssClass="pnlGrid" runat="server" Width="98%"
                                        ScrollBars="Auto" Height="340px">
                                        <asp:HiddenField ID="hdnPartNumNew" ClientIDMode="Static" runat="server" />
                                        <asp:GridView ID="gdvPartDetails" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                            OnRowCancelingEdit="gdvPartDetails_RowCancelingEdit" OnRowCommand="gdvPartDetails_RowCommand"
                                            EmptyDataText="No Record Found" OnRowEditing="gdvPartDetails_RowEditing" Width="100%"
                                            ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="Label1" runat="server" Text="Ref #" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtNewPartNum" ClientIDMode="Static" Width="90%" runat="server"
                                                                        onKeyUp="javascript:NewCatalogNumberKeyUp(this, event);" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartNum" runat="server" Text='<%# Eval("CatalogNumber") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label runat="server" Text="Lot #" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtNewLotNum" ClientIDMode="Static" runat="server" Width="90%" 
                                                                        CssClass="readonlytextbox" Enabled="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%# Eval("LotNum") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="45%">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" Text="Description" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtNewDescription" ClientIDMode="Static" runat="server" Width="95%"
                                                                         CssClass="readonlytextbox" Enabled="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label runat="server" Text="Available Qty" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtNewAvailableQty" ClientIDMode="Static" runat="server" Width="90%"
                                                                        Enabled="false" CssClass="readonlytextbox" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAvailableQty" runat="server" Text='<%# Eval("AvailableQty") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="Label3" runat="server" Text="Transfer Qty" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtNewTransferQty" MaxLength="3" Width="80%" runat="server" />
                                                                    <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                        FilterType="Custom" TargetControlID="txtNewTransferQty" ValidChars="0123456789">
                                                                    </ajaxtk:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTransferQty" runat="server" Text='<%# Eval("TransferQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtTransferQty" MaxLength="3" Width="90%" runat="server" Text='<%#Eval("TransferQty") %>'
                                                            CssClass="textbox" />
                                                        <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                            FilterType="Custom" TargetControlID="txtTransferQty" ValidChars="0123456789">
                                                        </ajaxtk:FilteredTextBoxExtender>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="Label4" runat="server" Text="Action" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <div style="width: 100%; padding-top: 20px;">
                                                                        <asp:LinkButton ID="lnkAdd" CausesValidation="false" CommandName="AddNewRow" runat="server">
                                                                            <asp:Image ID="imgAdd" runat="server" ImageUrl="~/Images/Add.gif" BorderStyle="None"
                                                                                ToolTip="Add" AlternateText="Add" /></asp:LinkButton>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" CausesValidation="false" CommandName="Edit" runat="server">
                                                            <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Images/Modify.gif" BorderStyle="None"
                                                                ToolTip="Edit" AlternateText="Edit" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="DeleteRec" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                            runat="server" OnClientClick="javascript:return confirm('Are you sure you want to delete this record ?');">
                                                            <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Images/Delete.gif" BorderStyle="None"
                                                                ToolTip="Delete" AlternateText="Delete" /></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" CommandName="UpdateRec" runat="server" CausesValidation="false">
                                                            <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Images/Update.gif" BorderStyle="None"
                                                                ToolTip="Update" AlternateText="Update" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" CausesValidation="false" CommandName="Cancel" runat="server">
                                                            <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Images/Cancel.gif" BorderStyle="None"
                                                                ToolTip="Cancel" AlternateText="Cancel" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
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
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click" />
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" ValidationGroup="search"
                                                        OnClick="btnSave_Click" />
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
