namespace DocUpdate2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DocStatusView")]
    public partial class DocStatusView
    {
        [Key]
        public Guid PropertyId { get; set; }

        public bool Agreement { get; set; }

        public bool Appraisal { get; set; }

        public bool SiteMap { get; set; }

        public bool Resume { get; set; }

        public bool Paperwork { get; set; }
    }
}
