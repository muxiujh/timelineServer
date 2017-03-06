
$(function () {
    // css define
    $.jh_color_css('body .color-color', 'body=color');
    $.jh_color_css('body .color-bg', 'body=bg');
    $.jh_color_css('body .color-bg-darker', 'body=bg=-10');
    $.jh_color_css('body .color-border', 'nav-tabs=border');
    
    // init
    $('[jh_color]').jh_color();
    $('[jh_node]').jh_node();
    $('.jh_dropdown').jh_dropdown();
    $(".jh_button").jh_button();
    $(".jh_placeholder").jh_placeholder();
    $(".jh_tab").jh_tab();
    /*
    //$('[jh_node="pop_login"]').click();
    $(".jhlazy img").lazyload({effect: "fadeIn"});
    $(".jhlazy embed").each(function () {
        var _this = $(this);
        var url = _this.attr('data-original');
        / *_this.css({display: 'none'});
        _this.attr('src', url);  
        _this.css({display: 'block'});* /
         _this[0].outerHTML = "<embed src='"+url+"' width='420' height='280'></embed>";
        
    });
    */
    
});

$(function(){
    var nav = $(".mainnav");
    var init = $(".mainnav .m").eq(ind);
    var block = $(".mainnav .block");
    block.css({
        "left": init.position().left - 3,
         "display": 'block'
    });
    nav.hover(function() {},
    function() {
        block.stop().animate({
            "left": init.position().left - 3
        },
        100);
    });
    $(".mainnav").slide({
        type: "menu",
        titCell: ".m",
        targetCell: ".dropdown-menu",
        delayTime: 300,
        triggerTime: 0,
        returnDefault: true,
        defaultIndex: ind,
        startFun: function(i, c, s, tit) {
            block.stop().animate({
                "left": tit.eq(i).position().left - 3
            },
            100);
        }
    });
});

var ind = $("#navIndex").val();
if(ind == undefined)
    ind = 0;

//…Ë÷√
/*
myFocus.set({
	id:'myFocus',//ID
	pattern:'mF_quwan'//∑Á∏Ò
});*/