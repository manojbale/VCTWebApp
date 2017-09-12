<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusCustomer.aspx.cs" Inherits="VCTWebApp.Shell.Views.Customer"
    Title="Customer" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript">
        function ConfirmSave() {
            var IsSave = confirm("Are you sure you want to save customer ?");
            if (IsSave == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function ConfirmUpdate() {
            var IsSave = confirm("Are you sure you want to update customer details ?");
            if (IsSave == true) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="3">
                                    <asp:Label ID="lblHeader" runat="server" Text="Customer"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="9">
                                    <asp:Panel ID="Panel1" CssClass="ActionPanel" runat="server">
                                        <table>
                                            <tr>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblCustomerNameFilter" runat="server" Text="Customer Name" CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblAccountNumberFilter" runat="server" Text="Account Number" CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblOwnershipStructureFilter" runat="server" Text="Ownership Structure"
                                                        CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblManagementStructureFilter" runat="server" Text="Management Structure"
                                                        CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 120px">
                                                    <asp:Label ID="lblSpineonlyMultiSpecialtyFilter" runat="server" Text="Spine only / Multi-Specialty"
                                                        CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblBranchAgencyFilter" runat="server" Text="Branch/ Agency" CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblManagerFilter" runat="server" Text="Regional Rep" CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblSalesRepresentativeFilter" runat="server" Text="Local Rep"
                                                        CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:Label ID="lblActiveInactiveFilter" runat="server" Text="Active/In-Active" CssClass="listboxheading"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="width: 110px">
                                                    <asp:TextBox ID="txtCustomerNameFilter" runat="server" MaxLength="100" ReadOnly="false"
                                                        Width="110px" Font-Size="8pt"></asp:TextBox>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:TextBox ID="txtAccountNumberFilter" runat="server" MaxLength="100" ReadOnly="false"
                                                        Width="110px" Font-Size="8pt"></asp:TextBox>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:DropDownList ID="ddlOwnershipStructureFilter" runat="server" AutoPostBack="false"
                                                        onchange="return OnChange();" CssClass="ListBox" Width="110px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:DropDownList ID="ddlManagementStructureFilter" runat="server" AutoPostBack="false"
                                                        CssClass="ListBox" Width="110px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:DropDownList ID="ddlSpineonlyMultiSpecialtyFilter" runat="server" AutoPostBack="false"
                                                        CssClass="ListBox" Width="110px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:DropDownList ID="ddlBranchAgencyFilter" runat="server" AutoPostBack="false"
                                                        CssClass="ListBox" Width="110px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:DropDownList ID="ddlManagerFilter" runat="server" AutoPostBack="false" CssClass="ListBox"
                                                        Width="110px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:DropDownList ID="ddlSalesRepresentativeFilter" runat="server" AutoPostBack="false"
                                                        CssClass="ListBox" Width="100px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:CheckBox Text="Active" CssClass="CheckBox" runat="server" ID="chkActiveInactiveFilter"
                                                        Checked="true" />
                                                </td>
                                                <td align="center" style="width: 110px">
                                                    <asp:LinkButton ID="lnkFilterData" runat="server" OnClick="lnkFilterCustomerListData_Click"
                                                        CausesValidation="false">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/view_small.png" BorderStyle="None"
                                                            ToolTip="Add" /></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkResetFilterCriteria" runat="server" OnClick="lnkResetFilterCriteria_Click"
                                                        Visible="true" CausesValidation="false">
                                                        <asp:Image ID="imgResetFilterCriteria" runat="server" ImageUrl="~/Images/refresh_small.png"
                                                            BorderStyle="None" ToolTip="Reset Filter Criteria" /></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0" style="height: 470px">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingCustomers" runat="server" Text="Existing Customer(s)" Width="150px"
                                                    CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingCustomers" Height="470px" CssClass="leftlistboxlong"
                                                    OnSelectedIndexChanged="lstExistingCustomers_SelectedIndexChanged" runat="server"
                                                    AutoPostBack="True"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top" align="left">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlCustomerSection1" runat="server" CssClass="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name *" Font-Bold="true"
                                                                    CssClass="label"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblAccountNumber" runat="server" Text="Account Number *" Font-Bold="true"
                                                                    CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtCustomerName" runat="server" MaxLength="100" ReadOnly="false"
                                                                    CssClass="textbox" Width="300px"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" TargetControlID="txtCustomerName"
                                                                    ValidChars=" ">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="100" ReadOnly="false"
                                                                    CssClass="textbox" Width="300px" />
                                                                <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" TargetControlID="txtAccountNumber"
                                                                    ValidChars=" ">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblStreetAddress" runat="server" Text="Street Name *" CssClass="label"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblCity" runat="server" CssClass="label" Text="City *"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtStreetAddress" runat="server" MaxLength="500" ReadOnly="false"
                                                                    Width="300px"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" TargetControlID="txtStreetAddress" 
                                                                    FilterType="Custom" FilterMode="InvalidChars" InvalidChars="`~!@$%^*()+=[]{}:;'<>/?">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="100" ReadOnly="false" Width="300px" />
                                                                  <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,Custom" TargetControlID="txtCity"
                                                                    ValidChars=" ">
                                                                </ajaxtk:FilteredTextBoxExtender>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblState" runat="server" CssClass="label" Text="State/Province *"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblZipCode" runat="server" Text="Zip Code *" CssClass="label"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">                                                                
                                                                <asp:DropDownList ID="ddlState" runat="server" AutoPostBack="false" CssClass="ListBox"
                                                                    Width="312px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtZipCode" runat="server" MaxLength="10" Width="300px"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtZipCode" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlCustomerSection2" runat="server" CssClass="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblOwnershipStructure" runat="server" Text="Ownership Structure *"
                                                                    CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblManagementStructure" runat="server" Text="Management Structure *"
                                                                    CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblSpineOnlyMultiSpecialty" runat="server" Text="Spine only / Multi-Specialty *"
                                                                    CssClass="label"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtOwnershipStructure" runat="server" MaxLength="100" Width="180px"
                                                                    Visible="false"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlOwnershipStructure" runat="server" AutoPostBack="false"
                                                                    CssClass="ListBox" Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20px">
                                                                <asp:LinkButton ID="lnkAddOwnershipStructure" runat="server" OnClick="lnkAddOwnershipStructure_Click">
                                                                    <asp:Image ID="imgAddOwnershipStructure" runat="server" ImageUrl="~/Images/Add.gif"
                                                                        BorderStyle="None" ToolTip="Add" /></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtManagementStructure" runat="server" MaxLength="100" Width="180px"
                                                                    Visible="false"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlManagementStructure" runat="server" AutoPostBack="false"
                                                                    CssClass="ListBox" Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20px">
                                                                <asp:LinkButton ID="lnkAddManagementStructure" runat="server" OnClick="lnkAddManagementStructure_Click">
                                                                    <asp:Image ID="imgAddManagementStructure" runat="server" ImageUrl="~/Images/Add.gif"
                                                                        BorderStyle="None" ToolTip="Add" /></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSpineOnlyMultiSpecialty" runat="server" MaxLength="100" Width="180px"
                                                                    Visible="false"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlSpineOnlyMultiSpecialty" runat="server" AutoPostBack="false"
                                                                    CssClass="ListBox" Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20px">
                                                                <asp:LinkButton ID="lnkAddSpineOnlyMultiSpecialty" runat="server" OnClick="lnkAddSpineOnlyMultiSpecialty_Click"
                                                                    Visible="false">
                                                                    <asp:Image ID="imgAddSpineOnlyMultiSpecialty" runat="server" ImageUrl="~/Images/Add.gif"
                                                                        BorderStyle="None" ToolTip="Add" /></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblQtyofOR" runat="server" Text="Qty of OR’s" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblBranchAgency" runat="server" Text="Branch/Agency *" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtQtyofOR" runat="server" MaxLength="10" Width="168px"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtQtyofOR" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBranchAgency" runat="server" MaxLength="100" Width="180px" Visible="false"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlBranchAgency" runat="server" AutoPostBack="false" CssClass="ListBox"
                                                                    Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20px">
                                                                <asp:LinkButton ID="lnkAddBranchAgency" runat="server" OnClick="lnkAddBranchAgency_Click">
                                                                    <asp:Image ID="imgAddBranchAgency" runat="server" ImageUrl="~/Images/Add.gif" BorderStyle="None"
                                                                        ToolTip="Add" /></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlCustomerSection3" runat="server" CssClass="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblRegionalRep" runat="server" Text="Regional Rep *" CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblLocalRep" runat="server" Text="Local Rep *"
                                                                    CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                            <asp:Label ID="lblSpecialistRep" runat="server" Text="Specialist Rep *"
                                                                    CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtManager" runat="server" MaxLength="100" Width="180px" Visible="false"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlManager" runat="server" AutoPostBack="false" CssClass="ListBox"
                                                                    Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 20px">
                                                                <asp:TextBox ID="txtSalesRepresentative" runat="server" MaxLength="100" Width="180px"
                                                                    Visible="false"></asp:TextBox>
                                                                <asp:DropDownList ID="ddlSalesRepresentative" runat="server" AutoPostBack="false"
                                                                    CssClass="ListBox" Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 20px">
                                                                <asp:DropDownList ID="ddlSpecialistRep" runat="server" AutoPostBack="false"
                                                                    CssClass="ListBox" Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlCustomerSection4" runat="server" CssClass="pnlDetail">
                                                    <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                        <tr>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblConsumptionInterval" runat="server" Text="Consumption Interval(In Mins) *"
                                                                    CssClass="label"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                                <asp:Label ID="lblAssetNearExpiryDays" runat="server" Text="Asset Near Expiry Days *"
                                                                    CssClass="labelbold"></asp:Label>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 25%">
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtConsumptionInterval" runat="server" MaxLength="4" Width="168px"
                                                                    Visible="true"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtConsumptionInterval" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAssetNearExpiryDays" runat="server" MaxLength="4" Width="168px"
                                                                    Visible="true"></asp:TextBox>
                                                                <ajaxtk:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtAssetNearExpiryDays" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                            <td style="width: 20px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table width="100%" cellspacing="0" cellpadding="2" border="0">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Panel ID="pnlCustomerProductLine" runat="server" CssClass="pnlDetail" ScrollBars="Auto">
                                                                <asp:GridView ID="gdvCustomerProductLine" runat="server" SkinID="GridView" Width="100%"
                                                                    OnRowDataBound="gdvCustomerProductLine_RowDataBound" AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Product Line">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductLineName" runat="server" Text='<%# Bind("ProductLineName") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="20%" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Product Line Desc">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductLineDesc" runat="server" Text='<%# Bind("ProductLineDesc") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="40%" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Use Product Line?">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("Selected") %>' CssClass="CheckBox" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="20%" />
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblExistingCustomer" runat="server" Text="Copy PAR Level from"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="ddlExistingCustomer" ClientIDMode="Static" runat="server" Width="95%"
                                                                                    CssClass="dropdown">
                                                                                </asp:DropDownList>
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
                                            <td>
                                                <asp:CheckBox Text="Active" CssClass="CheckBox" runat="server" ID="chkIsActive" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                </td>
                                                <td align="right" width="50%">
                                                    <asp:Panel ID="pnlButtonOnly" runat="server" Width="100%" Height="100%">
                                                        <asp:Button ID="btnReset" runat="server" Text="" OnClick="btnReset_Click" CssClass="resetbutton"
                                                            CausesValidation="False" />
                                                        <asp:Button ID="btnSave" runat="server" Text="" OnClick="btnSave_Click" CssClass="savebutton" />
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
            </td> </tr> </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxtk:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdatePnlProgress2"
        PopupControlID="UpdatePnlProgress2" BackgroundCssClass="modalPopup" />
</asp:Content>
