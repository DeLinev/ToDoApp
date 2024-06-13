namespace ToDoApp.Middleware
{
	public class StorageMiddleware
	{
		private readonly RequestDelegate _next;

		public StorageMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			var storageType = context.Request.Headers["Storage-Type"].FirstOrDefault();

			if (!string.IsNullOrEmpty(storageType))
				context.Items["StorageType"] = storageType;

			await _next(context);
		}
	}
}
