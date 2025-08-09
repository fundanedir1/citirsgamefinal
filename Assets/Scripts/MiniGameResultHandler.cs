using UnityEngine;

public class MiniGameResultHandler : MonoBehaviour
{
    public RewardManager rewardManager;

    public void OnMiniGameEnd(bool success)
    {
        if (success)
        {
            MiniGameRewardType randomReward = (Random.value > 0.5f)
                ? MiniGameRewardType.IncreaseDivisionCount
                : MiniGameRewardType.IncreaseLifespan;

            rewardManager.GiveReward(randomReward);
        }
        else
        {
            Debug.Log("MiniGame başarısız, ödül yok.");
        }
    }
}
