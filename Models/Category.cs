using System.ComponentModel.DataAnnotations;

namespace Eventhub.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        //public Event Events { get; set; }
    }
}
