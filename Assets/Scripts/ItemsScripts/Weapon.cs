using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public WeaponStats weaponStats;
    private Camera cam;
    [SerializeField] private Transform tipOfWeapon;
    
    private void Start()
    {
        cam = Camera.main;
    }
    public void UpgradeWeapons()
    {
        //level requiremnets and scaling
        switch (weaponStats.weaponType)
        {
            case WeaponType.ShortSword:
                if(playerDexterity>=10 && playerStrength>= 10)
                {
                   weaponStats.damage = weaponStats.damage + (playerDexterity * weaponStats.scaleDexterity) + (playerStrength*weaponStats.scaleStrenght);
                }
                break;
            case WeaponType.Spear:
                if (playerDexterity >= 10 && playerStrength >= 13)
                {
                    weaponStats.damage = weaponStats.damage + (playerDexterity * weaponStats.scaleDexterity) + (playerStrength * weaponStats.scaleStrenght);
                }
                break;
            case WeaponType.Trident:

                if (playerDexterity >= 12 && playerStrength >= 14)
                {
                    weaponStats.damage = weaponStats.damage + (playerDexterity * weaponStats.scaleDexterity) + (playerStrength * weaponStats.scaleStrenght);
                }
                break;
            case WeaponType.GreatAxe:
                if (playerDexterity >= 15 && playerStrength >= 10)
                {
                    weaponStats.damage = weaponStats.damage + (playerDexterity * weaponStats.scaleDexterity) + (playerStrength * weaponStats.scaleStrenght);
                }
                break;
        }
    }

}
