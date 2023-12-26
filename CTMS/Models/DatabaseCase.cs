using System.ComponentModel.DataAnnotations.Schema;

namespace CTMS.Models
{
    public class DatabaseCase
    {
        public int Id { get; set; }

        public string? DatabaseName { get; set; }
        public string? TableName { get; set; }
       // public string? FieldName { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public IFormFile? DatabaseSchemaFile { get; set; }
        public string? DatabaseSchemaFileName { get; set; }
        public string? DatabaseSchemaFileURL { get; set; }

        // R/Ship
        public RequestForm RequestForm { get; set; }

    }
}
