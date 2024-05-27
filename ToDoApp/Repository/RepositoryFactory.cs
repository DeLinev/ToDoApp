using Microsoft.Extensions.Configuration;
using ToDoApp.Data;

namespace ToDoApp.Repository
{
	public static class RepositoryFactory
	{
		public static IDapperContext DapperContext { get; set; }
		public static IConfiguration XmlConfiguration { get; set; }

		public static IRepository CreateRepository(string type)
		{
			if (type == "Dapper")
			{
				if (DapperContext == null)
					throw new ArgumentNullException("DapperContext is not set");

				return new DapperRepository(DapperContext);
			}
			else if (type == "XML")
			{
				if (XmlConfiguration == null)
					throw new ArgumentNullException("Configuration is not set");

				string tasksPath = XmlConfiguration.GetConnectionString("TasksPath");
				string categoriesPath = XmlConfiguration.GetConnectionString("CategoriesPath");

				return new XmlRepository(tasksPath, categoriesPath);
			}
			else
			{
				throw new ArgumentException("Invalid repository type");
			}
		}
	}
}
