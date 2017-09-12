function openReport(pageURL) {
    var wOpen;
    var sOptions;

    sOptions = 'status=yes,menubar=no,scrollbars=no,resizable=yes,toolbar=no';
    sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString();
    sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString();
    sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0';

    wOpen = window.open('', '', sOptions);
    wOpen.location = pageURL;
    wOpen.focus();
    wOpen.moveTo(0, 0);
    wOpen.resizeTo(screen.availWidth, screen.availHeight);
    return wOpen;
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

function getFileNameFromURL()
{
	var url = document.URL;
	var filename = url.substr(url.lastIndexOf('/')+1,url.length);
	filename = filename.split('.');
	return filename[0];	
}