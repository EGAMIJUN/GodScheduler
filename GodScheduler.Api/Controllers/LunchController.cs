using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GodScheduler.Api.Data;
using GodScheduler.Api.Models;

namespace GodScheduler.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LunchController : ControllerBase
{
    private readonly AppDbContext _context;

    public LunchController(AppDbContext context)
    {
        _context = context;
    }

    // 1. メニュー一覧を取得 (GET: /api/Lunch)
    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        var menus = await _context.LunchMenus.ToListAsync();
        var orders = await _context.LunchOrders.ToListAsync();
        return Ok(new { menus, orders });
    }

    // 2. 注文する (POST: /api/Lunch)
    [HttpPost]
    public async Task<IActionResult> Order([FromBody] LunchOrder order)
    {
        // 注文データを保存
        order.OrderDate = DateTime.Now;
        _context.LunchOrders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(new { message = "🍱 注文を受け付けたバイ！楽しみに待っとき！" });
    }
}