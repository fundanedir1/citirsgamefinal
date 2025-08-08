using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public static PlayerGold Instance;

    public int CurrentGold = 500;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SpendGold(int amount)
    {
        CurrentGold -= amount;
        if (CurrentGold < 0) CurrentGold = 0;
        Debug.Log("Altın harcandı. Kalan altın: " + CurrentGold);
    }
}

