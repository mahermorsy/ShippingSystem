namespace Domains.Models
{
    public class BaseTable
    {
        public Guid Id { get; set; }
        public Guid? UpdatedBy { get; set; }
        public int CurrentState { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
