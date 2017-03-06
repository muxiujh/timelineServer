using WxhnecServer.Logics.Attributes;

namespace WxhnecServer.Logics.Enums
{
    //
    // Summary:
    //      TValidate enum
    //
    public enum TV
    {
        [TVConfig("字段不能为空")]
        required,

        [TVConfig("最少{0}个字符")]
        minlength,

        [TVConfig("最多{0}个字符")]
        maxlength,

        [TVConfig("邮箱格式不正确", @"^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$")]
        email,

        [TVConfig("身份证号码不正确", @"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)")]
        idcard,

        [TVConfig("电话号码不正确", @"^([0-9]|[-])+$")]
        phone,

        [TVConfig("手机号码不正确", @"^[1][3-8]\d{9}$")]
        mobile,

        [TVConfig("必须为数字", @"^[0-9]*$")]
        number,

        [TVConfig("仅允许输入英文、数字、下划线", @"^[a-zA-Z0-9_]*$")]
        name,

        [TVConfig("仅允许输入中文、英文、数字、下划线", @"^([\u4E00-\uFA29]|[\uE7C7-\uE7F3]|[a-zA-Z0-9_])*$")]
        uname,

        [TVConfig("日期格式不正确", @"^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$")]
        dateiso
    }

}