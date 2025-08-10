using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniGame3 : MonoBehaviour, IMiniGame
{
    public RewardManager rewardManager;  // Ödül sistemi referansı

    public RectTransform playArea;
    public GameObject dotPrefab;
    public TMP_Text timerText;
    public TMP_Text feedbackText;

    private float timer;
    private bool gameActive;
    private bool success;

    public float baseTimeLimit = 10f;
    public int baseDotsToDestroyForWin = 3;  // İlk oyunda yok edilecek dot sayısı
    public int clicksPerDot = 10;            // Her dot'a tıklama sayısı

    private int destroyedDots = 0;
    private int currentTargetDots = 0;       // Bu turda yok edilmesi gereken dot sayısı

    private Dictionary<GameObject, int> dotClickCounts = new();
    private int totalRoundsPlayed = 0;       // Kaç kere oynandığını takip etmek için

    public void StartMiniGame(int difficultyLevel)
    {
        timer = baseTimeLimit;
        destroyedDots = 0;
        gameActive = true;
        success = false;
        feedbackText.text = "";
        timerText.text = "";

        // Her oynanışta hedef dot sayısını artır
        currentTargetDots = baseDotsToDestroyForWin + totalRoundsPlayed;
        totalRoundsPlayed++;

        SpawnDots(currentTargetDots);

        playArea.gameObject.SetActive(true);
    }

    public void UpdateMiniGame()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        // Zaman doldu
        if (timer <= 0f)
        {
            EndGame(false, "Time's up! Fail.");
        }
        // Tüm hedef dotlar yok edildi
        else if (destroyedDots >= currentTargetDots)
        {
            EndGame(true, "Success!");
        }
    }

    void SpawnDots(int count)
    {
        ClearDots();
        dotClickCounts.Clear();

        Vector2 size = playArea.rect.size / 2;

        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, playArea);
            dot.SetActive(true);
            dotClickCounts[dot] = clicksPerDot;

            Button btn = dot.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (!gameActive) return;

                dotClickCounts[dot]--;

                if (dotClickCounts[dot] <= 0)
                {
                    destroyedDots++;
                    dot.SetActive(false);
                    dotClickCounts.Remove(dot);
                }
            });

            // Rastgele konum
            Vector2 randomPos = new Vector2(
                Random.Range(0, size.x),
                Random.Range(0, size.y)
            );
            dot.GetComponent<RectTransform>().anchoredPosition = randomPos;
        }
    }

    void ClearDots()
    {
        foreach (Transform child in playArea)
            Destroy(child.gameObject);

        dotClickCounts.Clear();
        playArea.gameObject.SetActive(false); // Alanı da kapat
    }

    void EndGame(bool win, string message)
    {
        gameActive = false;
        success = win;
        feedbackText.text = message;

        // Başarı durumunda ödül ver
        if (win && rewardManager != null)
        {
            rewardManager.GiveReward(MiniGameRewardType.IncreaseLifespan);
        }

        ClearDots(); // Artık burada SetActive(false) otomatik olacak
    }

    public void CloseMiniGame()
    {
        ClearDots(); // Artık burada da ayrıca kapatmaya gerek yok
        feedbackText.text = "";
        timerText.text = "";
    }

    public bool IsFinished => !gameActive;
    public bool IsSuccess => success;
}
