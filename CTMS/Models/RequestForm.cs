using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CTMS.Models
{
    public class RequestForm
    {
        public int Id { get; set; }
        [DisplayName("App_Name")]
        public string? ApplicationName { get; set; }
        [DisplayName("Db_Name")]
        public string? DatabaseName { get; set; }
        [DisplayName("Requested By")]
        public string? RequestedBy { get; set; }
        [DisplayName("Department")]
        public string? Department { get; set; }
        [DisplayName("Request Description")]
      
        public string? Description { get; set; }
        [DisplayName("Title")]
        public string? ResonForChange { get; set; }
        public string? Remark { get; set; }
        public string? Status { get; set; } = "Resolved";
        public string? ChangeType { get; set; }
        [DisplayName("Application Version")]
        public string? ApplicationVersion { get; set; }
        public string? AssignedTo { get; set; }
        public string? CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }
        //

       // public int isSign { get; set; }
        public int SignApprove { get; set; }
        // RelationShips
        public ICollection<DatabaseCase> DatabaseCases { get; set; }
        public ICollection<ApplicationCase> ApplicationCases { get; set; }
   
        // for Multiple select DD
        [NotMapped]
        public IEnumerable<Department> DepartmentCollection { get; set; }
        [NotMapped]
        public string[] SelectedDepartment { get; set; }
    }
}
