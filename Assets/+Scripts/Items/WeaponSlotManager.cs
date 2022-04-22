using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony {
    public class WeaponSlotManager : MonoBehaviour
    {
        private GlobalInfo globalInfo;
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider rightWeaponCollider;
        ParryColider leftWeaponCollider;

        Animator animHandler;
        public bool rotateAttack;

        private PlayerInventory playerInventory;
        public Transform pos;

        private void Awake()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
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
            leftWeaponCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<ParryColider>();
        }
        #endregion

        #region Animation Events
        public void EnableRightCollider()
        {
            rightWeaponCollider.EnableCollider();
        }
        public void EnableLeftCollider()
        {
            leftWeaponCollider.EnableCollider();
        }

        public void DisableRightCollider()
        {
            rightWeaponCollider.DisableCollider();
        }
        public void DisableLeftCollider()
        {
            leftWeaponCollider.DisableCollider();
        }

        public void EndRiposte(){
            PlayerManager manager = GetComponentInParent<PlayerManager>();
            StartCoroutine(manager.EndRiposte(0));
        }

        public void FastParry(){
            GetComponent<Animator>().speed = 2;
        }
        public void SlowParry(){
            GetComponent<Animator>().speed = 1;
        }

        public void DisableCanMove(){
            GetComponentInParent<PlayerControllerScript>().canMove = false;
        }

        public void EnableCanMove()
        {
            GetComponentInParent<PlayerControllerScript>().canMove = true;
        }

        public void StopMove(){
            GetComponentInParent<PlayerControllerScript>().rb.velocity = Vector3.zero;
        }

        public void EnableKinematic(){
            GetComponentInParent<Rigidbody>().isKinematic = true;
        }

        public void DisableKinematic(){
            GetComponentInParent<Rigidbody>().isKinematic = false;
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

        public void StartDrink(){
            GetComponentInParent<PlayerControllerScript>().moveSpeed = GetComponentInParent<PlayerControllerScript>().drinkSpeed;
            rightWeaponCollider.gameObject.SetActive(false);
        }

        public void EndDrink(){
            GetComponentInParent<PlayerControllerScript>().moveSpeed = GetComponentInParent<PlayerControllerScript>().walkSpeed;
            rightWeaponCollider.gameObject.SetActive(true);
        }

        public void FinalDeath(){
            globalInfo.PlayerDeath();
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
            if(Input.GetKeyDown(KeyCode.V)){
                GetComponent<Animator>().Play("Drink");
            }


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
