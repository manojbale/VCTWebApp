<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VCTWebApp.Shell.Views.Default"
    Title="Default" MasterPageFile="~/Site1.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">  
<%--<script src="http://code.jquery.com/jquery-1.9.1.js"></script> 
  <script>

      $k = $.noConflict();
      $k(function () {
          $k("#content").load("DefaultInventoryManager.aspx");          
      }); 
</script> 
   --%>
    <%--<table class="maintable" align="center" border="0">
        <tr class="header">
            <td align="center">
                <asp:Label ID="lblHeader" runat="server" Text="Home"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <table border="0" align="center" cellpadding="3" cellspacing="0" width="100%" height="620px">
                    <tr>
                        <td valign="top">
                             <span id="content" width="400px" height="620px"></span>                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>--%>
</asp:Content>
