using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public int winCheckInterval = 5; // kaç saniyede bir kontrol yapılacak
    private float winCheckTimer;

    public TMP_Text statusText; // Win / Game Over mesajı için UI Text

    void Update()
    {
        winCheckTimer -= Time.deltaTime;
        if (winCheckTimer <= 0f)
        {
            CheckWinCondition();
            winCheckTimer = winCheckInterval;
        }
    }

    void CheckWinCondition()
    {
        int monsterCount = FindObjectsOfType<IsoMonster>().Length;
        int enemyCount = enemySpawner.enemies.Count;

        if (monsterCount > enemyCount)
        {
            statusText.text = "You Win!";
            // Oyun bitti, gerekli işlemler
            EndGame(true);
        }
        else if (monsterCount < enemyCount)
        {
            statusText.text = "Game Over!";
            // Oyun bitti, gerekli işlemler
            EndGame(false);
        }
        else
        {
            statusText.text = ""; // Eşitlikte devam
        }
    }

    void EndGame(bool win)
    {
        // Tüm minigameleri ve spawnları durdurabilirsin
        Time.timeScale = 0f; // Oyunu durdurmak için basit yöntem
    }
}
