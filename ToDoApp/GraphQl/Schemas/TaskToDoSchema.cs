using GraphQL.Types;
using ToDoApp.GraphQl.Queries;
using ToDoApp.GraphQl.Mutations;

namespace ToDoApp.GraphQl.Schemas
{
	public class TaskToDoSchema : Schema
	{
		public TaskToDoSchema(IServiceProvider provider) : base(provider)
		{
			Query = provider.GetRequiredService<TaskToDoQuery>();
			Mutation = provider.GetRequiredService<TaskToDoMutation>();
		}
	}
}
