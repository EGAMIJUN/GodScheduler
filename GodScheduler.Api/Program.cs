using Microsoft.EntityFrameworkCore;
using GodScheduler.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// --- 1. サービスの登録エリア ---

// DB接続の設定
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS設定
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

// =========================================================
// 👇【追加】ここバイ！起動時にDBがなければ自動で作る魔法！
// =========================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // データベースがなければ作成する！
        context.Database.EnsureCreated();
        Console.WriteLine("✅ データベースの準備完了バイ！ (GodSchedulerDb Created)");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ DB作成中にエラー発生バイ: {ex.Message}");
    }
}
// =========================================================

// --- 2. パイプライン設定エリア ---

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