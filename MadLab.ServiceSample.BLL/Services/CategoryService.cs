using MadLab.ServiceSample.DAL.Model;
using MadLab.ServiceSample.BLL.Dto.Category;
using MadLab.ServiceSample.DAL;
using MadLab.ServiceSample.BLL.Mapper;

namespace MadLab.ServiceSample.BLL.Services
{
    public class CategoryService : BaseService<Category, CategoryCreateDTO, CategoryReadDTO, CategoryUpdateDTO>
    {
        public CategoryService(DataBaseContext appDbContext) : base(appDbContext)
        {
        }

        protected override Category MapCreateDtoToEntity(CategoryCreateDTO dto)
        {
            return dto.ToEntity();
        }
        protected override CategoryReadDTO MapEntityToReadDto(Category entity)
        {
            return entity.ToReadDto();
        }
        protected override Category MapUpdateDtoToEntity(CategoryUpdateDTO dto, Category entity)
        {
            return dto.ToEntity(entity);
        }
    }
}
