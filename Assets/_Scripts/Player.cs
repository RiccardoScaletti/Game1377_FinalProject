using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;

    public WeaponLibrary weaponLibrary;
    public WeaponData currentWeapon;

    public int currentAmmo;

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
        EquipWeapon("1911");
    }

    void Update()
    {
        
    }

    void EquipWeapon(string weaponName)
    {
        WeaponData weapon = weaponLibrary.GetWeapon(weaponName);
        currentAmmo = weapon.maxAmmo;
        if (weapon != null)
        {
            currentWeapon = weapon;
            Debug.Log("Equipped: " + weapon.wpnName);
        }
        else
        {
            Debug.LogWarning("Weapon not found in the lib");
        }
    }
}
