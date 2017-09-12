<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusNotificationSetup.aspx.cs"
    Inherits="VCTWebApp.EParPlusNotificationSetup" Title="ePar+ Notification Setup"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
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

        $(function () {
            $(window).load(function () {
                fixedGrid();
            });

            var updm1 = Sys.WebForms.PageRequestManager.getInstance();

            updm1.add_endRequest(function () {
                fixedGrid();
            });

            function fixedGrid() {
                InitGridEvent('<%= gdvNotification.ClientID %>');
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
                                <td valign="top" align="left" style="width: 195px">
                                    <table class="leftlistboxborder" cellspacing="0" cellpadding="0" style="height: 440px">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblExistingCustomers" runat="server" Text="Existing Customer(s)" Width="150px"
                                                    CssClass="listboxheading"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <asp:ListBox ID="lstExistingCustomers" Height="450px" CssClass="leftlistboxlong"
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
                                                <asp:Panel ID="pnlNotificationSelection" runat="server" CssClass="pnlDetail" Enabled="false"
                                                    Style="padding: 0 10px 0 10px;">
                                                    <table width="100%" cellspacing="5" cellpadding="3" border="0">
                                                        <tr>
                                                            <td style="width: 20%;">
                                                                <asp:Label ID="lblSelectNotificationType" runat="server" Text="Select Notification Type"
                                                                    CssClass="listboxheading"></asp:Label>
                                                            </td>
                                                            <td style="width: 75%;">
                                                                <asp:DropDownList ID="ddlNotificationType" runat="server" Width="310px" CssClass="ListBox"
                                                                    OnSelectedIndexChanged="ddlNotificationType_SelectedIndexChanged" AutoPostBack="True">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlGrid" runat="server" CssClass="pnlDetail" Height="270px" ScrollBars="Auto">
                                                    <asp:GridView ID="gdvNotification" runat="server" SkinID="GridView" Width="100%"
                                                        OnRowCommand="gdvNotification_RowCommand" OnRowCancelingEdit="gdvNotification_RowCancelingEdit"
                                                        OnRowEditing="gdvNotification_RowEditing" OnPageIndexChanging="gdvNotification_OnPaging"
                                                        AllowPaging="true" PageSize="50" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Regional/ Local Rep">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNotificationDetailId" runat="server" Text='<%# Bind("NotificationDetailId") %>'
                                                                        Visible="False"></asp:Label>
                                                                    <asp:Label ID="lblReceiverType" runat="server" Text='<%# Bind("ReceiverType") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="12%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Receiver Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReceiverName" runat="server" Text='<%# Bind("ReceiverName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="14%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Receiver Email Id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReceiverEmailID" runat="server" Text='<%# Bind("ReceiverEmailID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="23%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="23%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Time">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTimeToSendEmail" runat="server" Text='<%# Bind("TimeToSendEmail") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <cc1:TimeSelector ID="ctlTimeEdit" runat="server" SelectedTimeFormat="TwentyFour" Width="90%" DisplaySeconds="false" />
                                                                    <asp:TextBox ID="txtTimeEdit" runat="server" MaxLength="100" Width="90%" Text="00:00" Enabled="false" Visible="false"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle Width="15%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Configuration Setting">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblConfigurationSetting" runat="server" Text='<%# Bind("ConfigurationSetting") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtConfigurationSettingEdit" MaxLength="3" Width="80%" runat="server"
                                                                        Text='<%#Eval("ConfigurationSetting") %>' CssClass="textbox" Visible="false" />
                                                                    <asp:CheckBoxList ID="chkConfigurationSettingEdit" runat="server" RepeatDirection="Horizontal"
                                                                        Visible="false" Width="100%">
                                                                        <asp:ListItem Text="Sun" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Mon" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="Tue" Value="3"></asp:ListItem>
                                                                        <asp:ListItem Text="Wed" Value="4"></asp:ListItem>
                                                                        <asp:ListItem Text="Thu" Value="5"></asp:ListItem>
                                                                        <asp:ListItem Text="Fri" Value="6"></asp:ListItem>
                                                                        <asp:ListItem Text="Sat" Value="7"></asp:ListItem>
                                                                    </asp:CheckBoxList>
                                                                    <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="false"
                                                                        FilterType="Custom" TargetControlID="txtConfigurationSettingEdit" ValidChars="0123456789">
                                                                    </ajaxtk:FilteredTextBoxExtender>
                                                                </EditItemTemplate>
                                                                <HeaderStyle Width="22%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="22%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEdit" CausesValidation="false" CommandName="Edit" runat="server"
                                                                        CommandArgument='<%#DataBinder.Eval(Container, "DataItem.NotificationDetailId")%>'>
                                                                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Images/Modify.gif" BorderStyle="None"
                                                                            ToolTip="Edit" AlternateText="Edit" OnClick="javascript:SavePnlGridScrollPos();" /></asp:LinkButton>&nbsp;&nbsp;
                                                                    <asp:LinkButton ID="lnkDelete" CausesValidation="false" CommandName="DeleteRec" CommandArgument='<%#DataBinder.Eval(Container, "DataItem.NotificationDetailId")%>'
                                                                        runat="server" OnClientClick="javascript:return ConfirmDelete();">
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
                                                                <HeaderStyle Width="6%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="6%" />
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
                                            <td align="left">
                                                <asp:Panel ID="pnlEmailDetails" runat="server" CssClass="pnlDetail" Height="100px">
                                                    <table width="100%" cellspacing="5" cellpadding="3" border="0">
                                                        <tr>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblReceiverType" runat="server" Text="Receiver Type *:&nbsp;" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label ID="lblTime" runat="server" Text="Time *:&nbsp;" />
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlReceiverType" runat="server" Width="310px" CssClass="ListBox"
                                                                    OnSelectedIndexChanged="ddlReceiverType_SelectedIndexChanged" AutoPostBack="True">
                                                                    <asp:ListItem Text="--Select--" Value="--Select--"></asp:ListItem>
                                                                    <asp:ListItem Text="Local Rep" Value="Local Rep"></asp:ListItem>
                                                                    <asp:ListItem Text="Regional Rep" Value="Regional Rep"></asp:ListItem>
                                                                    <asp:ListItem Text="Specialist Rep" Value="Specialist Rep"></asp:ListItem>
                                                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td colspan="2" style="width: 50%">
                                                                <cc1:TimeSelector ID="ctlTime" runat="server" SelectedTimeFormat="TwentyFour" DisplaySeconds="false" />
                                                                <asp:TextBox ID="txtTime" runat="server" MaxLength="100" Width="70px" Text="00:00" Enabled="false" Visible="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 50%">
                                                                <asp:Label ID="lblReceiverName" runat="server" Text="Receiver Name:&nbsp;" />
                                                            </td>
                                                            <td align="left" colspan="2">
                                                                <asp:Label ID="lblReceiverEmailId" runat="server" Text="Receiver Email Id:&nbsp;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtReceiverName" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReceiverEmailId" runat="server" MaxLength="100" Width="300px"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <%--<tr>
                                                            <td colspan="2" style="width: 600px">
                                                                <asp:Label ID="lblConfigurationSetting1" runat="server" Visible="false" Text="Send only when Effective Inventory for Unique Ref# has been below Par Level for"></asp:Label>
                                                                <asp:TextBox ID="txtlblConfigurationSetting" Text="" Visible="false" Enabled="true"
                                                                    runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                                                <asp:Label ID="lblConfigurationSetting2" runat="server" Visible="false" Text=" days."></asp:Label>
                                                                <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtlblConfigurationSetting" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>--%>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Panel ID="pnlDays" runat="server" CssClass="pnlDetail" Style="padding: 0 10px 0 10px;">
                                                    <table width="100%" cellspacing="5" cellpadding="3" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblConfigurationSetting1" runat="server" Visible="false" Text="Send only when Effective Inventory for Unique Ref# has been below Par Level for"></asp:Label>
                                                                <asp:TextBox ID="txtlblConfigurationSetting" Text="" Visible="false" Enabled="true"
                                                                    runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                                                <asp:Label ID="lblConfigurationSetting2" runat="server" Visible="false" Text=" days."></asp:Label>
                                                                <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    FilterType="Custom" TargetControlID="txtlblConfigurationSetting" ValidChars="0123456789">
                                                                </ajaxtk:FilteredTextBoxExtender>
                                                            </td>
                                                            <td align="left">
                                                                <asp:CheckBoxList ID="chkDays" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Sunday" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Monday" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Tuesday" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="Wednesday" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="Thursday" Value="5"></asp:ListItem>
                                                                    <asp:ListItem Text="Friday" Value="6"></asp:ListItem>
                                                                    <asp:ListItem Text="Saturday" Value="7"></asp:ListItem>
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
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
                                                <td align="right" width="50%">
                                                    <asp:Button ID="btnSave" runat="server" Text="" OnClick="btnSave_Click" CssClass="savebutton" />
                                                    <asp:Button ID="btnReset" runat="server" Text="" OnClick="btnReset_Click" CssClass="resetbutton"
                                                        CausesValidation="False" />
                                                    <%-- <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="false" OnClick="lnkAdd_Click">
                                                        <asp:Image ID="imgAdd" runat="server" ImageUrl="~/Images/save_small.png" BorderStyle="None"
                                                            ToolTip="Add Notification detail." /></asp:LinkButton>--%>
                                                    <%--<asp:LinkButton ID="lnkReset" runat="server" CausesValidation="false" OnClick="lnkReset_Click">
                                                        <asp:Image ID="imgReset" runat="server" ImageUrl="~/Images/refresh_small.png" BorderStyle="None"
                                                            ToolTip="Reset" /></asp:LinkButton>--%>
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
</asp:Content>
