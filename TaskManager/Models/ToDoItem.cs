using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class ToDoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public string ID { get; set; } = $"{Guid.NewGuid()}{Guid.NewGuid()}";
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime createdOn { get; set; }
        [Required]
        public DateTime updatedOn { get; set; }
        [Required]

        public string status { get; set; }
    }
}
