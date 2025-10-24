
using MadLab.ServiceSample.DAL;
using Microsoft.EntityFrameworkCore;

namespace MadLab.ServiceSample.BLL
{
    /// <summary>
    /// Provides a base service class for handling CRUD operations with generic types.
    /// If all we need is to perform basic CRUD operations, we can implement them here in this base class.
    /// so we don't need to repeat the same code in each service class. 
    /// 
    /// This is an abstrac class, can't create a new instance of it, you can only use it 
    ///  as some sort of "template" for concrete classes, like an interface, except this
    ///  contains logic that we can re use. 
    ///  
    ///  But of course, every service will work with a diferent entity type, and that means
    ///  we will need some way of telling this abstrac class "Hey, I still don't know
    ///  which type you will be working with, but I know you will be performing
    ///  insert, read, update, and delete operations, so let's call the entity "TEntity"
    ///  for now, and the derived class will tell you which entity you will be working with,
    ///  and same goes for the TCreateDTO, TReadDTO and TUpdateDTO
    ///  makes sense?
    ///  So, What is the advantage?, I pretty sure you wil notice once we 
    ///  create the service class. But for now let's continue
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

        //We will expose this DbSet to the derived class so we can create our own querys
        //or perform any custom CRUD operation no considered in a basic functionalty of this
        //base class. TodoService class is using it, we will see it when we get there. 
        protected DbSet<TEntity> DbSet => _appDbContext.Set<TEntity>();

        protected BaseService(DataBaseContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Notice TReadDTO is our expected output, we still don't know the type, but
        /// we know it should be a class hence the constraint "where TReadDTO : class"
        /// and we also know the input TCreateDTO should be a class. This is the 
        /// good thing about generics, we still don't know what that is, but we 
        /// are letting derived classes take care of it. 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public TReadDTO Create(TCreateDTO dto)
        {

            //MapCreateDtoToEntity method does not exists in this class
            //it is what we call a "Template method", we are still don't know
            //what logic it will contain, but we know for sure
            //the input and output expected.
            //And of course, we are not going to be using AutoMapper

            var entity = MapCreateDtoToEntity(dto);

            //Here we are using our generic TEntity to tell the DbContext which DbSet should interact with
            _appDbContext.Set<TEntity>().Add(entity);

            //If you already know how to work with EF Core you may notice something strange in here.
            //And, yes, I am not using async methods here for simplicity sake.
            //But in real world applications, we should use async methods to avoid blocking the main thread.
            //won't be covering that topic in here tho
            _appDbContext.SaveChanges();



            return MapEntityToReadDto(entity);
        }

        /// <summary>
        /// Same here, expected output a class TReadDTO,
        /// can be any DTO we want to
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TReadDTO GetById(int id)
        {
            var entity = _appDbContext.Set<TEntity>().Find(id);
            if (entity == null)
            {
                return null;
            }

            //MapEntityToReadDto is another template method 

            return MapEntityToReadDto(entity);
        }


        public IEnumerable<TReadDTO> GetAll()
        {
            var entities = _appDbContext.Set<TEntity>().ToList();


            //Notice here we are combining LINQ with our MapEntityToReadDto template method

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

        // And finally here we have our template methods that will help us to map entities to dtos and 
        // dtos to entities. 
        // Since we are not using any mapping library like AutoMapper, we created
        // abstract mapping methods to be implemented in derived classes. 
        // so every class that implements this base class must provide its own mapping logic.
        // Now let's take a look at MadLab.ServiceSample.BLL/Services/CategoryService class

        protected abstract TEntity MapCreateDtoToEntity(TCreateDTO dto);
        protected abstract TReadDTO MapEntityToReadDto(TEntity entity);
        protected abstract TEntity MapUpdateDtoToEntity(TUpdateDTO dto, TEntity entity);

    }
}
