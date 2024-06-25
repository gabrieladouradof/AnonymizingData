using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProtectedDba.Models;

public class AppDbContext : DbContext
{
    //User entity collection
    public DbSet<User> Users { get; set; } 

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    //Method for mapping MySQL entity classes and tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

//Creating instances of the database context, with an interface provided by EF
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    //receives an array of strings and returns an instance of the SQL context
    public AppDbContext CreateDbContext (string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql("Server=localhost;Database=anonymization;User=root;Password=23112019@arrasca.;",
            new MySqlServerVersion(new Version(8, 4, 0)));
        
        //return new instance
        return new AppDbContext(optionsBuilder.Options);
    }
}