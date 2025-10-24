// See https://aka.ms/new-console-template for more information
using MadLab.ServiceSample.BLL.Dto.Category;
using MadLab.ServiceSample.BLL.Dto.Todo;
using MadLab.ServiceSample.BLL.Services;
using MadLab.ServiceSample.DAL;



string connectionString = "Data Source=SampleService.db";

using (var context = new DataBaseContext(connectionString))
{
    context.Database.EnsureCreated();

    var categoryService = new CategoryService(context);

    var homeCategory = categoryService.Create(new CategoryCreateDTO() { Name = "Home" });
    var workCategory = categoryService.Create(new CategoryCreateDTO() { Name = "Work" });
    var leisureCategory = categoryService.Create(new CategoryCreateDTO() { Name = "Leisure" });

    var categories = categoryService.GetAll();


    var todoService = new TodoService(context);

    todoService.Create(new TodoCreateDTO() { Title = "Clean the house", CategoryId = homeCategory.Id });
    todoService.Create(new TodoCreateDTO() { Title = "Buy groceries", CategoryId = homeCategory.Id });
    todoService.Create(new TodoCreateDTO() { Title = "Finish the report", CategoryId = workCategory.Id });
    todoService.Create(new TodoCreateDTO() { Title = "Email the client", CategoryId = workCategory.Id });
    todoService.Create(new TodoCreateDTO() { Title = "Go for a walk", CategoryId = leisureCategory.Id });

    foreach (var category in categories)
    {
        Console.WriteLine($"Category: {category.Name}");
        var todos = todoService.GetTodoByCategoryId(category.Id);
        foreach (var todo in todos)
        {
            Console.WriteLine($"\tTodo: {todo.Title}");
        }
    }


}