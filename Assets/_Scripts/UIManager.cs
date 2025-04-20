using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image currentWeaponImg;

    public static UIManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("Warning, it is already present another instance of the Game Manager");
        }
        instance = this; 

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentWeaponImg.sprite = Player.instance.currentWeapon.wpnImage;
    }
}
