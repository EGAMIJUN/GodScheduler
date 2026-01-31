namespace GodScheduler.Api.Models;

public class WorkerCompatibility
{
    public int Id { get; set; }
    
    // 誰と
    public int WorkerId1 { get; set; }
    
    // 誰が
    public int WorkerId2 { get; set; }
    
    // 相性スコア (プラスなら相性良し、マイナスなら犬猿の仲)
    // 例: -100 (絶対混ぜるな), +50 (名コンビ)
    public int Score { get; set; }
}