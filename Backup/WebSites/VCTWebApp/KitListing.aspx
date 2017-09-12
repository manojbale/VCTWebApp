<%@ Page Language="C#" AutoEventWireup="true" Inherits="VCTWebApp.Shell.Views.KitListing"
    Title="KitListing" MasterPageFile="~/Site1.master" CodeBehind="KitListing.aspx.cs" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript">

        function pageLoad() {        
            SearchKitFamilyByNumber('txtHeadKitFamily', 'sKitFamily', 'GetKitFamilyByNumber', 'hdnHeadKitFamilyId');
            SearchKitFamilyByNumber('txtKitFamily', 'sKitFamily', 'GetKitFamilyByNumber', 'hdnKitFamilyId');
            SearchTextByCatalogNumberForFooter('txtNewCatalogNum', 'sCatalogNumber', 'GetCatalogByCatalogNumber');
            SearchTextByProcedureName('txtProcedureName',' ', 'sProcedureName', 'GetProceduresByProcedureName');
        }

        function NewCatalogNumberKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnCatalogNumberNew.ClientID %>');
                if (myHidden) {
                    myHidden.value = '';
                }
            }
        }

        function ProcedureNameKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnProcedureName.ClientID %>');
                if (myHidden) {
                    myHidden.value = '';
                }
            }
        }

        function KitFamilyHeadKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnHeadKitFamilyId.ClientID %>');
                if (myHidden) {
                    myHidden.value = '0';
                }
            }
        }

        function KitFamilyKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnKitFamilyId.ClientID %>');
                if (myHidden) {
                    myHidden.value = '0';
                }
            }
        }

        $(function () {
            $(window).load(function () {
                fixedGrid();
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                fixedGrid();
            });

            function fixedGrid() {
                InitGridEvent('<%= gridKitTable.ClientID %>');
            }
        });
    </script>
    <script type="text/javascript">
        // It is important to place this JavaScript code after ScriptManager1
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('<%=pnlGridKitTable.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=pnlGridKitTable.ClientID%>').scrollLeft;
                yPos = $get('<%=pnlGridKitTable.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('<%=pnlGridKitTable.ClientID%>') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('<%=pnlGridKitTable.ClientID%>').scrollLeft = xPos;
                $get('<%=pnlGridKitTable.ClientID%>').scrollTop = yPos;
            }
        }

        function SavePnlGridKitTableScrollPos() {
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);
        }
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
                                    <asp:Label ID="lblHeader" runat="server" Text="Kit Listing"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblHeadKitFamily" runat="server" Text="Kit Family *:&nbsp;" CssClass="labelbold"  />
                                            </td>
                                            <td>
                                                <%--<asp:DropDownList ID="ddlHeadKitFamily" Visible="false" runat="server" Width="300px" CssClass="ListBox"  AutoPostBack="true" onselectedindexchanged="ddlHeadKitFamily_SelectedIndexChanged"/>--%>
                                                
                                                <asp:TextBox ID="txtHeadKitFamily" runat="server" ClientIDMode="Static" 
                                                    Width="300px" AutoCompleteType="None" AutoPostBack="true" ontextchanged="txtHeadKitFamily_TextChanged" 
                                                    onKeyUp="javascript:KitFamilyHeadKeyUp(this, event);" ></asp:TextBox>
                                                    
                                                <asp:HiddenField ID="hdnHeadKitFamilyId" runat="server" ClientIDMode="Static" Value="0" />
                                            </td>
                                        </tr>
                                    </table>                                    
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingKit" runat="server" Text="Existing Kit(s)" CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingKit" Height="390px" CssClass="leftlistboxlong" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstExistingKit_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <asp:Panel ID="pnlKitDetail" runat="server" class="pnlDetail">
                                        <asp:HiddenField ID="hdnProcedureName" ClientIDMode="Static" runat="server" />
                                        <table width="100%" cellspacing="0" cellpadding="2">
                                            <tr>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblKitNumber" runat="server" Text="Kit Number *" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblKitName" runat="server" Text="Kit Name *" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblKitFamily" runat="server" Text="Kit Family *" CssClass="labelbold"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitNumber" runat="server" CssClass="textbox" Width="200px" MaxLength="24"></asp:TextBox>
                                                    <%--<cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator1" runat="server" ControlToValidate="txtKitNumber"
                                                        PropertyName="RoleName" SourceTypeName="VCTWeb.Core.Domain.Role" RulesetName="Role"
                                                        DisplayMode="SingleParagraph" Display="Dynamic" />--%>
                                                      <br />
                                                    <asp:RequiredFieldValidator ID="rfv_KitNumber" runat="server" 
                                                        ControlToValidate="txtKitNumber" Display="Dynamic" CssClass="required" ValidationGroup="submit">
                                                        </asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitName" runat="server" CssClass="textbox" Width="200px" MaxLength="24"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="rfv_KitName" runat="server" 
                                                        ControlToValidate="txtKitName" Display="Dynamic" CssClass="required" ValidationGroup="submit">
                                                        </asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left">
                                                    <%--<asp:TextBox ID="txtKitFamily" runat="server" CssClass="textbox" Width="200px" MaxLength="24"></asp:TextBox>--%>
                                                    <%--<asp:DropDownList ID="ddlKitFamily" runat="server" CssClass="ListBox" AutoPostBack="true" 
                                                        Width="200px" onselectedindexchanged="ddlKitFamily_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="rfv_KitFamily" runat="server" 
                                                       InitialValue="0"  ControlToValidate="ddlKitFamily" Display="Dynamic" CssClass="required" ValidationGroup="submit">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <asp:TextBox ID="txtKitFamily" runat="server" ClientIDMode="Static" 
                                                        Width="200px" AutoCompleteType="None" AutoPostBack="true"  ontextchanged="txtKitFamily_TextChanged" 
                                                     onKeyUp="javascript:KitFamilyKeyUp(this, event);"></asp:TextBox>
                                                     <br />                                                                                     
                                                    <asp:RequiredFieldValidator ID="rfv_KitFamily" runat="server" ControlToValidate="txtKitFamily"
                                                        Display="Dynamic" CssClass="required" ValidationGroup="submit">Required</asp:RequiredFieldValidator>

                                                <asp:HiddenField ID="hdnKitFamilyId" runat="server" ClientIDMode="Static" Value="0" />
                                                </td>
                                            </tr>
                                            <tr class="blank-table-row">
                                                <td>                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblKitDescription" runat="server" Text="Kit Description" CssClass="label"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblRentalFee" runat="server" Text="Rental Fee *" CssClass="labelbold"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtKitDescription" runat="server" CssClass="textbox" Width="457px"
                                                        MaxLength="64"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRentalFee" runat="server" CssClass="textbox" Width="100px" MaxLength="24" Text="0"></asp:TextBox>
                                                    <br />
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                        ControlToValidate="txtRentalFee" Display="Dynamic" CssClass="required" 
                                                        ErrorMessage="Required" ValidationGroup="submit">
                                                        </asp:RequiredFieldValidator>
                                                    <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                FilterType="Numbers, Custom" TargetControlID="txtRentalFee" ValidChars=".">
                                                            </ajaxtk:FilteredTextBoxExtender>
                                                    <%--<asp:CompareValidator id="CompareValidator1" runat="server" 
                                                          ErrorMessage="Invalid Amount" CssClass="required" Display="Dynamic"
                                                          ControlToValidate="txtRentalFee" Type="Currency"
                                                          Operator="DataTypeCheck"></asp:CompareValidator>--%>
                                                </td>
                                            </tr>          
                                             <tr>
                                                <td align="left" colspan="2">
                                                    <asp:CheckBox ID="chkIsActive" runat="server" Text="Active*" CssClass="CheckBox"
                                                        Checked="true" />
                                                </td>
                                                <td align="left">
                                                    
                                                </td>
                                            </tr>                                                                                     
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel CssClass="pnlGrid" ID="pnlKitTableGrid" runat="server">
                                        <table cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblInventoryItem" runat="server" Text="Inventory Items" CssClass="SectionHeaderText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:HiddenField ID="hdnCatalogNumberNew" ClientIDMode="Static" runat="server" />
                                                    <asp:Panel ID="pnlGridKitTable" Height="240px" ScrollBars="Auto" runat="server" Width="600">

                                                        <asp:GridView ID="gridKitTable" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            OnRowCommand="gridKitTable_RowCommand" Width="100%" GridLines="None"
                                                            OnRowDataBound="gridKitTable_RowDataBound" EmptyDataText="No Record Found">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="25%" ItemStyle-Width="25%" HeaderStyle-VerticalAlign="Middle" 
                                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"
                                                                        ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" ID="lblPartNumHeader" Text="Ref #" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>                                                                        
                                                                        <asp:Label ID="lblCatalogNum" runat="server" Text='<%#Eval("CatalogNumber") %>' />
                                                                    </ItemTemplate>                                                                                                                                        
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Width="60%" ItemStyle-Width="60%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" ID="lblDescriptionHeader" Text="Description" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCatalogDesc" runat="server" Text='<%#Eval("Description") %>' />
                                                                    </ItemTemplate>                                                                                                                                        
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" ID="lblActionHeader" Text="Action" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>                                                                        
                                                                        <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Convert.ToString(Eval("KitNumber")) == "" ? false : true %>' />
                                                                    </ItemTemplate>                                                                                                                                    
                                                                </asp:TemplateField> 
                                                                                                                              
                                                            </Columns>
                                                            <%--<EmptyDataTemplate>
                                                            <asp:TextBox ID="txtNewCatalogNum" Width="190" runat="server" BorderColor="Black" BorderWidth="1px" />
                                                            <asp:TextBox ID="TextBox1" Width="190" runat="server" BorderColor="Black" BorderWidth="1px" />
                                                            <asp:TextBox ID="txtNewQuantity" Width="90" runat="server" BorderColor="Black" BorderWidth="1px" />
                                                        </EmptyDataTemplate>--%>
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
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" CausesValidation="False"
                                                        OnClick="btnNew_Click" />
                                                    <%--<asp:Button ID="btnDelete" runat="server" Text="" CssClass="deletebutton" Enabled="false"
                                                        OnClick="btnDelete_Click" />--%>
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click"  ValidationGroup="submit" />
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
