// JavaScript Document
	var navcnt = 4;  //Number of navigation titles
	var myinterval;
	var mycurrentpage;
	
	try {
 		document.execCommand("BackgroundImageCache", false, true);
	} catch(err) {}

	function setCurrentPG(page) {
		mycurrentpage = page;
		document.getElementById("nav_"+page).className = "current";
		document.getElementById("sub_"+page).style.top="33px";
	}

	function showSub(id) {
		clearTimeout(myinterval);
		for (var i=1; i<(navcnt+1); i++) {
			document.getElementById("sub_"+i).style.display="none"
			if(document.getElementById("nav_"+i).className != 'current') {
				document.getElementById("nav_"+i).className = "";
			}
		}
		document.getElementById("sub_"+id).style.top="33px";
		document.getElementById("sub_"+id).style.display="inline";
		if(document.getElementById("nav_"+id).className != 'current')
			document.getElementById("nav_"+id).className = "rollover";
	}

	function fadeOut(id) {
		myid = id;
		myinterval = setTimeout("disappear(myid)",300);
		//myinterval = setTimeout("disappear(myid)",1500);
	}

	function disappear(id) {
		clearTimeout(myinterval);
		document.getElementById("sub_"+id).style.display="none";
		document.getElementById("nav_"+id).className = "";
		document.getElementById("sub_"+mycurrentpage).style.display="inline";
		document.getElementById("nav_"+mycurrentpage).className = "current";
	}

	/* For img's with rollovers */
	function swapImage(imgName,newImg){
    if ((navigator.appName == 'Netscape' && parseFloat(navigator.appVersion) >= 3) || (parseFloat(navigator.appVersion) >= 4)){
        eval('document.' + imgName + '.src = "' + newImg + '"');
    }
	}