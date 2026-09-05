using Microsoft.EntityFrameworkCore;
using ClassicsLiteratureAsp.Models;

namespace ClassicsLiteratureAsp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    public DbSet<Author> Authors { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Opus> Opera { get; set; }
    public DbSet<Line> Lines { get; set; }
}
