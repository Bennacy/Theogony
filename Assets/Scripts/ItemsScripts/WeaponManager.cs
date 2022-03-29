using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //I used this video https://www.youtube.com/watch?v=XK5petoRM9o&ab_channel=MajedMadamani

    private GameObject weaponInRadius;
    private Weapon currentWeapon;
    [SerializeField] private Transform weaponHolder;  
    private Camera cam;

    [SerializeField] private KeyCode pickUpKey = KeyCode.E;
    [SerializeField] private KeyCode dropKey = KeyCode.Q;
    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(pickUpKey) && weaponInRadius != null)
            PickUpWeapon();
        if (Input.GetKeyDown(dropKey) && weaponInRadius != null)
            DropWeapon();

    }

    private void DropWeapon()
    {
        throw new NotImplementedException();
    }

        private void PickUpWeapon()
    {
        throw new NotImplementedException();
    }
}
public enum WeaponClass
{
    Slashing,
    Thrusting,
    Heavy
}
public enum WeaponType
{
    ShortSword,
    Spear,
    Trident,
    GreatAxe

}
