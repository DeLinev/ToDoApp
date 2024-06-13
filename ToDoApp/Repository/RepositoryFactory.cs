using Microsoft.Extensions.Configuration;
using ToDoApp.Data;

namespace ToDoApp.Repository
{
	public class RepositoryFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public RepositoryFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IRepository CreateRepository(HttpContext context)
		{
			string? storageType = context.Items["StorageType"]?.ToString();

			switch(storageType)
			{
				case "xml":
					return _serviceProvider.GetService<XmlRepository>();
				case "db":
					return _serviceProvider.GetService<DapperRepository>();
				default:
					return _serviceProvider.GetService<DapperRepository>();
			}
		}

		public IRepository CreateRepository(string type)
		{
			if (type == "Dapper")
			{
				return _serviceProvider.GetService<DapperRepository>();
			}
			else if (type == "XML")
			{
				return _serviceProvider.GetService<XmlRepository>();
			}
			else
			{
				throw new ArgumentException("Invalid repository type");
			}
		}
	}
}
