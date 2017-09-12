
function InitGridEvent(gid) {
    var arr = "";


    var colHeight = $('#' + gid).find('tr:eq(0)').height();
    var totWidth = $('#' + gid).width();

    $('#' + gid).find('tr:eq(0)').css('width', totWidth).css('margin', '-1px 0 0 -1px').css('border', '1px solid #4f81bd');
    $('#' + gid).find('tr:eq(0)').after('<tr height="' + (colHeight) + '"px;"></tr>');

    arr = "";
    $('#' + gid).find('tr th').each(function (i) {
        arr += $(this).width() + ",";
    });

    $('#' + gid).find('tr:eq(0)').css('position', 'absolute');

    if (arr.length > 0) {

        var finalarr = arr.split(",");

        for (var j = 0; j < finalarr.length - 1; j++) {
            $('#' + gid).find('tr th:eq(' + j + ')').css('width', finalarr[j]);
        }
    }
}

//$.fn.hasScrollBar = function () {
//    return this.get(0).scrollHeight > this.height();
//}
    


(function ($) {
    $.fn.Scrollable = function (options) {
        var defaults = {
            ScrollHeight: 300

        };
        
        var options = $.extend(defaults, options);
        return this.each(function () {
            var grid = $(this).get(0);
            var gridWidth = grid.offsetwidth;
            var gridHeight = grid.offsetHeight;
          
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
          
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";

            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
          

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            if (options.Width > 0) {
                gridWidth = options.Width;
            }
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > options.ScrollHeight) {
                gridWidth = parseInt(gridWidth) + 17;
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + options.ScrollHeight + "px;width:" + gridWidth + "px";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
        });
    };
})(jQuery);
       