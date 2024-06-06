﻿using ToDoApp.Models;

namespace ToDoApp.Repository
{
    public interface IRepository
    {
        List<TaskToDo> GetAllTasks();
        TaskToDo GetTask(int id);
		List<Category> GetAllCategories();
        void Add(TaskToDo entity, int[] categoriesId);
        void Complete(int entityId);
        void Delete(int entityId);
    }
}
