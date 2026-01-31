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
    [HttpPost]
    public async Task<ActionResult<AllocationResult>> Post()
    {
        var workers = await _context.Workers.ToListAsync();
        var cargos = await _context.Cargos.ToListAsync();
        var compatibilities = await _context.WorkerCompatibilities.ToListAsync();
        if (!workers.Any() || !cargos.Any())
        {
            return BadRequest("DBが空っぽバイ！先に /api/Seed を実行してデータを投入してくれ！");
        }

        var result = _engine.Optimize(workers, cargos, compatibilities);
        
        return Ok(result);
    }

    // --- 2. 画面表示用 (GET: /api/Optimize) ---
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var workers = await _context.Workers.ToListAsync();
        var cargos = await _context.Cargos.ToListAsync();
        return Ok(new { workers, cargos });
    }

    // --- 3. 確定保存ボタン用 (POST: /api/Optimize/Confirm) ---
    // 👇 ここに Confirm は「1つだけ」あるべきバイ！
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
                cargoInDb.AssignedWorkerId = cargoDto.AssignedWorkerId;
            }
        }

        await _context.SaveChangesAsync();
        
        return Ok(new { message = "⚡️激速ホットリロード成功！明日もご安全に！" });
    }
}