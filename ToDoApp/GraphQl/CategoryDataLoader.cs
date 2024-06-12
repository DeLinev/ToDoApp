using GraphQL.DataLoader;
using ToDoApp.Models;
using ToDoApp.Repository;

namespace ToDoApp.GraphQl
{
	public class CategoryDataLoader : DataLoaderBase<int, List<Category>>
	{
		private readonly IRepository _repository;

		public CategoryDataLoader(IRepository repository)
		{
			_repository = repository;
		}

		protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, List<Category>>> taskCategories, CancellationToken cancellationToken)
		{
			var taskIds = taskCategories.Select(pair => pair.Key).ToList();
			var categoriesDict = await _repository.GetCategoriesAsync(taskIds, cancellationToken);

			foreach (var pair in taskCategories)
			{
				if (categoriesDict.TryGetValue(pair.Key, out var categories))
				{
					pair.SetResult(categories);
				}
				else
				{
					pair.SetResult(new List<Category>());
				}
			}
		}
	}
}
