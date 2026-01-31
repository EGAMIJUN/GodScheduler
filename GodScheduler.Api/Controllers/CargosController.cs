using Microsoft.AspNetCore.Mvc;
using GodScheduler.Api.Data;
using GodScheduler.Api.Models;

namespace GodScheduler.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CargosController(AppDbContext context)
        {
            _context = context;
        }

        // 📦 1. まとめて登録する機能 (Bulk Insert)
        // 営業が「SC 2人、FM 1人！」と送ってきたら、それをDBに保存する
        [HttpPost("Batch")]
        public async Task<IActionResult> CreateBatch([FromBody] List<Cargo> newCargos)
        {
            if (newCargos == null || !newCargos.Any())
            {
                return BadRequest("中身が空っぽバイ！");
            }

            // DBに追加
            await _context.Cargos.AddRangeAsync(newCargos);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"✅ {newCargos.Count}件の枠を作成しました！" });
        }
    }
}