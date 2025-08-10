using UnityEngine;
using UnityEngine.UI;

public class MiniGameCoinManager : MonoBehaviour
{
    public int currentCoins = 0;           // Baþlangýç coin miktarý
    public int coinsNeededForUpgrade = 50; // Max coin ihtiyacý (isteðe baðlý)

    public Text coinText;                  // Coin sayýsýný gösterecek UI Text

    private void Start()
    {
        UpdateCoinUI();
    }

    // Mini oyun bittiðinde success durumuna göre çaðýr
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

        // Coin sayýsýný 0 ile max arasýnda sýnýrla
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
