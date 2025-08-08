using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniGame3 : MonoBehaviour, IMiniGame
{
    public RectTransform playArea;
    public GameObject dotPrefab;
    public TMP_Text timerText;
    public TMP_Text feedbackText;

    private float timer;
    private bool gameActive;
    private bool success;

    public float baseTimeLimit = 10f;
    public int dotsToDestroyForWin = 3;  // Başlangıçta kaç dot yok etmen gerekiyor
    public int clicksPerDot = 10;         // Her dot kaç kere tıklanmalı

    private int destroyedDots = 0;

    private Dictionary<GameObject, int> dotClickCounts = new();

    public void StartMiniGame(int difficultyLevel)
    {
        timer = baseTimeLimit;
        destroyedDots = 0;
        gameActive = true;
        success = false;
        feedbackText.text = "";
        timerText.text = "";

        int dotCount = dotsToDestroyForWin + (difficultyLevel - 1) * 2;
        SpawnDots(dotCount);

        playArea.gameObject.SetActive(true);
    }

    public void UpdateMiniGame()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        if (timer <= 0f)
        {
            gameActive = false;
            success = false;
            feedbackText.text = "Time's up! Fail.";
            ClearDots();
            playArea.gameObject.SetActive(false);
        }
        else if (destroyedDots >= dotsToDestroyForWin)
        {
            gameActive = false;
            success = true;
            feedbackText.text = "Success!";
            ClearDots();
            playArea.gameObject.SetActive(false);
        }
    }

    void SpawnDots(int count)
    {
        ClearDots();
        dotClickCounts.Clear();

        Vector2 size = playArea.rect.size;

        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, playArea);
            dot.SetActive(true);
            dotClickCounts[dot] = clicksPerDot;

            Button btn = dot.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                Debug.Log("Dot clicked!");

                if (!gameActive) return;

                dotClickCounts[dot]--;

                if (dotClickCounts[dot] <= 0)
                {
                    destroyedDots++;
                    dot.SetActive(false);
                    dotClickCounts.Remove(dot);
                }
            });

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
    }

    public void CloseMiniGame()
    {
        ClearDots();
        playArea.gameObject.SetActive(false);
        feedbackText.text = "";
        timerText.text = "";
    }

    public bool IsFinished => !gameActive;
    public bool IsSuccess => success;
}
