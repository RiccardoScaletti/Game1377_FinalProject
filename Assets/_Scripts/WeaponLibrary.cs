using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "WeaponLibrary", menuName = "Weapons/Weapon Library")]
public class WeaponLibrary : ScriptableObject
{
    public List<WeaponData> weapons;

    private Dictionary<string, WeaponData> weaponDict;

    public void Initialize()
    {
        weaponDict = new Dictionary<string, WeaponData>();
        foreach (var weapon in weapons)
        {
            if (!weaponDict.ContainsKey(weapon.wpnName))
                weaponDict.Add(weapon.wpnName, weapon);
        }
    }

    public WeaponData GetWeapon(string name)
    {
        if (weaponDict == null) Initialize();

        weaponDict.TryGetValue(name, out WeaponData weapon);
        return weapon;
    }
}

