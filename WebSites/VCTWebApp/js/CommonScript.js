// JScript File
function ValidNumber()
{
if (event.keyCode < 45 ||event.keyCode == 47 || event.keyCode > 57)  event.returnValue = false;
}
function NumberOnly()
{
    alert('Only numbers allowed.');
if(event.keyCode < 48 || event.keyCode > 57) event.returnValue=false;
}

function ValidateInvalidchar()
{
if (event.keyCode == 34 || event.keyCode == 39)  event.returnValue = false;
}

function validatekeypress()
{
event.returnValue = false;
}

function ClearClip()
{
window.clipboardData.clearData();
}

function BlackListChar()
{
// if (event.keyCode == 35 || event.keyCode == 34 || event.keyCode == 94 ||event.keyCode == 95 || event.keyCode == 126 || event.keyCode == 36 || event.keyCode == 59 || event.keyCode == 39 ||event.keyCode == 37 || event.keyCode == 38 ||event.keyCode == 45 || event.keyCode == 42 || event.keyCode == 64
    //  || event.keyCode == 60 ||event.keyCode == 62 || event.keyCode == 63 || event.keyCode == 40 ||event.keyCode == 41 || event.keyCode == 42  || event.keyCode == 46 || event.keyCode == 123 ||event.keyCode == 124 || event.keyCode == 125 || event.keyCode == 47 )

    if (event.keyCode == 192 || event.keyCode == 33 ) 

  event.returnValue = false;

}

function ValidateUserPwd()
{
if (event.keyCode == 34 || event.keyCode == 39 ||event.keyCode==32)  event.returnValue = false;
}
function ValidateExperiance()
{
if(event.keyCode < 46 || event.keyCode > 57) event.returnValue=false;


}

function confirm_Activate()
 {
 if(confirm('Are you sure you want to Activate?')==true)
    return true;
 else
    return false;
 }
 function ValidateSpecialKey()
 {
    if(event.keyCode >=0)
        return false;
 }
 
 function ConfirmSave()
 {
 if(confirm('Are you sure you want to Save?')==true)
    return true;
 else
    return false;
 }