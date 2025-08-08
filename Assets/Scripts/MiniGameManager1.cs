using UnityEngine;

public class MiniGameManager1 : MonoBehaviour
{
    public MonoBehaviour[] miniGameScripts; // Inspector’da IMiniGame implement eden scriptler (MiniGame1, MiniGame2, MiniGame3)

    private IMiniGame[] miniGames;
    private IMiniGame currentMiniGame;
    private int currentMiniGameIndex = -1;

    private int[] difficultyCounters;

    private float timeSinceLastMiniGame = 0f;
    public float minigameCooldown = 15f; // Mini oyunlar arası bekleme süresi

    void Awake()
    {
        miniGames = new IMiniGame[miniGameScripts.Length];
        for (int i = 0; i < miniGameScripts.Length; i++)
        {
            miniGames[i] = miniGameScripts[i] as IMiniGame;
        }

        difficultyCounters = new int[miniGames.Length];
    }

    void Update()
    {
        if (currentMiniGame != null)
        {
            // Mini oyun aktifse güncelle
            currentMiniGame.UpdateMiniGame();

            if (currentMiniGame.IsFinished)
            {
                if (currentMiniGame.IsSuccess)
                {
                    difficultyCounters[currentMiniGameIndex]++;
                    // Başarı ödülleri veya oyun içi etkiler buraya
                }

                currentMiniGame.CloseMiniGame();
                currentMiniGame = null;
                timeSinceLastMiniGame = 0f;
            }
        }
        else
        {
            // Mini oyun yoksa zaman sayar ve cooldown sonrası mini oyun başlatır
            timeSinceLastMiniGame += Time.deltaTime;

            if (timeSinceLastMiniGame >= minigameCooldown)
            {
                StartRandomMiniGame();
            }

            // Ana oyun burada normal akışına devam eder
        }
    }

    void StartRandomMiniGame()
    {
        int randomIndex = Random.Range(0, miniGames.Length);
        currentMiniGameIndex = randomIndex;
        currentMiniGame = miniGames[randomIndex];

        int difficulty = difficultyCounters[randomIndex] + 1;
        currentMiniGame.StartMiniGame(difficulty);
    }
}
