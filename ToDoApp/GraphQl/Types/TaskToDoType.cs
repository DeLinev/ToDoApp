using ToDoApp.Models;
using GraphQL.Types;
using GraphQL.DataLoader;
using ToDoApp.Repository;

namespace ToDoApp.GraphQl.Types
{
	public class TaskToDoType : ObjectGraphType<TaskToDo>
	{
		public TaskToDoType(IDataLoaderContextAccessor accessor, RepositoryFactory repositoryFactory, IHttpContextAccessor httpContextAccessor)
		{
			Field(x => x.Id).Description("Task id");
			Field(x => x.Title, nullable: true).Description("Task title");
			Field(x => x.Description).Description("Task description");
			Field(x => x.CreationDate).Description("Task creation date");
			Field(x => x.DueDate, nullable: true).Description("Task due date");
			Field(x => x.IsCompleted).Description("Task status");
			Field<ListGraphType<CategoryType>>("categories")
				.Resolve(context =>
				{
					var repository = repositoryFactory.CreateRepository(httpContextAccessor.HttpContext);
					var loader = accessor.Context.GetOrAddBatchLoader<int, List<Category>>(
						"GetCategories", repository.GetCategoriesAsync);
					return loader.LoadAsync(context.Source.Id);
				})
				.Description("Task categories");
		}
	}
}
