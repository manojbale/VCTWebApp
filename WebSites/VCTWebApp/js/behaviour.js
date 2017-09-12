/*
   Behaviour v1.1 by Ben Nolan, June 2005. Based largely on the work
   of Simon Willison (see comments by Simon below).

   Description:
   	
   	Uses css selectors to apply javascript behaviours to enable
   	unobtrusive javascript in html documents.
   	
   Usage:   
   
	var myrules = {
		'b.someclass' : function(element){
			element.onclick = function(){
				alert(this.innerHTML);
			}
		},
		'#someid u' : function(element){
			element.onmouseover = function(){
				this.innerHTML = "BLAH!";
			}
		}
	};
	
	Behaviour.register(myrules);
	
	// Call Behaviour.apply() to re-apply the rules (if you
	// update the dom, etc).

   License:
   
   	My stuff is BSD licensed. Not sure about Simon's.
   	
   More information:
   	
   	http://ripcord.co.nz/behaviour/
   
*/   

var Behaviour = {
	list : new Array,
	
	register: function(sheet){
		Behaviour.list.push(sheet);
	},

	start : function(){
		// Replace with Prototype library function
		Event.observe(window, 'load', function() { Behaviour.apply(); });
	},
	
	apply : function(){
		for (h = 0; sheet = Behaviour.list[h]; ++h)
			Behaviour.applyRules(sheet);
	},

	applyRules: function(sheet) {
		for (selector in sheet) {
			list = document.getElementsBySelector(selector);
			if (!list)
				continue;
			for (i = 0; element = list[i]; ++i)
				sheet[selector](element);
		}
	}
}

Behaviour.start();

/*
   The following code is Copyright (C) Simon Willison 2004.

   document.getElementsBySelector(selector)
   - returns an array of element objects from the current document
     matching the CSS selector. Selectors can contain element names, 
     class names and ids and can be nested. For example:
     
       elements = document.getElementsBySelect('div#main p a.external')
     
     Will return an array of all 'a' elements with 'external' in their 
     class attribute that are contained inside 'p' elements that are 
     contained inside the 'div' element which has id="main"

   New in version 0.4: Support for CSS2 and CSS3 attribute selectors:
   See http://www.w3.org/TR/css3-selectors/#attribute-selectors

   Version 0.4 - Simon Willison, March 25th 2003
   -- Works in Phoenix 0.5, Mozilla 1.3, Opera 7, Internet Explorer 6, Internet Explorer 5 on Windows
   -- Opera 7 fails 
*/

function getAllChildren(e) {
  // Returns all children of element. Workaround required for IE5/Windows. Ugh.
  return e.all ? e.all : e.getElementsByTagName('*');
}

