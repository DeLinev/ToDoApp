using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Data;
using Dapper;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly IDapperContext _dapperContext;

        public TasksController(IDapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        // GET: TasksController
        public ActionResult Index()
        {
            List<ToDoApp.Models.Task> tasks = new List<ToDoApp.Models.Task>();
            List<TaskCategory> tasksCategories = new List<TaskCategory>();

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
                            task.Categories.Add(connection.QueryFirstOrDefault<Category>("SELECT * FROM Categories WHERE Id = @Id", new { Id = taskCategory.CategoryId }));
                        }
                    }
                }
            }

            return View(tasks);
        }

        // GET: TasksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TasksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TasksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: TasksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TasksController/Edit/5
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

        // GET: TasksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TasksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
