using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    public int killCount = 0;

    public WeaponLibrary weaponLibrary;
    public WeaponData currentWeapon;
    public int currentAmmo;
    [Header("Wpn sounds")]
    public AudioSource[] audioSources; //0 is for shooting, 1 is for reload

    public Image healthBar;

    [HideInInspector] public bool isReloading = false;
    public static Player instance { get; private set; }

    private void Awake()
    {
        if (instance != null) //common practice when setting up a singleton
        {
            Debug.LogWarning("Warning, it is already present another instance of the player");
        }
        instance = this; // instance initialization, needed to define a singleton
        //assigns the variable instance to itself
    }
    void Start()
    {
        health = 1;
        EquipWeapon("M4");
    }

    void Update()
    {
        healthBar.fillAmount = health;
       
    }

    void EquipWeapon(string weaponName)
    {
        WeaponData weapon = weaponLibrary.GetWeapon(weaponName);
        currentAmmo = weapon.maxAmmo;
        if (weapon != null)
        {
            currentWeapon = weapon;
            Debug.Log("Equipped: " + weapon.wpnName);
            audioSources[0].clip = currentWeapon.wpnSoundShot;
            audioSources[1].clip = currentWeapon.wpnSoundReload;
        }
        else
        {
            Debug.LogWarning("Weapon not found in the lib");
        }
    }
}
