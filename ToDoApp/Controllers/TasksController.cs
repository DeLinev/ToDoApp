using Microsoft.AspNetCore.Mvc;
using ToDoApp.Data;
using ToDoApp.ViewModels;
using ToDoApp.Repository;

namespace ToDoApp.Controllers
{
	public class TasksController : Controller
	{
		private IRepository _repository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public TasksController(IDapperContext dapperContext, IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
			RepositoryFactory.DapperContext = dapperContext;
			_repository = RepositoryFactory.CreateRepository(_httpContextAccessor.HttpContext.Session.GetString("repositoryType") ?? "Dapper");
		}

		public ActionResult Index()
		{
			MainPageViewModel mainPage = new MainPageViewModel();

			mainPage.RepositoryType = _httpContextAccessor.HttpContext.Session.GetString("repositoryType") ?? "Dapper";
			mainPage.Tasks = _repository.GetAllTasks();
			mainPage.Categories = _repository.GetAllCategories();

			return View(mainPage);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(MainPageViewModel mainPage, int[] categories)
		{
			_repository.Add(mainPage.NewTask, categories);

			return RedirectToAction(nameof(Index));
		}

		public ActionResult Complete(int id)
		{
			_repository.Complete(id);

			return RedirectToAction(nameof(Index));
		}

		public ActionResult Delete(int id)
		{
			_repository.Delete(id);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public IActionResult ChangeRepository(string repositoryType)
		{
			HttpContext.Session.SetString("repositoryType", repositoryType);
			_repository = RepositoryFactory.CreateRepository(repositoryType);
			return RedirectToAction(nameof(Index));
		}
	}
}
