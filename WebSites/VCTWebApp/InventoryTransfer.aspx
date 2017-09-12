<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InventoryTransfer.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.InventoryTransfer" Title="Inventory Transfer - Location"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            //SearchKitFamilyCountNLastUsageByName('txtKitFamily', 'sKitFamily', 'GetKitFamilyCountNLastUsageByName', 'hdnKitFamilyId', 'txtDes', 'txtLastUsage', 'txtAvailableQty');
            GetKitFamilyByLocationAndNumber('txtKitFamily', 'sKitFamily', 'GetKitFamilyByLocationAndNumber', 'hdnKitFamilyId', 'txtDes');
            SearchTextByCatalogNumberForHeader('txtNewPartNum', 'sCatalogNumber', 'GetCatalogByCatalogNumber', 'txtNewDescription', 'hdnPartNumNew');
            //               InitGridEvent('<%= gdvKit.ClientID %>');
            //               InitGridEvent('<%= gdvPart.ClientID %>');
        }

        $(function () {

            $(window).load(function () {
                InitGridEvent('<%= gdvKit.ClientID %>');
                InitGridEvent('<%= gdvPart.ClientID %>');
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                InitGridEvent('<%= gdvKit.ClientID %>');
                InitGridEvent('<%= gdvPart.ClientID %>');
            });


        });

        function FixedGrid() {

            if ($('#radKit').attr('checked')) {
                $('#tblKit').css('display', 'block');
                if ($('#pnlMain').hasScrollBar()) {
                    var tblWidth1 = $('#tblKit').width() - 17;
                    $('#tblKit').css('width', tblWidth1 + 'px');
                }

            }
            else {
                $('#tblPart').css('display', 'block');
                if ($('#pnlMain').hasScrollBar()) {
                    var tblWidth1 = $('#tblPart').width() - 17;
                    $('#tblPart').css('width', tblWidth1 + 'px');
                    $('#tblPart').css('width', tblWidth1 + 'px');
                }

            }

        }


        function KitFamilyKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnKitFamilyId.ClientID %>');
                if (myHidden) {
                    myHidden.value = '0';
                    document.getElementById('<%= txtDes.ClientID %>').value = '';
                }
            }
        }

        function keyPress(e) {
           var key;
            if (window.event)
                key = window.event.keyCode; //IE
            else
                key = e.which; //firefox
            var valid = (key >= 48 && key <= 57);
          
            if (!valid) {
                if (window.event) {
                    window.event.returnValue = false;
                }
                else {
                    e.preventDefault();
                }
            }
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
                                    <asp:Label ID="lblHeader" runat="server" Text="Inventory Transfer"></asp:Label>
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
                                            <td width="15%">
                                                <asp:Label ID="lblLocation" runat="server" Text="From :" />
                                            </td>
                                            <td width="55%">
                                                <asp:DropDownList ID="ddlRegionBranchLocation" runat="server">
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:RequiredFieldValidator ID="rfv_RegionBranchLocation" runat="server" ControlToValidate="ddlRegionBranchLocation"
                                                    InitialValue="0" ValidationGroup="search" CssClass="required" Display="Dynamic"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lblToLocation" runat="server" Text="To" />
                                                &nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlToRegionBranch" runat="server">
                                                </asp:DropDownList>
                                                &nbsp;
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlToRegionBranch"
                                                    InitialValue="0" ValidationGroup="search" CssClass="required" Display="Dynamic"
                                                    ErrorMessage="Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                            </td>
                                            <td width="30%">
                                                <asp:RadioButton ID="radKit" ClientIDMode="Static" CssClass="rad-type" runat="server"
                                                    GroupName="InvtType" Checked="true" />
                                                <asp:Label ID="lblKit" runat="server" Text="Kit" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:RadioButton ID="radPart" ClientIDMode="Static" CssClass="rad-type" runat="server"
                                                    GroupName="InvtType" />
                                                <asp:Label ID="lblParts" runat="server" Text="Parts" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td width="51%">
                                                            <asp:RadioButton ID="radUnUtilizeOpt" ClientIDMode="Static" CssClass="rad-type" runat="server"
                                                                GroupName="ItemOpt" Checked="true" AutoPostBack="true" OnCheckedChanged="radUnUtilizeOpt_CheckedChanged" />
                                                            <asp:Label ID="Label1" runat="server" Text="Un-Utilize Inventory" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:RadioButton ID="radAdhocOpt" ClientIDMode="Static" CssClass="rad-type" GroupName="ItemOpt"
                                                                runat="server" AutoPostBack="true" OnCheckedChanged="radAdhocOpt_CheckedChanged" />
                                                            <asp:Label ID="Label2" runat="server" Text="Ad-Hoc" />
                                                        </td>
                                                        <td width="40%">
                                                            <div style="float: left;">
                                                                <div style="float: left;">
                                                                    <asp:Label ID="lblInventoryNotInUse" runat="server" Text="Number of days Inv. not in use:" />
                                                                    <asp:TextBox ID="txtInventoryDays" runat="server" Text="0" Width="100px"  onkeypress="keyPress(event);" MaxLength="5" />
                                                                    <asp:RequiredFieldValidator ID="rfv_InventoryDays" runat="server" ControlToValidate="txtInventoryDays"
                                                                        ValidationGroup="search" CssClass="required" Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td width="10%">
                                                            <div style="float: left; padding-left: 20px;">
                                                                <asp:Button ID="btnSearch" runat="server" ValidationGroup="search" OnClick="btnSearch_Click"
                                                                    CssClass="smallviewbutton" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
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
                                    <asp:Panel ID="pnlMain" ClientIDMode="Static" CssClass="pnlGrid" runat="server" Width="98%"
                                        ScrollBars="Auto" Height="340px">
                                        <asp:Panel ID="pnlAdHocKit" runat="server" Visible="false">
                                            <table width="98%" border="0" cellpadding="5" cellspacing="0" align="center">
                                                <tr style="background-color: #4f81bd; color: White; font-weight: bold; height: 25px;">
                                                    <td align="center">
                                                        Kit Family
                                                    </td>
                                                    <td align="center">
                                                        Description
                                                    </td>
                                                    <td align="center">
                                                        Transfer Qty
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="15%" align="center">
                                                        <asp:TextBox ID="txtKitFamily" runat="server" Width="90%" AutoCompleteType="None"
                                                            ClientIDMode="Static" onKeyUp="javascript:KitFamilyKeyUp(this, event);"></asp:TextBox>
                                                    </td>
                                                    <td width="50%" align="center">
                                                        <asp:TextBox ID="txtDes" runat="server" Width="95%" Enabled="false" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <%-- <td width="15%" align="center">
                                                    <asp:TextBox ID="txtLastUsage" runat="server" width="80%" Enabled="false"  ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                                <td width="10%" align="center">
                                                    <asp:TextBox ID="txtAvailableQty" runat="server" width="80%" Enabled="false" ClientIDMode="Static"></asp:TextBox>
                                                </td>--%>
                                                    <td width="10%" align="center">
                                                        <asp:TextBox ID="txtTransferQty" runat="server" Width="80%" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAdHocPart" runat="server" Visible="false">
                                            <asp:HiddenField ID="hdnPartNumNew" ClientIDMode="Static" runat="server" />
                                            <asp:GridView ID="gdvPartDetails" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                OnRowCancelingEdit="gdvPartDetails_RowCancelingEdit" OnRowCommand="gdvPartDetails_RowCommand"
                                                EmptyDataText="No Record Found" OnRowEditing="gdvPartDetails_RowEditing" Width="100%"
                                                ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnKitFamilyItemId" runat="server" Value='<%# Eval("KitFamilyItemId") %>' />
                                                            <asp:Label ID="lblPartNum" runat="server" Text='<%# Eval("CatalogNumber") %>' />
                                                        </ItemTemplate>
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
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="60%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label2" runat="server" Text="Description" />
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
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPARLevelQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtPartQty" MaxLength="3" Width="90%" runat="server" Text='<%#Eval("Quantity") %>'
                                                                CssClass="textbox" />
                                                            <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                FilterType="Custom" TargetControlID="txtPartQty" ValidChars="0123456789">
                                                            </ajaxtk:FilteredTextBoxExtender>
                                                        </EditItemTemplate>
                                                        <HeaderTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label3" runat="server" Text="Quantity" />
                                                                        <br />
                                                                        <br />
                                                                        <hr size="1px" color="White" />
                                                                        <br />
                                                                        <asp:TextBox ID="txtNewPartQty" MaxLength="3" Width="90%" runat="server" />
                                                                        <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                            FilterType="Custom" TargetControlID="txtNewPartQty" ValidChars="0123456789">
                                                                        </ajaxtk:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" CausesValidation="false" CommandName="Edit" runat="server">
                                                                <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Images/Modify.gif" BorderStyle="None"
                                                                    ToolTip="Edit" AlternateText="Edit" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>&nbsp;&nbsp;
                                                            <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="DeleteRec" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                runat="server" OnClientClick= "javascriprt:return confirm('Are you sure you want to delete this record ? ');">
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
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <%--<table width="98%" border="0" cellpadding="5" cellspacing="0" align="center">
                                            <tr style="background-color:#4f81bd; color:White; font-weight:bold; height:25px;">
                                                <td align="center">
                                                    Part #
                                                </td>
                                                <td align="center">
                                                    Description
                                                </td>
                                                <td align="center">
                                                    Last Usage
                                                </td>
                                                <td align="center">
                                                    Available Qty
                                                </td>
                                                <td align="center">
                                                    Transfer Qty
                                                </td>
                                            </tr>
                                            <tr style="background-color:#4f81bd; color:White; font-weight:bold; height:25px;">
                                                <td width="15%" align="center">
                                                    <asp:TextBox ID="TextBox1" runat="server" width="90%" AutoCompleteType="None" ClientIDMode="Static" onKeyUp="javascript:KitFamilyKeyUp(this, event);"></asp:TextBox>
                                                </td>
                                                <td width="50%" align="center">
                                                    <asp:TextBox ID="TextBox2" runat="server" width="95%" Enabled="false"  ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                                <td width="15%" align="center">
                                                    <asp:TextBox ID="TextBox3" runat="server" width="80%" Enabled="false"  ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                                <td width="10%" align="center">
                                                    <asp:TextBox ID="TextBox4" runat="server" width="80%" Enabled="false" ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                                <td width="10%" align="center">
                                                    <asp:TextBox ID="TextBox5" runat="server"  width="80%" ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>--%>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlUnUtilizeKit" runat="server" Visible="false">
                                            <asp:GridView ID="gdvKit" runat="server" AutoGenerateColumns="False" Visible="false"
                                                SkinID="GridView" OnRowDataBound="gdvKit_RowDataBound" ShowHeader="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblSelectHeader" runat="server" Text="Select" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" Text="" runat="server" />
                                                            <asp:HiddenField ID="hdnKitFamilyId" runat="server" Value='<%#Eval("KitFamilyId") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblKitFamilyHeader" runat="server" Text="Kit Family" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblKitFamily" runat="server" Text='<%# Eval("KitFamilyName") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="30%" ItemStyle-Width="30%">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblLastUsageHeader" runat="server" Text="Last Usage" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastUsage" runat="server" Text='<%# Eval("LastUsage", "{0:d}") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAvailableQtyHeader" runat="server" Text="Available Qty" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAvailableQty" runat="server" Text='<%# Eval("AvailableQty") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblTransferQtyHeader" runat="server" Text="Transfer Qty" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQuantity" runat="server" Text="0" Width="80%"  onkeypress="keyPress(event);" MaxLength="5"/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblTransferToLocation" runat="server" Text="Transfer To" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlTransferToLocation" runat="server">                                                        
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />                                                    
                                                </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlUnUtilizePart" runat="server" Visible="false">
                                            <asp:GridView ID="gdvPart" runat="server" AutoGenerateColumns="False" Visible="false"
                                                SkinID="GridView" OnRowDataBound="gdvPart_RowDataBound" ShowHeader="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblSelectHeader" runat="server" Text="Select" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" Text="" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblPartNumHeader" runat="server" Text="Part Num" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPartNum" runat="server" Text='<%#Eval("PartNum") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="45%" ItemStyle-Width="45%" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Description") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblLastUsageHeader" runat="server" Text="Last Usage" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastUsage" runat="server" Text='<%#Eval("LastUsage","{0:d}") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblAvailableQtyHeader" runat="server" Text="Available Qty" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAvailableQty" runat="server" Text='<%# Bind("AvailableQty") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblTransferQtyHeader" runat="server" Text="Transfer Qty" ForeColor="White" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQuantity" runat="server" Text="0" Width="50%"  onkeypress="keyPress(event);" MaxLength="5"/>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <%-- <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblTransferToLocation" runat="server" Text="Transfer To" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlTransferToLocation" runat="server" Width="90%">                                                        
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />                                                    
                                                </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="radUnUtilizeOpt" />
            <asp:AsyncPostBackTrigger ControlID="radAdhocOpt" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