document.getElementsBySelector = function(selector) {
  // Attempt to fail gracefully in lesser browsers
  if (!document.getElementsByTagName) {
    return new Array();
  }
  // Split selector in to tokens
  var tokens = selector.split(' ');
  var currentContext = new Array(document);
  for (var i = 0; i < tokens.length; i++) {
    token = tokens[i].replace(/^\s+/,'').replace(/\s+$/,'');;
    if (token.indexOf('#') > -1) {
      // Token is an ID selector
      var bits = token.split('#');
      var tagName = bits[0];
      var id = bits[1];
      var element = document.getElementById(id);
      if (element && tagName && element.nodeName.toLowerCase() != tagName) {
        // tag with that ID not found, return false
        return new Array();
      }
      // Set currentContext to contain just this element
      currentContext = new Array(element);
      continue; // Skip to next token
    }
    if (token.indexOf('.') > -1) {
      // Token contains a class selector
      var bits = token.split('.');
      var tagName = bits[0];
      var className = bits[1];
      if (!tagName) {
        tagName = '*';
      }
      // Get elements matching tag, filter them for class selector
      var found = new Array;
      var foundCount = 0;
      for (var h = 0; h < currentContext.length; h++) {
        var elements;
        if (tagName == '*') {
            elements = getAllChildren(currentContext[h]);
        } else {
            elements = currentContext[h].getElementsByTagName(tagName);
        }
        for (var j = 0; j < elements.length; j++) {
          found[foundCount++] = elements[j];
        }
      }
      currentContext = new Array;
      var currentContextIndex = 0;
      for (var k = 0; k < found.length; k++) {
        if (found[k].className && found[k].className.match(new RegExp('\\b'+className+'\\b'))) {
          currentContext[currentContextIndex++] = found[k];
        }
      }
      continue; // Skip to next token
    }
    // Code to deal with attribute selectors
    if (token.match(/^(\w*)\[(\w+)([=~\|\^\$\*]?)=?"?([^\]"]*)"?\]$/)) {
      var tagName = RegExp.$1;
      var attrName = RegExp.$2;
      var attrOperator = RegExp.$3;
      var attrValue = RegExp.$4;
      if (!tagName) {
        tagName = '*';
      }
      // Grab all of the tagName elements within current context
      var found = new Array;
      var foundCount = 0;
      for (var h = 0; h < currentContext.length; h++) {
        var elements;
        if (tagName == '*') {
            elements = getAllChildren(currentContext[h]);
        } else {
            elements = currentContext[h].getElementsByTagName(tagName);
        }
        for (var j = 0; j < elements.length; j++) {
          found[foundCount++] = elements[j];
        }
      }
      currentContext = new Array;
      var currentContextIndex = 0;
      var checkFunction; // This function will be used to filter the elements
      switch (attrOperator) {
        case '=': // Equality
          checkFunction = function(e) { return (e.getAttribute(attrName) == attrValue); };
          break;
        case '~': // Match one of space seperated words 
          checkFunction = function(e) { return (e.getAttribute(attrName).match(new RegExp('\\b'+attrValue+'\\b'))); };
          break;
        case '|': // Match start with value followed by optional hyphen
          checkFunction = function(e) { return (e.getAttribute(attrName).match(new RegExp('^'+attrValue+'-?'))); };
          break;
        case '^': // Match starts with value
          checkFunction = function(e) { return (e.getAttribute(attrName).indexOf(attrValue) == 0); };
          break;
        case '$': // Match ends with value - fails with "Warning" in Opera 7
          checkFunction = function(e) { return (e.getAttribute(attrName).lastIndexOf(attrValue) == e.getAttribute(attrName).length - attrValue.length); };
          break;
        case '*': // Match ends with value
          checkFunction = function(e) { return (e.getAttribute(attrName).indexOf(attrValue) > -1); };
          break;
        default :
          // Just test for existence of attribute
          checkFunction = function(e) { return e.getAttribute(attrName); };
      }
      currentContext = new Array;
      var currentContextIndex = 0;
      for (var k = 0; k < found.length; k++) {
        if (checkFunction(found[k])) {
          currentContext[currentContextIndex++] = found[k];
        }
      }
      // alert('Attribute Selector: '+tagName+' '+attrName+' '+attrOperator+' '+attrValue);
      continue; // Skip to next token
    }
    
    if (!currentContext[0]){
    	return;
    }
    
    // If we get here, token is JUST an element (not a class or ID selector)
    tagName = token;
    var found = new Array;
    var foundCount = 0;
    for (var h = 0; h < currentContext.length; h++) {
      var elements = currentContext[h].getElementsByTagName(tagName);
      for (var j = 0; j < elements.length; j++) {
        found[foundCount++] = elements[j];
      }
    }
    currentContext = found;
  }
  return currentContext;
}

