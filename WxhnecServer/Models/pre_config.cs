namespace WxhnecServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wxhnec.pre_config")]
    public partial class pre_config
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [StringLength(30)]
        public string name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string value { get; set; }

        [Column(TypeName = "uint")]
        public long? type { get; set; }

        [StringLength(30)]
        public string title { get; set; }

        [StringLength(100)]
        public string desc { get; set; }

        [Column(TypeName = "uint")]
        public long? sort { get; set; }

        [StringLength(10)]
        public string cate { get; set; }

        public DateTime? ctime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ctime2 { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? ctime3 { get; set; }

        public TimeSpan? ctime4 { get; set; }

        [Column(TypeName = "year")]
        public short? ctime5 { get; set; }
    }
}
