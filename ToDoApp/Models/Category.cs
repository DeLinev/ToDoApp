namespace ToDoApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskToDo> Tasks { get; set; }

        public Category() { }
    }
}
