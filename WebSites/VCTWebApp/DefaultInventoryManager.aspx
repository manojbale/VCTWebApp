<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultInventoryManager.aspx.cs"
    Inherits="VCTWebApp.DefaultInventoryManager" MasterPageFile="~/Site1.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <%--<link rel="stylesheet" type="text/css" href="Scripts/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="js/jquery.jscrollpane.css" media="all" />
    <link rel="stylesheet" type="text/css" href="Style/VCTWeb.css" />
    <script src="js/VCTAutoComplete.js" type="text/javascript"></script>
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>    
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCQI4FVP0mjvsxydQJIgyhJdPWDfF-3UJM&sensor=true"
        type="text/javascript"></script>    --%>
    <script language="javascript" type="text/javascript">
        var map;
        var branchData;
        var casesData;
        var log;
        var lat;

        function GetData() {
            branchData = document.getElementById("hdnBranchData").value;
            casesData = document.getElementById("hdnCasesData").value;

            log = $('#hdnLongitude').val();
            lat = $('#hdnLatitude').val();

            if (log == 0 || lat == 0) {
                log = "260.644";
                lat = "42.397";
            }

        }

        function initialize() {

            var mapOptions = {
                center: new google.maps.LatLng(lat, log),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);

            createMarkerwithData(branchData, casesData);
        }

        function createMarkerwithData(branchinfo, Casesinfo) {

            try {
                if (branchinfo == "null") {
                    alert("Branch details not available.");
                }
                else {
                    var branchinfo = jQuery.parseJSON(branchinfo);

                    var partyId = 0;
                    var tblcase = "";
                    var branchIndex = 0;
                    var index = 0;
                    var flag = false;

                    tblcase = "";
                    branchIndex = 0;

                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(branchinfo.Latitude, branchinfo.Longitude),
                        map: map,
                        title: branchinfo.title
                    });

                    marker.setIcon("http://www.google.com/intl/en_us/mapfiles/ms/micons/blue-dot.png");

                    showInfoWindow("Branch", marker, branchinfo, branchIndex, tblcase);

                }

                if (Casesinfo != "null") {
                    var Casesinfo = jQuery.parseJSON(Casesinfo);

                    for (var i in Casesinfo) {
                        flag = true;
                        if (Casesinfo[index].PartyId != partyId) {

                            if (partyId != 0) {
                                showInfoWindow("Transfer", marker, Casesinfo, index - 1, tblcase);
                            }
                            tblcase = "";
                            partyId = Casesinfo[index].PartyId;

                            tblcase += "<tr class='map-content-header'><td>Shipping Date</td><td>Case Number</td><td>Case Type</td><td>Inventory Type</td><td>Case Status</td></tr>";
                            //alert(Casesinfo[index].ShippingDate);
                            tblcase += "<tr><td>" + Casesinfo[index].ShippingDate + "</td><td>" + Casesinfo[index].CaseNumber + "</td><td>" + Casesinfo[index].CaseType + "</td><td>"
                                    + Casesinfo[index].InventoryType + "</td><td>" + Casesinfo[index].CaseStatus + "</td></tr>";

                            var marker = new google.maps.Marker({
                                position: new google.maps.LatLng(Casesinfo[index].Latitude, Casesinfo[index].Longitude),
                                map: map,
                                title: Casesinfo[index].title
                            });
                            marker.setIcon("http://www.google.com/intl/en_us/mapfiles/ms/micons/green-dot.png");

                        }
                        else {
                           // alert(Casesinfo[index].ShippingDate);
                            tblcase += "<tr><td>" + Casesinfo[index].ShippingDate + "</td><td>" + Casesinfo[index].CaseNumber + "</td><td>" + Casesinfo[index].CaseType + "</td><td>"
                                    + Casesinfo[index].InventoryType + "</td><td>" + Casesinfo[index].CaseStatus + "</td></tr>";
                        }

                        index += 1;
                    }
                    if (flag == true) {
                        if (index > 0)
                            index = index - 1;
                        else
                            index = 0;

                        showInfoWindow("Transfer", marker, Casesinfo, index, tblcase);
                    }
                }
            } catch (e) {


            }

        }

        function showInfoWindow(recordType, marker, info, number, tblcase) {
            var msg = '';
           // alert(tblcase);
            if (recordType == "Branch") {
                msg += "<div class='info1' style='width:350px'>";
                msg += "<h3>" + info.BranchName + "</h3>";
                msg += "<label><b>Address1: </b>" + info.Address1 + " </label> <br />";
                msg += "<label><b>Address2: </b>" + info.Address2 + " </label> <br />";
                msg += "</div>";
            }
            else {
                msg += "<div class='info1'  style='height:300px; width:500px; overflow:auto;'>";
                msg += "<h3>" + info[number].HospitalName + "</h3>";
                msg += "<label><b>Address1: </b>" + info[number].Address1 + "</label> <br />";
                msg += "<label><b>Address2: </b>" + info[number].Address2 + "</label> <br />";
                msg += "<table border='1' width='100%'  cellpadding='5' cellspacing='0' bordercolor='gray'>";
                msg += tblcase;
                msg += "</table>";
                msg += "</div>";
            }

            var infowindow = new google.maps.InfoWindow({
                content: msg,
                size: new google.maps.Size(50, 50)
            });

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });

        }

        google.maps.event.addDomListener(window, 'load', initialize);
                
    </script>
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
            overflow: hidden;
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
        $(document).ready(function () {

            $(".tabContainer").not(":first").hide();

            $("ul.tabs li:first").addClass("active").show();

            $('#headtab1, #headtab2, #headtab3, #headtab4, #headtab5, #headtab6').click(function () {
                $("ul.tabs li.active").removeClass("active");
                $('.home-tab-container').fadeOut('slow');
                var aid = $(this).attr('id');
                var tabid = $('#' + aid + ' a').attr('href');
                $(tabid).fadeIn('slow');
                $(this).addClass("active");
            });
        });    
    </script>
    <script type="text/javascript">
        //          $(function () {
        //              $(window).load(function () {
        //                  fixedGrid();
        //              });

        //              function fixedGrid() {
        //                  InitGridEvent('<%= gvPartOrderList.ClientID %>');
        //                  InitGridEvent('<%= gdvPendingBuildKit.ClientID %>');
        //              }

        //          });


        function pageLoad() {
            InitGridEvent('<%= gvPartOrderList.ClientID %>');
            //InitGridEvent('<%= gdvPendingBuildKit.ClientID %>');
        }

        Sys.Application.add_load(function () {
            $('#gdvPendingBuildKit').Scrollable({
                ScrollHeight: 300
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
                                            <li id="headtab1"><a href="#tab1">Orders-Today</a></li>
                                               <li id="headtab2"><a href="#tab2">Aging Orders-Pick Up</a></li>
                                            <li id="headtab3"><a href="#tab3">Active Parts</a></li>
                                                 <li id="headtab4"><a href="#tab4">Active Kits</a></li>
                                                   <li id="headtab5"><a href="#tab5">Kit Family</a></li>
                                         
                                            <li id="headtab6"><a href="#tab6">Pending Build Kits</a></li>
                                       
                                          
                                        </ul>
                                        <div class="tabContainer">
                                            <div id="tab1" class="home-tab-container">
                                                <div id="map-canvas" style="float: left; width: 985px; height: 575px; margin: 160px 0 0 3px;" />
                                                <asp:HiddenField ID="hdnBranchData" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdnCasesData" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdnLongitude" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdnLatitude" runat="server" ClientIDMode="Static" />
                                                <script language="javascript" type="text/javascript">                                                    GetData(); </script>
                                            </div>
                                        </div>
                                             <div id="tab3" style="display: none;" class="home-tab-container">
                                            <br />
                                            <table align="center" border="0" width="100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblPartOrder" runat="server" CssClass="SectionHeaderText" Text="Part Report with High # of Order Last 90 Days"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <center>
                                            <asp:Panel ID="Panel1" runat="server" Height="530px" ScrollBars="Auto" Width="980">
                                                
                                                    <asp:GridView ID="gvPartOrderList" runat="server" AutoGenerateColumns="false" 
                                                        SkinID="GridView" EmptyDataText="No Record Found" Width="960" HeaderStyle-Height="25">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="250"  ItemStyle-Width="250" ItemStyle-HorizontalAlign="Center" >
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblPartNumberHeader" runat="server" Text=""></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPartNumber" runat="server" Text='<%# Eval("PartNum") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="500" ItemStyle-Width="500">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblDescriptionHeader" runat="server" Text=""></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="210" ItemStyle-Width="210" >
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblTotalHeader" runat="server" Text=""></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Count") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                
                                            </asp:Panel>
                                            </center>
                                        </div>
                                               <div id="tab4" style="display: none;" class="home-tab-container">
                                            <br />
                                            <table align="center" border="0" width="100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblKitOrder" runat="server" CssClass="SectionHeaderText" Text="Kit Report with High # of Order Last 90 Days"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <center>
                                            <asp:Panel ID="Panel3" runat="server" Height="530px" ScrollBars="Auto" Width="980">
                                                
                                                    <asp:GridView ID="gvKitOrderList" runat="server" AutoGenerateColumns="false" 
                                                        SkinID="GridView" EmptyDataText="No Record Found" Width="960" HeaderStyle-Height="25">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="250"  ItemStyle-Width="250" ItemStyle-HorizontalAlign="Center">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblKitFamilyHeader" runat="server" Text=""></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblKitFamily" runat="server" Text='<%# Eval("KitFamilyName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="500" ItemStyle-Width="500">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblDescriptionHeader" runat="server" Text=""></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("KitFamilyDescription") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="210" ItemStyle-Width="210" >
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblNumberOfOrdersHeader" runat="server" Text=""></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNumberOfOrders" runat="server" Text='<%# Eval("OrderCount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                
                                            </asp:Panel>
                                            </center>
                                        </div>
                                          <div id="tab5" style="display: none;" class="home-tab-container">
                                             <iframe src="LocationsKitFamily.aspx" width="100%" height="575px" scrolling="no" frameborder="0">
                                            </iframe>
                                        </div>
                                        <div id="tab2" style="display: none;" class="home-tab-container">
                                            <%--<iframe src="AgingOrders.aspx" width="100%" height="575px" frameBorder="0">
                                            </iframe>--%>
                                            <iframe src="AgingOrders.aspx" width="100%" height="575px" scrolling="no" frameborder="0">
                                            </iframe>
                                        </div>
                                        <div id="tab6" style="display: none;" class="home-tab-container">
                                            <br />
                                            <table align="center" border="0" width="100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblPendingBuildKitHeader" runat="server" CssClass="SectionHeaderText"
                                                            Text="Pending Build Kits"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <center>
                                            <asp:Panel ID="Panel2" runat="server" Height="530px" ScrollBars="Auto" Width="980">
                                             <table align="center" border="0" width="100%">
                                                <tr>
                                                    <td align="center">
                                                      <asp:GridView ID="gdvPendingBuildKit" runat="server" AutoGenerateColumns="false"
                                                         SkinID="GridView" EmptyDataText="No Record Found" OnRowCommand="gdvPendingBuildKit_RowCommand" 
                                                         Width="960" HeaderStyle-Height="25">
                                                        <Columns>
                                                            <asp:BoundField DataField="KitNumber" HeaderStyle-Width="200" ItemStyle-Width="200" HeaderText="Kit #"
                                                                ItemStyle-HorizontalAlign="Center"  />
                                                            <asp:BoundField DataField="KitDescription" HeaderText="Description" HeaderStyle-Width="300" ItemStyle-Width="300" />
                                                            <asp:TemplateField HeaderStyle-Width="200" ItemStyle-Width="200" HeaderText="Last Build On" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLastBuildOn" runat="server" Text='<%# Convert.ToDateTime(Eval("LastBuildOn"),System.Globalization.CultureInfo.CurrentCulture).ToShortDateString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="KitFamily" HeaderStyle-Width="200" ItemStyle-Width="200" HeaderText="Kit Family"
                                                                ItemStyle-HorizontalAlign="Center"/>
                                                            <asp:TemplateField HeaderStyle-Width="100" ItemStyle-Width="100" HeaderText="Build" ItemStyle-HorizontalAlign="Center" >
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkBuild" CausesValidation="false" CommandName="Build" runat="server"
                                                                        CommandArgument='<%# Eval("KitNumber")+","+Eval("KitDescription") %>'>
                                                                        <asp:Image ID="imgBuild" runat="server" ImageUrl="~/Images/element_next.png" BorderStyle="None"
                                                                            ToolTip="Edit" AlternateText="Edit" />
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    </td>
                                                    </tr>
                                                  
                                                </table>
                                            </asp:Panel>
                                            </center>
                                        </div>
                                 
                                      
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
