using ToDoApp.Models;
using GraphQL.Types;

namespace ToDoApp.GraphQl.Types
{
	public class TaskToDoType : ObjectGraphType<TaskToDo>
	{
		public TaskToDoType()
		{
			Field(x => x.Id).Description("Task id");
			Field(x => x.Title, nullable: true).Description("Task title");
			Field(x => x.Description).Description("Task description");
			Field(x => x.CreationDate).Description("Task creation date");
			Field(x => x.DueDate, nullable: true).Description("Task due date");
			Field(x => x.IsCompleted).Description("Task status");
			Field(x => x.Categories, type: typeof(ListGraphType<CategoryType>)).Description("Task categories");
		}
	}
}
