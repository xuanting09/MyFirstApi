using Microsoft.EntityFrameworkCore;

// 這裡我們把 record 改成 class，因為 EF Core 處理 class 的資料庫關聯更強大
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public int MinThreshold { get; set; }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    // 這行代表資料庫裡會有一張叫 Products 的表
    public DbSet<Product> Products => Set<Product>();
}
