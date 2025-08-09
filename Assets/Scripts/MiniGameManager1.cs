using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager1 : MonoBehaviour
{
    public MonoBehaviour[] miniGameScripts;
    public TMP_Text[] timerTexts;
    public TMP_Text[] feedbackTexts;

    private IMiniGame[] miniGames;
    private IMiniGame currentMiniGame;
    private int currentMiniGameIndex = -1;

    private int[] difficultyCounters;

    public float firstMiniGameDelay = 5f;
    public float miniGameDuration = 10f;
    public float feedbackDisplayTime = 3f;
    public float waitBetweenMiniGames = 2f;

    private Coroutine timerCoroutine;

    void Awake()
    {
        miniGames = new IMiniGame[miniGameScripts.Length];
        for (int i = 0; i < miniGameScripts.Length; i++)
        {
            miniGames[i] = miniGameScripts[i] as IMiniGame;

            if (timerTexts != null && i < timerTexts.Length && timerTexts[i] != null)
                timerTexts[i].gameObject.SetActive(false);
            if (feedbackTexts != null && i < feedbackTexts.Length && feedbackTexts[i] != null)
                feedbackTexts[i].gameObject.SetActive(false);
        }

        difficultyCounters = new int[miniGames.Length];
    }

    void Start()
    {
        StartCoroutine(MiniGameLoop());
    }

    IEnumerator MiniGameLoop()
    {
        yield return new WaitForSeconds(firstMiniGameDelay);

        while (true)
        {
            int randomIndex = Random.Range(0, miniGames.Length);
            currentMiniGameIndex = randomIndex;
            currentMiniGame = miniGames[randomIndex];
            int difficulty = difficultyCounters[randomIndex] + 1;

            // Timer ve feedback textlerini göster (feedback boş)
            if (timerTexts != null && timerTexts[randomIndex] != null)
                timerTexts[randomIndex].gameObject.SetActive(true);
            if (feedbackTexts != null && feedbackTexts[randomIndex] != null)
            {
                feedbackTexts[randomIndex].gameObject.SetActive(true);
                feedbackTexts[randomIndex].text = "";
            }

            currentMiniGame.StartMiniGame(difficulty);

            // Timer coroutine başlat
            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(UpdateTimer(randomIndex));

            // Mini oyun bitene kadar bekle veya süre dolana kadar
            float timer = miniGameDuration;
            while (timer > 0 && !currentMiniGame.IsFinished)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            // Eğer süre dolduysa oyun zorla kapatılır
            if (!currentMiniGame.IsFinished)
            {
                currentMiniGame.CloseMiniGame();
                currentMiniGame = null;
            }

            // Timer text gizle
            if (timerTexts != null && timerTexts[randomIndex] != null)
            {
                timerTexts[randomIndex].gameObject.SetActive(false);
                timerTexts[randomIndex].text = "";
            }

            // Timer coroutine durdur
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            // Feedback göster
            if (currentMiniGame != null && currentMiniGame.IsSuccess)
            {
                difficultyCounters[randomIndex]++;
                if (feedbackTexts != null && feedbackTexts[randomIndex] != null)
                    feedbackTexts[randomIndex].text = "Success!";
            }
            else
            {
                if (feedbackTexts != null && feedbackTexts[randomIndex] != null)
                    feedbackTexts[randomIndex].text = "Failed or Time Out!";
            }

            yield return new WaitForSeconds(feedbackDisplayTime);

            // Feedback gizle
            if (feedbackTexts != null && feedbackTexts[randomIndex] != null)
                feedbackTexts[randomIndex].gameObject.SetActive(false);

            currentMiniGame = null;

            yield return new WaitForSeconds(waitBetweenMiniGames);
        }
    }

    IEnumerator UpdateTimer(int index)
    {
        float timer = miniGameDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timerTexts != null && index < timerTexts.Length && timerTexts[index] != null)
                timerTexts[index].text = $"Time: {timer:F1}s";
            yield return null;
        }
    }

    void Update()
    {
        if (currentMiniGame != null)
            currentMiniGame.UpdateMiniGame();
    }
}
