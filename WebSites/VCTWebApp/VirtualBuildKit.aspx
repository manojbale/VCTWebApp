<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VirtualBuildKit.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.VirtualBuildKit" Title="Web Build Kit" MasterPageFile="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
  
    <script type="text/javascript">


         Sys.Application.add_load(function() {
            SearchTextByKitNumber('txtKitNumber', 'sKitNumber', 'GetMappedKitsByKitNumberOrDesc');
           $('#gdVirtualBuildKit').Scrollable({
                ScrollHeight: 310
                
            });
           //InitGridEvent('<%= gdVirtualBuildKit.ClientID %>');
            CheckUnCheckState();

        });



        function NewCatalogNumberKeyUp(textControl, event) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                var myHidden = document.getElementById('<%= hdnKitNumber.ClientID %>');
                if (myHidden) {
                    myHidden.value = '';
                }
            }
        }

        function CheckUnCheckState() {
            $('#chkSelect').click(function () {
                if (this.checked == true) {
                    $('input[type = "checkbox"]').prop('checked', true);
                }
                else {
                    $('input[type = "checkbox"]').prop('checked', false);
                }
            });
        }


//         $(".SelectOpt").click(function () {
//                var CheckAllCheckBox = document.getElementById('chkSelect');
//                if (this.checked == false && CheckAllCheckBox.checked) {
//                    CheckAllCheckBox.prop('checked', false);
//                }
//                
//            });
            
      


    </script>
    <script type="text/javascript">
        // It is important to place this JavaScript code after ScriptManager1
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('<%=pnlGrid.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=pnlGrid.ClientID%>').scrollLeft;
                yPos = $get('<%=pnlGrid.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('<%=pnlGrid.ClientID%>') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('<%=pnlGrid.ClientID%>').scrollLeft = xPos;
                $get('<%=pnlGrid.ClientID%>').scrollTop = yPos;
            }
        }

        function SavePnlGridScrollPos() {
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);
        }
    </script>
    <script type="text/javascript" language="javascript">
        function CheckUncheckCheckAll(Checkbox) {

            var CheckAllCheckBox = null;
            var CheckAllCheckBox = document.getElementById('chkSelect');
            if (!Checkbox.checked && CheckAllCheckBox.checked && (CheckAllCheckBox != 'undefined')) {
                CheckAllCheckBox.checked = false;
            }
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
                                    <asp:Label ID="lblHeader" runat="server" Text="Web Build Kit"></asp:Label>
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
                                                    onKeyUp="javascript:NewCatalogNumberKeyUp(this, event);" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSearch" CssClass="smallviewbutton" ClientIDMode="Static" runat="server"
                                                    OnClick="btnSearch_Click" />
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
                                <asp:HiddenField ID="hdnKitNumber" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnKitNumDesc" ClientIDMode="Static" runat="server" />
                                <td align="left" valign="top">
                                    <%-- <table id="tblKit" width="98%" class="header-freeze-table" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="10%" align="center">
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td width="10%" align="center">
                                                <asp:Label ID="lblHeaderPartNum" runat="server" Text="Ref #" ForeColor="White" />
                                            </td>
                                            <td width="50%" align="center">
                                                <asp:Label ID="lblHeaderDesc" runat="server" Text="Description" ForeColor="White" />
                                            </td>
                                            <td width="30%" align="center">
                                                <asp:Label ID="lblHeaderLotNum" runat="server" Text="Lot #"  ForeColor="White"/>
                                            </td>
                                        </tr>
                                    </table>  --%>
                                    <asp:Panel ID="pnlGrid" CssClass="pnlGrid" runat="server" Width="98%" ScrollBars="Auto"
                                        Height="350px" ClientIDMode="Static">
                                        <asp:GridView ID="gdVirtualBuildKit" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                            Width="955px" ShowHeader="true" OnRowDataBound="gdVirtualBuildKit_RowDataBound"
                                            ClientIDMode="Static">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="93px" ItemStyle-Width="93px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelect" ClientIDMode="Static" runat="server" AutoPostBack="false"
                                                            OnCheckedChanged="chkSelect_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkStatus" Text="" runat="server" CssClass="SelectOpt" onclick="CheckUncheckCheckAll(this);" />
                                                        <asp:HiddenField ID="hdnLocationPartDetailId" runat="server" Value='<%# Eval("LocationPartDetailId") %>' />
                                                        <asp:HiddenField ID="hdnLotNum" runat="server" Value='<%# Eval("LotNum") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="68px" ItemStyle-Width="68px" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeaderPartNum" runat="server" Text="Ref #" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartNumber" runat="server" Text='<%# Eval("PartNum") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="300px" ItemStyle-Width="300px" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeaderDesc" runat="server" Text="Description" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Description") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-Width="200px" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblHeaderLotNum" runat="server" Text="Lot #" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlLotNum" runat="server" Width="95%">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
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
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click"
                                                        CausesValidation="false" />
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
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
