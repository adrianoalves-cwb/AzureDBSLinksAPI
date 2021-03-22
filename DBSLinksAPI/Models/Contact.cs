using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSLinksAPI.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        public string ContactUserId { get; set; }
        public string ContactName { get; set; }
        public string ComputerName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public int TeamId { get; set; }

        [NotMapped]
        public string TeamName { get; set; }
    }
}
