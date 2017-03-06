namespace WxhnecServer.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class JDb : DbContext
    {
        public JDb()
            : base("name=JDb") {
        }

        public virtual DbSet<pre_activity> pre_activity { get; set; }
        public virtual DbSet<pre_activity_detail> pre_activity_detail { get; set; }
        public virtual DbSet<pre_config> pre_config { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            Configuration.LazyLoadingEnabled = false;

            modelBuilder.Entity<pre_activity>()
                .HasRequired(t => t.pre_activity_detail)
                .WithRequiredPrincipal(t => t.pre_activity);

        }
    }
}
