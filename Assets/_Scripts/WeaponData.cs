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
        Shotgun
    }
    public enum Rarity
    {
        common,
        uncommon,
        rare,
        legendary
    }

    public string wpnName;
    public WpnType type;
    public Rarity rarity;
    public float fireRate;
    public int bulletSpeed;
    public int dmg;
    public int maxAmmo;
    public int bulletsPerShot;
   
}