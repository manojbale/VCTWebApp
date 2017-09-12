<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgingOrders.aspx.cs" Inherits="VCTWebApp.AgingOrders" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>    
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCQI4FVP0mjvsxydQJIgyhJdPWDfF-3UJM&sensor=true"
        type="text/javascript"></script>  
    <link rel="stylesheet" type="text/css" href="Style/VCTWeb.css" />

    <script language="javascript" type="text/javascript">
        var map;
        var branchData;
        var casesData;
        var log;
        var lat;

        function GetData() {
            branchData = document.getElementById("hdnAgingBranchData").value;
            casesData = document.getElementById("hdnAgingOrdersData").value;

            log = $('#hdnAgingLongitude').val();
            lat = $('#hdnAgingLatitude').val();
                        
            if (log == 0 || log == '') {
                log = "260.644";
                lat = "43.397";
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
                    var partyId = 0;
                    var tblcase = "";
                    var branchIndex = 0;
                    var index = 0;
                    var flag = false;

                if (branchinfo == "null") {
                    alert("Branch details not available.");
                }
                else {
                    var branchinfo = jQuery.parseJSON(branchinfo);

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

                            tblcase += "<tr class='map-content-header'><td>Surgery Date</td><td>Case Number</td><td>Case Type</td><td>Inventory Type</td><td>Case Status</td></tr>";
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

                //alert("exception caught in createMarkerwithData " + e.Message);

            }

        }

        function showInfoWindow(recordType, marker, info, number, tblcase) {
            var msg = '';

            if (recordType == "Branch") {
                msg += "<div class='info1' style='width:350px'>";
                msg += "<h3>" + info.BranchName + "</h3>";
                msg += "<label><b>Address1: </b>" + info.Address1 + "</label> <br />";
                msg += "<label><b>Address1: </b>" + info.Address2 + "</label> <br />";
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
</head>
<body>
    <form id="form1" runat="server">
    <div id="map-canvas" style="width:100%; height:575px; margin-top:-30px; padding-left:980px; "/>    
     <asp:HiddenField ID="hdnAgingOrdersData" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAgingBranchData" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAgingLongitude" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnAgingLatitude" runat="server" ClientIDMode="Static" />
    <script language="javascript" type="text/javascript"> GetData(); </script>
    </form>
</body>
</html>
