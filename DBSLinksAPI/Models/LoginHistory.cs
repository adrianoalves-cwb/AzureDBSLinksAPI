using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSLinksAPI.Models
{
    [Table("LoginHistory")]
    public class LoginHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginHistoryId { get; set; }
        public string UserName { get; set; }
        public string ComputerName { get; set; }
        public DateTime LoginDateTimeUTC { get; set; }
        public string IPAddress { get; set; }
        public string LoginStatus { get; set; }
        public string LogDescription { get; set; }
        [StringLength(20)]
        public string UserAppVersion { get; set; }
    }
}
