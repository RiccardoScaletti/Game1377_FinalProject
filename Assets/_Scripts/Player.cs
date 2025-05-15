using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    public Image healthBar;

    public WeaponLibrary weaponLibrary;
    public WeaponData currentWeapon;
    public int currentAmmo;

    [Header("Wpn sounds")]
    public AudioSource[] audioSources; //0 is for shooting, 1 is for reload

    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public int killCount = 0;

    public event Action OnBossBattle;
    private bool bossCalled = false;

    public static Player instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("Warning, it is already present another instance of the player");
        }
        instance = this; 
       
    }
    void Start()
    {
        health = 1;
        //EquipWeapon("1911");
        EquipWeapon("MP5");
    }

    void Update()
    {
        healthBar.fillAmount = health;
        if (currentAmmo == 0 && currentWeapon.name != "1911") EquipWeapon("1911");

        if (killCount == 100 && !bossCalled)
        {
            Debug.Log("Boss spawned");
            OnBossBattle?.Invoke();
            bossCalled = true;
        }
    }

    public void EquipWeapon(string weaponName)
    {
        WeaponData weapon = weaponLibrary.GetWeapon(weaponName);
        currentAmmo = weapon.maxAmmo;
        if (weapon != null)
        {
            currentWeapon = weapon;
            Debug.Log("Equipped: " + weapon.wpnName);
            audioSources[0].clip = currentWeapon.wpnSoundShot;
        }
        else
        {
            Debug.LogWarning("Weapon not found in the lib");
        }
    }
}
