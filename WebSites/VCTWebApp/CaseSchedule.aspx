<%@ Page Title="Case Schedule" Language="C#" MasterPageFile="~/Site1.master" CodeBehind="CaseSchedule.aspx.cs"
    Inherits="VCTWebApp.CaseSchedule" EnableEventValidation="false" Culture="Auto"
    UICulture="Auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Src="~/Controls/Case.ascx" TagName="CasePopUp" TagPrefix="ucCase" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <asp:UpdatePanel ID="udpContent" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">
                function pageLoad() {
                    SearchKitFamilyByNumber('txtKitFamily', 'sKitFamily', 'GetKitFamilyByNumber', 'hdnKitFamilyId');
                    SearchTextByProcedureName('txtProcedureName', 'sProcedureName', 'GetProceduresByProcedureName');
                    SearchTextByPartyName('txtHospital', 'sPartyName', 'GetPartyByPartyName');
                    //SearchTextByProcedureNameCase('txtProcedureNameCase', 'txtPhysician', 'sProcedureName', 'GetProceduresByProcedureName1');
                    // SearchTextByProcedureNameCase('txtProcedureNameCase', 'hdnPhysicianId', 'sProcedureName', 'GetProceduresByProcedureName1');
                    SearchTextByProcedureNameCase('txtProcedureNameCase', 'txtPhysician', 'sProcedureName', 'GetProceduresByProcedureName1');
                    SearchTextByPartyNameCase('txtHospitalCase', 'sPartyName', 'GetPartyByPartyName');
                    SearchTextByCatalogNumberForHeader2('txtNewPartNum', 'sCatalogNumber', 'GetCatalogByCatalogNumber', 'txtNewDescription', 'hdnDescriptionNew', 'hdnPartNumNew');
                    //SearchTextByKitNumberCase('txtKitNumber', 'sKitNumber', 'GetKitsByKitNumber');
                    //SearchKitFamilyByNumber('txtChildKitFamily', 'sKitFamily', 'GetKitFamilyByNumber', 'hdnChildKitFamilyId');
                    SearchTextByKitFamilyForHeader2('txtNewKitFamily', 'sKitFamily', 'GetKitFamilyByNumber', 'txtNewKitFamily', 'hdnKitFamilyNameNew', 'hdnKitFamilyIdNew');
                    SearchPhysicianTextByPartyIdCase('txtPhysician', 'sPhysicianName', 'GetPhysicianByPartyId', 'hdnShipToPartyIdCase', 'hdnPhysicianId');
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

                function ShipToKeyUp(textControl, event) {
                    var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                    if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                        var myHidden = document.getElementById('<%= hdnShipToPartyId.ClientID %>');
                        if (myHidden) {
                            myHidden.value = '';
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

                function SetPositionOfPopUp() {
                    var x = $(window).width();
                    var y = $(window).height();
                    $("#pnlCase").css({ top: y / 2, left: x / 2 });
                }

                $(document).keypress(function (e) {
                    if (e.keyCode === 13) {
                        e.preventDefault();
                        return false;
                    }
                });



                function ShowDateValidationMessage() {

                    alert('You cannot create a case earlier than today.');
                    return false;
                }


             

            </script>
            <style type="text/css">
                ul
                {
                    z-index: 5000000;
                }
            </style>
            <asp:Panel ID="pnlHeader" runat="server" Width="100%">
                <table align="left" border="0" width="100%">
                    <tr>
                        <td align="center">
                            <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                                width="80%">
                                <tr class="header">
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblHeader" runat="server" Text="Case Schedule"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="center">
                                        <table width="100%">
                                            <tr>
                                                <td width="25%" valign="top">
                                                    <asp:Panel ID="pnlDetail" Width="100%" runat="server" BorderColor="#34609B" BorderWidth="1"
                                                        Height="870px">
                                                        <table cellspacing="0" cellpadding="2" width="100%">
                                                            <tr>
                                                                <td colspan="2" align="center" valign="middle">
                                                                    <asp:Panel runat="server" Width="100%" BackColor="#CFDDEE" Height="25px">
                                                                        <table>
                                                                            <tr>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:Label ID="lblFilters" runat="server" Text="Filters" Font-Size="Large" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblHospital" runat="server" Text="Hospital:&nbsp;"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <%--<asp:DropDownList ID="ddlHospital" runat="server" Width="90%">
                                                                        <asp:ListItem Text="All"></asp:ListItem>
                                                                    </asp:DropDownList>--%>
                                                                    <asp:HiddenField ID="hdnShipToPartyId" ClientIDMode="Static" runat="server" />
                                                                    <asp:TextBox ID="txtHospital" runat="server" ClientIDMode="Static" CssClass="textbox"
                                                                        Width="83%" onKeyUp="javascript:ShipToKeyUp(this, event);" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblKitFamily" runat="server" Text="Kit Family:&nbsp;"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <%--<asp:DropDownList ID="ddlKitFamily" runat="server" Width="90%">
                                                                    <asp:ListItem Text="All"></asp:ListItem>
                                                                        <asp:ListItem Text="009241"></asp:ListItem>
                                                                    </asp:DropDownList>--%>
                                                                    <asp:TextBox ID="txtKitFamily" runat="server" ClientIDMode="Static" Width="83%" AutoCompleteType="None"
                                                                        onKeyUp="javascript:KitFamilyKeyUp(this, event);"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnKitFamilyId" runat="server" ClientIDMode="Static" Value="0" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblPhysician" runat="server" Text="Physician:&nbsp;"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="ddlPhysician" runat="server" Width="90%">
                                                                        <asp:ListItem Text="All"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="35%" align="right">
                                                                    <asp:Label ID="lblSalesRep" runat="server" Text="Sales Rep:&nbsp;"></asp:Label>
                                                                </td>
                                                                <td align="left" width="65%">
                                                                    <asp:DropDownList ID="ddlSalesRep" runat="server" Width="90%">
                                                                        <asp:ListItem Text="All"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lblStatus" runat="server" Text="Status:&nbsp;"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="90%">
                                                                        <asp:ListItem Text="All"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="20%">
                                                                    <asp:Label ID="lblProcedure" runat="server" Text="Procedure:&nbsp;"></asp:Label>
                                                                </td>
                                                                <%--<td align="left" width="30%">
                                                                    <asp:DropDownList ID="ddlProcedure" runat="server" Width="90%">
                                                                        <asp:ListItem Text="All"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>--%>
                                                                <td align="left" width="30%">
                                                                    <asp:HiddenField ID="hdnProcedureName" ClientIDMode="Static" runat="server" />
                                                                    <asp:TextBox ID="txtProcedureName" runat="server" ClientIDMode="Static" CssClass="textbox"
                                                                        Width="83%" onKeyUp="javascript:ProcedureNameKeyUp(this, event);" />
                                                                </td>
                                                            </tr>
                                                             <tr>
                                                                    <td colspan="2" align="center">
                                                                       &nbsp;
                                                                    </td>
                                                                </tr>
                                                            <tr>
                                                                <tr>
                                                                    <td colspan="2" align="center">
                                                                        <asp:Label ID="lblError" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>

                                                                  <tr>
                                                                <tr>
                                                                    <td colspan="2" align="center">
                                                                       &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <tr>
                                                                        <td align="center" width="50%">
                                                                            <asp:Button runat="server" ID="btnApply" Text="Apply" Width="100px" />
                                                                        </td>
                                                                        <td align="center" width="40%">
                                                                            <asp:Button runat="server" ID="btnReset" Text="Reset" Width="100px" OnClick="btnReset_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <%--<tr>
                                                                <td colspan="2" align="center">
                                                                    <asp:Calendar Height="200" Font-Size="Small" TitleStyle-BackColor="#A5BFE1" OtherMonthDayStyle-ForeColor="Gray"
                                                                        BorderColor="Black" ID="calMain" runat="server" Width="90%" OnSelectionChanged="calMain_SelectionChanged">
                                                                    </asp:Calendar>
                                                                </td>
                                                            </tr>--%>
                                                                    <tr>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Calendar ID="calMain" Width="90%" runat="server" BackColor="White" BorderColor="#3366CC"
                                                                                CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                                                                                ForeColor="#003399" Height="200px" OnSelectionChanged="calMain_SelectionChanged">
                                                                                <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                                                                <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                                                                <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                                                                <WeekendDayStyle BackColor="#CCDDEE" />
                                                                                <OtherMonthDayStyle ForeColor="#999999" />
                                                                                <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                                                                <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                                                                                <TitleStyle BackColor="#3D77C3" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True"
                                                                                    Font-Size="10pt" ForeColor="White" Height="25px" />
                                                                            </asp:Calendar>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Label ID="Label13" runat="server" Text="Status Legends" Font-Size="Large" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Panel runat="server" Width="90%" BorderWidth="1">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="90px" BackColor="#C4BD97" Height="15px" ID="Label7"
                                                                                                runat="server" Text="New" />
                                                                                        </td>
                                                                                        <td>
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="100px" BackColor="#D9D9D9" Height="15px" ID="Label10"
                                                                                                runat="server" Text="Cancelled" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="90px" BackColor="#FCD5B4" Height="15px" ID="Label3"
                                                                                                runat="server" Text="Inv. Assigned" />
                                                                                        </td>
                                                                                        <td>
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="100px" BackColor="#FCD5B4" Height="15px" ID="Label1"
                                                                                                runat="server" Text="Part. Assigned" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="90px" BackColor="#CCC0DA" Height="15px" ID="Label19"
                                                                                                runat="server" Text="Shipped" />
                                                                                            <%--  <asp:Label BorderWidth="1px" Width="90px" BackColor="#B7DEE8" Height="15px" ID="Label5"
                                                                                            runat="server" Text="Closed" />--%>
                                                                                        </td>
                                                                                        <td>
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="100px" BackColor="#CCC0DA" Height="15px" ID="Label8"
                                                                                                runat="server" Text="Part. Shipped" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="90px" BackColor="#B8CCE4" Height="15px" ID="Label4"
                                                                                                runat="server" Text="CheckedIn" />
                                                                                            <%-- <asp:Label BorderWidth="1px" Width="90px" BackColor="#B7DEE8" Height="15px" ID="Label2"
                                                                                            runat="server" Text="Part. Assigned" />--%>
                                                                                        </td>
                                                                                        <td>
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="100px" BackColor="#B8CCE4" Height="15px" ID="Label9"
                                                                                                runat="server" Text="Part. CheckedIn" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="90px" BackColor="#B8CCE10" Height="15px" ID="Label2"
                                                                                                runat="server" Text="Delivered" />
                                                                                        </td>
                                                                                        <td>
                                                                                            &nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td  align="center">
                                                                                            <asp:Label BorderWidth="1px" Width="100px" BackColor="#B8CCE10" Height="15px" ID="Label5"
                                                                                                runat="server" Text="Part. Delivered" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center" colspan="3">
                                                                                            <asp:Label BorderWidth="1px" BackColor="#B8CCE90" Height="15px" ID="Label6" runat="server"
                                                                                                Text="Internally Requested" Width="150px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="3"  align="center">
                                                                                            Part. denotes Partially.
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                                <td width="85%" valign="top">
                                                    <asp:Panel ID="pnlDetail2" Width="100%" runat="server" BorderColor="#34609B" BorderWidth="1"
                                                        Height="870px">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="Panel2" runat="server" Width="100%" BackColor="#CFDDEE" Height="25px">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="btnPrevious" runat="server" ImageUrl="~/Images/MenuIcons/icon_left.png"
                                                                                        OnClick="btnPrevious_Click" />
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td valign="middle">
                                                                                    <asp:Button ID="btnToday" runat="server" Text="Today" Width="90px" BorderColor="#8DAED9"
                                                                                        Font-Size="Medium" BackColor="#D6E3F1" OnClick="btnToday_Click" />
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/Images/MenuIcons/icon_right.png"
                                                                                        OnClick="btnNext_Click" />
                                                                                </td>
                                                                                <td width="50%">
                                                                                    <asp:Label ID="lblPeriod" runat="server" Text="August 2013" Font-Size="Large" />
                                                                                </td>
                                                                                <td width="30%" align="right">
                                                                                    <asp:RadioButtonList runat="server" ID="rdo" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdo_SelectedIndexChanged"
                                                                                        AutoPostBack="true">
                                                                                        <asp:ListItem Text="Monthly" />
                                                                                        <asp:ListItem Text="Weekly" />
                                                                                        <asp:ListItem Text="Daily" />
                                                                                    </asp:RadioButtonList>
                                                                                    <%--<asp:Button ID="btnDay" runat="server" Text="Day" Font-Size="Medium" BorderColor="#8DAED9"
                                                                                        BackColor="#CFDDEE" Width="75px" OnClick="btnDay_Click" />
                                                                                    <asp:Button ID="btnWeek" runat="server" Text="Week" Font-Size="Medium" BorderColor="#8DAED9"
                                                                                        BackColor="#CFDDEE" Width="75px" OnClick="btnWeek_Click" />
                                                                                    <asp:Button ID="btnMonth" runat="server" Text="Month" Font-Size="Medium" BorderColor="Transparent"
                                                                                        BackColor="#CFDDEE" Width="75px" OnClick="btnMonth_Click" />--%>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Panel ID="pnlCalendar" CssClass="pnlGrid" runat="server" Width="735px" Height="800px"
                                                                        ScrollBars="None">
                                                                        <asp:GridView ID="gvWeekly" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvWeekly_RowDataBound"
                                                                            SkinID="GridViewCal" Visible="false" OnRowCommand="gvWeekly_RowCommand">
                                                                            <Columns>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol1" runat="server" Value='<%# Bind("Col1") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol1Hdr" runat="server" Text="11 - Sunday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol2" runat="server" Value='<%# Bind("Col2") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol2Hdr" runat="server" Text="12 - Monday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol3" runat="server" Value='<%# Bind("Col3") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol3Hdr" runat="server" Text="13 - Tuesday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol4" runat="server" Value='<%# Bind("Col4") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol4Hdr" runat="server" Text="14 - Wednesday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol5" runat="server" Value='<%# Bind("Col5") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol5Hdr" runat="server" Text="15 - Thursday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol6" runat="server" Value='<%# Bind("Col6") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol6Hdr" runat="server" Text="16 - Friday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol7" runat="server" Value='<%# Bind("Col7") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol7Hdr" runat="server" Text="17 - Saturday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:ButtonField CommandName="ColumnClick" Visible="false" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <asp:GridView ID="gvDaily" Visible="false" runat="server" AutoGenerateColumns="False"
                                                                            OnRowDataBound="gvDaily_RowDataBound" SkinID="GridViewCal" OnRowCommand="gvDaily_RowCommand">
                                                                            <Columns>
                                                                                <asp:TemplateField ControlStyle-Width="100%">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol1" runat="server" Value='<%# Bind("Col1") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblHdr" runat="server" Text="13 - Tuesday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:ButtonField CommandName="ColumnClick" Visible="false" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <asp:GridView ID="gvMonthly" Visible="true" runat="server" AutoGenerateColumns="False"
                                                                            OnRowDataBound="gvMonthly_RowDataBound" SkinID="GridViewCalMonth" OnRowCommand="gvMonthly_RowCommand">
                                                                            <Columns>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol1" runat="server" Value='<%# Bind("Col1") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol1Hdr" runat="server" Text="Sunday" Font-Bold="true" />
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol2" runat="server" Value='<%# Bind("Col2") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol2Hdr" runat="server" Text="Monday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol3" runat="server" Value='<%# Bind("Col3") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol3Hdr" runat="server" Text="Tuesday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol4" runat="server" Value='<%# Bind("Col4") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol4Hdr" runat="server" Text="Wednesday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol5" runat="server" Value='<%# Bind("Col5") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol5Hdr" runat="server" Text="Thursday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol6" runat="server" Value='<%# Bind("Col6") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol6Hdr" runat="server" Text="Friday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ControlStyle-Width="95px">
                                                                                    <ItemTemplate>
                                                                                        <asp:HiddenField ID="hdnCol7" runat="server" Value='<%# Bind("Col7") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label ID="lblCol7Hdr" runat="server" Text="Saturday" Font-Bold="true"></asp:Label>
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:ButtonField CommandName="ColumnClick" Visible="false" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
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
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" OnClientClick="javascript:SetPositionOfPopUp();" />
                <ajaxtk:ModalPopupExtender ID="mpeSelectKit" runat="server" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlCase" TargetControlID="btnShowPopUp" />
                <asp:Panel ID="pnlCase" runat="server">
                    <%--<asp:ImageButton ID="btnCancel" ImageUrl="~/Images/Cancel.gif"  runat="server" align="right" />--%>
                    <%--<iframe name="myIframe" id="myIframe" width="1000px" height="630px" runat="server"
                src="Case.aspx"></iframe>--%>
                    <ucCase:CasePopUp ID="ucCasePopup" runat="server" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
