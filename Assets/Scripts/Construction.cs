using UnityEngine;
using UnityEngine.UI;

public class Construction : MonoBehaviour
{
    public GameObject openButton;       // İnşaat üstündeki 'Altınla Aç' butonu
    public GameObject housePrefab;      // İnşaat hazır olduğunda instantiate edilecek ev
    public int goldCost = 100;          // İnşaat açmak için gereken altın miktarı

    private bool isOpened = false;

    public void OnOpenButtonClicked()
    {
        if (isOpened) return;

        if (PlayerGold.Instance.CurrentGold >= goldCost)
        {
            PlayerGold.Instance.SpendGold(goldCost);
            OpenConstruction();
        }
        else
        {
            Debug.Log("Yeterli altın yok!");
            // Burada UI ile bildirim verebilirsin
        }
    }

    private void OpenConstruction()
    {
        isOpened = true;
        openButton.SetActive(false);

        // İnşaat objesini silip ev prefabını instantiate et
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        Destroy(gameObject);

        Instantiate(housePrefab, pos, rot);
    }
}
