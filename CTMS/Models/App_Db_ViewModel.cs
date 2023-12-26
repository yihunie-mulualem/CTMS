using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTMS.Models
{
    public class App_Db_ViewModel
    {
        [Key]
        public int Id { get; set; }
        public int RequestId { get; set; }
        // Application
        [DisplayName("App Name")]
        public string? ApplicationName { get; set; }
        [DisplayName("Page Name")]
        public string? PageName { get; set; }
        [DisplayName("Application Description")]
        public string? AppDescription { get; set; }
        [NotMapped]
        public IFormFile? SourceCodeFile { get; set; }
        public string? SourceCodeFileName { get; set; }
        public string? SourceCodeFileURL { get; set; }

        // Batabase
        [DisplayName("DB Name")]
        public string? DatabaseName { get; set; }
        [DisplayName("Table Name")]
        public string? TableName { get; set; }
        [DisplayName("Field Name")]
        public string? FieldName { get; set; }
        [DisplayName("Database Description")]
        public string? DbDescription { get; set; }
        [NotMapped]
        public IFormFile? DatabaseSchemaFile { get; set; }
        public string? DatabaseSchemaFileName { get; set; }
        public string? DatabaseSchemaFileURL { get; set; }
        // for Change View Only
        [DisplayName("Application Version")]
        public string? ApplicationVersion { get; set; }
        public string? Description { get; set; }

        [DisplayName("Assined To")]
        public string? AssignedTo { get; set; }
        [DisplayName("Modified By")]
        public string? ModifiedBy { get; set; }

        [DisplayName("Modified Date")]
        public DateTime? ModifiedDate { get; set; }
       // public int isSign { get; set; }
        public int SignApprove { get; set; }

    }
}
