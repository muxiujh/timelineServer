/*
	user_event.js
	
*/


$(function(){

// ================================ events ================================
$.jh_add_events({
    clear_new : {
        click : function (_this) {            
            _this.find('.text-danger').addClass('hidden');
        },
        load : function (_this) {            
            _this.find('a').attr('target', '_blank');
        }
    },
    msg_detail : {
        click : function (_this) {
            location.href = _this.attr('href'); 
        }
    },
    user_follow : {
        click : function (_this) {
            var param = $.jh_arg(_this);
            
            var _callback = function (data) {
                $.alert({text:data.msg, url: 'reload'});
            };
            $.post('?c=agent&rc=friend&ra=follow', {uid: param.uid}, _callback, 'json');
        }
    },
    user_nofollow : {
        click : function (_this) {
            var param = $.jh_arg(_this);
            
            var _callback = function (data) {
                $.alert({text:data.msg, url: 'reload'});
            };
            $.post('?c=agent&rc=friend&ra=nofollow', {uid: param.uid}, _callback, 'json');
        }
    }
});

});