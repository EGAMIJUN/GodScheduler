namespace GodScheduler.Api.Models;

// 昼食注文先 (例: ほっともっと、セブンイレブン)
public class LunchVendor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // 名称
    public int WhFlg { get; set; } = 0; // 0:平日用, 1:休日用
}

// 昼食メニュー (例: のり弁、唐揚げ弁当)
public class LunchMenu
{
    public int Id { get; set; }
    public int LunchVendorId { get; set; } // どこの店のメニューか
    public string Name { get; set; } = string.Empty; // メニュー名
    public int Price { get; set; } // 価格（仕様書にはなかったが、あったほうがいい）
}

// 昼食注文 (誰が何を頼んだか)
public class LunchOrder
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } // 注文日
    public int UserId { get; set; } // 注文者 (Worker.Id)
    public string UserName { get; set; } = string.Empty; // 注文者名（履歴用）
    public int LunchVendorId { get; set; }
    public int LunchMenuId { get; set; }
    public int OrderNum { get; set; } = 1; // 注文数
}