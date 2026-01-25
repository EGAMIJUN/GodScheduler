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

    [Column("work_name")]
    [StringLength(32)]
    public string? WorkName { get; set; }

    [Column("cargo_name")]
    [StringLength(32)]
    public string? CargoName { get; set; }

    [Column("quantity")]
    [StringLength(16)]
    public string? Quantity { get; set; }

    [Column("work_place")]
    [StringLength(16)]
    public string? WorkPlace { get; set; }

    [Column("required_skill")]
    public string RequiredSkill { get; set; } = "なし";

    // PDFの項目
    [Column("s_time")] public TimeSpan? StartTime { get; set; }
    [Column("e_time")] public TimeSpan? EndTime { get; set; }
    [Column("conf_flg")] public int ConfFlg { get; set; }

    public int AssignedWorkerId { get; set; }
}