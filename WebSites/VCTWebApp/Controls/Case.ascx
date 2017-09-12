<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Case.ascx.cs" Inherits="VCTWebApp.Controls.Case" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <script type="text/javascript">
            


            function GetKitFamiltData() {
                document.getElementById('<%= btnGetKitFamily.ClientID %>').click();
            }

            function checkDate(sender, args) {
              
              //  alert(document.getElementById('<%= txtSurgeryDate.ClientID %>').value);

                var loadadedSurgeryDate = document.getElementById('<%= hdnSurgeryDate.ClientID %>').value;
                
                var currentDate = new Date();
                currentDate.setDate(currentDate.getDate() - 1);
                if (sender._selectedDate < currentDate) {
                    alert("You cannot select a day earlier than today!");
                    sender._textbox.set_Value(loadadedSurgeryDate);
                    
                }

            }

            function ProcedureNameKeyUpCase(textControl, event) {
                var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                    var myHiddenCase = document.getElementById('<%= hdnProcedureNameCase.ClientID %>');
                    if (myHiddenCase) {
                        myHiddenCase.value = '';
                    }
                }
            }

            function NewKitFamilyKeyUp(textControl, event) {
                var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                    var myHidden = document.getElementById('<%= hdnKitFamilyIdNew.ClientID %>');
                    if (myHidden) {
                        myHidden.value = '';
                    }
                }
            }

            function NewCatalogNumberKeyUp(textControl, event) {
                var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                    var myHidden = document.getElementById('<%= hdnPartNumNew.ClientID %>');
                    if (myHidden) {
                        myHidden.value = '';
                    }
                }
            }

            function ShipToKeyUpCase(textControl, event) {
                var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                    var myHiddenCase = document.getElementById('<%= hdnShipToPartyIdCase.ClientID %>');

                    var txtShippingDate = document.getElementById('<%= txtShippingDate.ClientID %>');
                    var txtRetrievalDate = document.getElementById('<%= txtRetrievalDate.ClientID %>');
                    txtShippingDate
                    if (myHiddenCase) {
                        myHiddenCase.value = '';
                        txtShippingDate.value = '';
                        txtRetrievalDate.value = '';
                    }
                }
            }

            function PhysicianeNameKeyUpCase(textControl, event) {
                var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                if (keyCode != 9 && keyCode != 16 && keyCode != 13 && (keyCode < 33 || keyCode > 40)) {
                    var myHiddenCase = document.getElementById('<%= hdnPhysicianId.ClientID %>');
                    if (myHiddenCase) {
                        myHiddenCase.value = '';
                    }
                }
            }
        </script>
        <style type="text/css">
            .Initial
            {
                display: block;
                padding: 4px 18px 4px 18px;
                float: left;
                background: url("../Images/InitialImage.png") no-repeat right top;
                color: Black;
                font-weight: bold;
            }
            .Initial:hover
            {
                color: White;
                background: url("../Images/SelectedButton.png") no-repeat right top;
            }
            .Clicked
            {
                float: left;
                display: block;
                background: url("../Images/SelectedButton.png") no-repeat right top;
                padding: 4px 18px 4px 18px;
                color: Black;
                font-weight: bold;
                color: White;
            }
        </style>
        <table>
            <tr>
                <td>
                    <asp:Button Text="Kit" ID="KitTab" runat="server" Enabled="false" Height="25px" Width="120px"
                        OnClick="KitTab_Click" />
                    <asp:Button Text="Replenishment" ID="ReplenishmentTab" Height="25px" Width="120px"
                        runat="server" OnClick="ReplenishmentTab_Click" />
                </td>
            </tr>
        </table>
        <table align="left" width="1000px">
            <tr>
                <td align="center">
                    <table class="maintablePopUp" align="center" width="100%">
                        <tr class="header">
                            <td align="center" width="100%" style="padding-left: 90px;">
                                <asp:Label ID="lblHeader" runat="server" Text="Case: #1234"></asp:Label>
                            </td>
                            <td align="right" valign="top">
                                <asp:Button ID="btnClose" runat="server" CssClass="closebutton" CausesValidation="False"
                                    OnClick="btnClose_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center" colspan="2">
                                <table cellpadding="3" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlDetail" runat="server" CssClass="pnlDetail">
                                                <table cellspacing="2" cellpadding="2" width="100%" style="padding-left: 15px">
                                                    <tr>
                                                        <td colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" width="25%">
                                                            <asp:Label ID="lblSalesRep" runat="server" Text="Sales Representative:&nbsp;" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:Label ID="lblSurgeryDate" runat="server" Text="Surgery Date:&nbsp;" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:Label ID="lblShippingDate" runat="server" Text="Shipping Date:&nbsp;" CssClass="label"></asp:Label>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:Label ID="lblRetrievalDate" runat="server" Text="Retrieval Date:&nbsp;" CssClass="label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" width="25%">
                                                            <asp:DropDownList ID="ddlSalesRep" runat="server" Width="180" CssClass="ListBox">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:TextBox ID="txtSurgeryDate" runat="server" CssClass="TextBoxCase" Enabled="false"
                                                                ClientIDMode="Static" />
                                                            <asp:Image ID="imgCalenderFrom" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif" />
                                                            <ajaxtk:CalendarExtender ID="CalendarExtenderFrom1" runat="server" PopupButtonID="imgCalenderFrom"
                                                                TargetControlID="txtSurgeryDate" OnClientDateSelectionChanged="checkDate" >
                                                            </ajaxtk:CalendarExtender>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:TextBox ID="txtShippingDate" runat="server" CssClass="TextBoxCase" ClientIDMode="Static"
                                                                Enabled="false" />
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:TextBox ID="txtRetrievalDate" runat="server" CssClass="TextBoxCase" ClientIDMode="Static"
                                                                Enabled="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" width="25%" colspan="2">
                                                            <asp:Label ID="lblParty" runat="server" Text="Hospital:&nbsp;" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td width="25%" align="left">
                                                            <asp:Label ID="lblPhysician" runat="server" Text="Physician:&nbsp;" CssClass="label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <asp:HiddenField ID="hdnShipToPartyIdCase" ClientIDMode="Static" runat="server" />
                                                            <asp:TextBox ID="txtHospitalCase" Width="420px" runat="server" ClientIDMode="Static"
                                                                CssClass="textbox" onKeyUp="javascript:ShipToKeyUpCase(this, event);" />
                                                        </td>
                                                        <td width="25%" align="left">
                                                            <asp:HiddenField ID="hdnPhysicianId" ClientIDMode="Static" runat="server" />
                                                            <asp:TextBox ID="txtPhysician" runat="server" CssClass="TextBoxCase" ClientIDMode="Static"  onKeyUp="javascript:PhysicianeNameKeyUpCase(this, event);"></asp:TextBox>
                                                        </td>
                                                        <td width="25%" align="left">
                                                            <asp:Label ID="lblCaseStatus" runat="server" Text="Case Status:&nbsp;" CssClass="labelbold"></asp:Label>
                                                            <asp:Label ID="lblCaseStatusValue" Font-Bold="true" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <asp:Label ID="lblSplInstruction" runat="server" Text="Special Instruction:&nbsp;"
                                                                CssClass="label"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="4">
                                                            <asp:TextBox ID="txtSplInstructions" Width="910" runat="server" CssClass="TextBoxCase"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlDetailKit" runat="server" CssClass="pnlDetail">
                                                <table cellspacing="2" cellpadding="2" width="100%" style="padding-left: 15px">
                                                    <tr>
                                                        <td width="25%" align="left" style="white-space: nowrap;">
                                                            <asp:Label ID="lblProcedureName" runat="server" Text="Procedure Name:&nbsp;" CssClass="labelbold"></asp:Label>
                                                        </td>
                                                        <td width="25%" align="left" colspan="2">
                                                            <asp:Label ID="lblPatientName" runat="server" Text="Patient Name:&nbsp;" CssClass="label"></asp:Label>
                                                            <%--<asp:Label ID="lblKitFamily" runat="server" Text="Kit Family:&nbsp;" CssClass="labelbold"></asp:Label>--%>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:Label ID="lblTotalPrice" runat="server" Text="Total Price:&nbsp;" CssClass="label"></asp:Label>
                                                            <%--<asp:Label ID="lblQuantity" runat="server" Text="Quantity:*" CssClass="labelbold"></asp:Label>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" width="25%">
                                                            <asp:HiddenField ID="hdnProcedureNameCase" ClientIDMode="Static" runat="server" />
                                                            <asp:TextBox ID="txtProcedureNameCase" ClientIDMode="Static" runat="server" CssClass="TextBoxCase"
                                                                onKeyUp="javascript:ProcedureNameKeyUpCase(this, event);"   onchange= "javascript:GetKitFamiltData();"/>
                                                        </td>
                                                        <td align="left" colspan="2">
                                                            <asp:TextBox ID="txtPatientName" runat="server" Width="420px" CssClass="TextBoxCase" />
                                                            <%--<asp:TextBox ID="txtChildKitFamily" runat="server" Width="420px" ClientIDMode="Static" 
                                                                AutoPostBack="true" CssClass="textbox" onKeyUp="javascript:ChildKitFamily(this, event);" 
                                                                OnTextChanged="txtChildKitFamily_TextChanged" >
                                                            </asp:TextBox>
                                                            <asp:HiddenField ID="hdnChildKitFamilyId" runat="server" ClientIDMode="Static" />--%>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:TextBox ID="txtTotalPrice" runat="server" CssClass="TextBoxCase"></asp:TextBox>
                                                            <ajaxtk:FilteredTextBoxExtender ID="txtQuantity_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtTotalPrice" ValidChars="0123456789.">
                                                            </ajaxtk:FilteredTextBoxExtender>
                                                            <%--<asp:TextBox ID="txtQuantity" runat="server" CssClass="TextBoxCase" MaxLength="2" />
                                                            <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                FilterType="Numbers" TargetControlID="txtQuantity" ValidChars="0123456789">
                                                            </ajaxtk:FilteredTextBoxExtender>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td align="left" width="25%">
                                                        </td>
                                                        <td align="left">
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" width="25%">
                                                            
                                                        </td>
                                                        <td align="left" width="25%" colspan="1">
                                                            
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:CheckBox ID="chkFlexibleDates" Text="Flexible Dates (+/- 2 Days)" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            &nbsp;
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" width="100%">
                                            <asp:Panel ID="pnlIndicativeParts" CssClass="pnlDetail" align="center" runat="server"
                                                Visible="true" Width="95%">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <%--<asp:Label ID="lblIndicativeParts" Text="Indicative Part(s)/Item(s) [Optional]" runat="server"
                                                                CssClass="SectionHeaderText" />--%>
                                                            <asp:Label ID="lblKitFamilyDetails" Text="Kit Family Details" runat="server" CssClass="SectionHeaderText" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%--<asp:Panel ID="pnlGrid1" CssClass="pnlGrid" runat="server" Height="180px" ScrollBars="Auto"
                                                                Width="100%" BorderWidth="1px">
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gdvIndicativeParts" runat="server" ClientIDMode="Static" AutoGenerateColumns="False"
                                                                            SkinID="GridView" ShowHeaderWhenEmpty="false" Width="100%" OnRowDataBound="gdvIndicativeParts_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label Text='<%# Bind("PartNum") %>' runat="server" ID="lblPartNum" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label Text="Part/Item #" ID="lblPartNumHeader" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="70%" ItemStyle-Width="70%" ItemStyle-HorizontalAlign="Left"
                                                                                    HeaderStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label Text='<%# Bind("Description") %>' runat="server" ID="lblDescription" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label Text="Description" ID="lblDescriptionHeader" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("Selected") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderTemplate>
                                                                                        <asp:Label Text="Select" ID="lblSelectHeader" runat="server" />
                                                                                    </HeaderTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="txtChildKitFamily" EventName="TextChanged" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </asp:Panel>--%>
                                                            <asp:Panel ID="pnlGrid1" CssClass="pnlGrid" runat="server" Width="100%" ScrollBars="Auto"
                                                                Height="200px" BorderWidth="1px">
                                                                <asp:HiddenField ID="hdnKitFamilyIdNew" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnKitFamilyNameNew" ClientIDMode="Static" runat="server" />
                                                                <asp:GridView ID="gdvKitFamilyDetail" runat="server" AutoGenerateColumns="False"
                                                                    SkinID="GridView" OnRowCommand="gdvKitFamilyDetail_RowCommand" Width="100%" ShowHeaderWhenEmpty="true"
                                                                    OnRowDataBound="gdvKitFamilyDetail_RowDataBound" >
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-Width="75%" ItemStyle-Width="75%">
                                                                            <ItemTemplate>
                                                                                <asp:HiddenField ID="hdnKitFamilyId" ClientIDMode="Static" runat="server" Value='<%# Eval("KitFamilyId") %>'/>
                                                                                <asp:Label ID="lblKitFamily" runat="server" Text='<%# Eval("KitFamilyName") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblKitFamilyHeader" runat="server" Text="Kit Family" />
                                                                                            <br />
                                                                                            <br />
                                                                                            <hr size="1px" color="White" />
                                                                                            <br />
                                                                                            <asp:TextBox ID="txtNewKitFamily" ClientIDMode="Static" Width="97%" runat="server"
                                                                                                onKeyUp="javascript:NewKitFamilyKeyUp(this, event);" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblQuantityHeader" runat="server" Text="Quantity" />
                                                                                            <br />
                                                                                            <br />
                                                                                            <hr size="1px" color="White" />
                                                                                            <br />
                                                                                            <asp:TextBox ID="txtNewQuantity" MaxLength="3" Width="90%" runat="server" />
                                                                                            <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtenderQuantity" runat="server"
                                                                                                Enabled="True" FilterType="Custom" TargetControlID="txtNewQuantity" ValidChars="0123456789">
                                                                                            </ajaxtk:FilteredTextBoxExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="DeleteRec" CommandArgument='<%#DataBinder.Eval(Container, "DataItem.KitFamilyId")%>'
                                                                                    runat="server" OnClientClick="javascript:return confirm('Are you sure you want to delete this record ?');">
                                                                                    <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Images/Delete.gif" BorderStyle="None"
                                                                                        ToolTip="Delete" AlternateText="Delete" /></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblActionHeader" runat="server" Text="Action" />
                                                                                            <br />
                                                                                            <br />
                                                                                            <hr size="1px" color="White" />
                                                                                            <br />
                                                                                            <asp:LinkButton ID="lnkAdd" CausesValidation="false" CommandName="AddNewRow" runat="server">
                                                                                                <asp:Image ID="imgAdd" runat="server" ImageUrl="~/Images/Add.gif" BorderStyle="None"
                                                                                                    ToolTip="Add" AlternateText="Add" /></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlPartDetail" CssClass="pnlDetail" align="center" runat="server"
                                                Visible="false" Width="95%">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblPartsDetails" Text="Part(s)/Item(s) Details" runat="server" CssClass="SectionHeaderText" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlGrid2" CssClass="pnlGrid" runat="server" Width="100%" ScrollBars="Auto"
                                                                Height="280px" BorderWidth="1px">
                                                                <asp:HiddenField ID="hdnPartNumNew" ClientIDMode="Static" runat="server" />
                                                                <asp:HiddenField ID="hdnDescriptionNew" ClientIDMode="Static" runat="server" />
                                                                <asp:GridView ID="gdvPartDetail" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                                    OnRowCommand="gdvPartDetail_RowCommand" Width="100%" ShowHeaderWhenEmpty="true"
                                                                    OnRowDataBound="gdvPartDetail_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPartNum" runat="server" Text='<%# Eval("PartNum") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblPartNumHeader" runat="server" Text="Ref #" />
                                                                                            <br />
                                                                                            <br />
                                                                                            <hr size="1px" color="White" />
                                                                                            <br />
                                                                                            <asp:TextBox ID="txtNewPartNum" ClientIDMode="Static" Width="90%" runat="server"
                                                                                                onKeyUp="javascript:NewCatalogNumberKeyUp(this, event);" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="60%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblDescriptionHeader" runat="server" Text="Description" />
                                                                                            <br />
                                                                                            <br />
                                                                                            <hr size="1px" color="White" />
                                                                                            <br />
                                                                                            <asp:TextBox ID="txtNewDescription" ClientIDMode="Static" runat="server" Width="95%"
                                                                                               Enabled="false" CssClass="readonlytextbox" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblQuantityHeader" runat="server" Text="Quantity" />
                                                                                            <br />
                                                                                            <br />
                                                                                            <hr size="1px" color="White" />
                                                                                            <br />
                                                                                            <asp:TextBox ID="txtNewQuantity" MaxLength="3" Width="90%" runat="server" />
                                                                                            <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtenderQuantity" runat="server"
                                                                                                Enabled="True" FilterType="Custom" TargetControlID="txtNewQuantity" ValidChars="0123456789">
                                                                                            </ajaxtk:FilteredTextBoxExtender>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="DeleteRec" CommandArgument='<%#DataBinder.Eval(Container, "DataItem.PartNum")%>'
                                                                                    runat="server" OnClientClick="javascript:return confirm('Are you sure you want to delete this record ?');">
                                                                                    <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Images/Delete.gif" BorderStyle="None"
                                                                                        ToolTip="Delete" AlternateText="Delete" /></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblActionHeader" runat="server" Text="Action" />
                                                                                            <br />
                                                                                            <br />
                                                                                            <hr size="1px" color="White" />
                                                                                            <br />
                                                                                            <asp:LinkButton ID="lnkAdd" CausesValidation="false" CommandName="AddNewRow" runat="server">
                                                                                                <asp:Image ID="imgAdd" runat="server" ImageUrl="~/Images/Add.gif" BorderStyle="None"
                                                                                                    ToolTip="Add" AlternateText="Add" /></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
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
                                        <td align="center" width="100%">
                                            <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server" Width="95%">
                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" width="50%">
                                                            <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                        </td>
                                                        <td align="right" width="50%">
                                                            <%--<asp:Button ID="btnAssign" runat="server" CausesValidation="false" CssClass="resetbutton"
                                                                Text="" ToolTip="Assign" OnClick="btnAssign_Click" Visible="false" />--%>                                                          
                                                            <asp:Button ID="btnCaseReset" runat="server" Text="" CssClass="resetbutton" 
                                                                OnClick="btnCaseReset_Click" />
                                                                  <asp:Button ID="btnSave" runat="server" CssClass="savebutton" Text="" ToolTip="Save"
                                                                OnClick="btnSave_Click" CausesValidation="false"/>
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
                </td>  <asp:Button ID="btnGetKitFamily" runat="server"  ClientIDMode="Static" Text="" style="display:none;" OnClick="btnGetKitFamily_Click" />
            </tr>
            <asp:HiddenField ID="hdnSelectedType" runat="server" />
            <asp:HiddenField ID="hdnSurgeryDate" runat="server" ClientIDMode="Static" />
        </table>
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>
