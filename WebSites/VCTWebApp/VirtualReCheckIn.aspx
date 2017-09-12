<%@ Page Title="Web Check-In Adj." Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" CodeBehind="VirtualReCheckIn.aspx.cs" Inherits="VCTWebApp.VirtualReCheckIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server"> 

 <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function pageLoad() {
            InitGridEvent('<%= gridKitTable.ClientID %>');
            InitGridEvent('<%= gridCatalog.ClientID %>');
        }

        $j = $.noConflict();

        $j(function () {

            //            $j('.CheckStatus').live("click", function () {
            //                alert('test');
            //                if ($j(this).checked)
            //                    alert('check');
            //                else
            //                    alert('uncheck');
            //            });
            //           $j(".PartOpt").live("click", function () {
            //                //                var vClass = $j('#' + actionId);
            //                //                alert(vClass); 
            //                //alert(actionId);
            //                //$j(':checkbox').not('#' + actionId).attr('checked', false);
            //                $j('input:checkbox.PartOpt').not('#' + actionId).attr('checked', false);
            //            });

            $j(".ExpandRow").live("click", function () {

                if ($j(this).attr("src").toLowerCase() == "images/plus.png") {
                    $j(this).next().show();
                    $j(this).closest("tr").after("<td style='width:5%'></td><td colspan = '999'>" + $j(this).next().html() + "</td>");
                    $j(this).next().hide();
                    $j(this).attr("src", "images/minus.png");
                }
                else {
                    $j(this).attr("src", "images/plus.png");
                    $j(this).closest("tr").next().next().hide();
                    $j(this).closest("tr").next().hide();
                }
            });
                                

        }); 
    </script>

    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%" >
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Web Check In Adjustment"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td align="center" colspan="2">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCaseType" runat="server" Text="Case Type:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCaseType" runat="server" Width="300px" 
                                                    CssClass="ListBox" AutoPostBack="True" 
                                                    onselectedindexchanged="ddlCaseType_SelectedIndexChanged" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%--<asp:Button runat="server" ID="btnView" CssClass="smallviewbutton" OnClick="btnView_Click" />--%>
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
                                                <asp:Label ID="lblExistingKit" runat="server" Text="Pending Case(s)" CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstPendingCases" Height="350px" CssClass="leftlistboxlong" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstPendingCases_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <asp:Panel ID="pnlDetailPartAndKit" runat="server" class="pnlDetail">
                                        <table width="100%" cellspacing="0" cellpadding="1">
                                            <tr>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblCaseNumber" runat="server" Text="Case #" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblRequiredOn" runat="server" Text="Required On" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblShipFromLocation" runat="server" Text="Ship From Location" CssClass="labelbold"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCaseNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRequiredOn" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShipFromLocation" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                            </tr>
                                             <tr class="blank-table-row">
                                                <td>                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblShipToLocation" runat="server" Text="Ship To Location" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblShipToLocationType" runat="server" Text="Ship To Location Type"
                                                        CssClass="labelbold"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtShipToLocation" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="455px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShipToLocationType" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDetailKit" runat="server" class="pnlDetail" Visible="false">
                                        <table width="100%" cellspacing="0" cellpadding="1">
                                            <%--<tr>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblKitFamily" runat="server" Text="Kit Family" CssClass="labelbold"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblKitDescription" runat="server" Text="Kit Family Description" CssClass="label"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 33%">
                                                    <asp:Label ID="lblQuantity" runat="server" Text="Quantity" CssClass="label"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitFamily" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitFamilyDescription" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtQuantity" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                            </tr>
                                             <tr class="blank-table-row">
                                                <td>                                                    
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblShippingDate" runat="server" Text="Shipping Date"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblRetrivalDate" runat="server" Text="Retrieval Date"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblProcedureName" runat="server" Text="Procedure Name"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShippingDate" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRetrievalDate" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtProcedureName" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td colspan="2">
                                                    <%--<asp:Label ID="Label1" Visible="false" CssClass="SectionHeaderText" Text="Requested inventory not available locally for the given date range. However the requested inventory is found available during 3/11/2014 - 3/18/2014. You can alternatively look out for the availability of inventory at other locations also. Do you want to change the dates?"
                                                        runat="server" />--%>
                                                </td>
                                                <td>
                                                    <%--<asp:Button Visible="false" ID="Button1" Text="Change Dates" runat="server" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel CssClass="pnlGrid" ID="pnlKitTableGrid" runat="server">
                                        <table cellspacing="0" cellpadding="0" width="99%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblInventoryDetail" runat="server" Text="Inventory Detail" CssClass="SectionHeaderText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                
                                                    <asp:Panel ID="pnlKitGrid" Visible="false" Height="240px" Width="100%" ScrollBars="Auto" runat="server">
                                                        <asp:GridView ID="gridKitTable" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" onrowdatabound="gridKitTable_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkKitSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-Width="5%">                                                                    
                                                                    <ItemTemplate> 
                                                                        <asp:HiddenField ID="hdnCaseKitId" runat="server" Value = '<%#Eval("CaseKitId") %>' />
                                                                         <asp:HiddenField ID="hdnBuildKitId" runat="server" Value = '<%#Eval("BuildKitId") %>' />
                                                                     
                                                                        <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                                                ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />                                                                                          
                                                                        <asp:Panel ID="pnlChild" runat="server"   style="display:none">
                                                                        
                                                                            <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" 
                                                                                SkinID="GridView" onrowdatabound="grdChild_RowDataBound">
                                                                                <Columns>    
                                                                                    <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-Width="3%"
                                                                                        ItemStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkPartSelect" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>                                                                                
                                                                                    <asp:BoundField ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center" DataField="PartNum" HeaderText="Part #" />
                                                                                    <asp:BoundField ItemStyle-Width="23%" DataField="Description" HeaderText="Description" />
                                                                                    <asp:BoundField ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Center" DataField="LotNum" HeaderText="Lot #" />
                                                                                    <asp:BoundField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:d}" />
                                                                                    <asp:TemplateField ControlStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblAvailableHead" runat="server" Text="Available" ForeColor="White" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>                                                                                                                                                                                        
                                                                                                <asp:RadioButton ID="radAvailable" runat="server" GroupName='group' Checked="true" />                                                                                                
                                                                                                <asp:HiddenField ID="hdnLocPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />
                                                                                                <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                                                <asp:HiddenField ID="hdnPartStatus" runat="server" Value='<%#Eval("PartStatus") %>' />
                                                                                        </ItemTemplate>                                                                                        
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ControlStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblConsumeHead" runat="server" Text="Consumed" ForeColor="White" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>                                                                                            
                                                                                            <asp:RadioButton ID="radConsume" runat="server" GroupName='group' />                                                                                                                                                                                  
                                                                                        </ItemTemplate>                                                                                        
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ControlStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblMissingHead" runat="server" Text="Missing" ForeColor="White" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>                                                                                                                                                                                        
                                                                                                <asp:RadioButton ID="radMissing" runat="server" GroupName='group'  />                                                                                                
                                                                                        </ItemTemplate>                                                                                        
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ControlStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblDamageHead" runat="server" Text="Damaged" ForeColor="White" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>                                        
                                                                                            <asp:RadioButton ID="radDamage" runat="server" GroupName='group' />                                       
                                                                                        </ItemTemplate>                                                                                        
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        
                                                                        </asp:Panel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitFamilyHeader" runat="server" Text="Kit Family"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitFamily" runat="server" Text='<%#Eval("KitFamilyName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Width="40%" ItemStyle-Width="40%"
                                                                    ItemStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        &nbsp;
                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField  HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitNumberHeader" runat="server" Text="Assign Kit Number" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitNumber" runat="server" Text='<%#Eval("KitNumber") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField  HeaderStyle-Width="15%" ItemStyle-Width="15%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblStatusHeader" runat="server" Text="Status" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("KitStatus") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>
                                                     </asp:Panel>

                                                     <asp:Panel ID="pnlPartGrid" Visible="false" Height="240px" Width="100%" ScrollBars="Auto" runat="server">
                                                        <asp:GridView ID="gridCatalog" Width="100%" runat="server" SkinID="GridView"
                                                            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" onrowdatabound="gridCatalog_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblPartNoHeader" runat="server" Text="Ref #"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                        <asp:HiddenField ID="hdnLocPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />
                                                                        <asp:HiddenField ID="hdnCasePartId" runat="server" Value='<%#Eval("CasePartId") %>' />
                                                                        <asp:Label ID="lblCatalogNum" runat="server" Text='<%#Eval("PartNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField ItemStyle-Width="40%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLotNumHeader" runat="server" Text="Assign Lot Number" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>                                                                        
                                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%#Eval("LotNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>                                                       
                                                                
                                                                <asp:TemplateField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" >
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblExpiryDateHeader" runat="server" Text="Expiry Date" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate", "{0:d}") %>' />
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
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="50%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" 
                                                        onclick="btnNew_Click" />
                                                    
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="checkinadjustmentbutton" 
                                                        onclick="btnSave_Click" />
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
