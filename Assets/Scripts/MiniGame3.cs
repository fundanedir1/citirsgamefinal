using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame3 : MonoBehaviour, IMiniGame
{
    public RectTransform playArea; // UI panel veya Canvas altı alan
    public GameObject dotPrefab;   // küçük basılabilir nokta prefabı
    public TMP_Text timerText;
    public TMP_Text feedbackText;

    private float timer;
    private bool gameActive;
    private bool success;

    private int requiredHits;
    private int currentHits;

    public float baseTimeLimit = 10f;
    public int baseRequiredHits = 3;

    public void StartMiniGame(int difficultyLevel)
    {
        requiredHits = baseRequiredHits + difficultyLevel - 1;
        timer = baseTimeLimit;
        currentHits = 0;
        gameActive = true;
        success = false;
        feedbackText.text = "";
        timerText.text = "";
        SpawnDots(requiredHits);
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
        else if (currentHits >= requiredHits)
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

        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, playArea);
            dot.GetComponent<Button>().onClick.RemoveAllListeners();
            dot.GetComponent<Button>().onClick.AddListener(() =>
            {
                currentHits++;
                dot.SetActive(false);
            });

            // Rastgele pozisyon: playArea içinde
            Vector2 size = playArea.rect.size;
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
