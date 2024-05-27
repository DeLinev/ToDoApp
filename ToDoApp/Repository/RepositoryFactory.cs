using ToDoApp.Data;

namespace ToDoApp.Repository
{
	public static class RepositoryFactory
	{
		public static IDapperContext DapperContext { get; set; }

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
				return new XmlRepository();
			}
			else
			{
				throw new ArgumentException("Invalid repository type");
			}
		}
	}
}
