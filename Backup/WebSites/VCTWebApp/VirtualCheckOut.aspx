<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VirtualCheckOut.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.VirtualCheckOut" Title="Web Check Out" MasterPageFile="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    
      <script type="text/javascript">


         Sys.Application.add_load(function() {
             $('#DefaultContent_gridKitTable').Scrollable({
                ScrollHeight: 205
            });

            $('#DefaultContent_gridCatalog').Scrollable({
                ScrollHeight: 265
            });
        });
</script>


    <script type="text/javascript">
        $(function () {
            $(window).load(function () {
                document.getElementById('DefaultContent_txtBarCode').focus();

            });
        });
    </script>


    <script type="text/javascript">

        function pageLoad() {

              //InitGridEvent('<%= gridKitTable.ClientID %>');
              //InitGridEvent('<%= gridCatalog.ClientID %>');

              $(".ExpandRow").on("click", function () {

                  if ($(this).attr("src").toLowerCase() == "images/plus.png") {
                      $(this).next().show();
                      $(this).closest("tr").after("<td style='width:5%'></td><td colspan = '999'>" + $('.ExpandRow').next().html() + "</td>");
                      $(this).next().hide();
                      $(this).attr("src", "images/minus.png");
                  }
                  else {
                      $(this).attr("src", "images/plus.png");
                      $(this).closest("tr").next().next().hide();
                      $(this).closest("tr").next().hide();
                  }
              });

        }

      
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

                    SetSelectedAssignemntForKitNo();
                }


            }
            return false;
        }

        function SetSelectedAssignemntForPartNo() {

            var result = ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value));
            var grid = document.getElementById('DefaultContent_gridCatalog');

            var IsItemAssigned = false;
            if (grid != 'undefined') {
                // var oRows = grid.rows;
                for (var i = 0; i <= grid.rows.length - 1; i++) {
                    var currRow = grid.rows[i];

                    if (currRow.cells[3].innerText.toUpperCase() == result.toUpperCase()) {
                        var checkBox = currRow.cells[0].getElementsByTagName('INPUT');

                        if (checkBox[0].disabled != true && checkBox[0].checked != true) {
                            checkBox[0].checked = true;
                            IsItemAssigned = true;
                            document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") successfully marked.";
                            document.getElementById('DefaultContent_txtBarCode').value = '';
                            document.getElementById('DefaultContent_txtBarCode').focus();
                            return false;
                        }
                    }

                }
                if (!IsItemAssigned) {
                    if (ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value)) == '') {
                    }
                    else {

                        document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") either not available or cannot marked to selected Case.";
                        document.getElementById('DefaultContent_txtBarCode').value = '';
                        document.getElementById('DefaultContent_txtBarCode').focus();
                    }
                }

            }

        }
          
           
             
    </script>
    <script type="text/javascript">
        function SetSelectedAssignemntForKitNo() {

            var result = ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value)).toUpperCase();
            var grid = document.getElementById('DefaultContent_gridKitTable');
            var IsItemAssigned = false;


            if (grid != 'undefined') {

                for (var i = 0; i <= grid.rows.length - 1; i++) {
                    var currRow = grid.rows[i];
                    
                    if (currRow.cells[4].innerText.toUpperCase() == result.toUpperCase()) {
                        var checkBox = currRow.cells[0].getElementsByTagName('INPUT');

                        if (checkBox[0].disabled != true && checkBox[0].checked != true) {
                            checkBox[0].checked = true;
                            //alert(checkBox[0].id);
                            IsItemAssigned = true;
                            // DisplayInnerGrid(checkBox[0].id);

                            document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") successfully marked.";
                            document.getElementById('DefaultContent_txtBarCode').value = '';
                            document.getElementById('DefaultContent_txtBarCode').focus();
                            return false;
                        }
                    }

                }
                if (!IsItemAssigned) {
                    if (ltrim(rtrim(document.getElementById('DefaultContent_txtBarCode').value)) == '') {
                    }
                    else {

                        document.getElementById('DefaultContent_lblError').innerText = "Scanned item (" + result + ") either not available or cannot marked to selected Case.";
                        document.getElementById('DefaultContent_txtBarCode').value = '';
                        document.getElementById('DefaultContent_txtBarCode').focus();
                    }
                }
            }
        }



        function DisplayInnerGrid(checkBoxId) {

            var checkBoxNameArray = checkBoxId.split('_');
            var childgrid = document.getElementById('DefaultContent_gridKitTable_grdChild_' + checkBoxNameArray[checkBoxNameArray.length - 1]);
            var imageButton = document.getElementById('DefaultContent_gridKitTable_imgChildKit_' + checkBoxNameArray[checkBoxNameArray.length - 1]);

            var div = document.getElementById('DefaultContent_gridKitTable_pnlChild_' + checkBoxNameArray[checkBoxNameArray.length - 1]);


            // DefaultContent_gridKitTable_pnlChild_1
            divexpandcollapse(div, imageButton);
            //alert('Setting the style');
            //            for (var m = 2; m <= childgrid.rows.length - 1; m++) {
            //                var currRow = childgrid.rows[m];
            //            }
        }
             
    </script>
    <script language="javascript" type="text/javascript">
        function divexpandcollapse(div, image) {

            if (div.style.display == "none") {
                div.style.display = "inline";
                image.src = "images/minus.png";
            } else {
                div.style.display = "none";
                image.src = "images/plus.png";
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function CheckAllCheckBox(Checkbox) {
       
            var InventoryType = document.getElementById('DefaultContent_hdnInventoryType').value;
            var GridVwHeaderChckbox = null;

            if (InventoryType == 'Part') {
                GridVwHeaderChckbox = document.getElementById('DefaultContent_gridCatalog');
            }
            else if (InventoryType == 'Kit') {
                GridVwHeaderChckbox = document.getElementById('DefaultContent_gridKitTable');
            }


            if (Checkbox.checked) {
                for (i = 0; i < GridVwHeaderChckbox.rows.length; i++) {
                    GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                   
                }
               
            }
            else if (!Checkbox.checked) {
                for (i = 0; i < GridVwHeaderChckbox.rows.length; i++) {
                    //alert(GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].disabled);
                    if (GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].disabled == false) {
                        GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = false;
                    }
                }
            }

        }
    </script>
    <script type="text/javascript" language="javascript">
        function CheckUncheckCheckAll(Checkbox) {
            var InventoryType = document.getElementById('DefaultContent_hdnInventoryType').value;
            var CheckAllCheckBox = null;


            if (InventoryType == 'Part') {
                CheckAllCheckBox = document.getElementById('DefaultContent_gridCatalog_chkboxSelectAll_Catalog');
            }
            else if (InventoryType == 'Kit') {

                CheckAllCheckBox = document.getElementById('DefaultContent_gridKitTable_chkboxSelectAll_Kit');
            }
            if (!Checkbox.checked && CheckAllCheckBox.checked && (CheckAllCheckBox != 'undefined')) {
                CheckAllCheckBox.checked = false;
            }
        }
                
            


            
       
    </script>
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnInventoryType" runat="server" />
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Virtual Check Out"></asp:Label>
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
                                            </td>
                                            <td align="right" width="300px">
                                                <asp:Label ID="lblScanedItem" runat="server" Text="Bar Code:&nbsp;" />
                                            </td>
                                            <td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBarCode" runat="server" Width="200px" onblur="return SetSelectedAssignemnt();"
                                                        TabIndex="1" />
                                                </td>
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
                                        <table cellspacing="0" cellpadding="0" width="95%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblInventoryDetail" runat="server" Text="Inventory Detail" CssClass="SectionHeaderText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Panel ID="pnlKitGrid" Visible="false" Height="245px" Width="100%" ScrollBars="Auto"
                                                        runat="server">
                                                        <asp:GridView ID="gridKitTable" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" OnRowDataBound="gridKitTable_RowDataBound" >
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="6.9%" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkboxSelectAll_Kit" runat="server" onclick="CheckAllCheckBox(this);">
                                                                        </asp:CheckBox>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckUncheckCheckAll(this);" ClientIDMode="Static" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4.4%" ItemStyle-Width="3%">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnCaseKitId" runat="server" Value='<%#Eval("CaseKitId") %>' />
                                                                        <asp:HiddenField ID="hdnBuildKitId" runat="server" Value='<%#Eval("BuildKitId") %>' />
                                                                        <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                            ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                        <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                            <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                OnRowDataBound="grdChild_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Ref #" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblPartNoHeader" runat="server" Text="Ref #"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                                            <%#Eval("PartNum")%>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--<asp:BoundField ItemStyle-Width="300px" DataField="Description" HeaderText="Description" />
                                                                                    <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="LotNum" HeaderText="Lot #" />
                                                                                    <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:d}" />                                                                                    
                                                                                    --%>
                                                                                    <asp:TemplateField ItemStyle-Width="300px">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblLotNumHeader" runat="server" Text="Lot #" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLotNum" runat="server" Text='<%#Eval("LotNum") %>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
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
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="14%" ItemStyle-Width="14%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitFamilyHeader" runat="server" Text="Kit Family" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitFamily" runat="server" Text='<%# Eval("KitFamilyName") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="39%" ItemStyle-Width="40%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="21%" ItemStyle-Width="21%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitNumberHeader" runat="server" Text="Assign Kit Number" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitNumber" runat="server" Text='<%#Eval("KitNumber") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitStatusHeader" runat="server" Text="Status"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblKitStatus" runat="server" Text='<%#Eval("KitStatus") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlPartGrid" Visible="false" Height="305px" Width="100%" ScrollBars="Auto"
                                                        runat="server">
                                                        <asp:GridView ID="gridCatalog" Width="100%" runat="server" SkinID="GridView" AutoGenerateColumns="False"
                                                            ShowHeaderWhenEmpty="true" OnRowDataBound="gridCatalog_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkboxSelectAll_Catalog" runat="server" onclick="CheckAllCheckBox(this);">
                                                                        </asp:CheckBox>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" onclick="CheckUncheckCheckAll(this);" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblPartNoHeader" runat="server" Text="Ref #"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                        <asp:HiddenField ID="hdnCasePartId" runat="server" Value='<%#Eval("CasePartId") %>' />
                                                                        <asp:Label ID="lblCatalogNum" runat="server" Text='<%#Eval("PartNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="28.5%" ItemStyle-Width="30%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLotNumHeader" runat="server" Text="Lot #" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%#Eval("LotNum") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblExpiryDateHeader" runat="server" Text="Expiry Date" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("ExpiryDate", "{0:d}") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="16.5%" ItemStyle-Width="15%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblKitStatusHeader" runat="server" Text="Status"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <%--<asp:HiddenField ID="hdnKitTotalQty" runat="server" Text='<%#Eval("Qty") %>' />--%>
                                                                        <asp:Label ID="lblPartStatus" runat="server" Text='<%#Eval("PartStatus") %>'></asp:Label>
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
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="checkoutbutton" OnClick="btnSave_Click" />
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
