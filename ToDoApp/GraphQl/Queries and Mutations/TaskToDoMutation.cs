using GraphQL;
using GraphQL.Types;
using ToDoApp.GraphQl.Types;
using ToDoApp.Models;
using ToDoApp.Repository;

namespace ToDoApp.GraphQl.Mutations
{
	public class TaskToDoMutation : ObjectGraphType
	{
		public TaskToDoMutation(IRepository repository)
		{
			Field<TaskToDoType>("createTask")
				.Arguments(new QueryArguments(new QueryArgument<TaskToDoInputType> { Name = "task" }, new QueryArgument<ListGraphType<IntGraphType>> { Name = "categoryIds" }))
				.Resolve(context =>
				{
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
					int taskId = context.GetArgument<int>("id");
					repository.Complete(taskId);
					return repository.GetTask(taskId);
				})
				.Description("Update task status");

			Field<TaskToDoType>("deleteTask")
				.Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }))
				.Resolve(context =>
				{
					int taskId = context.GetArgument<int>("id");
					TaskToDo deletedTask = repository.GetTask(taskId);
					repository.Delete(taskId);
					return deletedTask;
				})
				.Description("Delete a task");
		}
	}
}
