
using MadLab.ServiceSample.DAL;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MadLab.ServiceSample.BLL
{
    /// <summary>
    /// Provides a base service class for handling CRUD operations with generic types.
    /// If all we need is to perform basic CRUD operations, we can implement them here in this base class.
    /// so we don't need to repeat the same code in each service class. 
    /// </summary>
    /// <typeparam name="TEntity">Represents the entity type.</typeparam>
    /// <typeparam name="TCreateDTO">Represents the create DTO type.</typeparam>
    /// <typeparam name="TReadDTO">Represents the read  DTO type.</typeparam>
    /// <typeparam name="TUpdateDTO">Represents the update DTO type.</typeparam>
    public abstract class BaseService<TEntity, TCreateDTO, TReadDTO, TUpdateDTO>
        where TCreateDTO : class
 
        where TReadDTO : class
        where TUpdateDTO : class
        where TEntity : class
    {
        private readonly DataBaseContext _appDbContext;
        protected DbSet<TEntity> DbSet => _appDbContext.Set<TEntity>();

        protected BaseService(DataBaseContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public TReadDTO Create(TCreateDTO dto)
        {
            var entity = MapCreateDtoToEntity(dto);
            
            _appDbContext.Set<TEntity>().Add(entity);

            //If you already know how to work with EF Core you may notice something strange in here.
            //And, yes, I am not using async methods here for simplicity sake.
            //But in real world applications, we should use async methods to avoid blocking the main thread.
            _appDbContext.SaveChanges(); 

            return MapEntityToReadDto(entity);
        }

        public TReadDTO GetById(int id)
        {
            var entity = _appDbContext.Set<TEntity>().Find(id);
            if (entity == null)
            {
                return null;
            }
            return MapEntityToReadDto(entity);
        }

        public IEnumerable<TReadDTO> GetAll()
        {
            var entities = _appDbContext.Set<TEntity>().ToList();
            return entities.Select(e => MapEntityToReadDto(e)).ToList();
        }

        public TReadDTO Update(int id, TUpdateDTO dto)
        {
            var entity = _appDbContext.Set<TEntity>().Find(id);
            if (entity == null)
            {
                return null;
            }
            entity = MapUpdateDtoToEntity(dto, entity);
            _appDbContext.Set<TEntity>().Update(entity);
            _appDbContext.SaveChanges();
            return MapEntityToReadDto(entity);
        }
        public bool Delete(int id)
        {
            var entity = _appDbContext.Set<TEntity>().Find(id);
            if (entity == null)
            {
                return false;
            }
            _appDbContext.Set<TEntity>().Remove(entity);
            _appDbContext.SaveChanges();
            return true;
        }


        // Since we are not using any mapping library like AutoMapper, we created
        // abstract mapping methods to be implemented in derived classes. 
        // so every class that implements this base class must provide its own mapping logic.        
        protected abstract TEntity MapCreateDtoToEntity(TCreateDTO dto);
        protected abstract TReadDTO MapEntityToReadDto(TEntity entity);
        protected abstract TEntity MapUpdateDtoToEntity(TUpdateDTO dto, TEntity entity);

    }
}
