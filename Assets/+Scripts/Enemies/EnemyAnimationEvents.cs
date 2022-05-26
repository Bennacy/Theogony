using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyAnimationEvents : MonoBehaviour
    {
        private Animator animator;
        private EnemyController enemyController;
        public Collider weaponCollider;
        private GameObject dropPrefab;
        public EnemyWeapons weapon;
        private Rigidbody rb;
        public bool animating;
        public bool attacking;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.01f);
            rb = GetComponentInParent<Rigidbody>();
            animator = GetComponent<Animator>();
            enemyController = GetComponentInParent<EnemyController>();
            dropPrefab = enemyController.dropPreset;
            weapon = enemyController.weapon;
            Collider[] colliders = transform.GetComponentsInChildren<Collider>();
            foreach(Collider collider in colliders){
                if(collider != GetComponent<Collider>()){
                    weaponCollider = collider;
                    break;
                }
            }
        }

        void Update()
        {
            if(animator)
                animator.SetBool("Animating", animating);
        }

        public void Die(){
            Drop();
            Destroy(transform.parent.gameObject);
        }

        public void Drop(){
            List<weaponItems> playerWeapons = GlobalInfo.GetGlobalInfo().collectedWeaponsR;
            foreach(weaponItems playerWeapon in playerWeapons){
                if(playerWeapon == weapon.droppedWeapon){
                    return;
                }
            }
            Vector3 dropPos = transform.position;
            dropPos.y = 1;
            Interactable script = Instantiate(dropPrefab, dropPos, Quaternion.identity).GetComponent<Interactable>();
            script.itemGiven = weapon.droppedWeapon;
        }

        #region Events
        public void ResetEvents(){
            DisableKinematic();
            ColliderOff();
            AnimationOver();
            InvincibleOff();
        }

        public void StartAttack(){
            Debug.Log("Started Attack");
            enemyController.attacking = true;
        }

        public void EndAttack(){
            Debug.Log("Stopped Attack");
            enemyController.attacking = false;
        }

        public void EnableKinematic(){
            rb.isKinematic = true;
        }

        public void DisableKinematic(){
            rb.isKinematic = false;
        }

        public void ColliderOn(){
            weaponCollider.enabled = true;
        }

        public void ColliderOff(){
            weaponCollider.enabled = false;
        }
        
        public void AnimationOver(){
            animating = false;
        }

        public void AnimationStarted(){
            animating = true;
        }

        public void StandUp(){
            animator.Play("Standing");
        }

        public void Recover(){
            enemyController.staggered = false;
        }

        public void InvincibleOn(){
            enemyController.invincible = true;
        }

        public void InvincibleOff(){
            enemyController.GetComponent<Rigidbody>().isKinematic = false;
            enemyController.invincible = false;
        }
        #endregion
    }
}