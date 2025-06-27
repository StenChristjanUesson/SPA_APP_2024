using Backend.Model;
using Microsoft.EntityFrameworkCore;
namespace backend.Model;

public class DataContext: DbContext {
    public DataContext(DbContextOptions<DataContext> options): base(options) {}
    public DbSet<Event>? EventList {get; set;}
    public DbSet<People>? PeopleList { get; set; }
    public DbSet<User>? Users {  get; set; }
    public DbSet<PrivateNotes>? PrivateNotes { get; set; }
}



