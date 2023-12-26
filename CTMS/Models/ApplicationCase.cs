using System.ComponentModel.DataAnnotations.Schema;

namespace CTMS.Models
{
    public class ApplicationCase
    {
        public int Id { get; set; }
        public string? ApplicationName { get; set; }
        public string? PageName { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile? SourceCodeFile { get; set; }
        public string? SourceCodeFileName { get; set; }
        public string? SourceCodeFileURL { get; set; }
        //  R/n
        public RequestForm RequestForm { get; set; }

    }
}
