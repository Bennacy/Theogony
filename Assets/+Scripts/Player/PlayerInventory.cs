using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony { 
public  class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public weaponItems rightWeapon;
        public weaponItems leftWeapon;

     //   public weaponItems unarmedWeapon;
        
        public weaponItems[] weaponsInRightHandSlots = new weaponItems[1];
        public weaponItems[] weaponsInLeftHandSlots = new weaponItems[1];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;


        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {

            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);          
  
        }
        public void ChangeWeapon()
        {
             currentRightWeaponIndex ++;
            if(currentRightWeaponIndex < weaponsInRightHandSlots.Length)
            {
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];

            }else
            {
             currentRightWeaponIndex = 0;
             weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
             rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            }
        }

       /*
        public  void ChangeRightWeapon()
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;

            if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex],false);
            }else if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex +1;
            }else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);

            }else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex + 1;
            }

            if(currentRightWeaponIndex > weaponsInRightHandSlots.Length -1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon , false);
            }
        }
        */
    }
}

