namespace Eventhub.DTO
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class CategoryCreateUpdateDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

}
