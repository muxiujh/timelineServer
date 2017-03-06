/*
	events.js
	
*/


$(function(){

// ================================ events ================================
$.jh_add_events({
    search_button : {
        click : function (_this) {
            var _field = $("[jh_node='search_field']");
            var _val = _field.val();
            if(_val)
                location.href = '?c=Form&a=Rows&form=document&li__title=' + _val;
        }
    },
    search_field : {
        keydown : function (_this) {
            var _button = $("[jh_node='search_button']");
            if(event.keyCode==13){
                _button.click();
            }
        }
    },
    ajax_click : {
        click : function (_this) {
            var msg = _this.attr('msg');
            if(msg){
                if(!confirm(msg))
                    return;
            }            
            
            var _callback = function (data) {
                $.alert({text:data.msg, url: data.url});
            };
            
            var url = _this.attr('url');
            $.get(url, {}, _callback, 'json');
        }
    },
    sendsms : {
        click : function (_this) {
            var seconds = 120;
            var delay = 1000;
            var _form  = _this.parents('form');
            var _mobile = _form.find('[name="mobile"]');
            var _uname = _form.find('[name="uname"]');
            var _text = _this.text();
            var _url = _this.attr('url');
            
            var _reduce = function () {
                if(--seconds < 0){
                    _this.text(_text);
                    _this.removeAttr('disabled');
                    return;
                }
                _this.text(seconds + ' 秒后可重新获取');
                setTimeout(_reduce, delay);
                
            }
            
            var _callback = function (data) {
                if(data.success){
                    setTimeout(_reduce, delay);
                }
                else{
                    $.alert({
                        text: data.msg,
                        hidden: function () {
                            _this.removeAttr('disabled');
                        }
                    });
                    
                }
            }
            
            _this.attr('disabled', 'disabled');
            $.post(_url, {mobile: _mobile.val(), uname: _uname.val()}, _callback, 'json');
        }
    },
    auto_notify : {
        load : function (_this) {
            var param = $.jh_arg(_this);
            
            var count_all = _this.find('.notify_all').first();            
            var notify_panel = _this.find('.dropdown-menu');
            var bFirst = true;
            var timespan = param.timespan * 1000;
            
            var li_arr = {};
            var li_list = notify_panel.find('li');
            var badge_list = notify_panel.find('.badge');
            for(i = 0; i < li_list.length; ++i){
                var li = $(li_list[i]);
                var key = li.attr('notify');
                var item = {};
                item['li'] = li;
                item['badge'] = li.find('.badge').first();
                li_arr[key] = item;
            }
            
            var _callback = function (data) {
                if(data.success){
                    if(data.countAll > 0){
                        count_all.text(data.countAll);
                        if(bFirst){
                            notify_panel.show();
                            bFirst = false;
                        }
                        li_list.hide();;
                        for(key in data.countArr){
                            var li = li_arr[key];
                            li['li'].show();
                            li['badge'].text(data.countArr[key]);
                        }
                    }
                    else{
                        count_all.text('');
                        notify_panel.hide();
                        li_list.show();
                        badge_list.text('');
                    }
                    setTimeout(getcount, timespan);
                }
            }
            var getcount = function () {
                $.get('?c=agent&rc=User&ra=count', {}, _callback, 'json');
            }
            if(param.debug != '1'){
                getcount();
            }
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
        }
    },
    property_status : {
        load : function (_this) {
            var param = $.jh_arg(_this);
            
            var panel_sale = $('#panel_sale');
            var panel_contact = $('#panel_contact');
            
            var change_status = function (_status) {
                if(_status == 2){
                    panel_sale.show();
                    panel_contact.show();
                }
                else if(_status == 3){
                    panel_sale.hide();
                    panel_contact.show();
                }
                else{
                    panel_sale.hide();
                    panel_contact.hide();
                }
            }
            
            _this.find('[type="radio"]').click(function () {
                var _status = this.value;
                change_status(_status);
            });
            
            change_status(param.status);
        }
    },
    pic_edit : {
        load : function (_this) {
            var param = $.jh_arg(_this);
            var up = _this.find('.pic-up');
            var down = _this.find('.pic-down');
            var del = _this.find('.pic-remove');
            
            up.click(function () {
                var element = _this.prev();
                if(element.length > 0){
                    _this.insertBefore(element);
                }
            });
            
            down.click(function () {
                var element = _this.next();
                if(element.length > 0){
                    _this.insertAfter(element);
                }
            });
            
            del.click(function () {
                if(confirm('确定删除?')){
                    _this.remove();
                }
            });
            
            if(!param.id)
                return;
                
            var pic = _this.find('.img-thumbnail');  
            var desc = _this.find('[name="pictures_desc[]"]');
            var jh_editor = false;
            var src = '';
            
            pic.click(function () {
                if(!jh_editor){
                    jh_editor = _this.parents('form').first();
                    jh_editor = jh_editor[0].jh_editor;
                    src = this.src + '&type=big'
                }
                    
                var obj = {
                    picid : param.id,
                    src :   src,                
                    _src :  src
                };
                var _img = "<img jhimg='' src=" + src + " picid=" + param.id +" />";
                var _html = "<br /><br />";
                var _desc = desc.val();
                if(_desc != ''){
                    _html = "<br />" + _desc + _html;
                }
                
                jh_editor.fireEvent('beforeInsertImage', obj);
                jh_editor.execCommand("insertHtml", _img);
                //jh_editor.execCommand("insertImage", obj);
                jh_editor.execCommand("insertHtml", _html);
            });
            
        }
    },
    atshow : {
        hover : function (_this) {
            var obj = _this[0];
            if(!obj.init){
                 obj.atlabel = _this.find('.atlabel');
                 obj.atcontent = _this.find('.atcontent');
                 
                _this.find('.athide').click(function () {
                    obj.hiding = true;
                    obj.atlabel.show();
                    obj.atcontent.hide();
                });
                obj.init = true;
            }
            
            if(!obj.hiding){
                obj.atlabel.hide();
                obj.atcontent.show();
            }
        },
        mouseout : function (_this) {
            var obj = _this[0];
            obj.hiding = false;
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
    comment_send : {
        load : function (_this) {            
            var _callback = function (data) {
                if(data.success){
                    var floor = $(data.floor);
                    floor.hide();
                    
                    var comment_box = $(".comment_box");
                    comment_box.append(floor);                    
                    
                    floor.show(800);
                    _this.resetForm();
                }
                else{
                    $.alert(data.msg)
                }
            }
            var ajaxForm = _this.ajaxForm({success: _callback, dataType: 'json'});
        }
    },
    comment_delete : {
        click : function (_this) {
            var _callback = function (data) {
                if(data.success){
                    $.alert({
                        text:data.msg,
                        hidden: function () {
                            var floor = _this.parents('.comment_floor').first();
                            floor.hide(500);
                        }
                    });
                }
                else{
                    $.alert(data.msg);
                }
            };
            
            var param = $.jh_arg(_this);
            $.post('?c=comment&a=delete', {id: param.id}, _callback, 'json');
        }
    },
    to_focus : {
        load : function (_this) {
            _this.focus();
        }
    },
    message_box : {
        click : function (_this) {
            if(!check_login()) return;
            
            var param = $.jh_arg(_this);
            
            // _callback
            var _callback = function (data) {
            
                var _shown = function () {
                    _data.find('[jh_node]').jh_node();
                    _data.find('.jh_button').jh_button();
                };
                
                var _data = $(data);
                _data.jh_dialog({shown: _shown});
                
            };
            
            $.get('?c=user&a=message_paper_pop', {uid:param.uid}, _callback, 'html');
        }
    },
    message_send : {
        click : function (_this) {
            if(!check_login()) return;
            
            var _callback = function (data) {
                if(data.success){
                    var floor = $(data.floor);
                    floor.hide();
                    
                    var message_box = _this.parents(".message-box");
                    var tbody = message_box.find('tbody');
                    tbody.append(floor);
                    $.jh_button_reset();
                    textarea.val('');
                    
                    floor.show(800);
                    message_box.find('.to_focus').focus();
                }
                else{
                    $.alert(data.msg)
                }
            };
            
            var textarea = _this.siblings('textarea');
            var uid = _this.siblings('input');
            $.post('?c=agent&rc=message&ra=sendback', {content: textarea.val(), uid: uid.val()}, _callback, 'json');
        }
    },
    message_refresh : {
        click : function (_this) {
        }
    },
    follow : {
        click : function (_this) {
            if(!check_login()) return;
            
            var param = $.jh_arg(_this);
            
            var _callback = function (data) {
                if(data.success){
                    $.alert({text:data.msg, hidden: function () {
                        _this.hide();
                        _this.siblings('.hidden').removeClass('hidden');
                    }});
                }
                else{
                    $.alert(data.msg);
                }
            };
            $.post('?c=agent&rc=friend&ra=follow', {uid: param.uid}, _callback, 'json');
        }
    },
    img_verify : {
        click : function (_this) {
            var img = _this.find('img');
            var url = '?c=agent&rc=misc&ra=verify'+'&t='+rand();
            img.attr('src', url);
        }
    },
    need_login : {
        load : function (_this) {
            check_login(false);
            $(".need_login").click(function () {
                return check_login();
            });
        }
    },
    editor_form : {
        load : function (_this) {
            _this[0].instance = _this.jh_editor_form({
                imageUrl : '?c=agent&rc=upload',
                before_submit: function () { return check_login(); }
            });
        }
    },
    reply_topic : {
        load : function (_this) {
            _this.jh_editor_form({
                imageUrl : '?c=agent&rc=upload',
                before_submit: function () { return check_login(); },
                after_submit: function (data) { 
                    var floor = $(data.floor);
                    floor.hide();
                    $("#floors").append(floor);
                    floor.show(800);
                    floor.find('[jh_node]').jh_node();
                }
            });
        }
    },
    reply_delete : {
        click : function (_this) {
            var _callback = function (data) {
                if(data.success){
                    $.alert({
                        text:data.msg,
                        hidden: function () {
                            var floor = _this.parents('.floor').first();
                            floor.hide(500);
                        }
                    });
                }
                else{
                    $.alert(data.msg);
                }
            };
            
            var param = $.jh_arg(_this);
            $.post('?c=agent&rc=post&ra=delete', {pid: param.pid}, _callback, 'json');
        }
    },
    reply_reply_mini : {
        click : function (_this) {
            var param = $.jh_arg(_this);
            var _panel = _this.parents('.panel').first();
            
            _panel.find('[name="reply_pid"]').val(param.pid);
            var _btn_uname = _panel.find('.btn_uname');
            _btn_uname.show();
            _btn_uname.find('.reply_uname').text('回复 ' + param.floor + '楼 ' + param.uname);
            
            var _y = _btn_uname.offset().top - 120;
            var _doc = $(document);
            var _top = _doc.scrollTop();
            if(_top > _y)
                _doc.scrollTop(_y);
            
            _panel[0].jh_editor_form.do_focus();
            
            
        }
    },
    reply_mini : {
        click : function (_this) {
            var param = $.jh_arg(_this);
            
            var _panel = _this.parent().siblings('.panel').first();
            if(_panel.css('display') == 'none'){
                _panel.show();
                if(!_panel[0].jh_editor_form){
                    // init form
                    var reply_form = _panel.find('form.quick-reply');
                    var reply_template = $('#reply_mini_template');
                    reply_form.append(reply_template.html());
                    
                    var jh_editor_form = reply_form.jh_editor_form({
                        imageUrl : '?c=agent&rc=upload',
                        before_submit: function () { return check_login(); },
                        after_submit: function (data) { 
                            // show floor
                            var floor = $(data.floor);
                            floor.hide();
                            reply_list.prepend(floor);
                            floor.show(800);
                            floor.find('[jh_node]').jh_node();
                            
                            // add count_reply
                            var _sum = _panel.parent().find('.count_reply');
                            _sum.text(parseInt(_sum.text())+1);
                        }
                    });
                
                    reply_form.show();
                    _panel[0].jh_editor_form = jh_editor_form;
                    
                    // init reply
                    var _callback = function (data) {
                        if(data.success){
                            reply_list.append(data.floor);
                            reply_list.find('[jh_node]').jh_node();
                        }
                    };
                    var reply_list = _panel.find('.reply_list');
                    $.post('?c=topic&a=latestReplyList', {tid: param.tid}, _callback, 'json');
                    
                    // init button
                    var _btn_uname = _panel.find('.btn_uname');
                    _btn_uname.click(function () {
                        _btn_uname.hide();
                        _panel.find('[name="reply_pid"]').val('');
                        
                    });
                }
            }
            else{
                _panel.hide();
            }
        }
    },
    reply_reply : {
        click : function (_this) {
            if (!check_login()) return false;
            
            var show_form = function (bShow) {
                if(bShow){
                    _this.find('.form-show').show();
                    _this.find('.form-hide').hide();
                    _this[0].jh_editor_form.show(300);
                    _this[0].jh_editor_form.do_focus();
                }
                else{
                    _this.find('.form-show').hide();
                    _this.find('.form-hide').show();
                    _this[0].jh_editor_form.hide(500);
                }
            }
        
            var init_form = function () {
            
                if(_this[0].jh_editor_form){
                    show_form((_this[0].jh_editor_form.css('display') == 'none'));
                    return;
                }
                
                var floor = _this.parents('.floor').first();
                var reply_form = floor.find('form.quick-reply');
                var reply_template = $('#reply_template');
                reply_form.append(reply_template.html());
                var jh_editor_form = reply_form.jh_editor_form({
                    imageUrl : '?c=agent&rc=upload',
                    before_submit: function () { return check_login(); },
                    after_submit: function (data) { 
                        show_form(false); 
                        var reply_foor = $(data.floor);
                        reply_foor.find('[jh_node]').jh_node();
                        reply_foor.insertAfter(floor);
                    }
                });
                
                _this[0].jh_editor_form = jh_editor_form;
                show_form(true);
            }
            
            init_form();
        }
            
    },
    preview : {
        click : function () {
            // _callback
            var _callback = function (data) {
                var _data = $(data);
                _data.jh_dialog({hidden: function () { $.jh_button_reset(); }});
                var editor_form = $("#editor_form")[0];
                if(editor_form && editor_form.instance){
                    _data.find(".modal-body").html(editor_form.instance.getContent());                
                }
            };
            
            $.post('?c=dialog&type=pop_preview', {}, _callback, 'html');
        }
    },
    do_good : {
        click : function (_this) {
            if (!check_login()) return false;
            
            var param = $.jh_arg(_this);
            
            // _callback
            var _callback = function (data) {     
                $.alert({text:data.msg, hidden:function () {
                    if(data.success){
                        var _sum = _this.find('.goodnum').first();
                        _sum.text(parseInt(_sum.text())+1);
                    }
                }});
            };
            
            $.post('?c=topic&a=countGood', {tid: param.tid}, _callback, 'json');
        },
        load : function (_this) {
            var urlParam = $.jh_arg(_this);
            _this.jh_popover({
                url: '?c=Topic&a=popGood',
                urlParam: urlParam
            });
        }
    },
    pop_picture : {
        load : function (_this) {
            var urlParam = $.jh_arg(_this);
            _this.jh_popover({
                url: '?c=dialog&type=pop_picture',
                urlParam: urlParam
            });
        }        
    },
    usecss : {
        click : function () {
            var newcss = $('#newcss').val();
            var _callback = function (data) {
               $.alert({text:data.text, change:'jump', delay: 1000});
            };
            
            sync_begin();
            $.post('?c=agent&rc=misc&ra=color', {color:newcss}, _callback, 'json');
            sync_end();
        }        
    },
    syscolor : {
        click : function () {
            var _callback = function (data) {
               $.alert({text:data.color, change:'jump', delay: 1000});
            };
            
            sync_begin();
            $.post('c=agent&r?c=misc&ra=color', {color:_this.text()}, _callback, 'json');
            sync_end();
        }        
    },
    logout : {
        click : function () {
            var _callback = function (data) {
                $.alert({text:data.msg, url: '?'});
            };
            
            $.post('?c=login&a=logout', {}, _callback, 'json');
        }
        
    },
    pop_login : {
        click : function () {
            // _callback_pop_login
            var _callback_pop_login = function (data) {
                var _data = $(data);
                _data.jh_dialog();
                
                var _form = _data.find('form');
                _form.find('input[jh_placeholder]').jh_placeholder();
                
                // _callback_login
                var _callback_login = function (data) {
                    var _label = _form.find('.text-danger').first();
                    _label.text(data.msg);
                    if(data.success){
                        _label.addClass('text-success');
                        setTimeout(function () {location.reload();}, 2000);
                    }                    
                }
                
                _form.ajaxForm({success: _callback_login, dataType: 'json'});
            };
            
            $.post('?c=dialog&type=pop_login', {}, _callback_pop_login, 'html');
        }
        
    }    
});


// ================================ function ================================

function check_login(bLogin) {
    if(this.bLogin === undefined)
        this.bLogin = !(bLogin === false);
    else if(!this.bLogin){
        $.alert("请先登录");
        $.jh_button_reset();
    }
    return this.bLogin;
}

});