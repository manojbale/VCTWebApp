function SearchTextByKitNumberNew(docId, methodParamName, methodName, hdnValid) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitName,
                            value: item.KitName,
                            MyNumber: item.KitNumber,
                            MyKitName: item.KitName
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnKitNumber").val(ui.item.MyNumber);
            $("#txtKitName").val(ui.item.MyKitName);
            $("#hdnValid").val(ui.item.MyNumber);
         
        }
    });
}



function SearchTextByPartyName(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.Name,
                            MyId: item.PartyId
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnShipToPartyId").val(ui.item.MyId);
            //$("#hdnPhysicianId").val = 0;

        }
    });
}



function SearchPhysicianTextByPartyIdCase(docId, methodParamName, methodName, hdnShipToPartyIdCase, hdnPhysicianId) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "', 'sPartyId': '" + $("#" + hdnShipToPartyIdCase).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.PhysicianName,
                            value: item.PhysicianName,
                            MyPhysicianId: item.PhysicianId
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                    //alert(result)
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnPhysicianId).val(ui.item.MyPhysicianId);
            $("#" + docId).val(ui.item.value);
           

        }
    });
}



//function SearchTextByProcedureNameCase(docId, hdnPhysicianId, methodParamName, methodName) {
function SearchTextByProcedureNameCase(docId, textBoxControl, methodParamName, methodName) {  
   
   // alert($("#" + hdnPhysicianId).val());
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                //data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "', 'sPhysicianId': '" + $("#" + hdnPhysicianId).val() + "'}",
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "', 'sPhysicianName': '" + $("#" + textBoxControl).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.Name,
                            MyId: item.Name
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnProcedureNameCase").val(ui.item.MyId);
            $("#txtProcedureNameCase").val(ui.item.MyId);
        }
    });
}


function SearchTextByKitNumber(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitNumber,
                            value: item.KitNumber,
                            MyNumber: item.KitNumber,
                            MyKitName: item.KitName
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnKitNumber").val(ui.item.MyNumber);
            $("#txtKitName").val(ui.item.MyKitName);
        }
    });
}

function SearchMappedTextByKitNumber(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitNumber,
                            value: item.KitNumber,
                            MyNumber: item.KitNumber,
                            MyKitName: item.KitName
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnKitNumber").val(ui.item.MyNumber);
            $("#txtKitName").val(ui.item.MyKitName);
        }
    });
}

function SearchTextByKitNumberCase(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitNumber,
                            value: item.KitNumber,
                            MyNumber: item.KitNumber,
                            MyKitName: item.KitName
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnKitNumber").val(ui.item.MyNumber);
            $("#hdnKitNameCase").val(ui.item.MyKitName);

            // Post Back function
            JSFunction();
            return false;
        }
    });
}

function SearchTextByKitName(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitName,
                            value: item.KitName,
                            MyKitNumber: item.KitNumber
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnKitNumber").val(ui.item.MyKitNumber);
            $("#txtKitNumber").val(ui.item.MyKitNumber);
        }
    });
}


function SearchTextByProcedureName(docId, methodParamName, methodName) {


    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",

                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",

                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.Name,
                            MyId: item.Name
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnProcedureName").val(ui.item.MyId);
        }
    });
}


function SearchTextByCatalogNumber(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + extractLast($("#" + docId).val()) + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogFull,
                            value: item.CatalogNumber,
                            MyId: item.CatalogNumber
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function (event, ui) {
            var terms = split($("#" + docId).val());
            // remove the current input
            terms.pop();
            // add the selected item
            terms.push(ui.item.value);
            // add placeholder to get the comma-and-space at the end
            terms.push("");
            this.value = terms.join(", ");
            $("#hdnCatalogNumber").val(ui.item.MyId);
            return false;
            //            var option = $('<option/>').html(ui.item.MyId);
            //            $("#lstCatalog").append(option);
        }
    });

    function split(val) {
        return val.split(/,\s*/);
    }

    function extractLast(term) {
        return split(term).pop();
    }
}



// For KitListing.aspx


