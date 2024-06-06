using Dapper;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Repository
{
	public class DapperRepository : IRepository
	{
		private readonly IDapperContext _dapperContext;

		public DapperRepository(IDapperContext dapperContext)
		{
			_dapperContext = dapperContext;
		}

		public void Add(TaskToDo entity, int[] categoriesId)
		{
			entity.CreationDate = DateTime.Now;

			using (var connection = _dapperContext.CreateConnection())
			{
				int taskId = connection.QuerySingle<int>("INSERT INTO Tasks (Title, Description, CreationDate, DueDate, IsCompleted) " +
														 "OUTPUT INSERTED.Id " +
														 "VALUES (@Title, @Description, @CreationDate, @DueDate, @IsCompleted)", entity);

				if (categoriesId == null)
					return;

				foreach (var categoryId in categoriesId)
				{
					connection.Execute("INSERT INTO TasksCategories (TaskId, CategoryId) VALUES (@TaskId, @CategoryId)", new { TaskId = taskId, CategoryId = categoryId });
				}
			}
		}

		public void Delete(int entityId)
		{
			using (var connection = _dapperContext.CreateConnection())
			{
				connection.Execute("DELETE FROM TasksCategories WHERE TaskId = @Id", new { Id = entityId });
				connection.Execute("DELETE FROM Tasks WHERE Id = @Id", new { Id = entityId });
			}
		}

		public List<TaskToDo> GetAllTasks()
		{
			List<TaskToDo> tasks;
			List<TaskCategory> tasksCategories;

			using (var connection = _dapperContext.CreateConnection())
			{
				tasks = connection.Query<TaskToDo>("SELECT Id, Title, Description, CreationDate, DueDate, IsCompleted FROM Tasks").ToList();
				tasksCategories = connection.Query<TaskCategory>("SELECT TaskId, CategoryId FROM TasksCategories").ToList();

				foreach (var task in tasks)
				{
					task.Categories = new List<Category>();

					foreach (var taskCategory in tasksCategories)
					{
						if (taskCategory.TaskId == task.Id)
						{
							task.Categories.Add(connection.QuerySingle<Category>("SELECT Id, Name FROM Categories WHERE Id = @Id", new { Id = taskCategory.CategoryId }));
						}
					}
				}
			}

			return tasks;
		}

		public List<Category> GetAllCategories()
		{
			using (var connection = _dapperContext.CreateConnection())
			{
				return connection.Query<Category>("SELECT Id, Name FROM Categories").ToList();
			}
		}

		public void Complete(int entityId)
		{
			using (var connection = _dapperContext.CreateConnection())
			{
				connection.Execute("UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id", new { Id = entityId });
			}
		}

		public TaskToDo GetTask(int id)
		{
			using (var connection = _dapperContext.CreateConnection())
			{
				TaskToDo? task = connection.QuerySingleOrDefault<TaskToDo>("SELECT Id, Title, Description, CreationDate, DueDate, IsCompleted FROM Tasks WHERE Id = @Id", new { Id = id });

				if (task == null)
					return null;

				List<TaskCategory> tasksCategories = connection.Query<TaskCategory>("SELECT TaskId, CategoryId FROM TasksCategories WHERE TaskId = @Id", new { Id = id }).ToList();

				task.Categories = new List<Category>();

				foreach (var taskCategory in tasksCategories)
				{
					if (taskCategory.TaskId == task.Id)
					{
						task.Categories.Add(connection.QuerySingle<Category>("SELECT Id, Name FROM Categories WHERE Id = @Id", new { Id = taskCategory.CategoryId }));
					}
				}

				return task;
			}
		}
	}
}
