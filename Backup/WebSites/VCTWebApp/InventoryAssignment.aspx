<%@ Page Language="C#" AutoEventWireup="true" Inherits="VCTWebApp.Shell.Views.InventoryAssignment"
    Title="InventoryAssignment" MasterPageFile="~/Site1.master" CodeBehind="InventoryAssignment.aspx.cs" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript">
        $(function () {
            $(window).load(function () {
                document.getElementById('DefaultContent_txtBarCode').focus();
                fixedGrid();
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                fixedGrid();
            });

            function fixedGrid() {
                InitGridEvent('<%= gridKitTable.ClientID %>');
                InitGridEvent('<%= gridCatalog.ClientID %>');
            }

        });
    </script>
    <script type="text/javascript">


        function ltrim(str) {
            for (var k = 0; k < str.length && isWhitespace(str.charAt(k)); k++);
            return str.substring(k, str.length);
        }
        function rtrim(str) {
            for (var j = str.length - 1; j >= 0 && isWhitespace(str.charAt(j)); j--);
            return str.substring(0, j + 1);
        }
        function trim(str) {
            return ltrim(rtrim(str));
        }
        function isWhitespace(charToCheck) {
            var whitespaceChars = " \t\n\r\f";
            return (whitespaceChars.indexOf(charToCheck) != -1);
        }



        function SetSelectedAssignemnt() {
                
            var InventoryType = document.getElementById('DefaultContent_hdnInventoryType').value;
            if (InventoryType != '') {
                if (InventoryType == 'Part') {
                    SetSelectedAssignemntForPartNo();
                }
                else if (InventoryType == 'Kit') {
                    SetSelectedAssignemntForKitNO();
                }

                return false;
            }
        }


        function ItemAlreadyAssigned(partnum, index) {

            var grid = document.getElementById('DefaultContent_gridCatalog');
            for (var i = 2; i <= grid.rows.length - 1; i++) {
                var currRow = grid.rows[i];
                var checkBox = currRow.cells[0].getElementsByTagName('INPUT');
                var span = currRow.cells[1].getElementsByTagName('span');
                if (checkBox[0].disabled != true && checkBox[0].checked == true && span[0].innerHTML == partnum) {
                    var dropdown = currRow.cells[2].getElementsByTagName('select');
                    if (dropdown[0].options[index].selected == true) {
                        return true;
                    }
                }
            }
            return false;
        }




        function SetSelectedAssignemntForPartNo() {


            var result = ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value));
          
            var grid = document.getElementById('DefaultContent_gridCatalog');
            var IsBarCodeExists = false;

            var IsItemAssigned = false;
            if (grid != 'undefined') {
                var oRows = grid.rows;
                for (var i = 2; i <= grid.rows.length - 1; i++) {
                    var currRow = grid.rows[i];
                    if (result != '') {
                        var dropdown = currRow.cells[2].getElementsByTagName('select');

                        var lengthDropDown = dropdown[0].options.length;
                        var checkBox = currRow.cells[0].getElementsByTagName('INPUT');

                        for (k = 0; k < lengthDropDown; k++) {
                            if (dropdown[0].options[k].text != '') {
                                var SplitResult = dropdown[0].options[k].text.split(" ");
                                if (SplitResult[0].toUpperCase() == result.toUpperCase()) {

                                    if (checkBox[0].disabled != true && checkBox[0].checked != true) {
                                        var partNumberLable = currRow.cells[1].getElementsByTagName('span');
                                        //For forefox
                                        if (!ItemAlreadyAssigned(partNumberLable[0].innerHTML, k)) {

                                            dropdown[0].options[k].selected = true;
                                            checkBox[0].checked = true;
                                            IsBarCodeExists = true;
                                            IsItemAssigned = true;
                                            break;

                                        }
                                    }
                                }
                            }
                        }
                        if (IsItemAssigned) {
                            document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") successfully assigned.";
                            document.getElementById('DefaultContent_txtBarCode').value = '';
                            document.getElementById('DefaultContent_txtBarCode').focus();
                            break;
                        }
                    }


                }

                if (!IsItemAssigned) {

                    if (ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value)) == '') {
                      
                    }
                    else {
                       
                        document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") either not available or cannot assigned to selected Case.";
                        document.getElementById('DefaultContent_txtBarCode').value = '';
                        document.getElementById('DefaultContent_txtBarCode').focus();
                    }

                }

            }

        }

             
    </script>
    <script type="text/javascript">


        function ItemAlreadyAssignedForKit(partnum, index) {

            var grid = document.getElementById('DefaultContent_gridKitTable');
            for (var i = 2; i <= grid.rows.length - 1; i++) {
                var currRow = grid.rows[i];
                var checkBox = currRow.cells[0].getElementsByTagName('INPUT');
                if (checkBox[0].disabled != true && checkBox[0].checked == true && currRow.cells[1].innerText == partnum) {
                    var dropdown = currRow.cells[2].getElementsByTagName('select');
                    if (dropdown[0].options[index].selected == true) {
                        return true;
                    }
                }
            }
            return false;
        }


        function SetSelectedAssignemntForKitNO() {


            var result = ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value));

            var grid = document.getElementById('DefaultContent_gridKitTable');

            var IsBarCodeExists = false;

            var IsItemAssigned = false;
            if (grid != 'undefined') {
                for (var i = 2; i <= grid.rows.length - 1; i++) {
                    var currRow = grid.rows[i];
                    if (result != '') {
                        var dropdown = currRow.cells[2].getElementsByTagName('select');

                        var lengthDropDown = dropdown[0].options.length;
                        var checkBox = currRow.cells[0].getElementsByTagName('INPUT');
                        for (k = 0; k < lengthDropDown; k++) {

                            if (dropdown[0].options[k].text != '') {
                                if (ltrim(rtrim(dropdown[0].options[k].text)).toUpperCase() == result.toUpperCase()) {

                                    if (checkBox[0].disabled != true && checkBox[0].checked != true) {
                                        //For forefox
                                        if (!ItemAlreadyAssignedForKit(currRow.cells[1].innerText, k)) {
                                            dropdown[0].options[k].selected = true;
                                            checkBox[0].checked = true;
                                            IsBarCodeExists = true;
                                            IsItemAssigned = true;
                                            break;

                                        }
                                    }
                                }
                            }
                        }
                        if (IsItemAssigned) {
                            document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") successfully assigned.";
                            document.getElementById('DefaultContent_txtBarCode').value = '';
                            document.getElementById('DefaultContent_txtBarCode').focus();
                            break;
                        }
                    }


                }

                if (!IsItemAssigned) {

                    if (ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value)) == '') {                       
                    }
                    else {

                        document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") either not available or cannot assigned to selected Case.";
                        document.getElementById('DefaultContent_txtBarCode').value = '';
                        document.getElementById('DefaultContent_txtBarCode').focus();
                    }

                }

            }

        }
    </script>
    
    <script type="text/javascript" language="javascript">
        function CheckAllCheckBox(Checkbox) {
           
            var GridVwHeaderChckbox = document.getElementById('DefaultContent_gridKitTable');
          
            if (Checkbox.checked) {
                for (i = 2; i < GridVwHeaderChckbox.rows.length; i++) {                   
                    GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }
            else if (!Checkbox.checked) {
                for (i = 2; i < GridVwHeaderChckbox.rows.length; i++) {                   
                    GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = false;
                }
            }
            
        }
    </script>


     <script type="text/javascript" language="javascript">
         function CheckUncheckCheckAll(Checkbox) {

             var GridVwHeaderChckbox = document.getElementById('DefaultContent_gridKitTable');
             var CheckAllCheckBox = document.getElementById('DefaultContent_gridKitTable_chkboxSelectAll');
             if (!Checkbox.checked && document.getElementById('DefaultContent_gridKitTable_chkboxSelectAll').checked) {
                
                 CheckAllCheckBox.checked = false;
             }
            

         }
    </script>

    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnInventoryType" runat="server" />
            <asp:HiddenField ID="hdnPrevScannedBarCode" runat="server" />
            <asp:HiddenField ID="hdnSelectedPartNumber" runat="server" />
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Inventory Assignment"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <br />
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblCaseType" runat="server" Text="Case Type:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCaseType" runat="server" Width="300px" CssClass="ListBox"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCaseType_SelectedIndexChanged" />
                                                &nbsp;
                                                <asp:Label ID="lblCaseStatus" runat="server" Text="" Font-Bold="true" Visible="false"></asp:Label>
                                            </td>
                                            <td align="right" width="300px">
                                                <asp:Label ID="lblScanedItem" runat="server" Text="Bar Code:&nbsp;" />
                                            </td>
                                            <td>
                                                <td>
                                                    <asp:TextBox ID="txtBarCode" onblur="SetSelectedAssignemnt()" Text="" Width="200px"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                        </tr>
                                    </table>
                                    <br />
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
                                    <asp:Panel ID="pnlDetail" runat="server" class="pnlDetail">
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
                                                    <asp:TextBox ID="txtCaseNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtRequiredOn" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShipFromLocation" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
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
                                    <asp:Panel ID="pnlDetail2" runat="server" class="pnlDetail" Visible="false">
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
                                            <tr>
                                                <td>
                                                    &nbsp;
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label1" Visible="false" CssClass="SectionHeaderText" Text="Requested inventory not available locally for the given date range. However the requested inventory is found available during 3/11/2014 - 3/18/2014. You can alternatively look out for the availability of inventory at other locations also. Do you want to change the dates?"
                                                        runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Button Visible="false" ID="Button1" Text="Change Dates" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel CssClass="pnlGrid" ID="pnlKitTableGrid" runat="server">
                                        <table cellspacing="0" cellpadding="0" width="55%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblInventoryDetail" runat="server" Text="Inventory Detail" CssClass="SectionHeaderText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Panel ID="pnlKitGrid" Height="190px" Width="100%" ScrollBars="Auto" runat="server">
                                                        <asp:GridView ID="gridKitTable" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            OnRowDataBound="gridKitTable_RowDataBound" ShowHeaderWhenEmpty="true">
                                                            <Columns>
                                                            
                                                                
                                                                <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                               
                                                                <asp:BoundField HeaderText="Kit Family" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                                                    ItemStyle-HorizontalAlign="Center" DataField="KitFamilyName" />
                                                                <asp:TemplateField HeaderText="Assign Kit Number" HeaderStyle-Width="60%" ItemStyle-Width="60%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnBuildKitId" runat="server" Value='<%#Eval("BuildKitId") %>' />
                                                                        <asp:HiddenField ID="hdnKitFamilyid" runat="server" Value='<%#Eval("KitFamilyid") %>' />
                                                                        <asp:HiddenField ID="hdnKitNumber" runat="server" Value='<%#Eval("KitNumber") %>' />
                                                                        <asp:DropDownList runat="server" ID="ddlKitNumber" CssClass="ListBox" Width="80%">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlPartGrid" Visible="false" Height="270px" Width="100%" ScrollBars="Auto"
                                                        runat="server">
                                                        <asp:GridView ID="gridCatalog" Width="100%" runat="server" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" OnRowDataBound="gridCatalog_RowDataBound" SkinID="GridView">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ref #" HeaderStyle-Width="30%" ItemStyle-Width="30%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCatalogNum" runat="server" Text='<%#Eval("PartNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Assign Lot Number" HeaderStyle-Width="60%" ItemStyle-Width="60%"
                                                                    ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnCasePartId" runat="server" Value='<%#Eval("CasePartId") %>' />
                                                                        <asp:HiddenField ID="hdnLocationPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />
                                                                        <asp:HiddenField ID="hdnLotNum" runat="server" Value='<%#Eval("LotNum") %>' />
                                                                        <asp:DropDownList runat="server" ID="ddlLotNumber" CssClass="ListBox" Width="80%">
                                                                        </asp:DropDownList>
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
                                                    <asp:Button ID="btnFindMatch" runat="server" Text="" CssClass="searchbutton" CausesValidation="False"
                                                        OnClick="btnFindMatch_Click" Visible="false" />
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" CausesValidation="False"
                                                        Visible="false" />
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click"
                                                        Visible="true" />
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
