using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodScheduler.Api.Models;

[Table("CargoWorkers")]
public class CargoWorker
{
    [Key]
    public int Id { get; set; }

    [Column("base_no")]
    public int BaseNo { get; set; } // Workerとの紐付けキーと思われる

    // Cargoとの紐付けが必要だが、ER図上カラムが見当たらない場合、外部キーを追加検討
    // [Column("cargo_id")] public int CargoId { get; set; } 

    [Column("s_time")] public TimeSpan? StartTime { get; set; }
    [Column("e_time")] public TimeSpan? EndTime { get; set; }
    
    [Column("competence")] public int Competence { get; set; } // 必要な資格コード？
}