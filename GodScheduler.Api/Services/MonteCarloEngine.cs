using GodScheduler.Api.Models;

namespace GodScheduler.Api.Services;

public class MonteCarloEngine
{
    private Random _random = new Random();

    public class AllocationResult
    {
        public List<Cargo> AssignedCargos { get; set; } = new();
        public int Score { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public AllocationResult Solve(List<Worker> workers, List<Cargo> cargos, int iterations = 2000)
    {
        var bestScore = -99999;
        List<Cargo> bestAssignment = new List<Cargo>();

        for (int i = 0; i < iterations; i++)
        {
            // 1. スタッフリストをシャッフル（運要素）
            var shuffledWorkers = workers.OrderBy(x => _random.Next()).ToList();
            
            // 2. 割り当て管理用のリスト（誰が空いてるか）
            // IDだけでなく、オブジェクトそのものをコピーして管理
            var availableWorkers = shuffledWorkers.ToList();

            // 3. 案件リストのコピーを作る（毎回新しい割り当てを試すため）
            var currentCargos = cargos.Select(c => new Cargo 
            { 
                Id = c.Id, 
                WorkName = c.WorkName, 
                RequiredSkill = c.RequiredSkill,
                AssignedWorkerId = 0 
            }).ToList();

            // 4. 【進化ポイント】賢い割り当てループ
            foreach (var cargo in currentCargos)
            {
                // この案件に「適合する」スタッフを、空いてる人の中から探す！
                var candidate = availableWorkers.FirstOrDefault(w => IsQualified(w, cargo.RequiredSkill));

                if (candidate != null)
                {
                    // 適合者がいたら割り当て！
                    cargo.AssignedWorkerId = candidate.Id;
                    availableWorkers.Remove(candidate); // その人はもう埋まった
                }
                else
                {
                    // 適合者がいない場合、誰も割り当てない（0のまま）
                    // 無理やり割り当てると事故になるからな！
                }
            }

            // 5. スコア計算
            int score = CalculateScore(currentCargos, workers);

            // 6. 過去最高なら記録更新
            if (score > bestScore)
            {
                bestScore = score;
                bestAssignment = currentCargos;
            }
        }

        return new AllocationResult 
        { 
            AssignedCargos = bestAssignment, 
            Score = bestScore, 
            Message = $"最適化完了 (試行回数:{iterations})"
        };
    }

    // ★重要: 「こいつはこの仕事ができるか？」を判定する審判メソッド
    // ↓↓↓ このメソッドを修正 ↓↓↓
    private bool IsQualified(Worker worker, string requiredSkill)
    {
        // 1. 誰でもできる仕事ならOK
        if (requiredSkill == "なし" || string.IsNullOrEmpty(requiredSkill)) return true;

        // 2. DBのSkillsカラムを見て判定する（正規の方法）
        if (string.IsNullOrEmpty(worker.Skills)) return false;

        // "大型免許,リフト" みたいな文字列の中に、必要なスキルが含まれてるか？
        return worker.Skills.Contains(requiredSkill);
    }

    private int CalculateScore(List<Cargo> assignment, List<Worker> workers)
    {
        int score = 0;
        foreach (var cargo in assignment)
        {
            if (cargo.AssignedWorkerId != 0)
            {
                score += 10; // 割り当てられたらプラス
                
                // さらに、適切なスキルならボーナス（念のためここでもチェック）
                var worker = workers.FirstOrDefault(w => w.Id == cargo.AssignedWorkerId);
                if (worker != null && IsQualified(worker, cargo.RequiredSkill))
                {
                    score += 5;
                }
            }
            else
            {
                score -= 10; // 未割り当てはマイナス
            }
        }
        return score;
    }
}