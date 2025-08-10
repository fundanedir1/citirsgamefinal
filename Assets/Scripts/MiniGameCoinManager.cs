using UnityEngine;
using UnityEngine.UI;

public class MiniGameCoinManager : MonoBehaviour
{
    public int currentCoins = 0;           // Ba�lang�� coin miktar�
    public int coinsNeededForUpgrade = 50; // Max coin ihtiyac� (iste�e ba�l�)

    public Text coinText;                  // Coin say�s�n� g�sterecek UI Text

    private void Start()
    {
        UpdateCoinUI();
    }

    // Mini oyun bitti�inde success durumuna g�re �a��r
    public void OnMiniGameEnd(bool success)
    {
        if (success)
        {
            AddCoins(10);
        }
        else
        {
            AddCoins(-10);
        }
    }

    void AddCoins(int amount)
    {
        currentCoins += amount;

        // Coin say�s�n� 0 ile max aras�nda s�n�rla
        if (currentCoins > coinsNeededForUpgrade)
            currentCoins = coinsNeededForUpgrade;
        if (currentCoins < 0)
            currentCoins = 0;

        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + currentCoins + " / " + coinsNeededForUpgrade;
    }
}
