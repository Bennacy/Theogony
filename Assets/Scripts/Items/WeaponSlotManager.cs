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

        Animator animHandler;
        public bool rotateAttack;

        private PlayerInventory playerInventory;
        public Transform pos;

        private void Awake()
        {
            pos = GetComponentInParent<Transform>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animHandler = GetComponent<Animator>();
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

        #region Animation Events
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
            // LeftWeaponCollider.DisableCollider();
        }


        public void EnableCanMove()
        {
            GetComponentInParent<PlayerControllerScript>().canMove = true;
        }

        public void FinishRoll()
        {
            GetComponentInParent<PlayerControllerScript>().FinishRoll();
        }

        public void CanCombo()
        {
            animHandler.SetBool("Combo", true);
            playerInventory.rightWeapon.currattack++;
        }
        public void CanNotCombo()
        {
            animHandler.SetBool("Combo", false);
            playerInventory.rightWeapon.currattack = 0;

        }

        public void ResetEvents()
        {
            CanNotCombo();
            EnableCanMove();
            DisableLeftCollider();
            DisableRightCollider();
            IsNotOccupied();
            rotateAttack = false;
            transform.rotation = Quaternion.identity;
        }


        public void IsOccupied()
        {
            Debug.Log("AAA");
            animHandler.SetBool("Occupied", true);
        }
        public void IsNotOccupied()
        {
            animHandler.SetBool("Occupied", false);
        }

        #endregion


        public void RotateInAttacks()
        {
            rotateAttack = true;
            accely = 100;
        }
        public void ResetInAttacks()
        {
            accely = -100;
        }
        public void StopRotateInAttacks()
        {
            rotateAttack = false;
            accely = 0;
        }

        float accelx, accelz = 0;
        float accely = 0;
   

        void Update()
        {


            if (rotateAttack == true)
            {
                accelx = Input.acceleration.x;
                accelz = Input.acceleration.z;
                transform.Rotate(accelx * Time.deltaTime, accely * Time.deltaTime, accelz * Time.deltaTime);
            }
            else if (transform.rotation.y != 0)
            {
              //  transform.rotation = Quaternion.identity;
            }
            
           
        }
       
    }
}
