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
        BlockColider blockColider;
        PlayerManager playerManager;

        Animator animHandler;
        public bool rotateAttack;
        public bool resetAttack;

        private PlayerInventory playerInventory;
        public Transform pos;

        public Quaternion originalRotationValue;

        private void Awake()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            pos = GetComponentInParent<Transform>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerManager = GetComponentInParent<PlayerManager>();
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
                LoadBlockCollider();
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
        private void LoadBlockCollider()
        {
            blockColider = leftHandSlot.currentWeaponModel.GetComponentInChildren<BlockColider>();
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
        public void EnableBlockCollider()
        {
            blockColider.EnableCollider();
            playerManager.staminaRecharge = playerManager.blockingRecharge;
        }

        public void DisableRightCollider()
        {
            rightWeaponCollider.DisableCollider();
        }
        public void DisableLeftCollider()
        {
            leftWeaponCollider.DisableCollider();
        }
        public void DisableBlockCollider()
        {
            blockColider.DisableCollider();
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
            DisableKinematic();
            DisableLeftCollider();
            DisableRightCollider();
            IsNotOccupied();

            playerManager.staminaRecharge = playerManager.normalRecharge;
            rotateAttack = false;
            resetAttack = true;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }


        public void IsOccupied()
        {
            animHandler.SetBool("Occupied", true);
        }
        public void IsNotOccupied()
        {
            rightWeaponCollider.ResetList();
            animHandler.SetBool("Occupied", false);
        }

        public void PlayAudio(){
            // GetComponentInParent<AudioSource>().clip = rightHandSlot.scriptableObj.swingClip;
            GetComponentInParent<AudioSource>().PlayOneShot(rightHandSlot.scriptableObj.swingClip);
        }

        #endregion


        public void RotateInAttacks()
        {
            rotateAttack = true;
            accely = 100;
            originalRotationValue = transform.rotation;

        }
        public void ResetInAttacks()
        {
            resetAttack = true;
            rotateAttack = false;
        }
        public void StopRotateInAttacks()
        {
            resetAttack = false;
            rotateAttack = false;
            ResetEvents();
            accely = 0;         

        }
        public void Rot0()
        {
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
            else if (resetAttack == true)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 25f  * Time.deltaTime);

            }
        }
    }
}
