﻿// JScript File

		function getWindowHeight() {
			var windowHeight = 0;
			if (typeof(window.innerHeight) == 'number') {
				windowHeight = window.innerHeight;
			}
			else {
				if (document.documentElement && document.documentElement.clientHeight) {
					windowHeight = document.documentElement.clientHeight;
				}
				else {
					if (document.body && document.body.clientHeight) {
						windowHeight = document.body.clientHeight;
					}
				}
			}
			return windowHeight;
		}
		function setFooter() {
			if (document.getElementById) {
				var windowHeight = getWindowHeight();
				if (windowHeight > 0) {
//					var contentHeight = document.getElementById('mastercontent').offsetHeight;
//					var footerElement = document.getElementById('footer');
//					var footerHeight  = footerElement.offsetHeight;
//					if (windowHeight - (contentHeight + footerHeight) >= 0) {
//						
//						footerElement.style.top = (windowHeight - (contentHeight + footerHeight)) + 'px';
//					}
//					else {
//						footerElement.style.top = '0px';
//					}
				}
			}
		}
		window.onload = function() {
			setFooter();
		}
		window.onresize = function() {
			setFooter();
		}
		