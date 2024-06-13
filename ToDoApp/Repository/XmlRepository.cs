using ToDoApp.Models;
using System.Xml.Linq;

namespace ToDoApp.Repository
{
	public class XmlRepository : IRepository
	{
		private readonly string _tasksPath;
		private readonly string _categoriesPath;

		public XmlRepository(IConfiguration XmlConfiguration)
		{
			_tasksPath = XmlConfiguration.GetConnectionString("TasksPath");
			_categoriesPath = XmlConfiguration.GetConnectionString("CategoriesPath");
		}

		public int Add(TaskToDo entity, int[] categoriesId)
		{
			entity.CreationDate = DateTime.Now;
			XDocument tasksDoc = XDocument.Load(_tasksPath);

			Guid newGuid = Guid.NewGuid();
			int newTaskId = newGuid.GetHashCode();

			XElement taskElement = new XElement("task",
				new XAttribute("id", newTaskId),
				new XAttribute("isCompleted", entity.IsCompleted),
				new XElement("title", entity.Title),
				new XElement("description", entity.Description),
				new XElement("creationDate", entity.CreationDate),
				new XElement("dueDate", entity.DueDate),
				new XElement("categories",
					from categoryId in categoriesId
					select new XElement("category",
						new XAttribute("id", categoryId)
					)
				)
			);

			tasksDoc.Root.Add(taskElement);
			tasksDoc.Save(_tasksPath);

			return newTaskId;
		}

		public void Complete(int entityId)
		{
			XDocument tasksDoc = XDocument.Load(_tasksPath);
			XElement taskElement = tasksDoc.Root.Elements("task").FirstOrDefault(t => t.Attribute("id").Value == entityId.ToString());
			taskElement.Attribute("isCompleted").Value = "true";
			tasksDoc.Save(_tasksPath);
		}

		public void Delete(int entityId)
		{
			XDocument tasksDoc = XDocument.Load(_tasksPath);
			XElement taskElement = tasksDoc.Root.Elements("task").FirstOrDefault(t => t.Attribute("id").Value == entityId.ToString());
			taskElement.Remove();
			tasksDoc.Save(_tasksPath);
		}

		public List<Category> GetAllCategories()
		{
			XDocument doc = XDocument.Load(_categoriesPath);
			List<Category> categories = new List<Category>();

			foreach (XElement categoryElement in doc.Root.Elements("category"))
			{
				Category category = new Category();
				category.Id = int.Parse(categoryElement.Attribute("id").Value);
				category.Name = categoryElement.Element("name").Value;
				categories.Add(category);
			}

			return categories;
		}

		public List<TaskToDo> GetAllTasks()
		{
			XDocument doc = XDocument.Load(_tasksPath);
			List<TaskToDo> tasks = new List<TaskToDo>();

			foreach (XElement taskElement in doc.Root.Elements("task"))
			{
				TaskToDo task = new TaskToDo();
				task.Id = int.Parse(taskElement.Attribute("id").Value);
				task.IsCompleted = bool.Parse(taskElement.Attribute("isCompleted").Value);
				task.Title = taskElement.Element("title").Value;
				task.Description = taskElement.Element("description").Value;
				task.CreationDate = DateTime.Parse(taskElement.Element("creationDate").Value);

				bool isSuccess = DateTime.TryParse(taskElement.Element("dueDate").Value, out DateTime dueDate);
				task.DueDate = isSuccess ? dueDate : null;

				List<Category> categories = new List<Category>();
				foreach (XElement categoryElement in taskElement.Element("categories").Elements("category"))
				{
					Category category = new Category();
					category.Id = int.Parse(categoryElement.Attribute("id").Value);

					XDocument categoriesDoc = XDocument.Load(_categoriesPath);
					XElement categoryNameElement = categoriesDoc.Root.Elements("category")
						.FirstOrDefault(c => c.Attribute("id").Value == category.Id.ToString());
					category.Name = categoryNameElement.Element("name").Value;

					categories.Add(category);
				}

				task.Categories = categories;
				tasks.Add(task);
			}

			return tasks;
		}

		public async Task<IDictionary<int, List<Category>>> GetCategoriesAsync(IEnumerable<int> taskIds, CancellationToken cancellationToken)
		{
			var taskCategories = new Dictionary<int, List<Category>>();

			XDocument doc = XDocument.Load(_tasksPath);
			List<TaskToDo> tasks = new List<TaskToDo>();

			foreach (XElement taskElement in doc.Root.Elements("task"))
			{
				int taskId = int.Parse(taskElement.Attribute("id").Value);

				if (!taskIds.Contains(taskId))
					continue;

				if (!taskCategories.ContainsKey(taskId))
				{
					taskCategories[taskId] = new List<Category>();
				}

				foreach (XElement categoryElement in taskElement.Element("categories").Elements("category"))
				{
					Category category = new Category();
					category.Id = int.Parse(categoryElement.Attribute("id").Value);

					XDocument categoriesDoc = XDocument.Load(_categoriesPath);
					XElement categoryNameElement = categoriesDoc.Root.Elements("category")
						.FirstOrDefault(c => c.Attribute("id").Value == category.Id.ToString());
					category.Name = categoryNameElement.Element("name").Value;

					taskCategories[taskId].Add(category);
				}
			}

			return taskCategories;
		}

		public TaskToDo GetTask(int id)
		{
			XDocument doc = XDocument.Load(_tasksPath);
			XElement taskElement = doc.Root.Elements("task")
				.FirstOrDefault(t => t.Attribute("id").Value == id.ToString());

			if (taskElement == null)
				return null;

			TaskToDo task = new TaskToDo();

			task.Id = int.Parse(taskElement.Attribute("id").Value);
			task.IsCompleted = bool.Parse(taskElement.Attribute("isCompleted").Value);
			task.Title = taskElement.Element("title").Value;
			task.Description = taskElement.Element("description").Value;
			task.CreationDate = DateTime.Parse(taskElement.Element("creationDate").Value);

			bool isSuccess = DateTime.TryParse(taskElement.Element("dueDate").Value, out DateTime dueDate);
			task.DueDate = isSuccess ? dueDate : null;

			List<Category> categories = new List<Category>();
			foreach (XElement categoryElement in taskElement.Element("categories").Elements("category"))
			{
				Category category = new Category();
				category.Id = int.Parse(categoryElement.Attribute("id").Value);

				XDocument categoriesDoc = XDocument.Load(_categoriesPath);
				XElement categoryNameElement = categoriesDoc.Root.Elements("category")
					.FirstOrDefault(c => c.Attribute("id").Value == category.Id.ToString());
				category.Name = categoryNameElement.Element("name").Value;

				categories.Add(category);
			}

			task.Categories = categories;

			return task;
		}
	}
}
