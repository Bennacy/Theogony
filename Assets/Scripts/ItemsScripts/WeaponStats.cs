using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Items/Weapons Manager/Weapn Stats")]
public class WeaponStats : ScriptableObject
{
    public float damage;
    public float scaleDexterity;
    public float scaleStrenght;
    public WeaponClass weaponClass;
    public WeaponType weaponType;
}
