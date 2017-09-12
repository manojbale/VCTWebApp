function ShowAddressDetails(addressTitle) {
    window.showModalDialog("PartyAddress.aspx?AddressTitle=" + addressTitle);
}

function highlightRow(obj, newColor)
{
    obj.style.cursor = "hand";
    obj.style.backgroundColor = newColor;
}

function dehighlightRow(obj, originalColor)
{
    obj.style.backgroundColor = originalColor;
}

function DeselectOtherCheckBoxes(id) {

        var frm = document.forms[0];

        for (i=0;i<frm.elements.length;i++) {

            if (frm.elements[i].type == "checkbox") {

                if (document.getElementById(id).id != frm.elements[i].id)
                    frm.elements[i].checked = false;

            }

        }

    }  
    
//This function is used with multiline text box to limit the maximum length of the text box during user input
//function LimitText(limitField, limitNum) 
//{
//    if(limitField.value.length >limitNum - 1)
//    {
//        return false;
//    }
//    else
//    { 
//        return true;
//    } 
//}
function LimitText(limitField, limitNum) 
{
    limitField.value = limitField.value.substring(0, limitNum);
}

//This function is used with multiline text box to limit the maximum length during pasting from clipboard
function PreventPaste(limitField,limitNum)
{
    var CanInsertLength; 
    var sData = window.clipboardData.getData("Text");
    var newData ; 

    CanInsertLength = limitNum - limitField.value.length ; 

    if(CanInsertLength <= 0)
    {
        return false;
    }
    else
    {
        newData = sData.substr(0,CanInsertLength);
        window.clipboardData.setData("Text",newData);
        return true; 
    }
}
function GetControlID(controlId) {
    var controlObject;
    if (document.getElementById) // test if browser supports document.getElementById
    {
        controlObject = document.getElementById(controlId);
    }
    else if (document.all) // test if browser supports document.all
    {
        controlObject = document.all[controlId];
    }
    else if (document.layers) // test if browser supports document.layers
    {
        controlObject = document.layers[controlId];
    }
    return controlObject;
}

function OnStartDateSelected(ev) {

    var startDateTime = $find("AjaxCalendarStartTime");
    var d = startDateTime._selectedDate;
    var now = new Date();
    startDateTime.get_element().value = d.format("MM/dd/yyyy") + " " + now.format("hh:mm tt")
}

function OnStopDateSelected(ev) {

    var stopDateTime = $find("AjaxCalendarStopTime");
    var d = stopDateTime._selectedDate;
    var now = new Date();
    stopDateTime.get_element().value = d.format("MM/dd/yyyy") + " " + now.format("hh:mm tt")
}

function OnReceivingDateSelected(ev) {

    var stopDateTime = $find("AjaxCalendarReceivingDate");
    var d = stopDateTime._selectedDate;
    var now = new Date();
    stopDateTime.get_element().value = d.format("MM/dd/yyyy") + " " + now.format("hh:mm tt")
}


function CloseModalPrint() 
{
    $find("PrintBehaviour").hide();
    
    try
    {
        SetFocus();
    }
    catch (err) 
    {
        //  SetFocus function is the local function for setting the focus on default control
        //  On some pages it might be unavailable   
    }
}


