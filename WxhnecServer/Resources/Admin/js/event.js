/*
	events.js
	
*/


$(function(){

$.jh_add_events({
    search_table : {
        click : function (_this) {
            var s1 = "l__l";
            var s2 = "l_l";
            var _form = _this.parents('form');
            var keyArr = _form.find('[condition_key]');
            var valueArr = _form.find('[condition_value]');
            var count = keyArr.length;
            var condition = '';
            var comma = '';
            for (i = 0; i < count; ++i) {
                var key = keyArr[i].value;
                var value = valueArr[i].value;
                if (value != '') {
                    condition += comma + key + s2 + value;
                    comma = s1;
                }
            }
            var url = _form.attr('action') + "&c=" + condition;
            document.location.href = url;
        }
    },
    input_modify : {
        blur : function (_this) {
            var _callback = function (data) {
                if(data.success){
                    $.alert({text:data.msg, url: data.url});
                }
            };
            
            var tr = _this.parents("tr");
            var url = tr.attr('url');
            $.post(url, { name:_this.attr('name'), value:_this.val() }, _callback, 'json');
            
            // set size
            _this.width(_this[0]._width);
            _this.height(_this[0]._height);
            
            
        },
        focus : function (_this) {
            // remember size
            _this[0]._width = _this.width();
            _this[0]._height = _this.height();
        }
    },
    jh_delete : {
        click : function (_this) {
            if(!confirm('really delete?'))
                return;
            
            var _callback = function (data) {
                $.alert({text:data.msg, url: data.url});
            };
            
            var url = _this.attr('url');
            $.get(url, {}, _callback, 'json');
        }        
    },
    form_send : {
        load : function (_this) {            
            var _callback = function (data) {
                $.alert({
                    text: data.msg,
                    url: data.url,
                    hidden: function () {
                        $.jh_button_reset();
                    }
                });
            }
            var ajaxForm = _this.ajaxForm({success: _callback, dataType: 'json'});
        }
    },
    checkboxAll : {
        click : function (_this) {
            var bShow = _this.siblings('input');
            var bCheck = _this.is(':checked');
            var value = bCheck ? 1 : 0;
            bShow.val(value);
            
            var panel = _this.parents('.panel').first();
            var panelBody = panel.find('.panel-body');
            var checkboxList = panelBody.find('[type="checkbox"]');
            var checkboxSub = panelBody.find('[jh_name="checkbox_sub"]');
            checkboxList.each(function () {
                this.checked = bCheck;
            });
            checkboxSub.each(function () {
                this.value = value;
            });
        }
    },
    checkbox : {
        click : function (_this) {
            var bShow = _this.siblings('input');
            if(_this.is(':checked')){
                bShow.val(1);
            }
            else{
                bShow.val(0);
            }
            _this[0].jhclick = true;
        }
    },
    checkpanel : {
        click : function (_this) {
            var _checkbox = _this.find('[type="checkbox"]');
            if(!_checkbox[0].jhclick){
                _checkbox.click();
            }
            _checkbox[0].jhclick = false;
        }
    },
    admin_nav : {
        click : function (_this) {
            var param = $.jh_arg(_this);
            _this.blur();
            
            window.parent.frames["mainFrame"].location.href = "/" + param.a + "?t=" + param.t;
            if (param.left) {
                window.parent.frames["leftFrame"].location.href = "/Admin/AdminLeft/?left=" + param.left;
            }
        }
    },
    admin_refresh : {
        click : function (_this) {               
            var _callback = function (data) {
                var mainFrame = window.parent.frames["mainFrame"];
                if(mainFrame){
                    mainFrame.$.alert({text: data.msg, hidden:function(){$.jh_button_reset();}});
                }
                else{
                    $.alert({text: data.msg, hidden:function(){$.jh_button_reset();}});
                }
            }
            $.get('/Admin/Refresh', {}, _callback, 'json');
        }
    },
    img_verify : {
        click : function (_this) {           
            var img = _this.find('img');
            var url = '?c=agent&rc=misc&ra=verify'+'&t='+rand();
            img.attr('src', url);
        }
    }
});

});