# MadLab.ServiceSample

A .NET 8 demonstration project showcasing **generic service patterns**,**template method pattern**, **method extentions**, and how to use it for **CRUD operations** using Entity Framework Core and SQLite.

## Purpose

This repository serves as an educational template for building scalable, maintainable service layers in .NET applications. It demonstrates how to:

- Eliminate code duplication with **generic base services**
- Implement the **Template Method Pattern** for flexible entity-DTO mapping
- Extend base functionality with custom business logic
- Use the **DTO (Data Transfer Object) pattern** effectively

## 📦 Projects

| Project | Description |
|---------|-------------|
| **MadLab.ServiceSample.Console** | Entry point demonstrating service usage |
| **MadLab.ServiceSample.BLL** | Business logic layer with services, DTOs, and mappers |
| **MadLab.ServiceSample.DAL** | Data access layer with entities and DbContext |

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or Visual Studio Code
- Basic understanding of C# and Entity Framework Core

### Running the Application

1. **Clone the repository**
   ```bash
   git clone https://github.com/alfcanres/MadLab.ServiceSample.git
   cd MadLab.ServiceSample
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the console application**
   ```bash
   dotnet run --project MadLab.ServiceSample.Console
   ```

The SQLite database will be created automatically in your local file system on first run.

## Key Concepts

### 1. Generic Base Service Pattern

The `BaseService<TEntity, TCreateDTO, TReadDTO, TUpdateDTO>` provides reusable CRUD operations:

```csharp
public abstract class BaseService<TEntity, TCreateDTO, TReadDTO, TUpdateDTO>
{
    public TReadDTO Create(TCreateDTO dto);
    public TReadDTO GetById(int id);
    public IEnumerable<TReadDTO> GetAll();
    public TReadDTO Update(int id, TUpdateDTO dto);
    public bool Delete(int id);
    
    // Template methods to be implemented by derived classes
    protected abstract TEntity MapCreateDtoToEntity(TCreateDTO dto);
    protected abstract TReadDTO MapEntityToReadDto(TEntity entity);
    protected abstract TEntity MapUpdateDtoToEntity(TUpdateDTO dto, TEntity entity);
}
```

**Benefits:**
- Write CRUD logic once, reuse everywhere
- Type-safe generic implementation
- Consistent API across all services
- Easy to maintain and test

### 2. Template Method Pattern

Derived services implement mapping logic specific to their entity types:

```csharp
public class CategoryService : BaseService<Category, CategoryCreateDTO, CategoryReadDTO, CategoryUpdateDTO>
{
    protected override Category MapCreateDtoToEntity(CategoryCreateDTO dto)
        => dto.ToEntity();
    
    protected override CategoryReadDTO MapEntityToReadDto(Category entity)
        => entity.ToReadDto();
    
    protected override Category MapUpdateDtoToEntity(CategoryUpdateDTO dto, Category entity)
        => dto.ToEntity(entity);
}
```

### 3. Extending Base Functionality

Services can add custom methods beyond basic CRUD:

```csharp
public class TodoService : BaseService<Todo, TodoCreateDTO, TodoReadDTO, TodoUpdateDTO>
{
    // Custom query method
    public IEnumerable<TodoReadDTO> GetTodoByCategoryId(int categoryId)
    {
        var todos = base.DbSet.Where(t => t.CategoryId == categoryId).ToList();
        return todos.Select(MapEntityToReadDto);
    }
}
```

### 4. DTO Pattern

Separate data representation from domain models:

- **`TodoCreateDTO`**: Properties needed to create a new todo
- **`TodoReadDTO`**: Properties returned to clients (includes computed fields like `CategoryName`)
- **`TodoUpdateDTO`**: Properties that can be updated

### 5. Extension Method Mappers

Clean, fluent mapping syntax located in `DtoExtensionMethods.cs`:

```csharp
public static Todo ToEntity(this TodoCreateDTO dto)
{
    return new Todo
    {
        Title = dto.Title,
        CategoryId = dto.CategoryId,
        IsCompleted = dto.IsCompleted
    };
}
```

## 📂 Project Structure

## 🎓 Learning Path

Follow this order to understand the codebase:

1. **Start with `MadLab.ServiceSample.Console/Program.cs`**
   - Read the comments, will take you accross the code base
   - Run the app, and see the output

2. **Take a look at `MadLab.ServiceSample.BLL/Services/BaseService.cs`**
   - Understand the generic template and CRUD operations
   - Notice the abstract mapping methods

3. **Review `MadLab.ServiceSample.BLL/Services/CategoryService.cs`**
   - See the simplest implementation
   - Observe how mapping methods are implemented

4. **Examine `MadLab.ServiceSample.BLL/Services/TodoService.cs`**
   - See how to add custom methods beyond CRUD
   - Understand `DbSet` exposure for custom queries

5. **Check `MadLab.ServiceSample.BLL/Mapper/DtoExtensionMethods.cs`**
   - Understand the mapping strategy
   - See how extension methods provide clean syntax  

## How to Add a New Service

1. **Create your entity** in `MadLab.ServiceSample.DAL/Model/`
   ```csharp
   public class YourEntity
   {
       public int Id { get; set; }
       public string Name { get; set; } = string.Empty;
   }
   ```

2. **Add DbSet to `DataBaseContext`**
   ```csharp
   public DbSet<YourEntity> YourEntities { get; set; }
   ```

3. **Create DTOs** in `MadLab.ServiceSample.BLL/Dto/YourEntity/`
   - `YourEntityCreateDTO.cs`
   - `YourEntityReadDTO.cs`
   - `YourEntityUpdateDTO.cs`

4. **Add mapping extensions** in `DtoExtensionMethods.cs`
   ```csharp
   public static YourEntity ToEntity(this YourEntityCreateDTO dto) { ... }
   public static YourEntityReadDTO ToReadDto(this YourEntity entity) { ... }
   ```

5. **Create service class** in `MadLab.ServiceSample.BLL/Services/`
   ```csharp
   public class YourEntityService : BaseService<YourEntity, YourEntityCreateDTO, 
                                                 YourEntityReadDTO, YourEntityUpdateDTO>
   {
       public YourEntityService(DataBaseContext context) : base(context) { }
       
       protected override YourEntity MapCreateDtoToEntity(YourEntityCreateDTO dto)
           => dto.ToEntity();
       
       protected override YourEntityReadDTO MapEntityToReadDto(YourEntity entity)
           => entity.ToReadDto();
       
       protected override YourEntity MapUpdateDtoToEntity(YourEntityUpdateDTO dto, YourEntity entity)
           => dto.ToEntity(entity);
   }
   ```


## Technologies Used

- **.NET 8**: Latest LTS version of .NET
- **C# 12**: Modern C# language features
- **Entity Framework Core**: ORM for database operations
- **SQLite**: Lightweight database for development
- **Generic Types**: Type-safe reusable code
- **LINQ**: Query data in a declarative manner

## Design Patterns

- **Template Method Pattern**: Define algorithm structure, let subclasses implement details
- **Repository Pattern** (implicit): Data access abstraction through DbContext/DbSet
- **DTO Pattern**: Separate data contracts from domain models
- **Dependency Injection**: Constructor-based DI for DbContext

## Contributing

This is an educational project. Feel free to:
- Fork the repository
- Experiment with the code
- Submit pull requests with improvements
- Report issues

## 👨‍💻 Author

**Alfredo Can**
- GitHub: [@alfcanres](https://github.com/alfcanres)


**"Feels almost like cheating, right?"** - That's the beauty of generics and template methods! 
