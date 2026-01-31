using GodScheduler.Api.Data;
using GodScheduler.Api.Models;

namespace GodScheduler.Api.Services;

// 結果を返すための箱（DTO）
public class SeedResult
{
    public string Message { get; set; } = "";
}

// インターフェース（メニュー表）
public interface ISeedService
{
    SeedResult InitializeData();
}

// 実装クラス（シェフ）
public class SeedService : ISeedService
{
    private readonly AppDbContext _context;

    public SeedService(AppDbContext context)
    {
        _context = context;
    }

    public SeedResult InitializeData()
    {
        // 1. 既存データを全消去（リセット）
        _context.WorkerCompatibilities.RemoveRange(_context.WorkerCompatibilities);
        _context.Cargos.RemoveRange(_context.Cargos);
        _context.LunchOrders.RemoveRange(_context.LunchOrders);
        _context.LunchMenus.RemoveRange(_context.LunchMenus);
        _context.LunchVendors.RemoveRange(_context.LunchVendors);
        _context.Workers.RemoveRange(_context.Workers);
        _context.SaveChanges();

        // 2. 30人のスタッフを自動生成
        var workers = new List<Worker>();
        var random = new Random();
        string[] lastNames = { "佐藤", "鈴木", "高橋", "田中", "渡辺", "伊藤", "山本", "中村", "小林", "加藤" };

        for (int i = 1; i <= 30; i++)
        {
            var name = $"{lastNames[random.Next(lastNames.Length)]} {i}号";
            var mySkills = new List<string>();
            
            if (i % 3 == 0) mySkills.Add("SC");      
            if (i % 4 == 0) mySkills.Add("FM");      
            if (i % 2 == 0) mySkills.Add("大型免許"); 
            if (i % 5 == 0) mySkills.Add("リフト");   

            if (!mySkills.Any()) mySkills.Add("一般");

            workers.Add(new Worker
            {
                Name = name,
                Skills = string.Join(",", mySkills),
                FatigueLevel = random.Next(0, 50)
            });
        }
        _context.Workers.AddRange(workers);
        _context.SaveChanges();

        // 3. プロジェクト単位で作業オーダーを生成
        var cargos = new List<Cargo>();
        var projects = new[]
        {
            new { Name = "WAN HAI 101", Place = "RC-3" },
            new { Name = "EVER GREEN", Place = "RC-4" },
            new { Name = "ONE APUS", Place = "中央倉庫" },
            new { Name = "SITC OSAKA", Place = "RC-3" }
        };

        foreach (var proj in projects)
        {
            cargos.Add(CreateCargo(proj.Name, "監督", "FM", 1, proj.Place));
            cargos.Add(CreateCargo(proj.Name, "整理", "リフト", random.Next(2, 4), proj.Place));
            cargos.Add(CreateCargo(proj.Name, "搬送", "大型免許", random.Next(3, 6), proj.Place));
            cargos.Add(CreateCargo(proj.Name, "作業", "なし", random.Next(2, 5), proj.Place));
        }
        _context.Cargos.AddRange(cargos);

        // 4. 昼食データの初期化
        var vendor1 = new LunchVendor { Name = "港湾弁当サービス", WhFlg = 0 };
        var vendor2 = new LunchVendor { Name = "コンビニ配送", WhFlg = 0 };
        _context.LunchVendors.AddRange(vendor1, vendor2);
        _context.SaveChanges();

        _context.LunchMenus.AddRange(
            new LunchMenu { LunchVendorId = vendor1.Id, Name = "日替わりA (唐揚げ)", Price = 500 },
            new LunchMenu { LunchVendorId = vendor1.Id, Name = "日替わりB (魚フライ)", Price = 500 },
            new LunchMenu { LunchVendorId = vendor1.Id, Name = "特製カレー", Price = 600 },
            new LunchMenu { LunchVendorId = vendor2.Id, Name = "おにぎりセット", Price = 350 },
            new LunchMenu { LunchVendorId = vendor2.Id, Name = "幕の内弁当", Price = 550 }
        );

        // 5. 相性データの投入
        var compatibilities = new List<WorkerCompatibility>();
        compatibilities.Add(new WorkerCompatibility { WorkerId1 = workers[0].Id, WorkerId2 = workers[1].Id, Score = -9999 });
        compatibilities.Add(new WorkerCompatibility { WorkerId1 = workers[2].Id, WorkerId2 = workers[3].Id, Score = 100 });
        _context.WorkerCompatibilities.AddRange(compatibilities);
        _context.SaveChanges();

        return new SeedResult
        {
            Message = $"🎉 プロジェクト単位でデータ投入完了！\n" +
                      $"🚢 本日の船: {string.Join(", ", projects.Select(p => p.Name))}\n" +
                      $"👨‍🏭 スタッフ: {workers.Count}人\n" +
                      $"📦 作業枠: {cargos.Count}個 (定員合計: {cargos.Sum(c => c.RequiredCount)}名)"
        };
    }

    private Cargo CreateCargo(string baseName, string suffix, string skill, int count, string place)
    {
        return new Cargo
        {
            WorkDate = DateTime.Today,
            WorkName = $"{baseName} {suffix}",
            WorkPlace = place,
            RequiredSkill = skill,
            RequiredCount = count,
            AssignedWorkerId = 0
        };
    }
}