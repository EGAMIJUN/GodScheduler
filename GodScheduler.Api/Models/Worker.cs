using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodScheduler.Api.Models;

[Table("Wokers")] // PDFの定義に合わせてテーブル名を指定
public class Worker
{
    [Key]
    public int Id { get; set; } // EF Core用の主キー

    [Column("s_name")] 
    [StringLength(16)]
    public string Name { get; set; } = string.Empty;

    // ↓↓↓ この2行を追加するバイ！！ ↓↓↓
    public string Skills { get; set; } = string.Empty; // "大型,リフト" みたいな文字で入る
    public int FatigueLevel { get; set; } = 0;         // 疲労度 (0-100)
    // ↑↑↑ ここまで ↑↑↑

    [Column("competence_bhh")] public int CompetenceBhh { get; set; } // 資格情報の例
    [Column("competence_wwm")] public int CompetenceWwm { get; set; }

    // PDFにある共通カラム
    [Column("branch_cd")] [StringLength(8)] public string? BranchCd { get; set; }
    [Column("created_uid")] [StringLength(16)] public string? CreatedUid { get; set; }
    [Column("deleted_at")] public DateTime? DeletedAt { get; set; }

    [NotMapped]
    public string? LastAssignedLocation { get; set; }
}