function SearchTextByCatalogNumberForHeader(docId, methodParamName, methodName, docIdDescription, hdnId) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogFull,
                            value: item.CatalogFull,
                            MyId: item.CatalogNumber,
                            desc: item.Description
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + docId).val(ui.item.MyId);
            $("#" + docIdDescription).val(ui.item.desc);
            return false;
        }
    });
}

function SearchTextByCatalogNumberCountForHeader(docId, methodParamName, methodName, docIdDescription, hdnId, hdnLotNumNew, hdnAvailableQtyNew) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogFull,
                            value: item.CatalogFull,
                            MyId: item.CatalogNumber,
                            LotNum: item.LotNum,
                            desc: item.Description,
                            AvailableQty: item.AvailableQty
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + hdnLotNumNew).val(ui.item.LotNum);
            $("#" + hdnAvailableQtyNew).val(ui.item.AvailableQty);
            $("#" + docId).val(ui.item.MyId);
            $("#" + docIdDescription).val(ui.item.desc);
            return false;
        }
    });
}

function SearchRMAPartsByCatalogNumber(docId, methodParamName, methodName, docIdDescription, hdnId, hdnPartDesc, hdnPartDetail) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogFull,
                            value: item.CatalogNumber,
                            //MyId: item.LocationPartDetailId,
                            MyId: item.CatalogNumber,
                            desc: item.Description
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + hdnPartDetail).val(ui.item.label);
            $("#" + docId).val(ui.item.value);
            $("#" + hdnPartDesc).val(ui.item.desc);
            $("#" + docIdDescription).val(ui.item.desc);
            return false;
        }
    });
}

function SearchRMACasesByCaseNumber(docId, methodParamName, methodName, hdnId, hdnInvTypeId) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CaseNumber,
                            value: item.CaseNumber,
                            MyId: item.CaseId,
                            InvType: item.InventoryType
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + docId).val(ui.item.value);
            $("#" + hdnInvTypeId).val(ui.item.InvType);
            return false;
        }
    });
}

function SearchTextByKitFamilyForHeader2(docId, methodParamName, methodName, docIdDescription, hdnDescription, hdnId) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitFamilyName,
                            value: item.KitFamilyName,
                            MyId: item.KitFamilyId,
                            desc: item.KitFamilyName
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + docId).val(ui.item.desc);
            $("#" + docIdDescription).val(ui.item.desc);
            $("#" + hdnDescription).val(ui.item.desc);
            return false;
        }
    });
}

function SearchTextByCatalogNumberForHeader2(docId, methodParamName, methodName, docIdDescription, hdnDescription, hdnId) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogFull,
                            value: item.CatalogFull,
                            MyId: item.CatalogNumber,
                            desc: item.Description
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + docId).val(ui.item.MyId);
            $("#" + docIdDescription).val(ui.item.desc);
            $("#" + hdnDescription).val(ui.item.desc);
            return false;
        }
    });
}


function SearchTextByCatalogNumberForFooter(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogNumber,
                            value: item.Description,
                            MyId: item.CatalogNumber
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnCatalogNumberNew").val(ui.item.MyId);
            $("#txtNewCatalogNum").val(ui.item.MyId);
            $("#txtNewCatalogDesc").val(ui.item.value);
            return false;
        }
    });
}

function SearchTextByCatalogNumberForCreateBOM(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogFull,
                            value: item.CatalogFull,
                            MyId: item.CatalogNumber,
                            desc: item.Description
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnCatalogNumber").val(ui.item.MyId);
            $("#hdnDescription").val(ui.item.desc);
            // Post Back function
            JSFunction();
            return false;
        }
    });
}

function SearchTextByKitNumberForCreateBOM(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitNumber,
                            value: item.KitNumber,
                            MyNumber: item.KitNumber,
                            MyKitName: item.KitName
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnKitNumber").val(ui.item.MyNumber);
            $("#txtKitName").val(ui.item.MyKitName);

            __doPostBack('HistoricalBOM', '');
        }
    });
}



