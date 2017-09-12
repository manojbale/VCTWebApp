<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocationsKitFamily.aspx.cs" Inherits="VCTWebApp.LocationsKitFamily" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            branchData = document.getElementById("hdnSelfLocationKitFamily").value;
            casesData = document.getElementById("hdnChildLocationKitFamily").value;

            log = $('#hdnLongitude').val();
            lat = $('#hdnLatitude').val();

            if (log == 0 || log == '') {
                log = "260.644";
                lat = "43.397";
            }

        }

        function initialize() {

            var mapOptions = {
                center: new google.maps.LatLng(lat, log),
                zoom: 4,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);

            createMarkerwithData(branchData, casesData);
        }

        function createMarkerwithData(parentinfo, childinfo) {

            try {
                var LocationId = 0;                
                var tblcase = "";
                var branchIndex = 0;
                var index = 0;
                var flag = false;

                if (parentinfo == "null") {
                    alert("Parent Location details not available.");
                }
                else {
                    var ParentList = jQuery.parseJSON(parentinfo);
                    
                    tblcase = "";
                    index = 0;

                    tblcase += "<tr class='map-content-header'>";
                    tblcase += "    <td>Kit Family</td>";
                    tblcase += "    <td>Description</td>";
                    tblcase += "    <td>Available</td>";
                    tblcase += "    <td>Assigned To Case</td>";
                    tblcase += "    <td>Shipped</td>";
                    tblcase += "    <td>Received</td>";
                    tblcase += " </tr>";

                    for (var i in ParentList) {

                        tblcase += "<tr>";
                        tblcase += "    <td>" + ParentList[index].KitFamilyName + "</td>";
                        tblcase += "    <td>" + ParentList[index].KitFamilyDescription + "</td>";
                        tblcase += "    <td align='center'>" + ParentList[index].AvailableQuantity + "</td>";
                        tblcase += "    <td align='center'>" + ParentList[index].AssignedToCaseQuantity + "</td>";
                        tblcase += "    <td align='center'>" + ParentList[index].ShippedQuantity + "</td>";
                        tblcase += "    <td align='center'>" + ParentList[index].ReceivedQuantity + "</td>";
                        tblcase += "</tr>";

                        index += 1;
                    }

                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(ParentList[0].Latitude, ParentList[0].Longitude),
                        map: map,
                        title: ""
                    });

                    marker.setIcon("http://www.google.com/intl/en_us/mapfiles/ms/micons/blue-dot.png");

                    showInfoWindow("Parent", marker, ParentList, 0, tblcase);

                }
                
                index = 0;
                if (childinfo != "null") {
                    var Childinfo = jQuery.parseJSON(childinfo);

                    for (var i in Childinfo) {
                        flag = true;
                        if (Childinfo[index].LocationId != LocationId) {

                            if (LocationId != 0) {
                                showInfoWindow("Transfer", marker, Childinfo, index - 1, tblcase);
                            }
                            tblcase = "";
                            LocationId = Childinfo[index].LocationId;

                            tblcase += "<tr class='map-content-header'>";
                            tblcase += "    <td>Kit Family</td>";
                            tblcase += "    <td>Description</td>";
                            tblcase += "    <td>Available</td>";
                            tblcase += "    <td>Assigned To Case</td>";
                            tblcase += "    <td>Shipped</td>";
                            tblcase += "    <td>Received</td>";
                            tblcase += " </tr>";

                            if (Childinfo[index].KitFamilyName == "" || Childinfo[index].KitFamilyName == null) { 
                            
                            }
                            else {
                                tblcase += "<tr>";
                                tblcase += "    <td>" + Childinfo[index].KitFamilyName + "</td>";
                                tblcase += "    <td>" + Childinfo[index].KitFamilyDescription + "</td>";
                                tblcase += "    <td align='center'>" + Childinfo[index].AvailableQuantity + "</td>";
                                tblcase += "    <td align='center'>" + Childinfo[index].AssignedToCaseQuantity + "</td>";
                                tblcase += "    <td align='center'>" + Childinfo[index].ShippedQuantity + "</td>";
                                tblcase += "    <td align='center'>" + Childinfo[index].ReceivedQuantity + "</td>";
                                tblcase += "</tr>";
                            }

                            var marker = new google.maps.Marker({
                                position: new google.maps.LatLng(Childinfo[index].Latitude, Childinfo[index].Longitude),
                                map: map,
                                title: Childinfo[index].title
                            });
                            marker.setIcon("http://www.google.com/intl/en_us/mapfiles/ms/micons/green-dot.png");

                        }
                        else {

                            tblcase += "<tr>";
                            tblcase += "    <td>" + Childinfo[index].KitFamilyName + "</td>";
                            tblcase += "    <td>" + Childinfo[index].KitFamilyDescription + "</td>";
                            tblcase += "    <td align='center'>" + Childinfo[index].AvailableQuantity + "</td>";
                            tblcase += "    <td align='center'>" + Childinfo[index].AssignedToCaseQuantity + "</td>";
                            tblcase += "    <td align='center'>" + Childinfo[index].ShippedQuantity + "</td>";
                            tblcase += "    <td align='center'>" + Childinfo[index].ReceivedQuantity + "</td>";
                            tblcase += "</tr>";

                        }

                        index += 1;
                    }
                    if (flag == true) {
                        if (index > 0)
                            index = index - 1;
                        else
                            index = 0;

                        showInfoWindow("Transfer", marker, Childinfo, index, tblcase);
                    }
                }
            } catch (e) {

                //alert("exception caught in createMarkerwithData " + e.Message);

            }

        }

        function showInfoWindow(recordType, marker, info, number, tblcase) {
            var msg = '';

                msg += "<div class='info1' style='width:600px; height:350px;'>";
                msg += "<h3>" + info[number].LocationName + "</h3>";
                msg += "<table border='1' width='100%'  cellpadding='5' cellspacing='0' bordercolor='gray'>";
                msg += tblcase;
                msg += "</table>";
                msg += "</div>";

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
     <asp:HiddenField ID="hdnSelfLocationKitFamily" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnChildLocationKitFamily" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLongitude" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnLatitude" runat="server" ClientIDMode="Static" />
    <script language="javascript" type="text/javascript">        GetData(); </script>
    </form>
</body>
</html>
