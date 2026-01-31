using GodScheduler.Api.Models;

namespace GodScheduler.Api.Services
{
    // 結果を格納するクラス
    public class AllocationResult
    {
        // 誰をどこに配置したかのリスト
        public List<CargoWorker> Matches { get; set; } = new();
        public double Score { get; set; }
        public string LogicMessage { get; set; } = string.Empty;
    }

    public class MonteCarloEngine
    {
        // 試行回数 (多いほど良い結果が出るが遅くなる)
        private const int SIMULATION_COUNT = 3000;

        // メイン処理: 最適化を実行
        public AllocationResult Optimize(List<Worker> allWorkers, List<Cargo> allCargoes, List<WorkerCompatibility> compatibilities)
        {
            var bestResult = new AllocationResult { Score = -999999 };
            var rand = new Random();

            for (int i = 0; i < SIMULATION_COUNT; i++)
            {
                // 1. シャッフル
                var shuffledWorkers = allWorkers.OrderBy(x => rand.Next()).ToList();
                var currentMatches = new List<CargoWorker>();
                var availableWorkerIds = new HashSet<int>(shuffledWorkers.Select(w => w.Id));

                // 2. 割り当て試行
               // 2. 割り当て試行
                foreach (var cargo in allCargoes)
                {
                    // 👇 ループ変更！定員(RequiredCount)の分だけ人を採用する！
                    for (int count = 0; count < cargo.RequiredCount; count++)
                    {
                        // 条件に合う人を探す
                        var candidate = shuffledWorkers.FirstOrDefault(w => 
                            availableWorkerIds.Contains(w.Id) && 
                            CanAssign(w, cargo) // 必須スキルチェック
                        );

                        if (candidate != null)
                        {
                            // マッチングリストに追加
                            currentMatches.Add(new CargoWorker
                            {
                                CargoId = cargo.Id,
                                WorkerId = candidate.Id,
                                WorkerName = candidate.Name
                            });
                            availableWorkerIds.Remove(candidate.Id); // 割り当て済みリストへ
                        }
                        else
                        {
                            // もう条件に合う人がいない場合、この枠は空席になる
                            break;
                        }
                    }
                }

                // 3. スコア計算（賢さの源）
                // 引数に compatibilities を渡すのを忘れずに！
                double currentScore = CalculateScore(currentMatches, allWorkers, allCargoes, compatibilities);

                // 4. 最高記録更新なら保存
                if (currentScore > bestResult.Score)
                {
                    bestResult.Matches = new List<CargoWorker>(currentMatches);
                    bestResult.Score = currentScore;
                }
            }

            bestResult.LogicMessage = $"AI (MonteCarlo) Simulated {SIMULATION_COUNT} times. Best Score: {bestResult.Score:F1}";
            return bestResult;
        }

        // --- 必須スキルチェック ---
        private bool CanAssign(Worker worker, Cargo cargo)
        {
            // "なし" や 空の場合は誰でもOK
            if (string.IsNullOrEmpty(cargo.RequiredSkill) || cargo.RequiredSkill == "なし")
            {
                return true;
            }
            
            // スキルが必要な場合、持っているかチェック
            if (string.IsNullOrEmpty(worker.Skills) || !worker.Skills.Contains(cargo.RequiredSkill))
            {
                return false;
            }
            
            return true;
        }

        // --- スコアリング（評価関数） ---
        private double CalculateScore(
            List<CargoWorker> matches, 
            List<Worker> allWorkers, 
            List<Cargo> allCargoes, 
            List<WorkerCompatibility> compatibilities)
        {
            double score = 0;

            foreach (var match in matches)
            {
                var worker = allWorkers.First(w => w.Id == match.WorkerId);
                var cargo = allCargoes.First(c => c.Id == match.CargoId);

                // --- ルール1: 疲労度チェック ---
                if (worker.FatigueLevel > 80) score -= 50; 
                else if (worker.FatigueLevel < 30) score += 10;

                // --- ルール2: スキル適合ボーナス ---
                if (!string.IsNullOrEmpty(cargo.RequiredSkill) 
                    && cargo.RequiredSkill != "なし" 
                    && worker.Skills.Contains(cargo.RequiredSkill))
                {
                    score += 20;
                }

                // 🔥【ここが追加箇所！】ルール4: 人間関係（相性）チェック 🔥
                // 今日シフトに入っている「他の全員」との相性を見る
                foreach (var otherMatch in matches)
                {
                    // 自分自身とは比較しない
                    if (match.WorkerId == otherMatch.WorkerId) continue;

                    // DBの相性テーブルから、この2人のペアを探す
                    // (AとB、または BとA のどちらかで登録されているはず)
                    var compatibility = compatibilities.FirstOrDefault(c => 
                        (c.WorkerId1 == match.WorkerId && c.WorkerId2 == otherMatch.WorkerId) ||
                        (c.WorkerId1 == otherMatch.WorkerId && c.WorkerId2 == match.WorkerId)
                    );

                    if (compatibility != null)
                    {
                        // 相性スコアを加算！
                        // 仲が良い(+100)ならスコアアップ
                        // 仲が悪い(-9999)ならスコア激減 → この組み合わせは選ばれなくなる！
                        score += compatibility.Score;
                    }
                }
            }
            
            // 未割り当てのペナルティ (仕事があるのに人がいない場合)
            int unassignedCargos = allCargoes.Count - matches.Select(m => m.CargoId).Distinct().Count();
            score -= unassignedCargos * 100;

            return score;
        }
    }
}