using GraphQL.Types;

namespace ToDoApp.GraphQl.Types
{
	public class TaskToDoInputType : InputObjectGraphType
	{
		public TaskToDoInputType()
		{
			Name = "TaskToDoInputType";
			Field<StringGraphType>("title");
			Field<NonNullGraphType<StringGraphType>>("description");
			Field<DateTimeGraphType>("dueDate");
			Field<NonNullGraphType<BooleanGraphType>>("isCompleted");
			Field<ListGraphType<NonNullGraphType<IntGraphType>>>("categoryIds");
		}
	}
}
