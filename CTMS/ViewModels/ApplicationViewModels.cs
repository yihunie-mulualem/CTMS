using MessagePack;

namespace CTMS.ViewModels
{
    public class ApplicationViewModels
    {
      
        public int Id { get; set; }
        public string? ApplicationName { get; set; }
        public string? PageName { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
