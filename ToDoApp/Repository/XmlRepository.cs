using ToDoApp.Models;
using System.Xml.Linq;

namespace ToDoApp.Repository
{
	public class XmlRepository : IRepository
	{
		public void Add(TaskToDo entity, int[] categoriesId)
		{
			entity.CreationDate = DateTime.Now;
			XDocument tasksDoc = XDocument.Load("Data/Tasks.xml");

			XElement lastTask = tasksDoc.Root.Elements("task").LastOrDefault();
			int lastTaskId = lastTask != null ? int.Parse(lastTask.Attribute("id").Value) : 0;

			XElement taskElement = new XElement("task",
				new XAttribute("id", ++lastTaskId),
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
			tasksDoc.Save("Data/Tasks.xml");
		}

		public void Complete(int entityId)
		{
			XDocument tasksDoc = XDocument.Load("Data/Tasks.xml");
			XElement taskElement = tasksDoc.Root.Elements("task").FirstOrDefault(t => t.Attribute("id").Value == entityId.ToString());
			taskElement.Attribute("isCompleted").Value = "true";
			tasksDoc.Save("Data/Tasks.xml");
		}

		public void Delete(int entityId)
		{
			XDocument tasksDoc = XDocument.Load("Data/Tasks.xml");
			XElement taskElement = tasksDoc.Root.Elements("task").FirstOrDefault(t => t.Attribute("id").Value == entityId.ToString());
			taskElement.Remove();
			tasksDoc.Save("Data/Tasks.xml");
		}

		public List<Category> GetAllCategories()
		{
			XDocument doc = XDocument.Load("Data/Categories.xml");
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
			XDocument doc = XDocument.Load("Data/Tasks.xml");
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
					
					XDocument categoriesDoc = XDocument.Load("Data/Categories.xml");
					XElement categoryNameElement = categoriesDoc.Root.Elements("category").FirstOrDefault(c => c.Attribute("id").Value == category.Id.ToString());
					category.Name = categoryNameElement.Element("name").Value;

					categories.Add(category);
				}

				task.Categories = categories;
				tasks.Add(task);
			}

			return tasks;
		}
	}
}
