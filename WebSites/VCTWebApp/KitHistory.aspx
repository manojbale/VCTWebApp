<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KitHistory.aspx.cs" Inherits="VCTWebApp.Shell.Views.KitHistory"
    Title="Web Build Kit" MasterPageFile="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        

        function pageLoad() {
            SearchTextByKitNumberNew('txtKitNumber', 'sKitNumber', 'GetKitsByKitNumberOrDesc', 'hdnValid');
        }


        function NewCatalogNumberKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnKitNumber.ClientID %>');
                var myHiddenIsValid = document.getElementById('<%= hdnValid.ClientID %>');
                if (myHidden) {
                    myHidden.value = '';
                    myHiddenIsValid.value = '';
                }
            }
        }

        $j = $.noConflict();

        $j(".ExpandRow").live("click", function () {

            if ($j(this).attr("src").toLowerCase() == "images/plus.png") {
                $j(this).next().show();
                $j(this).closest("tr").after("<td style='width:3%'></td><td colspan = '999'>" + $j(this).next().html() + "</td>");
                $j(this).next().hide();
                $j(this).attr("src", "images/minus.png");
            }
            else {
                $j(this).attr("src", "images/plus.png");
                $j(this).closest("tr").next().next().hide();
                $j(this).closest("tr").next().hide();
            }
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
                                    <asp:Label ID="lblHeader" runat="server" Text="Kit History"></asp:Label>
                                </td>
                            </tr>
                            <tr class="blank-table-row">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="70%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblKitNumber" runat="server" Text="Kit Number / Kit Name:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtKitNumber" ClientIDMode="Static" runat="server" Width="500px"
                                                    onKeyUp="javascript:NewCatalogNumberKeyUp(this, event);"  />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSearch" CssClass="smallviewbutton" ClientIDMode="Static" runat="server"
                                                    OnClick="btnSearch_Click"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfv_KitNumber" runat="server" ControlToValidate="txtKitNumber"
                                                    Display="Dynamic" CssClass="required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr class="blank-table-row">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <asp:HiddenField ID="hdnKitNumber" ClientIDMode="Static" runat="server" />
                                    <asp:HiddenField ID="hdnKitNumDesc" ClientIDMode="Static" runat="server" />
                                       <asp:HiddenField ID="hdnValid" ClientIDMode="Static" runat="server" />
                                    <div id="controlHead">
                                    </div>
                                    <asp:Panel ID="pnlProductAttributes" runat="server" CssClass="pnlGrid" Height="350px"
                                        ScrollBars="Auto" Width="98%">
                                        <table cellpadding="0" cellspacing="0" width="99%">
                                            <tr>
                                                <td align="center" valign="top">
                                                    <asp:GridView ID="gdvCaseDetail" runat="server" AutoGenerateColumns="false" OnRowDataBound="gdvCaseDetail_RowDataBound"
                                                        ShowHeaderWhenEmpty="false" SkinID="GridView" >
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgChildKit" runat="server" CssClass="ExpandRow" ImageUrl="~/Images/plus.PNG"
                                                                        Style="cursor: pointer; vertical-align: top;" />
                                                                    <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                        <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Ref #" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPartNum" runat="server" Text='<%# Bind("PartNum") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Lot #" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%# Bind("LotNum") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Description" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="50%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Expiry Date" ItemStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-Width="20%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Surgery Date" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSurgeryDate" runat="server" Text='<%# Convert.ToDateTime(Eval("SurgeryDate"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Case #" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCaseNumber" runat="server" Text='<%# Bind("CaseNumber") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Hospital" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPartyName" runat="server" Text='<%# Bind("PartyName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Procedure" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProcedureName" runat="server" Text='<%# Bind("ProcedureName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Sales Rep." ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesRep" runat="server" Text='<%# Bind("SalesRep") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="CaseStatus" ItemStyle-HorizontalAlign="Center"
                                                                ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCaseStatus" runat="server" Text='<%# Bind("CaseStatus") %>'></asp:Label>
                                                                </ItemTemplate>
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
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="50%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click" />
                                                    <%--<asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click" />--%>
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