/* That revolting regular expression explained 
/^(\w+)\[(\w+)([=~\|\^\$\*]?)=?"?([^\]"]*)"?\]$/
  \---/  \---/\-------------/    \-------/
    |      |         |               |
    |      |         |           The value
    |      |    ~,|,^,$,* or =
    |   Attribute 
   Tag
*/
// JavaScript Document
var rules = {
	// Rich Text Editors
	'textarea.richtext': function(elt) {
		new Control.RTE(elt, '/js/controls/rte/images', { fileLister: listUserFiles });
	},
	'input.datepicker': function(elt) {
		new Control.DatePicker(elt, { icon: 'Images/calendar.png' });
	},
	'input.timepicker': function(elt) {
		new Control.DatePicker(elt, { icon: '/js/controls/datepicker/clock.png', datePicker: false, timePicker: true });
	},
	'input.datetimepicker': function(elt) {
		new Control.DatePicker(elt, { icon: 'Images/calendar.png', timePicker: true, timePickerAdjacent: true, use24hrs: true });
	},
	'input.datetimepicker_es': function(elt) {
		new Control.DatePicker(elt, { icon: 'Images/calendar.png', locale:'es_AR', timePicker: true });
	},
	'input.colorpicker': function(elt) {
		new Control.ColorPicker(elt);
	},
	'input.filechooser': function(elt) {
		new Control.FileChooser(elt, listUserFiles, {
				icon: '/js/controls/filechooser/filechooser.png',
				parentImage: '/js/controls/filechooser/parent.gif',
				fileImage: '/js/controls/filechooser/file.gif',
				directoryImage: '/js/controls/filechooser/directory.gif'
			});
	},
	'.rating_bar': function(elt) {
		var code = elt.id.replace(/rating_/, '');
		new Control.RatingBar(elt, {
			starClass: 'rating_star',
			onClass: 'rating_on',
			hoverClass: 'rating_hover',
			halfClass: 'rating_half',
			onclick: rateItem(code)
			});
	},
	'.tabcontrol': function(elt) {
		new Control.TabStrip(elt, {
				activeClass: 'active',
				hoverClass: 'hover',
				disabledClass: 'disabled',
				disabled: null
			});
	},
	'.treeselect': function(elt) {
		new TreeSelect(elt);
	},
	'#livegrid': function(elt) {
		new Control.LiveGrid(elt, 10, 100, getData, {
				prefetchBuffer: 'active',
				selectable: true,
				rowIdPrefix: 'result_',
				onrowopen: openRows,
				onrowselect: selectRows,
				onscroll: scrollRows,
				sortHeader: 'livegrid_header',
				sortField: 'name',
				sortDir: 'asc',
				sortAscendImg: '/js/controls/livegrid/sort_asc.png',
				sortDescendImg: '/js/controls/livegrid/sort_desc.png',
				imageWidth: 9,
				imageHeight: 9
			});
	},
	'.treelist': function (elt) {
		new Control.TreeList(elt, {
			topOffset: 5,
			collapseIcon: '/js/controls/treelist/down_arrow_outline.gif',
			collapseIconHover: '/js/controls/treelist/down_arrow_filled.gif',
			expandIcon: '/js/controls/treelist/right_arrow_outline.gif',
			expandIconHover: '/js/controls/treelist/right_arrow_filled.gif'
			});
	}
};
Behaviour.register(rules);

function listUserFiles(directory, callback) {
	new Ajax.Request('/software/js/fileaccess.php', {
			parameters: 'a=listdir&d=' + (directory || ''),
			onComplete: function(transport) {
				try {
					callback(eval('(' + transport.responseText + ')'));
				} catch(e) {
					callback({status:'error'});
				}
			}
		});
}

function getData(offset, limit, sortField, sortDir, callback) {
    new Ajax.Request('/software/js/getData.php', {
                parameters: {
                    'offset': offset,
                    'limit': limit,
                    'sort': sortField,
                    'dir': sortDir
                },
                onComplete: function(transport) {
					try {
						callback(eval('(' + transport.responseText + ')'));
					} catch (e) {
						alert(e.message);
					}
                }
            });
}

function selectRows(e, selector) {
	var selected = selector.selectedRows();
	var desc = selected.length ? selected.join(', ') : 'None';
	$('livegrid_selected_label').innerHTML = 'Selected rows: ' + desc;
}
function openRows(e, selector) {
	alert('Open row event: ' + selector.selectedRows().join(', '));
}
function scrollRows(start, count, total) {
	$('livegrid_label').innerHTML = 'Viewing '+(start+1)+' to '+(start+count)+' of '+total+' results';
}
function rateItem(code) {
	return function(ratingbar) {
		var rating = ratingbar.rating;
		ratingbar.setLoading(true);
		new Ajax.Request('/ratings/rate.php', {
				parameters: {'m': 'rpc', 'r': rating, 'c': code},
				onSuccess: function(transport) {
					ratingbar.setLoading(false);
					try {
						var response = eval('(' + transport.responseText + ')');
						ratingbar.rating = response.rating;
						ratingbar.resetRating();
						$('rating_'+code+'_average').innerHTML = response.rating;
						$('rating_'+code+'_votes').innerHTML = response.votes;
					} catch(e) {
						alert(e.message);
					}
				},
				onFailure: function(transport) {
					ratingbar.setLoading(false);
					ratingbar.resetRating();
				}
			});
	}
}
