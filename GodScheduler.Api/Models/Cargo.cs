using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodScheduler.Api.Models;

[Table("Cargos")]
public class Cargo
{
    [Key]
    public int Id { get; set; }

    [Column("work_date")]
    public DateTime WorkDate { get; set; }

    // ❌ 削除: 下で定義しとるから、こっちは消すバイ！
    // public string WorkPlace { get; set; } = "";

    [Column("work_name")]
    [StringLength(32)]
    public string? WorkName { get; set; }

    [Column("cargo_name")]
    [StringLength(32)]
    public string? CargoName { get; set; }

    // DBには文字で入るかもしれんけど、ロジックでは下の RequiredCount を使うぞ
    [Column("quantity")]
    [StringLength(16)]
    public string? Quantity { get; set; }

    // ✅ 生かすのはこっち（カラム定義付き）
    [Column("work_place")]
    [StringLength(16)]
    public string? WorkPlace { get; set; }

    [Column("required_skill")]
    public string? RequiredSkill { get; set; } // null許容にしておくと安全

    // 👇 定員数（カラム名も付けておいたぞ）
    [Column("required_count")]
    public int RequiredCount { get; set; } = 1; 

    // PDF用などの項目
    [Column("s_time")] public TimeSpan? StartTime { get; set; }
    [Column("e_time")] public TimeSpan? EndTime { get; set; }
    [Column("conf_flg")] public int ConfFlg { get; set; }

    [Column("assigned_worker_id")]
    public int AssignedWorkerId { get; set; }
}