using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSLinksAPI.Models
{
    [Table("ApplicationCategory")]
    public class ApplicationCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationCategoryId { get; set; }
        public string ApplicationCategoryName { get; set; }
    }
}
