using GraphQL;
using GraphQL.Types;
using ToDoApp.GraphQl.Types;
using ToDoApp.Repository;

namespace ToDoApp.GraphQl.Queries
{
	public class TaskToDoQuery : ObjectGraphType
	{
		public TaskToDoQuery(IRepository repository)
		{
			Field<ListGraphType<TaskToDoType>>("tasks")
				.Resolve(context => repository.GetAllTasks())
				.Description("Returns all tasks");

			Field<TaskToDoType>("task")
				.Arguments(new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }))
				.Resolve(context => repository.GetTask(context.GetArgument<int>("id")))
				.Description("Returns task by id");

			Field<ListGraphType<CategoryType>>("categories")
				.Resolve(context => repository.GetAllCategories())
				.Description("Returns all categories");
		}
	}
}
