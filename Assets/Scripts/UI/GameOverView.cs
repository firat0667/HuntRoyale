using Game.UI;
using Helper.MatchResults;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class GameOverView : UIBase
    {
        [SerializeField] private TextMeshProUGUI m_rewardText;
        private void OnEnable()
        {
            var controller = FindObjectOfType<GameLoopController>();
            if (controller != null)
            {
                var result = controller.LastMatchResult;
                m_rewardText.text = $"+{result.Reward}";
            }
        }
    }
}
