using Microsoft.EntityFrameworkCore;
using GodScheduler.Api.Data;
using GodScheduler.Api.Services; // 👈 1. これを追加！

var builder = WebApplication.CreateBuilder(args);

// --- サービスの登録エリア ---

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 👇 2. 【ここに追加！】SeedServiceを使えるように登録するバイ
builder.Services.AddScoped<ISeedService, SeedService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------

var app = builder.Build();

// (以下、既存のDB自動作成ロジックなどはそのままでOK)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        Console.WriteLine("✅ データベースの準備完了バイ！");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ DB作成エラー: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();