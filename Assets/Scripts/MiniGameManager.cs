using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MiniGameManager : MonoBehaviour
{
    [Header("Oyun Ayarları")]
    public int startSequenceLength = 3;
    public float timeLimit = 10f;
    public MonsterSpawner spawner;
    public float successSpawnMultiplier = 0.8f;

    [Header("UI Referansları")]
    public Transform arrowContainer;
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject rightArrowPrefab;
    public TMP_Text timerText;
    public TMP_Text feedbackText;


    private KeyCode[] currentSequence;
    private int currentInputIndex = 0;
    private float timer;
    private bool gameActive = false;

    void Start()
    {
        StartMiniGame();
    }

    void StartMiniGame()
    {
        feedbackText.text = "";
        timer = timeLimit;
        currentInputIndex = 0;
        currentSequence = GenerateSequence(startSequenceLength);
        CreateArrowUI(currentSequence);
        gameActive = true;
    }

    void Update()
    {
        if (!gameActive)
            return;

        timer -= Time.deltaTime;
        UpdateTimerUI();

        if (timer <= 0f)
        {
            OnFail();
            return;
        }

        if (Input.anyKeyDown)
        {
            CheckInput();
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
    }

    KeyCode[] GenerateSequence(int length)
    {
        KeyCode[] keys = new KeyCode[length];
        KeyCode[] possibleKeys = new KeyCode[] {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };

        for (int i = 0; i < length; i++)
        {
            keys[i] = possibleKeys[Random.Range(0, possibleKeys.Length)];
        }

        return keys;
    }

    void CreateArrowUI(KeyCode[] sequence)
    {
        foreach (Transform child in arrowContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyCode key in sequence)
        {
            GameObject arrowGO = null;

            switch (key)
            {
                case KeyCode.UpArrow:
                    arrowGO = Instantiate(upArrowPrefab, arrowContainer);
                    break;
                case KeyCode.DownArrow:
                    arrowGO = Instantiate(downArrowPrefab, arrowContainer);
                    break;
                case KeyCode.LeftArrow:
                    arrowGO = Instantiate(leftArrowPrefab, arrowContainer);
                    break;
                case KeyCode.RightArrow:
                    arrowGO = Instantiate(rightArrowPrefab, arrowContainer);
                    break;
            }

            if (arrowGO != null)
            {
                Image arrowImage = arrowGO.GetComponent<Image>();
                if (arrowImage != null)
                    arrowImage.color = Color.white;
            }
        }
    }

    void CheckInput()
    {
        KeyCode expectedKey = currentSequence[currentInputIndex];

        bool correct = false;

        switch (expectedKey)
        {
            case KeyCode.UpArrow:
                correct = Input.GetKeyDown(KeyCode.UpArrow);
                break;
            case KeyCode.DownArrow:
                correct = Input.GetKeyDown(KeyCode.DownArrow);
                break;
            case KeyCode.LeftArrow:
                correct = Input.GetKeyDown(KeyCode.LeftArrow);
                break;
            case KeyCode.RightArrow:
                correct = Input.GetKeyDown(KeyCode.RightArrow);
                break;
        }

        if (correct)
        {
            MarkArrowColor(currentInputIndex, Color.green);
            currentInputIndex++;

            if (currentInputIndex >= currentSequence.Length)
            {
                OnSuccess();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) ||
                 Input.GetKeyDown(KeyCode.DownArrow) ||
                 Input.GetKeyDown(KeyCode.LeftArrow) ||
                 Input.GetKeyDown(KeyCode.RightArrow))
        {
            MarkArrowColor(currentInputIndex, Color.red);
            ResetInput();
        }
    }

    void MarkArrowColor(int index, Color color)
    {
        if (index < arrowContainer.childCount)
        {
            Image img = arrowContainer.GetChild(index).GetComponent<Image>();
            if (img != null)
                img.color = color;
        }
    }

    void ResetInput()
    {
        currentInputIndex = 0;
        for (int i = 0; i < arrowContainer.childCount; i++)
        {
            Image img = arrowContainer.GetChild(i).GetComponent<Image>();
            if (img != null)
                img.color = Color.white;
        }
    }

    void OnSuccess()
    {
        feedbackText.text = "Success! Spawn speed increased!";
        spawner.spawnInterval *= successSpawnMultiplier;
        startSequenceLength += 2;
        gameActive = false;

        Invoke(nameof(StartMiniGame), 2f);
    }

    void OnFail()
    {
        feedbackText.text = "Fail! Try again.";
        ResetInput();
        timer = timeLimit;
    }
}
