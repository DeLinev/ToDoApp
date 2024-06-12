using ToDoApp.Models;
using GraphQL.Types;
using ToDoApp.Repository;
using GraphQL.DataLoader;

namespace ToDoApp.GraphQl.Types
{
	public class TaskToDoType : ObjectGraphType<TaskToDo>
	{
		public TaskToDoType(IDataLoaderContextAccessor dataLoaderAccessor, IRepository repository)
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
					var loader = context.RequestServices.GetRequiredService<CategoryDataLoader>();
					return loader.LoadAsync(context.Source.Id);
				})
				.Description("Task categories");

		}
	}
}
