
using MadLab.ServiceSample.BLL.Dto.Category;
using MadLab.ServiceSample.BLL.Dto.Todo;
using MadLab.ServiceSample.BLL.Services;
using MadLab.ServiceSample.DAL;
using System.Threading;

/*********************************************************************************/
//                          MadLab Service Sample 
// Main idea here is to show how to use and abstract class, generics an extension methods to:
// - Minimize code duplication
// - Avoid using AutoMapper or similar libraries
// - Keep the code simple and easy to understand
//
// This repository is a sample .NET 8 solution demonstrating a layered architecture
// for a simple Todo using Entity Framework Core.It is organized into three main projects:
//
//  1.MadLab.ServiceSample.Console
//     A console application that acts as the entry point and demonstrates
//     CRUD operations for categories and todos.
//
//  2.MadLab.ServiceSample.BLL
//     The Business Logic Layer (BLL) containing service classes, DTOs, and business logic
//     for managing categories and todos.
//
//  3.MadLab.ServiceSample.DAL
//     The Data Access Layer (DAL) containing the Entity Framework Core DbContext
//     and entity models.
//
//  
//                  Go ahead and run this app to see the results
//
// Now,to start reviewing this code, go to MadLab.ServiceSample.BLL/BaseService class
/*********************************************************************************/




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