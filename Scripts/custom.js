$(document).ready(function () {
    bindConfirmationToDeleteLinks();

    // Datepicker
    $('#Date').datepicker({
        dateFormat: 'dd.mm.yy'
    });

    // Tablesorter
    $('#expensetable').tablesorter();
    $('#expensetable>td').click(function () { alert(); });

    $('#stattable').tablesorter({ sortList: [[3, 1]] });

    $(".alert").pulse();

    //$("#polling").poll();


});

function mock(target, value) {
    
        var $target = $('#' + target);
        $target.text(value);
        $target.pulse("#FFF29E", 1000);
    }


// attach confirmation
function bindConfirmationToDeleteLinks() {

    $("a.del").click(function () {
        if ($(this).text() == 'sikker?') {
            return true;
        }
        $(this).text('sikker?').delay(3000).queue(function (n) {
            $(this).text('slett'); n();
        });
        return false;
    });
}

function blink(sender) {
    var $sender = $('#' + sender);
    $sender.pulse("#FFF29E", 5000);
}


function prepare(sender) {

    var $sender = $('#' + sender);
    var offset = $sender.offset();

    var $selector = $("#CategorySelector");
    $selector.css({ "left": (offset.left) + "px", "top": offset.top + 20 + "px" }).show();
    $selector.click(function () { $selector.fadeOut(500); });
}


$.fn.pulse = function(highlightColor, duration) {
    var highlightBg = highlightColor || "#FFF29E";
    var animateMs = duration || 1000;
    var originalBg = this.css("backgroundColor");
    this.stop().css("background-color", highlightBg).animate({backgroundColor: originalBg}, animateMs);
};

$.fn.poll = function () {
    var target = this;
    alert(target);

    setInterval(function () {
        if (target.hasClass("stop") == false) {
            //target.addClass("stop");
            update(target);
            target.pulse();
        }
    }, 5000);
};




