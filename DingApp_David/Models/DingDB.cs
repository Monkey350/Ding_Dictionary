using System;
using System.Data.Entity;
using System.Linq;

namespace DingApp_David.Models
{

    public class DingDb : DbContext, IDingDb
    {
        // Your context has been configured to use a 'DingDb' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DingApp_David.Models.DingDb' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DingDb' 
        // connection string in the application configuration file.


        public DbSet<WordModel> Words { get; set; }

        public DingDb()
            : base("name=DingDb")
        {
        }

        IQueryable<T> IDingDb.Query<T>()
        {
            return Set<T>();
        }

        T IDingDb.Add<T>(T entity)
        {
            Set<T>().Add(entity);
            SaveChanges();
            return entity;
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}