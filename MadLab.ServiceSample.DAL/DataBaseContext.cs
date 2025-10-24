
using MadLab.ServiceSample.DAL.Model;
using Microsoft.EntityFrameworkCore;


namespace MadLab.ServiceSample.DAL
{
    public class DataBaseContext : DbContext
    {

        /// <summary>
        /// This constructor is used to create a new instance of the DataBaseContext with a connection string.
        /// notice is using SQLite as the database provider, which is suitable for lightweight applications or testing purposes.
        /// and it is not recommended for production use. The database will be created in the local file system, that means your computer
        /// </summary>
        /// <param name="connectionString"></param>
        public DataBaseContext(string connectionString)
              : base(new DbContextOptionsBuilder<DataBaseContext>()
                  .UseSqlite(connectionString)
                  .Options)
        {
        }

        /// <summary>
        /// We are using this constructor so we can  use InMemory database for testing purposes.
        /// </summary>
        /// <param name="options"></param>
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

        }
  
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Todo> Todos => Set<Todo>();

    }
}
