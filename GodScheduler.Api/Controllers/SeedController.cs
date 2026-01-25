using Microsoft.AspNetCore.Mvc;
using GodScheduler.Api.Data;
using GodScheduler.Api.Models;

namespace GodScheduler.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _context;

    public SeedController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // 1. 既存データを全消去（リセット）
        _context.Cargos.RemoveRange(_context.Cargos);
        _context.Workers.RemoveRange(_context.Workers);
        _context.SaveChanges();

        // 2. 30人のスタッフを自動生成
        var workers = new List<Worker>();
        var random = new Random();
        string[] skillsList = { "大型免許", "リフト", "玉掛け", "クレーン", "危険物" };
        string[] lastNames = { "佐藤", "鈴木", "高橋", "田中", "渡辺", "伊藤", "山本", "中村", "小林", "加藤" };

        for (int i = 1; i <= 30; i++)
        {
            // ランダムに名前を決める
            var name = $"{lastNames[random.Next(lastNames.Length)]} {i}号";
            
            // ランダムにスキルを付与 (30%の確率で大型、40%でリフト...など)
            var mySkills = new List<string>();
            if (random.NextDouble() < 0.3) mySkills.Add("大型免許");
            if (random.NextDouble() < 0.4) mySkills.Add("リフト");
            if (random.NextDouble() < 0.2) mySkills.Add("玉掛け");
            
            // 何もスキルがないと可哀想なので、たまに「見習い」をつける
            if (!mySkills.Any()) mySkills.Add("見習い");

            // DB保存用にカンマ区切り文字列にする ("大型免許,リフト")
            string skillsString = string.Join(",", mySkills);

            workers.Add(new Worker
            {
                Name = name,
                Skills = skillsString, // ★ここ重要！DBにちゃんとスキル文字を入れる
                FatigueLevel = random.Next(0, 100) // 疲労度もランダム
            });
        }
        _context.Workers.AddRange(workers);

        // 3. 20件の作業オーダーを自動生成
        var cargos = new List<Cargo>();
        for (int i = 1; i <= 20; i++)
        {
            string workName;
            string reqSkill;

            // ランダムに仕事内容を決める
            int type = random.Next(3);
            if (type == 0) { workName = $"#{i} コンテナ搬送"; reqSkill = "大型免許"; }
            else if (type == 1) { workName = $"#{i} 倉庫内整理"; reqSkill = "リフト"; }
            else { workName = $"#{i} ゲート管理"; reqSkill = "なし"; }

            cargos.Add(new Cargo
            {
                WorkName = workName,
                RequiredSkill = reqSkill,
                AssignedWorkerId = 0
            });
        }
        _context.Cargos.AddRange(cargos);

        // ... (前略: Workers と Cargos の投入処理) ...

        // ↓↓↓ 4. 昼食データの初期化（ここから追記） ↓↓↓
        _context.LunchOrders.RemoveRange(_context.LunchOrders);
        _context.LunchMenus.RemoveRange(_context.LunchMenus);
        _context.LunchVendors.RemoveRange(_context.LunchVendors);
        _context.SaveChanges();

        // 業者作成
        var vendor1 = new LunchVendor { Name = "港湾弁当サービス", WhFlg = 0 };
        var vendor2 = new LunchVendor { Name = "コンビニ配送", WhFlg = 0 };
        _context.LunchVendors.AddRange(vendor1, vendor2);
        _context.SaveChanges(); // IDを確定させるために一旦保存

        // メニュー作成
        _context.LunchMenus.AddRange(
            new LunchMenu { LunchVendorId = vendor1.Id, Name = "日替わりA (唐揚げ)", Price = 500 },
            new LunchMenu { LunchVendorId = vendor1.Id, Name = "日替わりB (魚フライ)", Price = 500 },
            new LunchMenu { LunchVendorId = vendor1.Id, Name = "特製カレー", Price = 600 },
            new LunchMenu { LunchVendorId = vendor2.Id, Name = "おにぎりセット", Price = 350 },
            new LunchMenu { LunchVendorId = vendor2.Id, Name = "幕の内弁当", Price = 550 }
        );
        // ↑↑↑ ここまで ↑↑↑
        
        // 保存！
        _context.SaveChanges();

        return Ok(new { message = $"🎉 大規模データ投入完了！ スタッフ: {workers.Count}人, 案件: {cargos.Count}件" });
    }
}