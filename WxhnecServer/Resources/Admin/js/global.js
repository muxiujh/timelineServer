
$(function () {    
    
    // init
    $('[jh_node]').jh_node();
    $(".jh_button").jh_button();
    
    document.onkeydown = function (e) {
        var ev = window.event || e;
        var code = ev.keyCode || ev.which;
        if (code == 116) {
            if(ev.preventDefault) {
                var mainFrame = window.parent.frames["mainFrame"];
                if(mainFrame){
                    ev.preventDefault();
                    mainFrame.location.reload();
                }
            } else {
                ev.keyCode = 0;
                ev.returnValue = false;
            }
    }
}
});