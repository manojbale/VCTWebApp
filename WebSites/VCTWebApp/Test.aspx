<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true"
    CodeBehind="Test.aspx.cs" Inherits="VCTWebApp.Test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <style type="text/css">
        .table-row
        {
            border:1px solid red;
            height:30px;
            }
        .grid-row1, .grid-alt-row1
        {
            height:30px;
            }
        .pnl table tr
        {
            height:28px;
            }
        .btn1
        {
            background-color:Green;            
            height:20px;
            border:0px solid;            
            }
            
        .btn2
        {
            background-color:orange;
            height:20px;
            border:0px solid;
            }
          
          .btn3
        {
            background-color:Red;
            height:20px;
            border:0px solid;
            }
            
        .btn4
        {
            background-color:Gray;
            height:20px;
            border:0px solid;
            }
            
        .btn5
        {
            background-color:blue;
            height:20px;
            border:0px solid;
            }
                
        .btn6
        {
            background-color:pink;
            height:20px;
            border:0px solid;
    
            }   
         
         .table-color tr td
         {
             text-align:left;
             vertical-align:middle;            
             }      
      
    </style>
    <script>
        $(function () {
            $('input[type:submit]').click(function(){
                return false;
            });
            });
        });
    </script>
    <table align="left" border="0" width="100%">
        <tr>
            <td align="center">
                <table class="maintable" border="0" align="center" cellpadding="3" cellspacing="0" height="660px">
                    <tr class="header">
                        <td align="center">
                            <asp:Label ID="lblHeader" runat="server" Text="View Transaction"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td  height="70px" align="center" valign="top">
                            <br />                            
                            <table border="0" align="center" cellspacing="0" cellpadding="0" style="border:1px solid #51749e; padding:10px 15px;">
                                <tr class="table-row">
                                    <%--<td>
                                       Case Type
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList1" runat="server" Width="200px">
                                            <asp:ListItem >InventoryTransfer</asp:ListItem>
                                            <asp:ListItem >NewProductTransfer</asp:ListItem>
                                            <asp:ListItem >ReplenishmentTransfer</asp:ListItem>
                                            <asp:ListItem >ReturnInventoryRMATransfer</asp:ListItem>
                                            <asp:ListItem >RoutineCase</asp:ListItem>
                                            <asp:ListItem >InternalRequest</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>--%>
                                    <td>
                                        Start Date
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server" Width="100" CssClass="TextBoxCase"
                                            Text="04/01/2014" ClientIDMode="Static" Enabled="false" />
                                        <asp:Image ID="Image2" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif" />
                                        <ajaxtk:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgCalenderFrom"
                                            TargetControlID="TextBox1">
                                        </ajaxtk:CalendarExtender>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        End Date
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" Width="100" CssClass="TextBoxCase" 
                                            ClientIDMode="Static" Enabled="false" Text="06/30/2014"/>
                                        <asp:Image ID="Image3" runat="server" Height="15" ImageUrl="~/Images/calbtn.gif" />
                                        <ajaxtk:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="Image1"
                                            TargetControlID="TextBox2">
                                        </ajaxtk:CalendarExtender>
                                    </td>
                                </tr>                                                         
                            </table>                           
                        </td>
                    </tr>                    
                    <tr>
                        <td align="center" valign="top">
                            <table width="95%" align="center" border="0">
                                <tr>
                                    <td align="center">
                                        <br />
                                        <asp:Label ID="lblSummaryHeader" runat="server" Text="Cases Detail" CssClass="SectionHeaderText"></asp:Label>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <%--<table border="0" width="99%" class="table-color">
                                            <tr>
                                                <td width="20px">
                                                  <asp:ImageButton ID="btn1" runat="server" CssClass="btn1" ImageUrl="~/Images/step1.png" /> 
                                                </td>
                                                <td width="80px">
                                                  New
                                                </td>
                                                <td width="20px">
                                                  <asp:ImageButton ID="btn2" runat="server" CssClass="btn2" ImageUrl="~/Images/step2.png" />
                                                </td>
                                                <td width="130px">
                                                Inventory Assigned
                                                </td>
                                                <td width="20px">
                                                    <asp:ImageButton ID="btn3" runat="server" CssClass="btn3" ImageUrl="~/Images/step3.png" /> </td>
                                                <td> Cancelled
                                                </td>
                                                <td width="20px">
                                                    <asp:ImageButton ID="btn4" runat="server" CssClass="btn4" ImageUrl="~/Images/step4.png" /></td>
                                                <td> Shipped
                                                </td>
                                                <td width="20px">
                                                    <asp:ImageButton ID="btn5" runat="server" CssClass="btn5" ImageUrl="~/Images/step5.png" /> </td>
                                                <td>Delivered
                                                </td>
                                                <td width="20px">
                                                    <asp:ImageButton ID="btn6" runat="server" CssClass="btn6" ImageUrl="~/Images/step6.png" /> </td>
                                                <td>Received
                                                </td>
                                            </tr>
                                        </table>                                                                               --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnl" runat="server" height="470px" ScrollBars="auto" CssClass="pnl">
                                            <asp:GridView ID="GridView1" runat="server" SkinID="GridView" onrowdatabound="GridView1_RowDataBound" 
                                             AutoGenerateColumns="false"   >
                                                <Columns>                                              
                                                     <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" >
                                                        <HeaderTemplate>
                                                            Case Type
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:DropDownList runat="server" Width="95%">
                                                                <asp:ListItem Text="All" />
                                                                <asp:ListItem Text="RoutineCase" />
                                                                <asp:ListItem Text="ReplenishmentTransfer" />
                                                            </asp:DropDownList>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("CaseType") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" >
                                                        <HeaderTemplate>
                                                            Case #
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:TextBox ID="txt2" runat="server" Width="90%"></asp:TextBox>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("Case#") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" >
                                                        <HeaderTemplate>
                                                            Inv Type
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:DropDownList ID="ddlInvType" runat="server" Width="95%">
                                                                <asp:ListItem >All</asp:ListItem>
                                                                <asp:ListItem >Kit</asp:ListItem>
                                                                <asp:ListItem >Part</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("InvType")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                      <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" >
                                                        <HeaderTemplate>
                                                           Surgery Date                                                            
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("SurgeryDate")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Left" >
                                                        <HeaderTemplate>
                                                           Ship To Location
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:TextBox ID="txt4" runat="server" Width="95%"></asp:TextBox>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("ShipToLocation")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center" >
                                                        <HeaderTemplate>
                                                           Location Type
                                                            <br />
                                                            <br />
                                                              <asp:DropDownList ID="ddlLocType" runat="server" width="95%">
                                                                <asp:ListItem >All</asp:ListItem>
                                                                <asp:ListItem >Hospital</asp:ListItem>                                                                
                                                            </asp:DropDownList>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%#Eval("LocationType")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-HorizontalAlign="Left" >
                                                        <HeaderTemplate>
                                                           Status
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:DropDownList ID="ddl" runat="server" width="95%">
                                                                <asp:ListItem >All</asp:ListItem>
                                                                <asp:ListItem >New</asp:ListItem>                                                                
                                                                <asp:ListItem >Inventory Assigned</asp:ListItem>
                                                                <asp:ListItem >Cancelled</asp:ListItem>                                                         
                                                                <asp:ListItem >Shipped</asp:ListItem>
                                                                <asp:ListItem >Delivered</asp:ListItem>                                                                
                                                                <asp:ListItem >Received</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>                                                                                                                        
                                                            <asp:ImageButton ID="ImgStatus" runat="server" CommandArgument='<%#Eval("Status")%>' />
                                                            <br />
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                  
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        <br />
                    </td>
                </tr>
                </table>
            </td>
        </tr>        
    </table>
</asp:Content>
