<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="LocateKit.aspx.cs"
    Inherits="VCTWebApp.Shell.Views.LocateKit" Title="Locate Inventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="server">
    <script language="javascript" type="text/javascript">
        var map;
        var kitData;

        function initialize() {
            var mapOptions = {
                center: new google.maps.LatLng(42.397, 260.644),
                zoom: 4,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);
            //createMarker();

            createMarkerwithData(kitData);

            
//            var flightPlanCoordinates1 = [
//                                            new google.maps.LatLng(48.53,-122.05),
//                                            new google.maps.LatLng(42.42,-124.33),
//                                            new google.maps.LatLng(34.35,-120.22),
//                                            new google.maps.LatLng(32.31,-117.04),
//                                            new google.maps.LatLng(49.04,-112.50),
//                                            new google.maps.LatLng(48.53,-122.05)
//                                          ];
//            var day1 = '#Ff0000';
//            DrawZones(flightPlanCoordinates1, day1);
//            flightPlanCoordinates1 = [
//                                            new google.maps.LatLng(49.04, -112.50),
//                                            new google.maps.LatLng(32.31, -117.04),
//                                            new google.maps.LatLng(42.46,-101.21),
//                                            new google.maps.LatLng(49.04, -112.50)                                            
//                                          ];
//            day1 = '#F06000';
//            DrawZones(flightPlanCoordinates1, day1);

//            flightPlanCoordinates1 = [
//                                            new google.maps.LatLng(49.04, -112.50),
//                                            new google.maps.LatLng(42.46, -101.21),
//                                            new google.maps.LatLng(32.31, -117.04),
//                                            new google.maps.LatLng(28.53, -95.07),
//                                            new google.maps.LatLng(48.08, -91.20),
//                                            new google.maps.LatLng(49.04, -112.50)
//                                          ];
//            day1 = '#7C583D';
//            DrawZones(flightPlanCoordinates1, day1);

//            flightPlanCoordinates1 = [
//                                            new google.maps.LatLng(48.08, -91.20),
//                                            new google.maps.LatLng(28.53, -95.07),
//                                            new google.maps.LatLng(35.38, -75.26),
//                                            new google.maps.LatLng(43.28, -74.17),
//                                            new google.maps.LatLng(41.35, -87.28),
//                                            new google.maps.LatLng(48.08, -91.20)
//                                          ];
//            day1 = '#CE07BA';
//            DrawZones(flightPlanCoordinates1, day1);

        }

        function DrawZones(Path, DayColor) {
            var flightPath = new google.maps.Polygon({
                map: map,
                path: Path,
                geodesic: true,                
                strokeOpacity: 0,
                strokeWeight: 0.01,
                fillColor: DayColor,
                fillOpacity: 0.40
            });

            flightPath.setMap(map);
        }

        function trythis() {
            kitData = document.getElementById("hdKitData").value;
        }

        function createMarker() {
            var info = [
                { lat: 44, long: 271, title: 'Wisconsin Sales Office', KitNumber: 'T2 Trauma Kit', KitName: 'T2 Trauma Kit', BranchName: 'Delaware Valley 2' },
                { lat: 40, long: 285, title: 'NY Sales Office', KitNumber: 'T2 Trauma Kit', KitName: 'T2 Trauma Kit', BranchName: 'Delaware Valley' },
                { lat: 42, long: 270, title: 'Chicago Sales Office', KitNumber: 'T2 Trauma Kit', KitName: 'T2 Trauma Kit', BranchName: 'Chicago Sales Office' },
                { lat: 42, long: 290, title: 'Sales Office Rhode Island', KitNumber: 'T2 Trauma Kit', KitName: 'T2 Trauma Kit', BranchName: 'Sales Office Rhode Island' }
            ];

            for (var i in info) {
                if (i == 0) {
                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(info[0].lat, info[0].long),
                        map: map,
                        title: info[0].title
                    });
                    marker.setIcon("http://www.google.com/intl/en_us/mapfiles/ms/micons/blue-dot.png");
                }
                else {
                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(info[i].lat, info[i].long),
                        map: map,
                        title: info[i].title
                    });
                }

                showInfoWindow(marker, info, i);
            }
        }

        function createMarkerwithData(strinfo) {
            //strinfo = '[{ "lat": "44", "long": "271", "title": "Wisconsin Sales Office", "KitNumber": "T2 Trauma Kit", "KitName": "T2 Trauma Kit", "BranchName": "Delaware Valley 2"},{ "lat": 47, "long": 271, "title": "Wisconsin Sales Office", "KitNumber": "T2 Trauma Kit", "KitName": "T2 Trauma Kit", "BranchName": "Delaware Valley 2"}]';
            var info = jQuery.parseJSON(strinfo);
            for (var i in info) {
                if (i == 0) {
                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(info[i].lat, info[i].long),
                        map: map,
                        title: info[i].title
                    });
                    marker.setIcon("http://www.google.com/intl/en_us/mapfiles/ms/micons/blue-dot.png");
                }
                else if (i == 1) {
                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(info[i].lat, info[i].long),
                        map: map,
                        title: info[i].title
                    });
                    marker.setIcon("http://www.google.com/intl/en_us/mapfiles/ms/micons/green-dot.png");
                }
                else {
                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(info[i].lat, info[i].long),
                        map: map,
                        title: info[i].title
                    });
                }
                showInfoWindow(marker, info, i);
            }
        }

        function showInfoWindow(marker, info, number) {
            var msg = "<div class='info'>";
            if (number == 0) {
                msg += "<h3>" + info[number].title + "</h3>";
                msg += "<label>Requested Location: </label>" + info[number].RequestedLocation + "<br />";
                msg += "<label>Address1: </label>" + info[number].BranchAddress + "<br />";
                msg += "<label>Address2: </label>" + info[number].Address2 + "<br />";
                msg += "<label>Requested By: </label>" + info[number].RequestedBy + "<br />";
                msg += "</div>";
            }
            else if (number == 1) {
                msg += "<h3>" + info[number].ShipToCustomer + "</h3>";
                msg += "<label>Address1: </label>" + info[number].BranchAddress + "<br />";
                msg += "<label>Address2: </label>" + info[number].Address2 + "<br />";
                msg += "<label>Requested By: </label>" + info[number].RequestedBy + "<br />";
                msg += "</div>";
            }
            else {
                msg += "<h3>" + info[number].title + "</h3>";
                //            msg += "<label>Kit Number: </label> " + info[number].KitNumber + "<br />";
                //            msg += "<label>Kit Name: </label>" + info[number].KitName + "<br />";
                msg += "<label>Branch Name: </label>" + info[number].BranchName + "<br />";
                msg += "<label>Address1: </label>" + info[number].BranchAddress + "<br />";
                msg += "<label>Address2: </label>" + info[number].Address2 + "<br />";
                msg += "<label>Catalog#: </label>" + info[number].CatalogNumber + "<br />";
                msg += "<label>Excess: </label>" + info[number].Excess + "<br />";
                msg += "<label>TotalKitBuilt: </label>" + info[number].TotalKitBuilt + "<br />";
                msg += "<label>TotalKitShipped: </label>" + info[number].TotalKitShipped + "<br />";
                msg += "<label>TotalKitHold: </label>" + info[number].TotalKitHold + "<br />";
                msg += "<label>ReservedQty: </label>" + info[number].ReservedQty + "<br />";
                msg += "<label>Previous Check In: </label>" + info[number].PreviousCheckInDate + "<br />";
                msg += "<label>Requested By: </label>" + info[number].RequestedBy + "<br />";
                //            msg += "<label>RequestedLocation</label>" + info[number].RequestedLocation + "<br />";
                msg += "<label>Ship To: </label>" + info[number].ShipToCustomer + "<br />";
                //            msg += "<label>Contact PersonName</label>" + info[number].ContactPersonName + "<br />";
                //            msg += "<a href='google.com' style='color:red'>Click Here</a>" + info[number].ContactPersonEmail + "<br />";
                msg += "</div>";
            }
            var infowindow = new google.maps.InfoWindow({
                content: msg,
                size: new google.maps.Size(50, 50)
            });

            google.maps.event.addListener(marker, 'mouseover', function () {
                infowindow.open(map, marker);
            });

            google.maps.event.addListener(marker, 'mouseout', function () {
                infowindow.close();
            });
        }

        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <div id="map-canvas" />
    <asp:HiddenField ID="hdKitData" runat="server" ClientIDMode="Static" />
    <script language="javascript" type="text/javascript">        trythis(); </script>
</asp:Content>
