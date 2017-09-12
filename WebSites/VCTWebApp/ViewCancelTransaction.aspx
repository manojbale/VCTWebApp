<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCancelTransaction.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.ViewCancelTransaction" Title="Default" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <style type="text/css">
        .ClickedPage
        {
            font-weight: bold;
        }
        .NotClickedPage
        {
            font-weight: normal;
        }
    </style>
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        Sys.Application.add_load(function () {

            $('#DefaultContent_gdvRoutineCases').Scrollable({
                ScrollHeight: 300

            });

        });
    </script>
    <script type="text/javascript">
        function pageLoad() {
            ViewTransactionByCaseNumber('txtCaseNumber', 'CaseNumber', 'GetTransactionsByFilter');
            //ViewTransactionByLocation('txtPartyName', 'PartyName', 'GetTransactionsByFilter');
            ViewTransactionLocationsById('txtPartyName', 'PartyName', 'GetVCTLocationsByFilter');

        }


        var updm1 = Sys.WebForms.PageRequestManager.getInstance();

        updm1.add_endRequest(function () {
            //  InitGridEvent('<%= gdvRoutineCases.ClientID %>');

        });

        $j = $.noConflict();
        $j(function () {

            $j(window).load(function () {
                //InitGridEvent('<%= gdvRoutineCases.ClientID %>');
            });


            var btnCancelId = "", remarks = "", DispositionType = 0, saveFlag = false;

            $j('.ValidateCancel').live("click", function () {

                ShowPopup();
                debugger;
                btnCancelId = $j(this).attr('id');

                remarks = $j('#txtCancelRemarks').val();
                DispositionType = $j('#ddlDispositionType').val();

                if ($j('#ddlDispositionType').val() == 0) {
                    if (saveFlag) {
                        alert("please select Disposition Type");
                    }
                    $j('#ContainerBox').fadeIn('slow');
                    return false;
                }
                else {
                    $j('#hdnCancelRemarks').val(remarks);
                    $j('#hdnDispositionTypeId').val(DispositionType)

                    saveFlag = false;
                    $j('#txtCancelRemarks').val('');
                    $j('#ddlDispositionType').val('0');

                    $j('#ContainerBox').fadeOut('slow');
                }

            });

            function ShowPopup() {
                $j('#ContainerBox').fadeIn('slow');

                var totWidth = $j(window).width();
                var totHeight = $j(window).height();
                var boxWidth = $j('#ConfirmBox').width() / 2;
                var boxHeight = $j('#ConfirmBox').height() / 2;

                var netWidth = totWidth / 2 - boxWidth;
                var netHeight = totHeight / 2 - (boxHeight + 80);

                //$j('#ContainerBox').css({ 'width': totWidth + 3 + 'px', 'height': totHeight - 33 + 'px' });
                $j('#ContainerBox').css({ 'width': totWidth + 'px', 'height': totHeight - 2 + 'px' });

                $j('#ConfirmBox').css('left', '' + netWidth + 'px').css('top', '' + netHeight + 'px');
            }

            $j('#btnSaveRemarks').click(function () {

                if ($('#ddlDispositionType option:selected').text() == "Others" && $('#txtCancelRemarks').val() == "") {
                    alert("Remarks required for Other type of Disposition type.");
                }
                else {
                    saveFlag = true;
                    $j('#' + btnCancelId).click();
                }
                return false;
            });

            $j('#btnCancelRemarks').click(function () {

                saveFlag = false;
                $j('#txtCancelRemarks').val('');
                $j('#ddlDispositionType').val('0');

                $j('#ContainerBox').fadeOut('slow');
                return false;
            });


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

            $j('.dropdown').live("change", function () {
                $j('#btnSearch').click();
            });

            var txtCaseNumber = "", txtPartyName = "";

            $j('.TextBoxCase').live("change, blur", function () {

                if ($j(this).attr('id') == "txtCaseNumber") {

                    if (txtCaseNumber != $j(this).val()) {
                        txtCaseNumber = $j(this).val();
                        $j('#btnSearch').click();
                    }

                }
                else {

                    if (txtPartyName != $j(this).val()) {
                        txtPartyName = $j(this).val();
                        $j('#btnSearch').click();
                    }

                }

            });

            $j('#txtStartDate, #txtEndDate').live("change", function () {
                var StartDate = new Date($j('#txtStartDate').val());
                var EndDate = new Date($j('#txtEndDate').val());

                if (EndDate < StartDate) {
                    alert("End date should be greater than Start Date");
                    return false;
                }
                $j('#btnSearch').click();
            });



        });

      
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
                        <asp:Button ID="btnSaveRemarks" ClientIDMode="Static" runat="server" Text="Save" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancelRemarks" ClientIDMode="Static" runat="server" Text="Cancel" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnDispositionTypeId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnCancelRemarks" ClientIDMode="Static" runat="server" />
        </div>
    </div>
    <asp:UpdatePanel ID="udpContent" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="View/Cancel Transaction"></asp:Label>
                                </td>
                            </tr>
                            <tr height="30px;">
                                <td align="center" valign="top">
                                    <br />
                                    <table border="0" align="center" cellspacing="0" cellpadding="0" class="header-table">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStartDate" Text="Start Date:" runat="server" />
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStartDate" runat="server" Width="100" Enabled="false" Text=""
                                                    ClientIDMode="Static" />
                                                <asp:Image ID="imgCalenderFrom" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif" />
                                                <ajaxtk:CalendarExtender ID="CalendarExtenderFrom1" runat="server" PopupButtonID="imgCalenderFrom"
                                                    TargetControlID="txtStartDate">
                                                </ajaxtk:CalendarExtender>
                                                <br />
                                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                                                    Display="Dynamic" CssClass="required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEndDate" Text="End Date:" runat="server" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEndDate" runat="server" Width="100" Text="" Enabled="false" ClientIDMode="Static" />
                                                <asp:Image ID="Image1" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif" />
                                                <ajaxtk:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="Image1"
                                                    TargetControlID="txtEndDate">
                                                </ajaxtk:CalendarExtender>
                                                <br />
                                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                                                    Display="Dynamic" CssClass="required"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblDetailHeader" runat="server" Text="View Transaction" CssClass="SectionHeaderText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSearch" ClientIDMode="Static" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                    Style="display: none;" />
                                                <div id="controlHead">
                                                </div>
                                                <asp:Panel ID="pnlProductAttributes" CssClass="pnlGrid" runat="server" Width="1000px"
                                                    ScrollBars="Auto" Height="370px" ClientIDMode="Static">
                                                    <table cellspacing="0" cellpadding="0" width="100%">
                                                        <tr>
                                                            <td align="center" valign="top">
                                                                <asp:GridView ID="gdvRoutineCases" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                                    ShowHeaderWhenEmpty="true" OnRowCommand="gdvRoutineCases_RowCommand" OnRowDataBound="gdvRoutineCases_RowDataBound"
                                                                    Width="100%" >
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-Width="4%" ItemStyle-Width="3%" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnCaseId" runat="server" Value='<%#Eval("CaseId") %>' />
                                                                                <asp:HiddenField ID="hdnCaseType" runat="server" Value='<%#Eval("CaseType") %>' />
                                                                                <asp:HiddenField ID="hdnInventoryType" runat="server" Value='<%#Eval("InventoryType") %>' />
                                                                                <asp:HiddenField ID="hdnPartyId" runat="server" Value='<%#Eval("PartyId") %>' />
                                                                                <asp:HiddenField ID="hdnPartyName" runat="server" Value='<%#Eval("PartyName") %>' />
                                                                                <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%#Eval("LocationId") %>' />
                                                                                <asp:HiddenField ID="hdnFrLocName" runat="server" Value='<%#Eval("FromLocationName") %>' />
                                                                                <asp:HiddenField ID="hdnToLocationId" runat="server" Value='<%#Eval("ToLocationId") %>' />
                                                                                <asp:HiddenField ID="hdnToLocName" runat="server" Value='<%#Eval("ToLocationName") %>' />
                                                                                <asp:HiddenField ID="hdnLTParty" runat="server" Value='<%#Eval("LTParty") %>' />
                                                                                <asp:HiddenField ID="hdnFromLTName" runat="server" Value='<%#Eval("FromLTName") %>' />
                                                                                <asp:HiddenField ID="hdnToLTName" runat="server" Value='<%#Eval("ToLTName") %>' />
                                                                                <asp:HiddenField ID="hdnDispositionType" runat="server" Value='<%#Eval("DispositionType") %>' />
                                                                                <asp:HiddenField ID="hdnRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
                                                                                <asp:HiddenField ID="hdnRowType" runat="server" Value='<%#Eval("RowType") %>' />
                                                                                <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                                    ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                                <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                                    <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                        OnRowDataBound="grdChild_RowDataBound" EnableViewState="false">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Ref #" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                                                    <%# Eval("PartNum")%>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField ItemStyle-Width="40%" DataField="Description" HeaderText="Description" />
                                                                                            <asp:BoundField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                                                                HeaderText="Lot #" />
                                                                                            <asp:BoundField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate"
                                                                                                HeaderText="Expiry Date" />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                    <asp:GridView ID="grdChildKit" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                        OnRowDataBound="grdChildKit_RowDataBound" EnableViewState="false">
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <HeaderStyle Width="10%" />
                                                                                                <ItemStyle Width="10%" />
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnBuildKitId" runat="server" Value='<%#Eval("BuildKitId") %>' />
                                                                                                    <asp:HiddenField ID="hdnCaseId" runat="server" Value='<%#Eval("CaseId") %>' />
                                                                                                    <asp:HiddenField ID="hdnCaseStatus" runat="server" Value='<%#Eval("CaseStatus") %>' />
                                                                                                    <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                                                        ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                                                    <asp:Panel ID="pnlChildKit" runat="server" Style="display: none">
                                                                                                        <asp:GridView ID="grdChildKitDetail" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                                            OnRowDataBound="grdChildKitDetail_RowDataBound" EnableViewState="false">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="Ref #" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                                                                        <%#Eval("PartNum")%>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField ItemStyle-Width="300px" DataField="Description" HeaderText="Description" />
                                                                                                                <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                                                                                    HeaderText="Lot #" />
                                                                                                                <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate"
                                                                                                                    HeaderText="Expiry Date" DataFormatString="{0:d}" />
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="KitFamilyName"
                                                                                                HeaderText="Kit Family" />
                                                                                            <asp:BoundField ItemStyle-Width="300px" DataField="Description" HeaderText="Description" />
                                                                                            <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="KitNumber"
                                                                                                HeaderText="Kit Number" />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="12.3%" ItemStyle-Width="12%" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <div style="border: 0px solid red; padding-right: 6px;">
                                                                                    <asp:Label ID="lblCaseTypeHeader" runat="server" Text="Case Type"></asp:Label>
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:DropDownList ID="ddlCaseType" ClientIDMode="Static" runat="server" Width="95%"
                                                                                        CssClass="dropdown">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCaseType" runat="server" Text='<%#Eval("CaseType") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="8%" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblCaseNumberHeader" runat="server"></asp:Label>
                                                                                <br />
                                                                                <br />
                                                                                <asp:TextBox ID="txtCaseNumber" ClientIDMode="Static" CssClass="TextBoxCase" runat="server"
                                                                                    Width="75%"></asp:TextBox>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <%--<asp:Label ID="lblCaseNumber" runat="server" Text='<%# Eval("CaseNumber") %>'></asp:Label>--%>
                                                                                <%# Eval("CaseNumber") %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="8%" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblInventoryTypeHeader" runat="server"></asp:Label>
                                                                                <br />
                                                                                <br />
                                                                                <asp:DropDownList ID="ddlInvType" ClientIDMode="Static" runat="server" Width="95%"
                                                                                    CssClass="dropdown">
                                                                                    <asp:ListItem>All</asp:ListItem>
                                                                                    <asp:ListItem>Kit</asp:ListItem>
                                                                                    <asp:ListItem>Part</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblInventoryType" runat="server" Text='<%# Eval("InventoryType") %>'></asp:Label>
                                                                                <%--<%# Eval("InventoryType") %>--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="8%" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblSurgeryDateHeader" runat="server"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <%--<asp:Label ID="lblSurgeryDate" runat="server" Text='<%# Eval("SurgeryDate", "{0:d}") %>'></asp:Label>--%>
                                                                                <%# Eval("SurgeryDate", "{0:d}") %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblFromTo" runat="server" Text="From / To"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFromToType" runat="server" Text=""></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="13%" ItemStyle-Width="13%" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <div style="border: 0px solid red; padding-left: 8px;">
                                                                                    <asp:Label ID="lblPartyNameHeader" runat="server"></asp:Label>
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtPartyName" ClientIDMode="Static" runat="server" CssClass="TextBoxCase"
                                                                                        Width="85%"></asp:TextBox>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLocationName" runat="server" Text=""></asp:Label>
                                                                                <%--<%# Eval("PartyName") %>--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="16%" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <div style="border: 0px solid red; padding-left: 12px;">
                                                                                    <asp:Label ID="lblLocationTypeHeader" runat="server"></asp:Label>
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:DropDownList ID="ddlLocType" runat="server" ClientIDMode="Static" Width="95%"
                                                                                        CssClass="dropdown">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLocationType" runat="server" Text=""></asp:Label>
                                                                                <%--<%# Eval("LocationType") %>--%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="14%" ItemStyle-Width="14%" ItemStyle-HorizontalAlign="Left"
                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <div style="border: 0px solid red; padding-left: 5px;">
                                                                                    <asp:Label ID="lblCaseStatusHeader" runat="server"></asp:Label>
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:DropDownList ID="ddlCaseStatus" ClientIDMode="Static" runat="server" Width="100%"
                                                                                        CssClass="dropdown">
                                                                                        <asp:ListItem>All</asp:ListItem>
                                                                                        <asp:ListItem>New</asp:ListItem>
                                                                                        <asp:ListItem>InventoryAssigned</asp:ListItem>
                                                                                        <asp:ListItem> PartiallyInventoryAssigned</asp:ListItem>
                                                                                        <asp:ListItem>Cancelled</asp:ListItem>
                                                                                        <asp:ListItem>Shipped</asp:ListItem>
                                                                                        <asp:ListItem>PartiallyShipped</asp:ListItem>
                                                                                        <asp:ListItem>Delivered</asp:ListItem>
                                                                                        <asp:ListItem>PartiallyDelivered</asp:ListItem>
                                                                                        <asp:ListItem>CheckedIn</asp:ListItem>
                                                                                        <asp:ListItem>PartiallyCheckedIn</asp:ListItem>
                                                                                        <asp:ListItem>InternallyRequested</asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                &nbsp; &nbsp;
                                                                                <asp:Image ID="imgCaseStatus" runat="server" Height="20px" />
                                                                                <br />
                                                                                &nbsp; &nbsp;
                                                                                <%--<asp:Label ID="lblCaseStatus" runat="server" Text='<%# Eval("CaseStatus") %>'></asp:Label>--%>
                                                                                <%# Eval("CaseStatus") %>
                                                                                <asp:HiddenField ID="hdnCaseStatus" runat="server" Value='<%#Eval("CaseStatus") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="8%" ItemStyle-Width="8%" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <HeaderTemplate>
                                                                                <div style="border: 0px solid red; padding-left: 20px;">
                                                                                    <asp:Label ID="lblInvoiceHeader" runat="server" Text="Invoice Adv." Font-Bold="true" />
                                                                                    <%--<hr class="hrstyle" size="1px" />--%>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImgBtnInvoice" runat="server" Height="20px" Width="20px" CommandName="Invoice"
                                                                                    CommandArgument='<%# Eval("CaseId") %>' ImageUrl="~/Images/invoice.png" Visible="false" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="6%" ItemStyle-Width="6%" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <HeaderTemplate>
                                                                                <div style="border: 0px solid red; padding-left: 20px;">
                                                                                    <asp:Label ID="lblActionHeader" runat="server" Text="Action" Font-Bold="true" />
                                                                                    <%--<hr class="hrstyle" size="1px" />--%>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="lnkCancel" runat="server" CommandName="CancelTransaction" Height="25px"
                                                                                    Width="25px" CommandArgument='<%# Eval("CaseId") %>' CssClass="ValidateCancel" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Panel ID="pnlPager" runat="server">
                                        <asp:LinkButton ID="btnPrevious" runat="server" Text="<<" Visible="false" OnClick="btnPrevious_Click"></asp:LinkButton>
                                        <asp:Repeater ID="rptPager" runat="server">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                    Enabled='<%# Eval("Enabled") %>' OnClick="Page_Changed"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:LinkButton ID="btnNext" runat="server" Text=">>" OnClick="btnNext_Click"></asp:LinkButton>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server" Visible="true">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="Right">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click"
                                                        CausesValidation="False" />
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
