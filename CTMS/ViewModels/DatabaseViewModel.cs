namespace CTMS.ViewModels
{
    public class DatabaseViewModel
    {
        public int Id { get; set; }

        public string? DatabaseName { get; set; }
        public string? TableName { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
