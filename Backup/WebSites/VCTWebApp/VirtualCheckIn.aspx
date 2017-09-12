<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VirtualCheckIn.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.VirtualCheckIn" Title="Web Check In" MasterPageFile="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function pageLoad() {
            InitGridEvent('<%= gridKitTable.ClientID %>');
            InitGridEvent('<%= gridCatalog.ClientID %>');
            InitGridEvent('<%= gridRMA.ClientID %>');

        }

        $j = $.noConflict();

        //        function CheckStatus(e) {
        //            var Id = e.id;
        //            //alert(document.getElementById(Id).checked);
        //            //alert($j('#'+Id).attr('checked'));      
        //        }

        //        var actionId = "";
        //        function CheckAction(e) {
        //            actionId = e.id;                       
        //        }

        $j(function () {

            //            $j('.chk-send-replacement').live("click", function () {

            //                $j('.chk-send-replacement').each(function (i) {

            //                    alert($j(this).attr('id'));

            //                });

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
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Virtual Check In"></asp:Label>
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
                                                <asp:DropDownList ID="ddlCaseType" runat="server" Width="300px" CssClass="ListBox"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCaseType_SelectedIndexChanged" />
                                            </td>
                                            <td>
                                                &nbsp;
                                                <asp:Label ID="lblCaseStatus" runat="server" Text="" Font-Bold="true" Visible="false" />
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
                                                <asp:ListBox ID="lstPendingCases" Height="400px" CssClass="leftlistboxlong" runat="server"
                                                    AutoPostBack="True" OnSelectedIndexChanged="lstPendingCases_SelectedIndexChanged">
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <asp:Panel ID="pnlDetailPartAndKit" runat="server" class="pnlDetail">
                                        <table width="100%" cellspacing="0" cellpadding="2">
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
                                                    <asp:TextBox ID="txtCaseNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRequiredOn" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShipFromLocation" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
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
                                                    <asp:TextBox ID="txtShipToLocation" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="455px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShipToLocationType" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDetailKit" runat="server" class="pnlDetail" Visible="false">
                                        <table width="100%" cellspacing="0" cellpadding="2">
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
                                                    <asp:TextBox ID="txtShippingDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRetrievalDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtProcedureName" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                                        Width="200px" />
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
                                                    <asp:Panel ID="pnlKitGrid" Visible="false" Height="240px" Width="100%" ScrollBars="Auto"
                                                        runat="server">
                                                        <asp:GridView ID="gridKitTable" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" OnRowDataBound="gridKitTable_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle Width="5%" />
                                                                    <ItemStyle Width="5%" />
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnCaseKitId" runat="server" Value='<%#Eval("CaseKitId") %>' />
                                                                        <asp:HiddenField ID="hdnBuildKitId" runat="server" Value='<%#Eval("BuildKitId") %>' />
                                                                        <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                            ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                        <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                            <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                OnRowDataBound="grdChild_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:BoundField ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" DataField="PartNum"
                                                                                        HeaderText="Part #" />
                                                                                    <asp:BoundField ItemStyle-Width="20%" DataField="Description" HeaderText="Description" />
                                                                                    <asp:BoundField ItemStyle-Width="19%" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                                                        HeaderText="Lot #" />
                                                                                    <asp:BoundField ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate"
                                                                                        HeaderText="Expiry Date" DataFormatString="{0:d}" />
                                                                                    <asp:TemplateField ControlStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblAvailableHead" runat="server" Text="Available" ForeColor="White" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:RadioButton ID="radAvailable" runat="server" GroupName='group' Checked="true" />
                                                                                            <asp:HiddenField ID="hdnPartStatus" runat="server" Value='<%#Eval("PartStatus") %>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ControlStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblConsumeHead" runat="server" Text="Consumed" ForeColor="White" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:RadioButton ID="radConsume" runat="server" GroupName='group' />
                                                                                            <asp:HiddenField ID="hdnLocPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />
                                                                                            <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ControlStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblMissingHead" runat="server" Text="Missing" ForeColor="White" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:RadioButton ID="radMissing" runat="server" GroupName='group' />
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
                                                                                    <asp:BoundField ItemStyle-Width="12%" DataField="IsUsageMarked" ItemStyle-HorizontalAlign="Center"
                                                                                        HeaderText="Is Marked" />
                                                                                   <asp:BoundField ItemStyle-Width="14%" DataField="PartStatus" ItemStyle-HorizontalAlign="Center"
                                                                                        HeaderText="Status" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitFamilyHeader" runat="server" Text="Kit Family"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitFamily" runat="server" Text='<%#Eval("KitFamilyName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="35%" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitNumberHeader" runat="server" Text="Assign Kit Number" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitNumber" runat="server" Text='<%#Eval("KitNumber") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="15%" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitStatusHeader" runat="server" Text="Status" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitStatus" runat="server" Text='<%#Eval("KitStatus") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlPartGrid" Visible="false" Height="290px" Width="100%" ScrollBars="Auto"
                                                        runat="server">
                                                        <asp:GridView ID="gridCatalog" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" OnRowDataBound="gridCatalog_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblPartNoHeader" runat="server" Text="Part / Item #"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                        <asp:HiddenField ID="hdnLocPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />
                                                                        <asp:HiddenField ID="hdnCasePartId" runat="server" Value='<%#Eval("CasePartId") %>' />
                                                                        <asp:HiddenField ID="hdnCaseType" runat="server" Value='<%#Eval("CaseType") %>' />
                                                                        <asp:HiddenField ID="hdnDescription" runat="server" Value='<%#Eval("Description") %>' />
                                                                        <asp:HiddenField ID="hdnRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
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
                                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLotNumHeader" runat="server" Text="Lot #" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%#Eval("LotNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblExpiryDateHeader" runat="server" Text="Expiry Date" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate", "{0:d}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="20%" HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblPartStatusHeader" runat="server" Text="Status" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPartStatus" runat="server" Text='<%#Eval("PartStatus") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:GridView ID="gridRMA" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" OnRowDataBound="gridRMA_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblPartNoHeader" runat="server" Text="Part / Item #"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                        <asp:HiddenField ID="hdnLocPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />
                                                                        <asp:HiddenField ID="hdnCasePartId" runat="server" Value='<%#Eval("CasePartId") %>' />
                                                                        <asp:HiddenField ID="hdnCaseType" runat="server" Value='<%#Eval("CaseType") %>' />
                                                                        <asp:HiddenField ID="hdnDescription" runat="server" Value='<%#Eval("Description") %>' />
                                                                        <asp:HiddenField ID="hdnRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
                                                                        <asp:HiddenField ID="hdnPartStatus" runat="server" Value='<%#Eval("PartStatus") %>' />
                                                                        <asp:HiddenField ID="hdnSendReplacement" runat="server" Value='<%#Eval("SendReplacement") %>' />
                                                                        <asp:Label ID="lblCatalogNum" runat="server" Text='<%#Eval("PartNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLotNumHeader" runat="server" Text="Lot #" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%#Eval("LotNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblExpiryDateHeader" runat="server" Text="Expiry Date" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate", "{0:d}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDispoTypeHeader" runat="server" Text="Disposition" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDispositionType" runat="server" Text='<%#Eval("DispositionType") %>'></asp:Label>
                                                                        <%--<asp:DropDownList ID="ddlDispositionType" runat="server" Visible="false"></asp:DropDownList>--%>
                                                                        <asp:HiddenField ID="hdnDispositionTypeId" runat="server" Value='<%#Eval("DispositionTypeId") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblSeekReturnHeader" runat="server" Text="Seek Return" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSeekReturn" runat="server" Checked='<%#Eval("SeekReturn") %>'
                                                                            Enabled="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblSendReplacementHeader" runat="server" Text="Send Replacement" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSendReplacement" runat="server" CssClass="chk-send-replacement" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblAvailableHeader" runat="server" Text="Available" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="radAvailable" runat="server" GroupName="g1" Checked="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblMissing" runat="server" Text="Missing" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="radMissing" runat="server" GroupName="g1" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDamage" runat="server" Text="Damage" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="radDamage" runat="server" GroupName="g1" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblExpired" runat="server" Text="Expired" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="radExpire" runat="server" GroupName="g1" />
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
                                                <td align="left" width="70%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="30%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click" />
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="checkinbutton" OnClick="btnSave_Click" />
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
