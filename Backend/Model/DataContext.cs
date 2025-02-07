using Microsoft.EntityFrameworkCore;
namespace backend.Model;

public class DataContext: DbContext {
    public DataContext(DbContextOptions<DataContext> options): base(options) {}
    public DbSet<Event>? EventList {get; set;}
    public DbSet<Person>? PersonList { get; set;}
}



