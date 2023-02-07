using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class editTaskModel
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime updatedOn { get; set; }
        [Required]
        public string status { get; set; }

    }
}
