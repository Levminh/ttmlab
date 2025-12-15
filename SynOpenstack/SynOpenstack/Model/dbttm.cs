namespace SynOpenstack.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class dbttm : DbContext
    {
        public dbttm()
            : base("name=dbttm")
        {
        }

        public virtual DbSet<tbComputer> tbComputers { get; set; }
        public virtual DbSet<tbProjectOpenStack> tbProjectOpenStacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbProjectOpenStack>()
                .HasMany(e => e.tbComputers)
                .WithOptional(e => e.tbProjectOpenStack)
                .WillCascadeOnDelete();
        }
    }
}
