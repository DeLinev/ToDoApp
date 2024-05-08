using Microsoft.AspNetCore.Mvc;
using ToDoApp.Data;
using Dapper;
using ToDoApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            List<Models.Task> tasks;
            List<TaskCategory> tasksCategories;

            using (var connection = _dapperContext.CreateConnection())
            {
                tasks = connection.Query<Models.Task>("SELECT * FROM Tasks").ToList();
                tasksCategories = connection.Query<TaskCategory>("SELECT * FROM TasksCategories").ToList();
            }

            foreach (var task in tasks)
            {
                task.Categories = new List<Category>();
                foreach (var taskCategory in tasksCategories)
                {
                    if (taskCategory.TaskId == task.Id)
                    {
                        using (var connection = _dapperContext.CreateConnection())
                        {
                            task.Categories.Add(connection.QuerySingle<Category>("SELECT * FROM Categories WHERE Id = @Id", new { Id = taskCategory.CategoryId }));
                        }
                    }
                }
            }

            return View(tasks);
        }

        public ActionResult Create()
        {
            var categories = new List<Category>();
            using (var connection = _dapperContext.CreateConnection())
            {
                categories = connection.Query<Category>("SELECT * FROM Categories").ToList();
            }

			ViewBag.Categories = new SelectList(categories, "Id", "Name");
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Task task)
        {
            try
            {
                task.CreationDate = DateTime.Now;

				using (var connection = _dapperContext.CreateConnection())
				{
					int taskId = connection.QuerySingle<int>("INSERT INTO Tasks (Title, Description, CreationDate, DueDate, IsCompleted) " +
                                                             "OUTPUT INSERTED.Id " +
                                                             "VALUES (@Title, @Description, @CreationDate, @DueDate, @IsCompleted)", task);

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
            catch
            {
                return View(task);
            }
        }

        public ActionResult Complete(int id)
        {
			using (var connection = _dapperContext.CreateConnection())
            {
				connection.Execute("UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id", new { Id = id });
			}

			return RedirectToAction(nameof(Index));
		}

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
