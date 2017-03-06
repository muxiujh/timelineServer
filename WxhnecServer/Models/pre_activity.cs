namespace WxhnecServer.Models
{
    using Logics.Attributes;
    using Logics.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wxhnec.pre_activity")]
    [TEntity("活动")]
    public partial class pre_activity
    {
        //[TElement(TE.select, "yesorno")]
        [TField(TF.title, "隐藏")]
        [Column(TypeName = "uint")]
        public long? is_del { get; set; }

        [Column(TypeName = "uint")]
        public long? id { get; set; }

        [TField(TF.reserved)]
        public int? ctime { get; set; }

        [TField(TF.reserved)]
        [Column(TypeName = "uint")]
        public long? status { get; set; }

        [TElement(TE.date)]
        [TField(TF.title, "日期")]
        public DateTime? date { get; set; }

        [TField(TF.title, "地点")]
        public string place { get; set; }

        [TField(TF.title, "名称")]
        [TValidate(TV.minlength, 3)]
        [TValidate(TV.required)]
        //[TValidate(TV.email)]
        //[TValidate(TV.maxlength, 3)]
        public string title { get; set; }

        [TElement(TE.textarea)]
        [TField(TF.title, "摘要")]
        public string summary { get; set; }

        [TField(TF.title, "原文链接")]
        public string url { get; set; }

        [TElement(TE.sub)]
        public virtual pre_activity_detail pre_activity_detail { get; set; }
    }
}
