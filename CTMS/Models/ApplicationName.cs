using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CTMS.Models
{
    public class ApplicationName
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? DatabaseName { get; set; }
        [Required]
        public string? Description { get; set; }
        [DefaultValue(true)]
        public bool viewStatus { get; set; } = true;

    }
}
