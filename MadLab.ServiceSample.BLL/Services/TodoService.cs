using MadLab.ServiceSample.DAL.Model;
using MadLab.ServiceSample.BLL.Dto.Todo;
using MadLab.ServiceSample.DAL;
using MadLab.ServiceSample.BLL.Mapper;

namespace MadLab.ServiceSample.BLL.Services
{
    /// <summary>
    /// As you can see we are implementing a new service with almost no effort at all
    /// feels almost like cheating right?. 
    /// But, how about those methods that are not so generic?, like let's say
    /// we want to get all Todos by category Id, well that is why we exposed
    /// DbSet to the derived classes. Let's take a look at GetTodoByCategoryId method
    /// </summary>
    public class TodoService : BaseService<Todo, TodoCreateDTO, TodoReadDTO, TodoUpdateDTO>
    {
        public TodoService(DataBaseContext appDbContext) : base(appDbContext)
        {
        }

        /// <summary>
        /// This method will retrieve all data from Todo DbSet by category, something
        /// our basic BaseSerice class does not do. 
        /// But still we can extend this class so it can do more than it's base class
        /// can handle. 
        /// 
        /// And that's it. The beauty behind generics and template methods.
        /// 
        /// 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IEnumerable<TodoReadDTO> GetTodoByCategoryId(int categoryId)
        {
            var todos = base.DbSet.Where(t => t.CategoryId == categoryId).ToList();
            return todos.Select(MapEntityToReadDto);
        }

        protected override Todo MapCreateDtoToEntity(TodoCreateDTO dto)
        {
            return dto.ToEntity();
        }

        protected override TodoReadDTO MapEntityToReadDto(Todo entity)
        {
            return entity.ToReadDto();
        }

        protected override Todo MapUpdateDtoToEntity(TodoUpdateDTO dto, Todo entity)
        {
            return dto.ToEntity(entity);
        }
    }
}
