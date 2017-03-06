namespace WxhnecServer.Models
{
    using Logics.Attributes;
    using Logics.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wxhnec.pre_activity_detail")]
    public partial class pre_activity_detail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long? id { get; set; }

        [TField(TF.title, "描述")]
        [TValidate(TV.required)]
        public string content { get; set; }

        [TField(TF.title, "图片")]
        public string pictures { get; set; }

        public virtual pre_activity pre_activity { get; set; }
    }
}
