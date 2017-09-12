<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultSalesPerson.aspx.cs"
    Inherits="VCTWebApp.DefaultSalesPerson" MasterPageFile="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <style type="text/css">
        ul.tabs
        {
            float: left;
            list-style: none;
            height: 32px;
            width: 100%;
            border-radius: 8px 0 -50px 0;
            margin: 0;
            padding: 0;
            padding-top: 2px;
            padding-left: 3px;
        }
        
        ul.tabs li
        {
            float: left;
            height: 31px;
            line-height: 31px;
            border: 1px solid #999;
            overflow: hidden;
            position: relative;
            background: #e0e0e0;
            -webkit-border-top-left-radius: 8px;
            -webkit-border-top-right-radius: 8px;
            -moz-border-radius-topleft: 8px;
            -moz-border-radius-topright: 8px;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            margin: 0 5px -1px 0;
            padding: 0;
        }
        
        ul.tabs li a
        {
            text-decoration: none;
            color: #000;
            display: block;
            font-size: 1.2em;
            border: 1px solid #fff;
            outline: none;
            -webkit-border-top-left-radius: 8px;
            -webkit-border-top-right-radius: 8px;
            -moz-border-radius-topleft: 8px;
            -moz-border-radius-topright: 8px;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            padding: 0 20px;
        }
        
        ul.tabs li a:hover
        {
            background: #ccc;
        }
        
        html ul.tabs li.active, html ul.tabs li.active a:hover
        {
            background: #fff;
            border-bottom: 1px solid #fff;
        }
        
        .tabContainer
        {
            border: 1px solid #999;
            overflow:auto;
            clear: both;
            float: left;
            background: #fff;
            -webkit-border-radius: 8px;
            -webkit-border-top-left-radius: 0;
            -moz-border-radius: 8px;
            -moz-border-radius-topleft: 0;
            border-radius: 8px;
            border-top-left-radius: 0;
            margin-left: 3px;
            width: 990px;
            height: 580px;
        }
        
        .tabContent
        {
            font-size: 12px;
            padding: 20px;
        }
    </style>
    <script type="text/javascript">
        $j = $.noConflict();

        $j(function () {

            $j(".tabContainer").not(":first").hide();

            $j("ul.tabs li:first").addClass("active").show();

            $j('#headtab1').click(function () {
                $j("ul.tabs li.active").removeClass("active");
                $j('.home-tab-container').fadeOut('slow');
                var aid = $j(this).attr('id');
                var tabid = $j('#' + aid + ' a').attr('href');
                $j(tabid).fadeIn('slow');
                $j(this).addClass("active");
            });

            $j(".ExpandRow").live("click", function () {

                if ($j(this).attr("src").toLowerCase() == "images/plus.png") {
                    $j(this).next().show();
                    $j(this).closest("tr").after("<td style='width:5%'></td><td colspan = '999'>" + $j(this).next().html() + "</td>");
                    $j(this).next().hide();
                    $j(this).attr("src", "images/minus.png");
                }
                else {
                    $j(this).attr("src", "images/plus.png");
                    $j(this).closest("tr").next().next().hide();
                    $j(this).closest("tr").next().hide();
                }
            });

        });       
        
    </script>
    <table class="maintable" align="center" border="0">
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
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td valign="top">
                                        <ul class="tabs">
                                            <li id="headtab1"><a href="#tab1">Case List</a></li>
                                            <%--<li id="headtab2"><a href="#tab2">Active Parts</a></li>--%>
                                        </ul>
                                        <div class="tabContainer">
                                            <div id="tab1" class="home-tab-container">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" align="center">
                                                    <tr>
                                                        <td align="center" style="padding: 10px 0;">
                                                            <asp:Label ID="lblPartOrder" runat="server" CssClass="SectionHeaderText" Text="Pending Cases Report"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <asp:GridView ID="gdvCases" runat="server" AutoGenerateColumns="False" SkinID="GridView"
                                                                ShowHeaderWhenEmpty="true" Width="98%" OnRowDataBound="gdvCases_RowDataBound"
                                                                EmptyDataText="No Record Found">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderStyle Width="30" />
                                                                        <ItemStyle Width="30" />
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnCaseId" runat="server" Value='<%#Eval("CaseId") %>' />
                                                                            <asp:HiddenField ID="hdnPartyId" runat="server" Value='<%#Eval("PartyId") %>' />
                                                                            <asp:HiddenField ID="hdnInventoryType" runat="server" Value='<%#Eval("InventoryType") %>' />
                                                                            <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%#Eval("LocationId") %>' />
                                                                            <asp:HiddenField ID="hdnDispositionType" runat="server" Value='<%#Eval("DispositionType") %>' />
                                                                            <asp:HiddenField ID="hdnRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
                                                                            <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                                ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                            <asp:Panel ID="pnlChild" runat="server" Style="display: none">
                                                                                <asp:GridView ID="grdChild" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                    OnRowDataBound="grdChild_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Ref #" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                                                <%# Eval("PartNum")%>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField ItemStyle-Width="40%" DataField="Description" HeaderText="Description" />
                                                                                        <asp:BoundField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                                                            HeaderText="Lot #" />
                                                                                        <asp:BoundField ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate"
                                                                                            HeaderText="Expiry Date" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                                <asp:GridView ID="grdChildKit" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                    OnRowDataBound="grdChildKit_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderStyle Width="10%" />
                                                                                            <ItemStyle Width="10%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hdnBuildKitId" runat="server" Value='<%#Eval("BuildKitId") %>' />
                                                                                                <asp:Image ID="imgChildKit" runat="server" Style="cursor: pointer; vertical-align: top;"
                                                                                                    ImageUrl="~/Images/plus.PNG" CssClass="ExpandRow" />
                                                                                                <asp:Panel ID="pnlChildKit" runat="server" Style="display: none">
                                                                                                    <asp:GridView ID="grdChildKitDetail" runat="server" AutoGenerateColumns="false" SkinID="GridView"
                                                                                                        OnRowDataBound="grdChildKitDetail_RowDataBound">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Ref #" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:HiddenField ID="hdnIsNearExpiry" runat="server" Value='<%#Eval("IsNearExpiry") %>' />
                                                                                                                    <%#Eval("PartNum")%>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField ItemStyle-Width="300px" DataField="Description" HeaderText="Description" />
                                                                                                            <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="LotNum"
                                                                                                                HeaderText="Lot #" />
                                                                                                            <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="ExpiryDate"
                                                                                                                HeaderText="Expiry Date" DataFormatString="{0:d}" />
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </asp:Panel>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataField="KitFamilyName"
                                                                                            HeaderText="Kit Family" />
                                                                                        <asp:BoundField ItemStyle-Width="300px" DataField="Description" HeaderText="Description" />
                                                                                        <asp:BoundField ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" DataField="KitNumber"
                                                                                            HeaderText="Kit Number" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblCaseTypeHeader" runat="server" Text="Case Type"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <%#Eval("CaseType") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderStyle Width="50" />
                                                                        <ItemStyle Width="50" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblCaseNumberHeader" runat="server" Text="Case Number"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCaseNumber" runat="server" Text='<%# Eval("CaseNumber") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderStyle Width="50" />
                                                                        <ItemStyle Width="50" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblInventoryTypeHeader" runat="server" Text="Inventory Type"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInventoryType" runat="server" Text='<%# Eval("InventoryType") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderStyle Width="50" />
                                                                        <ItemStyle Width="50" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblSurgeryDateHeader" runat="server" Text="Surgery Date"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSurgeryDate" runat="server" Text='<%# Eval("SurgeryDate", "{0:d}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderStyle Width="150" />
                                                                        <ItemStyle Width="150" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblPartyNameHeader" runat="server" Text="Ship To Location"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPartyName" runat="server" Text='<%# Eval("PartyName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderStyle Width="50" />
                                                                        <ItemStyle Width="50" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblLocationTypeHeader" runat="server" Text="Location Type"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocationType" runat="server" Text='<%# Eval("LocationType") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                        <HeaderStyle Width="100" />
                                                                        <ItemStyle Width="100" HorizontalAlign="Left" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblCaseStatusHeader" runat="server" Text="Case Status"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            &nbsp; &nbsp;
                                                                            <asp:Label ID="lblCaseStatus" runat="server" Text='<%# Eval("CaseStatus") %>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnCaseStatus" runat="server" Value='<%#Eval("CaseStatus") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <%-- <div id="tab2" style="display: none;" class="home-tab-container">
                    <br />
                    Code here
                </div>   --%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
