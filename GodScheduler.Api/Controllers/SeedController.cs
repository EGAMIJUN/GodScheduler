using Microsoft.AspNetCore.Mvc;
using GodScheduler.Api.Services; // Serviceを使うためのusing

namespace GodScheduler.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    // DBの代わりに Service を持つ
    private readonly ISeedService _seedService;

    // コンストラクタで Service を受け取る (これがDIバイ)
    public SeedController(ISeedService seedService)
    {
        _seedService = seedService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // ロジックは全部 Service に任せて、結果を受け取るだけ！
        var result = _seedService.InitializeData();
        return Ok(result);
    }
}