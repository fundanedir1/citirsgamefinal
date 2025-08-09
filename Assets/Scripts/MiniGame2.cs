using UnityEngine;
using TMPro;
using System.Collections;

public class MiniGame2 : MonoBehaviour, IMiniGame
{
    public TMP_Text promptText;
    public TMP_InputField inputField;
    public TMP_Text timerText;
    public TMP_Text feedbackText;

    private string currentPrompt;
    private float timer;
    private bool gameActive;
    private bool success;

    public float baseTimeLimit = 10f;
    public string[] prompts = new string[] { "hello", "unity", "game", "code", "test" };

    public void StartMiniGame(int difficultyLevel)
    {
        currentPrompt = prompts[Random.Range(0, prompts.Length)];
        promptText.text = currentPrompt;

        inputField.text = "";
        inputField.gameObject.SetActive(true);
        promptText.gameObject.SetActive(true);
        feedbackText.text = "";
        timerText.text = "";

        inputField.ActivateInputField();

        timer = baseTimeLimit;
        gameActive = true;
        success = false;
    }
    private void Start()
    {
        inputField.gameObject.SetActive(false);
        promptText.gameObject.SetActive(false);
    }

    public void UpdateMiniGame()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        if (timer <= 0f)
        {
            // Süre bitti - başarısız
            gameActive = false;
            success = false;
            StartCoroutine(EndGameWithFeedback("Time's up! Try again."));
        }
        else if (inputField.text.ToLower() == currentPrompt.ToLower())
        {
            // Başarılı
            gameActive = false;
            success = true;
            StartCoroutine(EndGameWithFeedback("Success!"));
        }
    }

    private IEnumerator EndGameWithFeedback(string message)
    {
        // Feedback göster
        feedbackText.text = message;

        // Input ve prompt hala açık kalsın feedback görünürken
        inputField.gameObject.SetActive(false);
        promptText.gameObject.SetActive(false);
        timerText.text = "";

        // Feedback 2 saniye görünsün
        yield return new WaitForSeconds(2f);

        // Feedback temizle ve oyunu kapat
        feedbackText.text = "";
        CloseMiniGame();
    }

    public void CloseMiniGame()
    {
        inputField.gameObject.SetActive(false);
        promptText.gameObject.SetActive(false);
        feedbackText.text = "";
        timerText.text = "";
        gameActive = false;
    }

    public bool IsFinished => !gameActive;
    public bool IsSuccess => success;
}
