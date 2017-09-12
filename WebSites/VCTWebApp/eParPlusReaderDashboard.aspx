<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eParPlusReaderDashboard.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.eParPlusReaderDashboard" Title="Default" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <table align="left" border="0" width="100%">
                <tr>
                    <td align="center">
                        <table class="maintable" border="0" align="center" cellpadding="0" cellspacing="0"
                            width="100%" style="height: 560px;" >
                            <tr class="header">
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblHeader" runat="server" Text="RFID Reader Management"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">                                    
                                    <div id="divDashboard" runat="server" style="height: 560px; width: 100%;">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
