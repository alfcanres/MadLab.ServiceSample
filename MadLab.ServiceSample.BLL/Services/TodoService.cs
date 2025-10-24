using MadLab.ServiceSample.DAL.Model;
using MadLab.ServiceSample.BLL.Dto.Todo;
using MadLab.ServiceSample.DAL;
using MadLab.ServiceSample.BLL.Mapper;

namespace MadLab.ServiceSample.BLL.Services
{
    public class TodoService : BaseService<Todo, TodoCreateDTO, TodoReadDTO, TodoUpdateDTO>
    {
        public TodoService(DataBaseContext appDbContext) : base(appDbContext)
        {
        }

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
