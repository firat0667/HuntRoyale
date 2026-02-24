using Firat0667.WesternRoyaleLib.Patterns;
using Managers.Score;
using System.Collections.Generic;
using System.Linq;

namespace Managers.Leaderboard
{
    public class LeaderboardManager : FoundationSingleton<LeaderboardManager>, IFoundationSingleton
    {
        public bool Initialized { get; set; }

        private List<BaseEntity> m_participants = new();

        public void RegisterParticipant(BaseEntity entity)
        {
            if (!m_participants.Contains(entity))
                m_participants.Add(entity);
        }

        public void UnregisterParticipant(BaseEntity entity)
        {
            m_participants.Remove(entity);
        }

        public List<BaseEntity> GetParticipants()
        {
            return m_participants;
        }
        public List<LeaderboardEntry> Build(List<BaseEntity> participants)
        {
            if (participants == null) return new List<LeaderboardEntry>();

            return participants
                .Where(p => p != null)
                .Select(p => new LeaderboardEntry
                {
                    Name = p.name,
                    Score = ScoreManager.Instance.GetScore(p),
                    Entity = p
                })
                .OrderBy(x => x.Entity.IsDead)              
                .ThenByDescending(x => x.Score)           
                .ToList();
        }
        public class LeaderboardEntry
        {
            public string Name;
            public int Score;
            public BaseEntity Entity;
        }
    }
}
