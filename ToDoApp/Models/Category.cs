﻿namespace ToDoApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<TaskCategory> TaskCategories { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
