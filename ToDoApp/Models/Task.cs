using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Status")]
        public bool IsCompleted { get; set; }
        //public List<TaskCategory> TaskCategories { get; set; }
        public List<Category> Categories { get; set; }
    }
}