function SearchTextByPartyNameCase(docId, methodParamName, methodName) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Name,
                            value: item.Name,
                            MyId: item.PartyId,
                            ShippingDaysGap: item.ShippingDaysGap,
                            RetrievalDaysGap: item.RetrievalDaysGap
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#hdnShipToPartyIdCase").val(ui.item.MyId);
            var strDate = $("#txtSurgeryDate").val();
            
                var date = new Date(strDate);
                date.setDate(date.getDate() - ui.item.ShippingDaysGap);
                $("#txtShippingDate").val(date.format("MM/dd/yyyy"));
                date = new Date(strDate);
                date.setDate(date.getDate() + ui.item.RetrievalDaysGap);
                $("#txtRetrievalDate").val(date.format("MM/dd/yyyy"));
           
        }
    });
}


function ViewTransactionByCaseNumber(docId, methodParamName, methodName) {

    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "', 'FilterBy':'CaseNumber', 'StartDate':'" + $('#txtStartDate').val() + "', 'EndDate':'" + $('#txtEndDate').val() + "', 'CaseType':'" + $('#ddlCaseType').val() + "', 'InvType':'" + $('#ddlInvType').val() + "', 'PartyName':'" + $('#txtPartyName').val() + "', 'LocationType':'" + $('#ddlLocType').val() + "', 'CaseStatus':'" + $('#ddlCaseStatus').val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {

                            label: item.CaseNumber,
                            value: item.CaseNumber,
                            MyId: item.CaseNumber
                        }

                    }))
                },

                error: function (result) {
                    //alert(result.text());
                }
            });
        },
        select: function (event, ui) {
            $("#" + docId).val(ui.item.MyId);


            // Post Back function
            JSFunction();
            return false;
        }
    });
}

function ViewTransactionByLocation(docId, methodParamName, methodName) {

    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "', 'FilterBy':'ShipToLocation', 'StartDate':'" + $('#txtStartDate').val() + "', 'EndDate':'" + $('#txtEndDate').val() + "', 'CaseType':'" + $('#ddlCaseType').val() + "', 'InvType':'" + $('#ddlInvType').val() + "', 'CaseNumber':'" + $('#txtCaseNumber').val() + "', 'LocationType':'" + $('#ddlLocType').val() + "', 'CaseStatus':'" + $('#ddlCaseStatus').val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {

                        return {
                            label: item.PartyName,
                            value: item.PartyName,
                            MyId: item.PartyName
                        }

                    }))
                },

                error: function (result) {
                    //alert(result);
                }
            });
        },
        select: function (event, ui) {
            $("#" + docId).val(ui.item.MyId);

            // Post Back function
            JSFunction();
            return false;
        }
    });
}

function ViewTransactionLocationsById(docId, methodParamName, methodName) {

    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {

                        return {
                            label: item.PartyName,
                            value: item.PartyName,
                            MyId: item.PartyName
                        }

                    }))
                },

                error: function (result) {
                    //alert(result);
                }
            });
        },
        select: function (event, ui) {
            $("#" + docId).val(ui.item.MyId);

            // Post Back function
            JSFunction();
            return false;
        }
    });
}

function SearchKitFamilyByNumber(docId, methodParamName, methodName, hdnId) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitFamilyName,
                            value: item.KitFamilyName,
                            MyNumber: item.KitFamilyId,
                            MyKitName: item.KitFamilyName
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyNumber);
            $("#" + docId).val(ui.item.MyKitName);

        }
    });
}


function GetKitFamilyByLocationAndNumber(docId, methodParamName, methodName, hdnId, txtDescription) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.KitFamilyName,
                            value: item.KitFamilyName,
                            MyId: item.KitFamilyId,
                            Description: item.KitFamilyDescription
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {

            $("#" + docId).val(ui.item.label);
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + txtDescription).val(ui.item.Description);

            return false;
        }
    });
}


function SearchTextByPartNumberForHeader(docId, methodParamName, methodName, docIdDescription, hdnId) {
    $("#" + docId).autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "VCTWebService.asmx/" + methodName,
                data: "{'" + methodParamName + "':'" + $("#" + docId).val() + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.CatalogFull,
                            value: item.CatalogFull,
                            MyId: item.RefNum,
                            desc: item.Description
                        }
                    }))
                },

                error: function (result) {
                    //alert("Error");
                }
            });
        },
        select: function (event, ui) {
            $("#" + hdnId).val(ui.item.MyId);
            $("#" + docId).val(ui.item.MyId);
            $("#" + docIdDescription).val(ui.item.desc);
            return false;
        }
    });
}