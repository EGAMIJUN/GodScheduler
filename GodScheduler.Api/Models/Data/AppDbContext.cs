using Microsoft.EntityFrameworkCore;
using GodScheduler.Api.Models;

namespace GodScheduler.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ここにテーブルを登録！
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Cargo> Cargos { get; set; }

    // ↓↓↓ これを追加するバイ！！ ↓↓↓
    public DbSet<LunchVendor> LunchVendors { get; set; }
    public DbSet<LunchMenu> LunchMenus { get; set; }
    public DbSet<LunchOrder> LunchOrders { get; set; }
    // ↑↑↑ ここまで ↑↑↑
    public DbSet<WorkerCompatibility> WorkerCompatibilities { get; set; }
}