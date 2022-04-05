using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony { 
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider rightWeaponCollider;
        DamageCollider LeftWeaponCollider;



        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                    
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                    
                }
            }
        }

        public void LoadWeaponOnSlot(weaponItems weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftCollider();
            }
            else
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightCollider();
            }
        }

        #region GetColliders
        private void LoadRightCollider()
        {
            rightWeaponCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        private void LoadLeftCollider()
        {
            LeftWeaponCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        #endregion


        public void EnableRightCollider()
        {
            rightWeaponCollider.EnableCollider();
        }
        public void EnableLeftCollider()
        {
            LeftWeaponCollider.EnableCollider();
        }

        public void DisableRightCollider()
        {
            rightWeaponCollider.DisableCollider();
        }
        public void DisableLeftCollider()
        {
            LeftWeaponCollider.DisableCollider();
        }

    }
}
