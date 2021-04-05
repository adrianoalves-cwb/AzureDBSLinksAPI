using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSLinksAPI.Models
{
    [Table("DealerContact")]
    public class DealerContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DealerContactId { get; set; }
        public int MainDealerId { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string JobRole { get; set; }
        public string Department { get; set; }
        public int CountryId { get; set; }

        [NotMapped]
        public string DealerName { get; set; }
 
        [NotMapped]
        public string CountryName { get; set; }
    }
}
