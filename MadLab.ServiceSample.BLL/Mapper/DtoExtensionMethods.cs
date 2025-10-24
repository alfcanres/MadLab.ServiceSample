using MadLab.ServiceSample.BLL.Dto.Todo;
using MadLab.ServiceSample.BLL.Dto.Category;
using MadLab.ServiceSample.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadLab.ServiceSample.BLL.Mapper
{
    /// <summary>
    /// And here we have our extension methods. 
    /// What this is doing is to extend the existing funtionality of our classes
    /// so the can do the mapping themselves. If we need a new mapping, we just need
    /// to add a new mapping in here. However if you are working with a dev team
    /// may be a good idea to have a different file for every DTO or entity
    /// 
    /// 
    /// Now, let's go to MadLab.ServiceSample.BLL/TodoService class
    /// </summary>
    public static class DtoExtensionMethods
    {
        
        //Todo extension methods

        public static Todo ToEntity(this TodoCreateDTO dto)
        {
            return new Todo
            {
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                IsCompleted = dto.IsCompleted
            };
        }

        public static TodoReadDTO ToReadDto(this Todo entity)
        {
            return new TodoReadDTO
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                IsCompleted = entity.IsCompleted,
                CategoryName = entity.Category != null ? entity.Category.Name : string.Empty
            };
        }

        public static Todo ToEntity(this TodoUpdateDTO dto, Todo entity)
        {
            entity.CategoryId = dto.CategoryId;
            entity.Title = dto.Title;
            entity.IsCompleted = dto.IsCompleted;
            return entity;
        }

        // Category extension methods
        public static Category ToEntity(this CategoryCreateDTO dto)
        {
            return new Category
            {
                Name = dto.Name
            };
        }

        public static CategoryReadDTO ToReadDto(this Category entity)
        {
            return new CategoryReadDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static Category ToEntity(this CategoryUpdateDTO dto, Category entity)
        {
            entity.Name = dto.Name;
            return entity;
        }
    }
}
