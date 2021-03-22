using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSLinksAPI.Models
{
    [Table("Dealer")]
    public class Dealer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DealerId { get; set; }
        public int MainDealerId { get; set; }
        public int CountryCode { get; set; }
        public int CTDI { get; set; }
        public string DealerName { get; set; }
        public string Branch { get; set; }
        public string PhoneNumber { get; set; }
        public string BaldoPartner { get; set; }
        public Boolean IsActive { get; set; }
    }
}
