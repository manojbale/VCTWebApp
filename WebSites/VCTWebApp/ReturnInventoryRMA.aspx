<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnInventoryRMA.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.ReturnInventoryRMA" Title="Return Inventory (RMA)" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <style>
                .DispositionType, .DispositionRemarks
                {
                    display: none;
                }
            </style>
            <script type="text/javascript">

                var btnCancelId = "", remarks = "", DispositionType = 0, saveFlag = false, lblDispositionTypeId = 0, lblRemarks = "", count = 0; btnSave = "", btnCancel = "";

                $(function () {
                    $(window).load(function () {
                        fixedGrid();
                    });

                    var updm1 = Sys.WebForms.PageRequestManager.getInstance();

                    updm1.add_endRequest(function () {
                        fixedGrid();
                    });

                    function fixedGrid() {
                        InitGridEvent('<%= gdvKit.ClientID %>');
                        InitGridEvent('<%= gdvPart.ClientID %>');
                    }
                });

                function pageLoad() {

                    SearchRMAPartsByCatalogNumber('txtPartNum', 'sCatalogNumber', 'GetRMAPartsByPartNum', 'txtNewDescription', 'hdnPartNum', 'hdnPartDesc', 'hdnPartDetail');
                    //SearchRMAPartsByCatalogNumber('txtPartNum', 'sCatalogNumber', 'GetRMAPartsByPartNum', 'txtNewDescription', 'hdnLocationPartDetailId', 'hdnPartDetail');
                    SearchRMACasesByCaseNumber('txtCaseNumber', 'sCaseNumber', 'GetRMACasesByCaseNum', 'hdnKitDetail', 'hdnInvTypeDetail');

                    $('.ValidateCancel').click(function () {
                        saveFlag = false;

                        lblDispositionTypeId = $(this).parent().find('.DispositionType').attr('id');
                        lblRemarks = $(this).parent().find('.DispositionRemarks').attr('id');

                        remarks = $('#' + lblRemarks).val();
                        DispositionType = $('#' + lblDispositionTypeId).val();

                        if (DispositionType == 0 || DispositionType == null) {
                            $('#txtCancelRemarks').val('');
                            $('#ddlDispositionType').val('0');
                        }
                        else {
                            $('#txtCancelRemarks').val(remarks);
                            $('#ddlDispositionType').val(DispositionType);
                        }

                        ShowPopup();

                        return false;
                    });

                    function ShowPopup() {
                        $('#ContainerBox').fadeIn('slow');

                        var totWidth = $(window).width();
                        var totHeight = $(window).height();
                        var boxWidth = $('#ConfirmBox').width() / 2;
                        var boxHeight = $('#ConfirmBox').height() / 2;

                        var netWidth = totWidth / 2 - boxWidth;
                        var netHeight = totHeight / 2 - (boxHeight + 80);

                        $('#ContainerBox').css({ 'width': totWidth + 'px', 'height': totHeight - 2 + 'px' });

                        $('#ConfirmBox').css('left', '' + netWidth + 'px').css('top', '' + netHeight + 'px');
                    }
                    
                    $('#btnSaveRemarks').click(function () {
                        if ($('#ddlDispositionType option:selected').text() == "Others" && $('#txtCancelRemarks').val() == "") {
                            //if (saveFlag == false) {
                            alert("Remarks required for Other type of Disposition type.");
                            //saveFlag = true;
                            //}
                            return false;
                        }
                        else {
                            if (saveFlag == false) {
                                remarks = $('#txtCancelRemarks').val();
                                DispositionType = $('#ddlDispositionType').val();

                                $('#' + lblRemarks).val(remarks);
                                $('#' + lblDispositionTypeId).val(DispositionType);

                                saveFlag = true;
                                $('#ContainerBox').fadeOut('slow');
                            }
                        }
                        
                    });

                    $('#btnCancelRemarks').click(function () {
                        //                if (saveFlag == false) {
                        //                    saveFlag = true;
                        $('#ContainerBox').fadeOut('slow');
                        //}
                        return false;
                    });

                }
            </script>
            <div id="ContainerBox">
                <div id="ConfirmBox" class="view-trans-confirm-box">
                    <table width="100%" cellpadding="0" cellspacing="0" style="padding: 25px 0 0 25px;">
                        <tr height="30px">
                            <td valign="top" align="left">
                                Disposition Type :
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlDispositionType" ClientIDMode="Static" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                Remarks :
                            </td>
                            <td>
                                <asp:TextBox ID="txtCancelRemarks" ClientIDMode="Static" TextMode="MultiLine" Width="250px"
                                    Rows="3" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr height="50px">
                            <td colspan="2" align="center">
                                <asp:Button ID="btnSaveRemarks" ClientIDMode="Static" runat="server" Text="Save" OnClick="btnSaveRemarks_Click" />
                                &nbsp; &nbsp;
                                <asp:Button ID="btnCancelRemarks" ClientIDMode="Static" runat="server" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnDispositionTypeId" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnCancelRemarks" ClientIDMode="Static" runat="server" />
                </div>
            </div>
            <asp:HiddenField ID="hdnPartNum" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPartDesc" runat="server" ClientIDMode="Static" />
            <%--<asp:HiddenField ID="hdnLocationPartDetailId" runat="server" ClientIDMode="Static" />--%>
            <asp:HiddenField ID="hdnPartDetail" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnKitDetail" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnInvTypeDetail" runat="server" ClientIDMode="Static" />
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Return Inventory (RMA)"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="100%" align="center" border="0" cellspacing="0" cellpadding="3px">
                                        <tr>
                                            <td width="150px" align="right">
                                                Return From :
                                            </td>
                                            <td align="left" width="200px">
                                                <asp:RadioButton ID="radRFSelf" runat="server" Text="Self" GroupName="grpReturnFrom"
                                                    CssClass="cls-return-from" Checked="true" AutoPostBack="true" OnCheckedChanged="radReturnFrom_CheckedChanged" />
                                                <asp:RadioButton ID="radRFHospital" runat="server" Text="Hospital" GroupName="grpReturnFrom"
                                                    CssClass="cls-return-from" AutoPostBack="true" OnCheckedChanged="radReturnFrom_CheckedChanged" />
                                            </td>
                                            <td width="150px" align="right" id="Td1" runat="server">
                                                Case Type :
                                            </td>
                                            <td align="left" width="200px" id="Td2" runat="server">
                                                <asp:RadioButton ID="radCase" runat="server" Text="Case" GroupName="grpCaseType"
                                                    Checked="true" AutoPostBack="true" OnCheckedChanged="radInventoryType_CheckedChanged" />
                                                &nbsp; &nbsp;
                                                <asp:RadioButton ID="radPart" runat="server" Text="Part" GroupName="grpCaseType"
                                                    AutoPostBack="true" OnCheckedChanged="radInventoryType_CheckedChanged" />
                                            </td>
                                            <td width="300px">
                                                <asp:Panel ID="pnlReturnTo" runat="server">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td width="100px">
                                                                Return To :
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="radRTCorp" runat="server" Text="Corp" GroupName="grpReturnTo"
                                                                    Checked="true" OnCheckedChanged="radReturnTo_CheckedChanged" AutoPostBack="true" />
                                                                <asp:RadioButton ID="radRTRegion" runat="server" Text="Region" GroupName="grpReturnTo"
                                                                    OnCheckedChanged="radReturnTo_CheckedChanged" AutoPostBack="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr id="rowHospital" runat="server">
                                            <td align="right">
                                                <asp:Label ID="lblToLocation" runat="server" Text="Hospital :"></asp:Label>
                                            </td>
                                            <td align="left" colspan="4">
                                                <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="rowCaseNum" runat="server">
                                            <td align="right">
                                                <asp:Label ID="lblCaseNumber" runat="server" Text="Case Number" />
                                                :
                                            </td>
                                            <td colspan="4" align="left">
                                                <asp:TextBox ID="txtCaseNumber" runat="server" ClientIDMode="Static" Text="" OnTextChanged="txtCaseNumber_TextChanged"
                                                    AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <asp:Panel ID="pnlKit" CssClass="pnlGrid" runat="server" Width="980px" ScrollBars="Auto"
                                        Height="320px" Visible="true">
                                        <asp:GridView ID="gdvKit" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                            OnRowDataBound="gdvKit_RowDataBound" Width="980">
                                            <Columns>
                                                <%--<asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblKitNumberHeader" runat="server" Text="Kit Number" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblKitNumber" runat="server" Text='<%# Eval("KitNumber") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderStyle-Width="140" ItemStyle-Width="140" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblPartNoHeader" runat="server" Text="Part #" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartNo" runat="server" Text='<%# Eval("PartNum") %>' />
                                                        <asp:HiddenField ID="hdnCasePartId" runat="server" Value='<%#Eval("CasePartId") %>' />
                                                        <asp:HiddenField ID="hdnLocationPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />
                                                        <asp:HiddenField ID="hdnDispositionTypeId" runat="server" Value='<%#Eval("DispositionTypeId") %>' />
                                                        <asp:HiddenField ID="hdnSeekReturn" runat="server" Value='<%#Eval("SeekReturn") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="140" ItemStyle-Width="140" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLotNumHeader" runat="server" Text="Lot #" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%# Eval("LotNum") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="300" ItemStyle-Width="300" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDescHeader" runat="server" Text="Description" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Description") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>                                                        
                                                        <asp:Label ID="lblPartStatusHeader" runat="server" Text="Status" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>                                                        
                                                        <asp:Label ID="lblPartStatus" runat="server" Text='<%# Eval("PartStatus") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderStyle-Width="100" ItemStyle-Width="100" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSeekReturnHeader" runat="server" Text="Return" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkReturn" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150" ItemStyle-Width="150" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSeekReturnHeader" runat="server" Text="Seek Replacement" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSeekReturn" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150" ItemStyle-Width="150" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDispositionType" runat="server" Text="Disposition Type" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="main-panel">
                                                            <asp:TextBox ID="lblDispositionTypeId" runat="server" CssClass="DispositionType"
                                                                Text=""></asp:TextBox>
                                                            <asp:TextBox ID="lblDispositionRemarks" runat="server" CssClass="DispositionRemarks"
                                                                Text=""></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="CancelRMA" ImageUrl="~/Images/message.png"
                                                                Height="25px" Width="25px" CommandArgument='<%# Eval("LocationPartDetailId") %>'
                                                                CssClass="ValidateCancel" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>

                                    <table id="tblPart" runat="server" visible="false" width="980" style="background-color: #4f81bd;"
                                            cellpadding="5" cellspacing="0">
                                            <tr>
                                                <td width="80" style="color: White; font-weight: bold; text-align: center;">
                                                    Part Num
                                                </td>
                                                <td width="140">
                                                    <asp:TextBox ID="txtPartNum" runat="server" Width="95%" ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                                 <td width="80" style="color: White; font-weight: bold; text-align: center;">
                                                    Description
                                                </td>
                                                <td width="570">
                                                    <asp:TextBox ID="txtNewDescription" runat="server" Width="98%" Enabled="false" ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                                <td width="80" style="color: White; font-weight: bold; text-align: center;">
                                                    Quantity
                                                </td>
                                                <td width="80">
                                                    <asp:TextBox ID="txtQty" runat="server" Width="100%" ClientIDMode="Static" MaxLength="5"></asp:TextBox>
                                                    <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                FilterType="Numbers" TargetControlID="txtQty" >
                                                            </ajaxtk:FilteredTextBoxExtender>
                                                </td>
                                                <td width="110" align="center" valign="middle">
                                                    <asp:ImageButton ID="ÏmgBtnAdd" runat="server" ImageUrl="~/Images/Add.gif" BorderStyle="None"
                                                        ToolTip="Add" AlternateText="Add" OnClick="ÏmgBtnAdd_Click" />
                                                </td>
                                            </tr>                                            
                                        </table>
                                    <asp:Panel ID="pnlPart" CssClass="pnlGrid" runat="server" Width="980px" ScrollBars="Auto"
                                        Height="320px" Visible="false">
                                                                                
                                        <asp:GridView ID="gdvPart" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                            OnRowDataBound="gdvPart_RowDataBound" OnRowCommand="gdvPart_RowCommand" width="980">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="80" ItemStyle-Width="80" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblPartNoHeader" runat="server" Text="Part #" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>                                                        
                                                        <%--<asp:HiddenField ID="hdnLocationPartDetailId" runat="server" Value='<%#Eval("LocationPartDetailId") %>' />--%>
                                                        <asp:Label ID="lblPartNum" runat="server" Text='<%# Eval("PartNum") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="300" ItemStyle-Width="300" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDescHeader" runat="server" Text="Description" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Description") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="100" ItemStyle-Width="100" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLotNumHeader" runat="server" Text="Lot #" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLotNum" runat="server" Text='<%# Eval("LotNum") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                              <%--  <asp:TemplateField HeaderStyle-Width="80" ItemStyle-Width="80" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblQtyHeader" runat="server" Text="Quantity" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                 <asp:TemplateField HeaderStyle-Width="90" ItemStyle-Width="90" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblCaseNumHeader" runat="server" Text="Case Number (Optional)" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlCaseNumber" runat="server" AutoPostBack="true" onselectedindexchanged="ddlCaseNumber_SelectedIndexChanged">                                                            
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>                                                        
                                                        <asp:Label ID="lblPartStatusHeader" runat="server" Text="Status" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>                                                        
                                                        <asp:Label ID="lblPartStatus" runat="server" Text='<%# Eval("PartStatus") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <%--<asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSeekReturnHeader" runat="server" Text="Return" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkReturn" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderStyle-Width="120" ItemStyle-Width="120" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSeekReturnHeader" runat="server" Text="Seek Replacement" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSeekReturn" runat="server" Checked='<%# Eval("SeekReturn") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="150" ItemStyle-Width="150" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDispositionType" runat="server" Text="Disposition Type" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<div class="main-panel">--%>
                                                            <asp:TextBox ID="lblDispositionTypeId" runat="server" CssClass="DispositionType"
                                                                Text='<%#Eval("DispositionTypeId") %>'></asp:TextBox>
                                                            <asp:TextBox ID="lblDispositionRemarks" runat="server" CssClass="DispositionRemarks"
                                                                Text='<%#Eval("DispositionType") %>'></asp:TextBox>
                                                            <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="CancelRMA" ImageUrl="~/Images/message.png"
                                                                Height="25px" Width="25px" CommandArgument='<%# Eval("LocationPartDetailId") %>'
                                                                CssClass="ValidateCancel" />
                                                        <%--</div>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="60" ItemStyle-Width="60" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDispositionType" runat="server" Text="Action" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:ImageButton ID="ÏmgBtnRemove" runat="server" ImageUrl="~/Images/Delete.gif"
                                                            BorderStyle="None" ToolTip="Remove" AlternateText="Remove" CommandName="remove"
                                                            CommandArgument='<%#Container.DataItemIndex + 1%>' />--%>
                                                        <asp:ImageButton ID="ImgBtnRemove" runat="server" ImageUrl="~/Images/Delete.gif"
                                                            BorderStyle="None" ToolTip="Remove" AlternateText="Remove" CommandName="remove"
                                                            CommandArgument='<%# Eval("Index") %>'   OnClientClick= "javascriprt:return confirm('Are you sure you want to delete this record ? ');"/>
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
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click" />
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
