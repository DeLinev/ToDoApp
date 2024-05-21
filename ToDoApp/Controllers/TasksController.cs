using Microsoft.AspNetCore.Mvc;
using ToDoApp.Data;
using Dapper;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Controllers
{
	public class TasksController : Controller
	{
		private readonly IDapperContext _dapperContext;

		public TasksController(IDapperContext dapperContext)
		{
			_dapperContext = dapperContext;
		}

		public ActionResult Index()
		{
			MainPageViewModel mainPage = new MainPageViewModel();
			List<TaskCategory> tasksCategories;

			using (var connection = _dapperContext.CreateConnection())
			{
				mainPage.Tasks = connection.Query<TaskToDo>("SELECT Id, Title, Description, CreationDate, DueDate, IsCompleted FROM Tasks").ToList();
				tasksCategories = connection.Query<TaskCategory>("SELECT TaskId, CategoryId FROM TasksCategories").ToList();


				foreach (var task in mainPage.Tasks)
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

				mainPage.Categories = connection.Query<Category>("SELECT Id, Name FROM Categories").ToList();
			}

			return View(mainPage);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(MainPageViewModel mainPage)
		{
			mainPage.NewTask.CreationDate = DateTime.Now;

			using (var connection = _dapperContext.CreateConnection())
			{
				int taskId = connection.QuerySingle<int>("INSERT INTO Tasks (Title, Description, CreationDate, DueDate, IsCompleted) " +
														 "OUTPUT INSERTED.Id " +
														 "VALUES (@Title, @Description, @CreationDate, @DueDate, @IsCompleted)", mainPage.NewTask);

				foreach (var item in Request.Form["Categories"])
				{
					bool isSuccess = int.TryParse(item, out int categoryId);

					if (isSuccess)
					{
						connection.Execute("INSERT INTO TasksCategories (TaskId, CategoryId) VALUES (@TaskId, @CategoryId)", new { TaskId = taskId, CategoryId = categoryId });
					}
				}
			}


			return RedirectToAction(nameof(Index));
		}

		public ActionResult Complete(int id)
		{
			using (var connection = _dapperContext.CreateConnection())
			{
				connection.Execute("UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id", new { Id = id });
			}

			return RedirectToAction(nameof(Index));
		}

		public ActionResult Delete(int id)
		{
			using (var connection = _dapperContext.CreateConnection())
			{
				connection.Execute("DELETE FROM TasksCategories WHERE TaskId = @Id", new { Id = id });
				connection.Execute("DELETE FROM Tasks WHERE Id = @Id", new { Id = id });
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
