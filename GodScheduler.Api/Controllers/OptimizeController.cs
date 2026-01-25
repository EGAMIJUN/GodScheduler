using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GodScheduler.Api.Models;
using GodScheduler.Api.Services;
using GodScheduler.Api.Data;

namespace GodScheduler.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptimizeController : ControllerBase
{
    private readonly MonteCarloEngine _engine = new MonteCarloEngine();
    private readonly AppDbContext _context;

    public OptimizeController(AppDbContext context)
    {
        _context = context;
    }

    // --- 1. 計算ボタン用 (POST: /api/Optimize) ---
    // 画面からは何も受け取らない！勝手にDBを見る！
    [HttpPost]
    public async Task<ActionResult<MonteCarloEngine.AllocationResult>> Post()
    {
        // DBからデータを引っ張ってくる
        var workers = await _context.Workers.ToListAsync();
        var cargos = await _context.Cargos.ToListAsync();

        if (!workers.Any() || !cargos.Any())
        {
            return BadRequest("DBが空っぽバイ！ /api/Seed を叩いてデータを入れてくれ！");
        }

        // 計算実行！
        var result = _engine.Solve(workers, cargos);
        return Ok(result);
    }

    // ... (前略) ...

    // ★追加: 画面を開いた時に、DBの最新状態を返す (GET: /api/Optimize)
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var workers = await _context.Workers.ToListAsync();
        var cargos = await _context.Cargos.ToListAsync();
        
        // 作業員と案件をセットで返す
        return Ok(new { workers, cargos });
    }

    // ... (以下、Postメソッドなどが続く) ...

    // --- 2. 確定保存ボタン用 (POST: /api/Optimize/Confirm) ---
    // 画面から「確定したシフト」を受け取る！
    [HttpPost("Confirm")]
    public async Task<ActionResult> Confirm([FromBody] List<Cargo> confirmedCargos)
    {
        if (confirmedCargos == null || !confirmedCargos.Any())
        {
            return BadRequest("保存するデータがないバイ！");
        }

        foreach (var cargoDto in confirmedCargos)
        {
            var cargoInDb = await _context.Cargos.FindAsync(cargoDto.Id);
            if (cargoInDb != null)
            {
                // 担当者を更新
                cargoInDb.AssignedWorkerId = cargoDto.AssignedWorkerId;
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "シフトを確定保存しました！明日もご安全に！" });
    }
}