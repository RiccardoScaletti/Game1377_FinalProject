using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    public enum WpnType
    {
        None,
        Pistol,
        SMG,
        Assault,
        Shotgun
    }
    public enum Rarity
    {
        common,
        uncommon,
        rare,
        legendary
    }

    //customizable for each weapon
    public string wpnName;
    public WpnType type;
    public Rarity rarity;
    public int maxAmmo;
    public int bulletsPerShot;
    public Sprite wpnImage;
    public AudioClip wpnSoundShot;
    public AudioClip wpnSoundReload;

    //This depend on the rarity of the gun
    [HideInInspector] public float fireRate;
    [HideInInspector] public int bulletSpeed;
    [HideInInspector] public int dmg;

    public void InitializeWeaponStats()
    {
        switch (type)
        {
            case WpnType.None:
                Debug.LogError("Weapon's settings are incorrect. check SO"); break;
            case WpnType.Pistol:
                switch (rarity)
                {
                    case Rarity.common:
                        fireRate = 1;
                        bulletSpeed = 50;
                        dmg = 1;
                        break;
                    case Rarity.uncommon:
                        fireRate = 1;
                        bulletSpeed = 60;
                        dmg = 2;
                        break;
                    case Rarity.rare:
                        fireRate = 2;
                        bulletSpeed = 70;
                        dmg = 2;
                        break;
                    case Rarity.legendary:
                        fireRate = 2;
                        bulletSpeed = 80;
                        dmg = 3;
                        break;
                    default:
                        break;
                }
                break;
            case WpnType.Assault:
                switch (rarity) 
                {
                    case Rarity.common:
                        fireRate = 2;
                        bulletSpeed = 70;
                        dmg = 3;
                        break;
                    case Rarity.uncommon:
                        fireRate = 2;
                        bulletSpeed = 90;
                        dmg = 4;
                        break;
                    case Rarity.rare:
                        fireRate = 3;
                        bulletSpeed = 80;
                        dmg = 5;
                        break;
                    case Rarity.legendary:
                        fireRate = 3;
                        bulletSpeed = 100;
                        dmg = 6;
                        break;
                    default:
                        break;
                }
                break;
        }
    }
}