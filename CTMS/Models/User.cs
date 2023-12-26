using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTMS.Models
{
    public class User
    {
        public int Id { get; set; }
       // [Required]
        public string FullName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 3)]
        public string? Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 3)]
        public string? NewPassword { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 3)]
        public string? ConfiermPassword { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool viewStatus { get; set; } = true;
        // R/n
        [Display(Name = "Role")]
        public virtual int RoleId { get; set; }
        public Role Role { get; set; }



    }
}
