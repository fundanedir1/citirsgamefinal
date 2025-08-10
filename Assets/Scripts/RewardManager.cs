using UnityEngine;
// Eğer MiniGameRewardType farklı namespace’deyse, onu da ekle
// using YourNamespace;

public class RewardManager : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // Bölünme sayısını kontrol eden sistem

    public void GiveReward(MiniGameRewardType rewardType)
    {
        switch (rewardType)
        {
            case MiniGameRewardType.IncreaseDivisionCount:
                monsterSpawner.maxDivisionCount += 1;
                Debug.Log("Bölünme sayısı artırıldı!");
                break;

            case MiniGameRewardType.IncreaseLifespan:
                IsoMonster[] monsters = FindObjectsOfType<IsoMonster>();
                foreach (var monster in monsters)
                {
                    monster.extraLifeTime += 5f; // 5 saniye ekle
                }
                Debug.Log("Tüm canavarların yaşam süresi artırıldı!");
                break;
        }
    }
}
