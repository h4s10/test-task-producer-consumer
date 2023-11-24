using Consumer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Consumer.DAL;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) {
    }

    public DbSet<ResponseLog> Responses { get; set; }
}