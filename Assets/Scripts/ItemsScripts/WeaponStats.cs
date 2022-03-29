using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Items/Weapons Manager/Weapn Stats")]
public class WeaponStats : ScriptableObject
{
    public float damage;
    public float scaling;
    //add requirments to upgrade
    public WeaponType weaponType;
}
