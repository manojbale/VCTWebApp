<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewProductTransfer.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.NewProductTransfer" Title="Default" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
     <script type="text/javascript">

         function ValidateEmptyKItFamily() {

             var txtKitFamily = document.getElementById('<%= txtKitFamily.ClientID %>');
             var lblError = document.getElementById('<%= lblError.ClientID %>');

             if (txtKitFamily.value == '') {
                 lblError.innerHTML = "Please enter kit family";
                 return false;
             }
         }

         function pageLoad() {
             SearchKitFamilyByNumber('txtKitFamily', 'sKitFamily', 'GetKitFamilyByNumber', 'hdnKitFamilyId');

         }

         $(function () {
             $(window).load(function () {
                 fixedGrid();
             });

             var updm1 = Sys.WebForms.PageRequestManager.getInstance();

             updm1.add_endRequest(function () {
                 fixedGrid();
             });

             function fixedGrid() {
                 InitGridEvent('<%= gvKitParts.ClientID %>');
                 InitGridEvent('<%= gvLocations.ClientID %>');
             }
         });

         function checkDate(sender, args) {
             var currentDate= new Date();
             currentDate.setDate(currentDate.getDate() - 1);
             if (sender._selectedDate < currentDate) {
                 alert("You cannot select a day earlier than today!");
                 sender._textbox.set_Value('')
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
                                    <asp:Label ID="lblHeader" runat="server" Text="New Product Transfer"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTransDate" runat="server" /> *
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransDate" runat="server" Width="85px" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                                <asp:Image ID="ImgTransDate" runat="server" ImageUrl="~/Images/calbtn.gif" />
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImgTransDate"
                                                    TargetControlID="txtTransDate" OnClientDateSelectionChanged="checkDate" >
                                                </asp:CalendarExtender>                                              
                                                <asp:RequiredFieldValidator ID="rfvTransDate" runat="server" ControlToValidate="txtTransDate"
                                                    Display="Dynamic" CssClass="required" ValidationGroup="submit"></asp:RequiredFieldValidator>
                                            </td>                                            
                                            <td width="20px">
                                            </td>                                            
                                            <td>
                                                <asp:Label ID="lblSearchHeading" runat="server" Text="Kit Family / Master BOM : *" />
                                            </td>
                                            <td>
                                                &nbsp;
                                                <%--<asp:DropDownList ID="ddlKitFamilyId" runat="server" Width="100px" 
                                                    onselectedindexchanged="ddlKitFamilyId_SelectedIndexChanged" 
                                                    AutoPostBack="true" Visible="false" >
                                                </asp:DropDownList>--%>
                                                <asp:TextBox ID="txtKitFamily" runat="server" ClientIDMode="Static" 
                                                    Width="300px" AutoCompleteType="None"
                                                     onKeyUp="javascript:KitFamilyKeyUp(this, event);"></asp:TextBox>
                                                <asp:HiddenField ID="hdnKitFamilyId" runat="server" ClientIDMode="Static" Value="0" />                                                                                           
                                              <%-- <asp:RequiredFieldValidator ID="rfvKitFamilyId" runat="server" ControlToValidate="txtKitFamily"
                                                    Display="Dynamic" CssClass="required"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="smallviewbutton"
                                                    ValidationGroup="submit" />
                                            </td>                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="left" valign="top">
                                    <%--<table id="tblPart" width="98%" class="header-freeze-table" cellpadding="0" cellspacing="0" style="padding:8px 0;">
                                        <tr>
                                            <td width="20%" align="center">      
                                               <asp:Label ID="lblPartNum" runat="server" Text="Ref #" ForeColor="White" />                                           
                                            </td>
                                            <td width="60%" align="center">
                                                <asp:Label ID="lblDesc" runat="server" Text="Description" ForeColor="White" />
                                            </td>
                                            <td width="20%" align="center">
                                                <asp:Label ID="lblQty" runat="server" Text="Quantity" ForeColor="White" />
                                            </td>
                                        </tr>
                                    </table>  --%>
                                    <asp:Panel ID="pnlParts" ClientIDMode="Static" CssClass="pnlGrid" runat="server" Width="98%"
                                        ScrollBars="Auto" Height="140px">

                                        <asp:GridView ID="gvKitParts" runat="server" AllowPaging="false" AutoGenerateColumns="False" 
                                             SkinID="GridView" OnRowDataBound="gvKitParts_RowDataBound" ShowHeader="true">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblPartNum" runat="server" Text="Ref #" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCatalogNumber" runat="server" Text='<%# Bind("CatalogNumber") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />                                                    
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Width="60%" ItemStyle-Width="60%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text="Description" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' />
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblQty" runat="server" Text="Quantity" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />                                                    
                                                </asp:TemplateField>
                                            </Columns>           
                                            <HeaderStyle CssClass="grid-freeze-row" />                               
                                        </asp:GridView>
                                    </asp:Panel>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblgvHeading" runat="server" Text="Select Branch to transfer the Inventory"
                                        CssClass="SectionHeaderTextBig"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                   <%-- <table id="tblLocation" width="98%" class="header-freeze-table" border="0" cellpadding="0" cellspacing="0" style="padding:8px 0;">
                                        <tr>
                                            <td width="10%" align="center">      
                                               <asp:Label ID="lblSelect" runat="server" Text="Select" ForeColor="White" />
                                            </td>
                                            <td width="30%" align="center">
                                                <asp:Label ID="lblLocationNameHeader" runat="server" Text="Locations" ForeColor="White" />
                                            </td>
                                            <td width="30%" align="center">
                                                <asp:Label ID="lblLocationTypeNameHeader" runat="server" Text="Location Type" ForeColor="White" />
                                            </td>
                                            <td width="15%" align="center">
                                                <asp:Label ID="lblStatusHeader" runat="server" Text="Region/Office" ForeColor="White" />
                                            </td>
                                            <td width="15%" align="center">
                                                <asp:Label ID="lblQuantity" runat="server" Text="Quantity" ForeColor="White" />
                                            </td>
                                        </tr>
                                    </table>   --%>
                                    <asp:Panel ID="pnlLocation" ClientIDMode="Static" CssClass="pnlGrid" runat="server" Width="98%" ScrollBars="Auto"
                                        Height="170px">
                                        <asp:GridView ID="gvLocations" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                            SkinID="GridView" OnRowDataBound="gvLocations_RowDataBound"  ShowHeader="true">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSelect" runat="server" Text="Select" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkStatus" Text="" runat="server" />
                                                        <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%# Eval("LocationId") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLocationNameHeader" runat="server" Text="Locations" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocationName" runat="server" Text='<%# Eval("LocationName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                  <asp:TemplateField HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLocationTypeNameHeader" runat="server" Text="Location Type" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocationTypeName" runat="server" Text='<%# Eval("LocationTypeName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblStatusHeader" runat="server" Text="Region/Office Mapped" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Convert.ToInt32(Eval("LocationStatus")) > 0 ? "Yes" : "No" %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity" ForeColor="White" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' Width="80%"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="grid-freeze-row" />
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
                                                    <asp:Button ID="btnNew" runat="server" CssClass="resetbutton" OnClick="btnNew_Click" />
                                                    
                                                    <asp:Button ID="btnSave" runat="server" CssClass="savebutton" OnClick="btnSave_Click"
                                                        ValidationGroup="submit" />
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
