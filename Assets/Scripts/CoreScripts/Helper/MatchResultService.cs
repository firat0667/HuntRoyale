using Firat0667.WesternRoyaleLib.Game;
using Managers.Game;
using Managers.Score;
using UnityEngine;

namespace Helper.MatchResults
{
    public class MatchResultService
    {
        private readonly int[] rewardTable = { 50, 30, 15 };

        public MatchResult Evaluate()
        {
            var player = GameRegistry.Instance
                .Get<GameObject>(KeyTags.KEY_PLAYER)
                .GetComponent<BaseEntity>();
            int playerRank = ScoreManager.Instance.GetPlayerRank(player);
            int reward = 0;

            if (playerRank > 0 && playerRank <= rewardTable.Length)
                reward = rewardTable[playerRank - 1];

            if (reward > 0)
                GameManager.Instance.AddGold(reward);

            return new MatchResult(playerRank, reward);
        }
    }

    public struct MatchResult
    {
        public int Rank;
        public int Reward;

        public MatchResult(int rank, int reward)
        {
            Rank = rank;
            Reward = reward;
        }
    }
}
