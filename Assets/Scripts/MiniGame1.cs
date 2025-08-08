using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGame1 : MonoBehaviour, IMiniGame
{
    public Transform arrowContainer;
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject rightArrowPrefab;
    public TMP_Text timerText;
    public TMP_Text feedbackText;

    private KeyCode[] currentSequence;
    private int currentInputIndex;
    private float timer;
    private bool gameActive;
    private bool success;

    public int baseSequenceLength = 3;
    public float baseTimeLimit = 10f;

    public void StartMiniGame(int difficultyLevel)
    {
        int sequenceLength = baseSequenceLength + (difficultyLevel - 1) * 2;
        currentSequence = GenerateSequence(sequenceLength);
        CreateArrowUI(currentSequence);
        timer = baseTimeLimit;
        currentInputIndex = 0;
        gameActive = true;
        success = false;
        feedbackText.text = "";
        timerText.text = "";
        arrowContainer.gameObject.SetActive(true);
    }

    public void UpdateMiniGame()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();

        if (timer <= 0)
        {
            gameActive = false;
            success = false;
            feedbackText.text = "Time's up! Fail.";
            arrowContainer.gameObject.SetActive(false);
            return;
        }

        if (Input.anyKeyDown)
        {
            CheckInput();
        }
    }

    public void CloseMiniGame()
    {
        arrowContainer.gameObject.SetActive(false);
        feedbackText.text = "";
        timerText.text = "";
    }

    public bool IsFinished => !gameActive;
    public bool IsSuccess => success;

    KeyCode[] GenerateSequence(int length)
    {
        KeyCode[] sequence = new KeyCode[length];
        KeyCode[] options = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
        for (int i = 0; i < length; i++)
        {
            sequence[i] = options[Random.Range(0, options.Length)];
        }
        return sequence;
    }

    void CreateArrowUI(KeyCode[] sequence)
    {
        foreach (Transform child in arrowContainer)
            Destroy(child.gameObject);

        foreach (KeyCode key in sequence)
        {
            GameObject prefab = GetPrefabForKey(key);
            Instantiate(prefab, arrowContainer);
        }
    }

    GameObject GetPrefabForKey(KeyCode key)
    {
        return key switch
        {
            KeyCode.UpArrow => upArrowPrefab,
            KeyCode.DownArrow => downArrowPrefab,
            KeyCode.LeftArrow => leftArrowPrefab,
            KeyCode.RightArrow => rightArrowPrefab,
            _ => upArrowPrefab,
        };
    }

    void CheckInput()
    {
        if (currentInputIndex >= currentSequence.Length) return;

        if (Input.GetKeyDown(currentSequence[currentInputIndex]))
        {
            MarkArrowColor(currentInputIndex, true);
            currentInputIndex++;
            if (currentInputIndex >= currentSequence.Length)
            {
                success = true;
                gameActive = false;
                feedbackText.text = "Success!";
                arrowContainer.gameObject.SetActive(false);
            }
        }
        else if (Input.anyKeyDown)
        {
            MarkArrowColor(currentInputIndex, false);
            ResetInput();
        }
    }

    void MarkArrowColor(int index, bool correct)
    {
        if (index < 0 || index >= arrowContainer.childCount) return;

        Image img = arrowContainer.GetChild(index).GetComponent<Image>();
        if (img != null)
        {
            img.color = correct ? Color.green : Color.red;
        }
    }

    void ResetInput()
    {
        for (int i = 0; i < arrowContainer.childCount; i++)
        {
            Image img = arrowContainer.GetChild(i).GetComponent<Image>();
            if (img != null)
                img.color = Color.white;
        }
        currentInputIndex = 0;
        feedbackText.text = "Try again!";
    }
}
