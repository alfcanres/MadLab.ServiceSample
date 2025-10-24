using MadLab.ServiceSample.DAL.Model;
using MadLab.ServiceSample.BLL.Dto.Category;
using MadLab.ServiceSample.DAL;
using MadLab.ServiceSample.BLL.Mapper;

namespace MadLab.ServiceSample.BLL.Services
{

    /// <summary>
    /// As you may notice th is class has almost no code, but still will handle all CRUD operations for us.
    /// What is going on here?
    /// 
    /// CategoryService will inhenit from BaseService telling it that should use the following classes
    /// - Category as TEntity
    /// - CategoryCreateDTO as TCreateDTO
    /// - CategoryReadDTO as TReadDTO
    /// - CategoryUpdateDTO as TUpdateDTO
    /// 
    /// So now BaseService knows which types should be working with and that is all what it needs to 
    /// perform basic CRUD operations, that's the magic behind generics!
    /// </summary>
    public class CategoryService : BaseService<Category, CategoryCreateDTO, CategoryReadDTO, CategoryUpdateDTO>
    {
        public CategoryService(DataBaseContext appDbContext) : base(appDbContext)
        {
        }

        // I almost forgot about our Template Methods
        // As you can see actual mapping logic is not here, we of course
        // can implement that logic in our MapCreateDtoToEntity, MapEntityToReadDto and MapUpdateDtoToEntity
        // but the main idea here is to be able to re use the mapping funtionality, kind of like we 
        // do with Auto Mapper. So, instead I created some Extention Methods for our Dtos, let's
        // open Mapper/DtoExtensionMethods class

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
