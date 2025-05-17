
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    public enum WpnType
    {
        None,
        Pistol,
        SMG,
        Assault,
        Sniper
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

    //This depend on the rarity of the gun
    [HideInInspector] public float fireRate;
    [HideInInspector] public int bulletSpeed;
    [HideInInspector] public int dmg;
    [HideInInspector] public int pierced = 0;

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
                        //TODO
                        break;
                    case Rarity.rare:
                        //TODO
                        break;
                    case Rarity.legendary:
                        //TODO
                        break;
                    default:
                        break;
                }
                break;
            case WpnType.Assault:
                switch (rarity) 
                {
                    case Rarity.common:
                        //TODO
                        break;
                    case Rarity.uncommon:
                        fireRate = 1.5f;
                        bulletSpeed = 90;
                        dmg = 4;
                        break;
                    case Rarity.rare:
                        //TODO
                        break;
                    case Rarity.legendary:
                        //TODO
                        break;
                    default:
                        break;
                }
                break;
            case WpnType.SMG:
                switch (rarity) 
                {
                    case Rarity.common:
                       //TODO
                        break;
                    case Rarity.uncommon:
                        fireRate = 3;
                        bulletSpeed = 120;
                        dmg = 2;
                        break;
                    case Rarity.rare:
                        //TODO
                        break;
                    case Rarity.legendary:
                        //TODO
                        break;
                    default:
                        break;
                }
                break;
            case WpnType.Sniper:
                switch (rarity)
                {
                    case Rarity.common:
                        //TODO
                        break;
                    case Rarity.uncommon:
                        fireRate = 0.5f;
                        bulletSpeed = 200;
                        dmg = 15;
                        break;
                    case Rarity.rare:
                        //TODO
                        break;
                    case Rarity.legendary:
                        //TODO
                        break;
                    default:
                        break;
                }
                break;
        }
    }
}