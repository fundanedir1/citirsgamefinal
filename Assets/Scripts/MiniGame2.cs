using UnityEngine;
using TMPro;

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
        currentPrompt = prompts[Mathf.Min(difficultyLevel - 1, prompts.Length - 1)];
        promptText.text = currentPrompt;
        inputField.text = "";
        inputField.ActivateInputField();
        timer = baseTimeLimit;
        gameActive = true;
        success = false;
        feedbackText.text = "";
        timerText.text = "";
        inputField.gameObject.SetActive(true);
        promptText.gameObject.SetActive(true);
    }

    public void UpdateMiniGame()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        if (timer <= 0f)
        {
            success = false;
            feedbackText.text = "Time's up! Try again.";
            inputField.text = "";
            inputField.ActivateInputField();
            timer = baseTimeLimit;
        }
        else if (inputField.text == currentPrompt)
        {
            success = true;
            gameActive = false;
            feedbackText.text = "Success!";
            inputField.gameObject.SetActive(false);
            promptText.gameObject.SetActive(false);
        }
    }

    public void CloseMiniGame()
    {
        inputField.gameObject.SetActive(false);
        promptText.gameObject.SetActive(false);
        feedbackText.text = "";
        timerText.text = "";
    }

    public bool IsFinished => !gameActive;
    public bool IsSuccess => success;
}
