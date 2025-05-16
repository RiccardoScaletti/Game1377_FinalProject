using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image currentWeaponImg;
    [SerializeField] private TextMeshProUGUI ammoCountText;
    [SerializeField] private TextMeshProUGUI killcountText;

    public static UIManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("Warning, it is already present another instance of the Game Manager");
        }
        instance = this;
        ammoCountText.text = Player.instance.currentAmmo.ToString() +"/"+Player.instance.currentWeapon.maxAmmo;
    }
    void Update()
    {
        currentWeaponImg.sprite = Player.instance.currentWeapon.wpnImage;
        ammoCountText.text = Player.instance.currentAmmo.ToString() + "/" + Player.instance.currentWeapon.maxAmmo;
        killcountText.text = "KILLCOUNT: " + Player.instance.killCount.ToString();
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
