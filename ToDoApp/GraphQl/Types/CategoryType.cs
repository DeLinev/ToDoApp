using GraphQL.Types;
using ToDoApp.Models;

namespace ToDoApp.GraphQl.Types
{
	public class CategoryType : ObjectGraphType<Category>
	{
		public CategoryType()
		{
			Field(x => x.Id).Description("Category id");
			Field(x => x.Name).Description("Category name");
		}
	}
}
