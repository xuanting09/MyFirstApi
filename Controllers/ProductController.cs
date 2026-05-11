using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // 記得加這個

namespace MyFirstApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    // 這裡用了「相依注入 (DI)」，讓 .NET 把資料庫連線傳進來
    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("analyze")]
    public async Task<IActionResult> Analyze()
    {
        // 這是從真正的資料庫 (shop.db) 撈資料，並使用 LINQ 過濾
        var result = await _context.Products
            .Where(p => p.Price > 5000)
            .Select(p => new { p.Name, p.Price })
            .ToListAsync();

        return Ok(result);
    }

    // 為了讓你展示「新增資料」到資料庫，多加這個 API
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync(); // 真正存進 shop.db
        return Ok(product);
    }
    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        // 利用 LINQ 篩選：庫存 < 門檻 的產品
        var lowStockProducts = await _context.Products
            .Where(p => p.Stock < p.MinThreshold)
            .ToListAsync();

        return Ok(lowStockProducts);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        // 更新欄位
        product.Stock = updatedProduct.Stock;
        product.Price = updatedProduct.Price;
        
        await _context.SaveChangesAsync();
        return Ok(product);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }
}