<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusCustomerPARLevel.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.CustomerPARLevel" Title="Default" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script type="text/javascript">

        //        function ConfirmDelete() {
        //            alert('Hi');
        //            return ConfirmDelete("Are you sure you want to delete this record ?");
        //        }

        function ConfirmDelete() {
            var IsDeelete = confirm("Are you sure you want to delete this record ?");

            if (IsDeelete == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function pageLoad() {
            SearchTextByPartNumberForHeader('txtNewPartNum', 'RefNum', 'FetchPartCatalogByPartNum', 'txtNewDescription', 'hdnPartNumNew');
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
                InitGridEvent('<%= gdvCustomerPartDetails.ClientID %>');
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
                        <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0"
                            width="80%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="PAR Level Management"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCustomer" runat="server" Text="Customer:&nbsp;" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCustomer" runat="server" Width="300px" CssClass="ListBox"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <br />
                                    <asp:HiddenField ID="hdnPartNumNew" ClientIDMode="Static" runat="server" />
                                    <asp:Panel ID="pnlGrid" CssClass="pnlGrid" runat="server" Width="95%" ScrollBars="Auto"
                                        Height="485px">
                                        <asp:GridView ID="gdvCustomerPartDetails" runat="server" AutoGenerateColumns="False"
                                            SkinID="GridView" 
                                            OnRowCancelingEdit="gdvCustomerPartDetails_RowCancelingEdit"
                                            OnRowCommand="gdvCustomerPartDetails_RowCommand" 
                                            OnRowEditing="gdvCustomerPartDetails_RowEditing"
                                            OnRowDataBound="gdvCustomerPartDetails_RowDataBound" 
                                            Width="100%"  ShowHeaderWhenEmpty="true"
                                            AllowPaging="true" OnPageIndexChanging="gdvCustomerPartDetails_OnPaging" PageSize="50">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPartNum" runat="server" Text='<%# Eval("RefNum") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label runat="server" Text="Ref #" />
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
                                                <asp:TemplateField HeaderStyle-Width="60%" ItemStyle-Width="60%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label runat="server" Text="Description" />
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
                                                <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPARLevelQty" runat="server" Text='<%# Eval("PARLevelQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtPARLevelQty" MaxLength="3" Width="90%" runat="server" Text='<%#Eval("PARLevelQty") %>'
                                                            CssClass="textbox" />
                                                        <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                            FilterType="Custom" TargetControlID="txtPARLevelQty" ValidChars="0123456789">
                                                        </ajaxtk:FilteredTextBoxExtender>
                                                    </EditItemTemplate>
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label runat="server" Text="PAR Level Qty" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <br />
                                                                    <asp:TextBox ID="txtNewPARLevelQty" MaxLength="3" Width="90%" runat="server" />
                                                                    <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                        FilterType="Custom" TargetControlID="txtNewPARLevelQty" ValidChars="0123456789">
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
                                                        <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="DeleteRec" CommandArgument='<%#DataBinder.Eval(Container, "DataItem.RefNum")%>'
                                                            runat="server" OnClientClick="Javascript:return ConfirmDelete();">
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
                                                                    <asp:Label runat="server" Text="Action" />
                                                                    <br />
                                                                    <br />
                                                                    <hr size="1px" color="White" />
                                                                    <div style="width: 100%; padding-top: 20px;">
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
                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                PageButtonCount="5" Position="Bottom" />
                                            <PagerStyle CssClass="pagination" HorizontalAlign="Center" VerticalAlign="Middle" />
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
