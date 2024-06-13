using GraphQL;
using GraphQL.Types;
using ToDoApp.GraphQl.Types;
using ToDoApp.Repository;

namespace ToDoApp.GraphQl.Queries
{
	public class TaskToDoQuery : ObjectGraphType
	{
		public TaskToDoQuery(RepositoryFactory repositoryFactory, IHttpContextAccessor httpContextAccessor)
		{
			Field<ListGraphType<TaskToDoType>>("tasks")
				.Resolve(context => {
					var repository = repositoryFactory.CreateRepository(httpContextAccessor.HttpContext);
					return repository.GetAllTasks();
					})
				.Description("Returns all tasks");

			Field<TaskToDoType>("task")
				.Arguments(new QueryArguments(new QueryArgument<IdGraphType> { Name = "id" }))
				.Resolve(context => {
					var repository = repositoryFactory.CreateRepository(httpContextAccessor.HttpContext);
					return repository.GetTask(context.GetArgument<int>("id"));
					})
				.Description("Returns task by id");

			Field<ListGraphType<CategoryType>>("categories")
				.Resolve(context => {
					var repository = repositoryFactory.CreateRepository(httpContextAccessor.HttpContext);
					return repository.GetAllCategories();
					})
				.Description("Returns all categories");
		}
	}
}
