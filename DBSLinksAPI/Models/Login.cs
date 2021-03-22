using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSLinksAPI.Models
{
    [Table("Login")]
    public class Login
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginId { get; set; }

        public string UserName { get; set; }

        public string ComputerName { get; set; }

        [StringLength(20)]
        public string UserRole { get; set; }

        public DateTime LastLoginDateTime { get; set; }

        public Boolean IsContactUpdateAvailable { get; set; }
        public Boolean IsDealerUpdateAvailable { get; set; }
        public Boolean IsApplicationLinksUpdateAvailable { get; set; }
        public Boolean IsUserActive{ get; set; }

        [NotMapped]
        [StringLength(20)]
        public string UserAppVersion { get; set; }
    }
}
