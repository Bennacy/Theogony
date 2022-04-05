using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony { 
public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public weaponItems rightWeapon;
        public weaponItems leftWeapon;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }
    }
}

