<%@ Page Title="Kit Family" Language="C#" MasterPageFile="~/Site1.master" CodeBehind="KitFamily.aspx.cs"
    Inherits="VCTWebApp.KitFamily" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript">

        function pageLoad() {
            SearchTextByCatalogNumberForHeader('txtNewPartNum', 'sCatalogNumber', 'GetCatalogByCatalogNumber', 'txtNewDescription', 'hdnPartNumNew');

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

        $(function () {

            $(window).load(function () {
                fixedGrid();
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                fixedGrid();
            });

            function fixedGrid() {
                InitGridEvent('<%= gvKitFamilyLocation.ClientID %>');
                InitGridEvent('<%= gdvPartDetails.ClientID %>');
                
            }


        });
        
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
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
           
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="0" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="Kit Family"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingKitFamilies" runat="server" Text="Existing Kit Families"
                                                    CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingKitFamilies" Height="400px" CssClass="leftlistboxlong"
                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstExistingKitFamilies_SelectedIndexChanged" >
                                                </asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <asp:Panel ID="pnlKitFamilyDetail" runat="server" class="pnlDetail">
                                        <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                            <tr>
                                                <td>
                                                    <table border="0">
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:Label ID="lblName" runat="server" Text="Name" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:TextBox ID="txtName" runat="server" CssClass="textbox" Width="300px" MaxLength="100"></asp:TextBox>
                                                                <%--<cc1:MyPropertyProxyValidator ID="MyPropertyProxyValidator2" runat="server" ControlToValidate="txtName"
                                                                    PropertyName="KitFamilyName" SourceTypeName="VCTWeb.Core.Domain.KitFamily" RulesetName="KitFamily"
                                                                    DisplayMode="SingleParagraph" Display="Dynamic" />--%>
                                                                <br />
                                                                <asp:RequiredFieldValidator ID="rfv_Name" runat="server" 
                                                                    ControlToValidate="txtName" Display="Dynamic" CssClass="required" ValidationGroup="submit">
                                                                 </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                         <tr class="blank-table-row1">
                                                            <td>                                                    
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:Label ID="lblKitType" runat="server" Text="Kit Type" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:DropDownList ID="ddlKitType" runat="server" CssClass="ListBox" Width="300px" />
                                                                 <br />
                                                                <asp:RequiredFieldValidator ID="rfv_KitType" runat="server" 
                                                                    InitialValue="0" ControlToValidate="ddlKitType" Display="Dynamic" CssClass="required" ValidationGroup="submit">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                         <tr class="blank-table-row1">
                                                            <td>                                                    
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="textbox" Width="300px"
                                                                    MaxLength="250"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr class="blank-table-row1">
                                                            <td>                                                    
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:Label ID="lblNumberOfTubs" runat="server" Text="Number of Tubs" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:TextBox ID="txtNumberOfTubs" runat="server" CssClass="textbox" Width="300px"
                                                                    MaxLength="5"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="txt_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtNumberOfTubs" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                                <br />
                                                                <asp:RequiredFieldValidator ID="rfv_NumberOfTube" runat="server" 
                                                                    ControlToValidate="txtNumberOfTubs" Display="Dynamic" CssClass="required" ValidationGroup="submit" >
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:CheckBox ID="chkActive" runat="server" Text="Active" CssClass="CheckBox" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center" valign="top">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left">
                                                               &nbsp; <asp:Label ID="lblFamilyLocation" runat="server" Text="Publish To Locations" CssClass="SectionHeaderText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="Panel1" CssClass="pnlGrid" runat="server" Height="210px" ScrollBars="Auto" Width="420px">
                                                                    <asp:GridView ID="gvKitFamilyLocation" runat="server" AutoGenerateColumns="False"
                                                                        SkinID="GridView" ShowHeaderWhenEmpty="true">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-Width="50%" ItemStyle-Width="50%" HeaderStyle-VerticalAlign="Middle">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label2" runat="server" Text="Location Name" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%# Eval("LocationId") %>' />
                                                                                    <asp:Label ID="lblLocatioName" runat="server" Text='<%# Eval("LocationName") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-Width="35%" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label3" runat="server" Text="Location Type" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLocationType" runat="server" Text='<%# Eval("LocationType") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center"  HeaderStyle-VerticalAlign="Middle">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="Label4" runat="server" Text="Action" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Eval("LocationExists") %>'
                                                                                        Enabled='<%# Convert.ToBoolean(Eval("LocationExists")) ? false : true %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top" colspan="2">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left">
                                                                <br />
                                                                &nbsp; <asp:Label ID="Label5" runat="server" Text="Family Parts/Items" CssClass="SectionHeaderText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                
                                                                <asp:HiddenField ID="hdnPartNumNew" ClientIDMode="Static" runat="server" />
                                                                <asp:Panel ID="pnlGrid" CssClass="pnlGrid" runat="server" Width="95%" Height="150px"
                                                                    ScrollBars="Auto">
                                                                    <asp:GridView ID="gdvPartDetails" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                                        OnRowCancelingEdit="gdvPartDetails_RowCancelingEdit" OnRowCommand="gdvPartDetails_RowCommand"
                                                                        EmptyDataText="No Record Found" OnRowEditing="gdvPartDetails_RowEditing" Width="100%"
                                                                        ShowHeaderWhenEmpty="true">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:HiddenField ID="hdnKitFamilyItemId" runat="server" Value='<%# Eval("KitFamilyItemId") %>' />
                                                                                    <asp:Label ID="lblPartNum" runat="server" Text='<%# Eval("CatalogNumber") %>' />
                                                                                </ItemTemplate>
                                                                                <HeaderTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="Label1" runat="server" Text="Ref #" />
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
                                                                                                <asp:Label ID="Label2" runat="server" Text="Description" />
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
                                                                            <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPARLevelQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:TextBox ID="txtPartQty" MaxLength="3" Width="90%" runat="server" Text='<%#Eval("Quantity") %>'
                                                                                        CssClass="textbox" />
                                                                                    <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                        FilterType="Custom" TargetControlID="txtPartQty" ValidChars="0123456789">
                                                                                    </ajaxtk:FilteredTextBoxExtender>
                                                                                </EditItemTemplate>
                                                                                <HeaderTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="Label3" runat="server" Text="Quantity" />
                                                                                                <br />
                                                                                                <br />
                                                                                                <hr size="1px" color="White" />
                                                                                                <br />
                                                                                                <asp:TextBox ID="txtNewPartQty" MaxLength="3" Width="90%" runat="server" />
                                                                                                <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                                    FilterType="Custom" TargetControlID="txtNewPartQty" ValidChars="0123456789">
                                                                                                </ajaxtk:FilteredTextBoxExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </HeaderTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkEdit" CausesValidation="false" CommandName="Edit" runat="server">
                                                                                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Images/Modify.gif" BorderStyle="None"
                                                                                            ToolTip="Edit" AlternateText="Edit" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>&nbsp;&nbsp;
                                                                                   
                                                                                    <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="DeleteRec" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                        runat="server" OnClientClick= "javascriprt:return confirm('Are you sure you want to delete this record ? ');">
                                                                                        <asp:Image ID="imgDelete" runat="server" ImageUrl="~/Images/Delete.gif" BorderStyle="None"
                                                                                            ToolTip="Delete" AlternateText="Delete" /></asp:LinkButton>

                                                                                </ItemTemplate>
                                                                                <EditItemTemplate>
                                                                                    <asp:LinkButton ID="lnkUpdate" CommandName="UpdateRec" runat="server" CausesValidation="false">
                                                                                        <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Images/Update.gif" BorderStyle="None"
                                                                                            ToolTip="Update" AlternateText="Update" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>
                                                                                    <asp:LinkButton ID="lnkCancel" CausesValidation="false" CommandName="Cancel" runat="server">
                                                                                        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Images/Cancel.gif" BorderStyle="None"
                                                                                            ToolTip="Cancel" AlternateText="Cancel" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>
                                                                                </EditItemTemplate>
                                                                                <HeaderTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="Label4" runat="server" Text="Action" />
                                                                                                <br />
                                                                                                <br />
                                                                                                <hr size="1px" color="White" />
                                                                                                <div style="width:100%;padding-top:20px;">
                                                                                                    <asp:LinkButton ID="lnkAdd" CausesValidation="false" CommandName="AddNewRow" runat="server">
                                                                                                        <asp:Image ID="imgAdd" runat="server" ImageUrl="~/Images/Add.gif" BorderStyle="None"
                                                                                                            ToolTip="Add" AlternateText="Add" /></asp:LinkButton>
                                                                                                </div>
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
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="70%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="30%" valign="top">
                                                    <asp:Button ID="btnNew" runat="server" Text="" CssClass="resetbutton" OnClick="btnNew_Click"
                                                        CausesValidation="False" />
                                                    <asp:Button ID="btnSave" runat="server" Text="" CssClass="savebutton" OnClick="btnSave_Click" ValidationGroup="submit" />
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
