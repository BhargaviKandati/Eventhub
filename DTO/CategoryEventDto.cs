namespace Eventhub.DTO
{
    public class CategoryEventDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int VenueId { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
