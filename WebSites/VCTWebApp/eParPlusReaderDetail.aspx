<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusReaderDetail.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.eParPlusReaderDetail" Title="RFID Reader Detail"
    MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script type="text/javascript">
        // It is important to place this JavaScript code after ScriptManager1
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function SavePnlGridScrollPosReader() {
            prm.add_beginRequest(BeginRequestHandlerReader);
            prm.add_endRequest(EndRequestHandlerReader);
        }

        function BeginRequestHandlerReader(sender, args) {
            if ($get('<%=pnlGrid.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=pnlGrid.ClientID%>').scrollLeft;
                yPos = $get('<%=pnlGrid.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandlerReader(sender, args) {
            if ($get('<%=pnlGrid.ClientID%>') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('<%=pnlGrid.ClientID%>').scrollLeft = xPos;
                $get('<%=pnlGrid.ClientID%>').scrollTop = yPos;
            }
        }

        function SavePnlGridScrollPosAntenna() {
            prm.add_beginRequest(BeginRequestHandlerAntenna);
            prm.add_endRequest(EndRequestHandlerAntenna);
        }

        function BeginRequestHandlerAntenna(sender, args) {
            if ($get('<%=pnlGridAntenna.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=pnlGridAntenna.ClientID%>').scrollLeft;
                yPos = $get('<%=pnlGridAntenna.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandlerAntenna(sender, args) {
            if ($get('<%=pnlGridAntenna.ClientID%>') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('<%=pnlGridAntenna.ClientID%>').scrollLeft = xPos;
                $get('<%=pnlGridAntenna.ClientID%>').scrollTop = yPos;
            }
        }    

    </script>
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="1" cellspacing="0"
                            width="100%">
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="RFID Reader Detail"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">
                                    <table cellpadding="1" width="100%">
                                        <tr>
                                            <td align="center" valign="middle" style="width: 20%">
                                                Account #
                                            </td>
                                            <td align="center" valign="middle" style="width: 20%">
                                                Shelf Name
                                            </td>
                                            <td align="center" valign="middle" style="width: 20%">
                                                Health Last Updated at
                                            </td>
                                            <td align="center" valign="middle" style="width: 20%">
                                                Reader IP
                                            </td>
                                            <td align="center" valign="middle" style="width: 20%">
                                                Total Antenna
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top" style="width: 20%">
                                                <asp:Label Text="Account Number" Font-Bold="true" runat="server" ID="lblAccountNumber" />
                                            </td>
                                            <td align="center" valign="top" style="width: 20%">
                                                <asp:Label Text="Shelf Name" Font-Bold="true" runat="server" ID="lblShelfName" />
                                                <asp:Label Text="Shelf Code" Font-Bold="true" runat="server" Visible="false" ID="lblShelfCode" />
                                            </td>
                                            <td align="center" valign="top" style="width: 20%">
                                                <asp:Label Text="Health Last Updated at" Font-Bold="true" runat="server" ID="lblHealthLastUpdatedOn" />
                                            </td>
                                            <td align="center" valign="top" style="width: 20%">
                                                <asp:Label Text="Reader IP" Font-Bold="true" runat="server" ID="lblReaderIP" />
                                            </td>
                                            <td align="center" valign="top" style="width: 20%">
                                                <asp:Label Text="Total Antenna" Font-Bold="true" runat="server" ID="lblTotalAntenna" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" colspan="2">
                                    <table cellpadding="1" width="100%">
                                        <tr style="height: 205px;">
                                            <td align="center" width="100%">
                                                <asp:Panel ID="pnlGrid" CssClass="pnlGrid" runat="server" Width="99%" ScrollBars="Auto"
                                                    Height="205px" BorderWidth="1px">
                                                    <asp:GridView ID="gdvReaderDeatil" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                        Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="gdvReaderDeatil_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="35%" ItemStyle-Width="35%" HeaderText="Reader Property"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReaderProperty" runat="server" Text='<%# Eval("PropertyDescription") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hndReaderPropertyId" runat="server" Value='<%# Eval("ReaderPropertyId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderText="Current Property Value"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPropertyValue" runat="server" Text='<%# Eval("PropertyValue") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="35%" ItemStyle-Width="35%" HeaderText="Modified Property Value"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtModifiedPropertyValue" Width="85.5%" runat="server" CssClass="ListBox"
                                                                        Visible="false" Height="10px" />
                                                                    <asp:DropDownList ID="ddlModifiedPropertyValue" runat="server" AutoPostBack="false"
                                                                        CssClass="ListBox" Width="90%" Visible="false">
                                                                    </asp:DropDownList>
                                                                    <asp:RadioButton ID="rdoTrue" runat="server" Text="True" GroupName="TrueFalse" Visible="False" />
                                                                    <asp:RadioButton ID="rdofalse" runat="server" Text="False" GroupName="TrueFalse"
                                                                        Visible="False" />
                                                                    <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtModifiedPropertyValue">
                                                                    </ajaxtk:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderText="Info"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgInformation" runat="server" ImageUrl="~/Images/Information_blue.png"
                                                                        BorderStyle="None" ToolTip="" AlternateText="Connected" Visible="false" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label1" runat="server" Style="font-size: 12pt;" Text="Reader Antenna Detail"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server" Style="font-size: 10pt;" Text="Select Antenna :"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAntenna" runat="server" AutoPostBack="True" CssClass="ListBox"
                                                                OnSelectedIndexChanged="ddlAntenna_SelectedIndexChanged" Width="150px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;&nbsp;
                                                            <asp:Image ID="imgAntennaStatus" runat="server" ImageUrl="~/Images/LedGreen.png"
                                                                BorderStyle="None" ToolTip="Connected" AlternateText="Connected" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 210px;">
                                            <td align="center" width="100%">
                                                <asp:Panel ID="pnlGridAntenna" Height="210px" CssClass="pnlGrid" runat="server" Width="99%"
                                                    ScrollBars="Auto" BorderWidth="1px">
                                                    <asp:GridView ID="grdAntennaDetails" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                        Width="100%" ShowHeaderWhenEmpty="true" OnRowDataBound="grdAntennaDetails_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="35%" ItemStyle-Width="35%" HeaderText="Antenna Property"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAntennaProperty" runat="server" Text='<%# Eval("PropertyDescriptionAntenna") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hndAntennaPropertyId" runat="server" Value='<%# Eval("AntennaPropertyId") %>' />
                                                                    <asp:HiddenField ID="hndCustomerShelfAntennaId" runat="server" Value='<%# Eval("CustomerShelfAntennaId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="30%" ItemStyle-Width="30%" HeaderText="Property Value"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAntennaPropertyValue" runat="server" Text='<%# Eval("PropertyValueAntenna") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtAntennaPropertyValue" MaxLength="50" Width="90%" runat="server"
                                                                        Text='<%#Eval("PropertyValueAntenna") %>' CssClass="textbox" Visible="false" />
                                                                    <asp:DropDownList ID="ddlAntennaPropertyValue" runat="server" AutoPostBack="True"
                                                                        CssClass="ListBox" Width="90%" Visible="false">
                                                                    </asp:DropDownList>
                                                                    <asp:RadioButton ID="rdoAntennaTrue" runat="server" Text="True" GroupName="TrueFalse"
                                                                        Visible="False" />
                                                                    <asp:RadioButton ID="rdoAntennafalse" runat="server" Text="False" GroupName="TrueFalse"
                                                                        Visible="False" />
                                                                    <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtenderAntenna" runat="server"
                                                                        Enabled="True" TargetControlID="txtAntennaPropertyValue">
                                                                    </ajaxtk:FilteredTextBoxExtender>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="35%" ItemStyle-Width="35%" HeaderText="Modified Property Value"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtModifiedPropertyValueAntenna" Width="85.5%" runat="server" CssClass="ListBox"
                                                                        Visible="false" Height="10px" />
                                                                    <asp:DropDownList ID="ddlModifiedPropertyValueAntenna" runat="server" AutoPostBack="false"
                                                                        CssClass="ListBox" Width="90%" Visible="false">
                                                                    </asp:DropDownList>
                                                                    <asp:RadioButton ID="rdoTrueAntenna" runat="server" Text="True" GroupName="TrueFalse"
                                                                        Visible="False" />
                                                                    <asp:RadioButton ID="rdofalseAntenna" runat="server" Text="False" GroupName="TrueFalse"
                                                                        Visible="False" />
                                                                    <ajaxtk:FilteredTextBoxExtender ID="txtFilteredTextBoxExtenderAntenna" runat="server"
                                                                        Enabled="True" TargetControlID="txtModifiedPropertyValueAntenna">
                                                                    </ajaxtk:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" HeaderText="Info"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgInformationAntenna" runat="server" ImageUrl="~/Images/Information_blue.png"
                                                                        BorderStyle="None" ToolTip="" AlternateText="Connected" Visible="false" />
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
                                <td colspan="2" align="left">
                                    <asp:Panel ID="pnlButton" CssClass="ActionPanel" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="left" width="50%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="ErrorText"></asp:Label>
                                                    <asp:Label ID="lblCustomerShelfId" runat="server" Text="CustomerShelfId" Visible="False" />
                                                </td>
                                                <td align="right" width="50%">
                                                    <asp:Panel ID="pnlButtonOnly" runat="server" Width="100%" Height="100%">
                                                        <asp:Button ID="btnReStart" runat="server" OnClick="btnReStart_Click" CssClass="rebootbutton"
                                                            ToolTip="Reboot RFID Reader" OnClientClick="return confirm('Are you sure you want to Re-boot RFID reader ?');" />
                                                        <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="" CssClass="resetbutton"
                                                            CausesValidation="False" ToolTip="Refresh" />
                                                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="" CssClass="savebutton"
                                                            ToolTip="Save Reader details" />
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
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
