using GraphQL;
using GraphQL.Types;
using ToDoApp.GraphQl.Types;
using ToDoApp.Models;
using ToDoApp.Repository;

namespace ToDoApp.GraphQl.Mutations
{
	public class TaskToDoMutation : ObjectGraphType
	{
		public TaskToDoMutation(RepositoryFactory repositoryFactory, IHttpContextAccessor httpContextAccessor)
		{
			Field<TaskToDoType>("createTask")
				.Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<TaskToDoInputType>> { Name = "task" }, 
						   new QueryArgument<NonNullGraphType<ListGraphType<IntGraphType>>> { Name = "categoryIds" }))
				.Resolve(context =>
				{
					var repository = repositoryFactory.CreateRepository(httpContextAccessor.HttpContext);

					TaskToDo taskInput = context.GetArgument<TaskToDo>("task");
					int[] categoryIds = context.GetArgument<int[]>("categoryIds");
					int taskId = repository.Add(taskInput, categoryIds);
					return repository.GetTask(taskId);
				})
				.Description("Create a new task");

			Field<TaskToDoType>("updateTask")
				.Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }))
				.Resolve(context =>
				{
					var repository = repositoryFactory.CreateRepository(httpContextAccessor.HttpContext);

					int taskId = context.GetArgument<int>("id");
					repository.Complete(taskId);
					return repository.GetTask(taskId);
				})
				.Description("Update task status");

			Field<TaskToDoType>("deleteTask")
				.Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }))
				.Resolve(context =>
				{
					var repository = repositoryFactory.CreateRepository(httpContextAccessor.HttpContext);

					int taskId = context.GetArgument<int>("id");
					TaskToDo deletedTask = repository.GetTask(taskId);
					repository.Delete(taskId);
					return deletedTask;
				})
				.Description("Delete a task");
		}
	}
}
