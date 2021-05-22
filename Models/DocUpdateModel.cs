using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DocUpdate2.Models
{
    public partial class DocUpdateModel : DbContext
    {
        public DocUpdateModel()
            : base("name=DocUpdateModel")
        {
        }

        public virtual DbSet<DocStatusView> DocStatusViews { get; set; }
        public virtual DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
