using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
	public class MainPageViewModel
	{
		public TaskToDo NewTask { get; set; }
		public List<TaskToDo> Tasks { get; set; }
		public List<Category> Categories { get; set; }
		public string RepositoryType { get; set; }

		public MainPageViewModel()
		{
			Tasks = new List<TaskToDo>();
			Categories = new List<Category>();
		}
	}
}
