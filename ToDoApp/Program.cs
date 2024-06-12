using GraphQL;
using GraphQL.Types;
using ToDoApp.Data;
using ToDoApp.GraphQl.Queries;
using ToDoApp.GraphQl.Mutations;
using ToDoApp.GraphQl.Schemas;
using ToDoApp.GraphQl.Types;
using ToDoApp.Repository;
using ToDoApp.GraphQl;
using GraphQL.DataLoader;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IDapperContext, DapperContext>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IRepository, DapperRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<TaskToDoType>();
builder.Services.AddSingleton<CategoryType>();
builder.Services.AddSingleton<TaskToDoInputType>();

builder.Services.AddSingleton<TaskToDoQuery>();
builder.Services.AddSingleton<TaskToDoMutation>();

builder.Services.AddSingleton<CategoryDataLoader>();
builder.Services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
builder.Services.AddSingleton<DataLoaderDocumentListener>();

builder.Services.AddSingleton<ISchema, TaskToDoSchema>();
builder.Services.AddGraphQL(x => x
	.AddAutoSchema<ISchema>() 
	.AddSystemTextJson()
    .AddDataLoader()
    );

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseGraphQLAltair();
app.UseGraphQL<ISchema>();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

app.Run();
