using Microsoft.EntityFrameworkCore;

namespace MagniseTask.Data;

public class MarketDbContext : DbContext
{
	public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }
	
	public DbSet<Asset> Assets { get; set; }
}