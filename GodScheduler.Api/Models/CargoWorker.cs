using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodScheduler.Api.Models;

[Table("CargoWorkers")]
public class CargoWorker
{
    [Key]
    public int Id { get; set; }

    // 👇 AIエンジンは "WorkerId" を探してる！ DBの "base_no" と紐付けるぞ
    [Column("base_no")]
    public int WorkerId { get; set; } 

    // 👇 これがないと「どの仕事か」分からんバイ！コメントアウト解除！
    [Column("cargo_id")] 
    public int CargoId { get; set; } 

    [Column("s_time")] public TimeSpan? StartTime { get; set; }
    [Column("e_time")] public TimeSpan? EndTime { get; set; }
    
    [Column("competence")] public int Competence { get; set; } // 資格コード

    // ---------------------------------------------------------
    // 👇【追加】AI配番の表示用プロパティ
    // DBには保存しないから [NotMapped] をつけて無視させる！
    // ---------------------------------------------------------
    [NotMapped]
    public string WorkerName { get; set; } = string.Empty;
}