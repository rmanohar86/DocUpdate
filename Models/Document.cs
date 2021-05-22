namespace DocUpdate2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Document
    {
        public Guid Id { get; set; }

        public Guid? PropertyId { get; set; }

        [StringLength(20)]
        public string DocType { get; set; }

        [StringLength(40)]
        public string FileName { get; set; }

        public byte[] DocBlob { get; set; }
    }
}
