using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyAnimationEvents : MonoBehaviour
    {
        private Animator animator;
        private EnemyController enemyController;
        public Collider weaponCollider;
        public bool animating;
        public bool attacking;

        void Start()
        {
            animator = GetComponent<Animator>();
            enemyController = GetComponentInParent<EnemyController>();
            Collider[] colliders = transform.GetComponentsInChildren<Collider>();
            foreach(Collider collider in colliders){
                if(collider != GetComponent<Collider>()){
                    weaponCollider = collider;
                    return;
                }
            }
        }

        void Update()
        {
            animator.SetBool("Animating", animating);
        }

        public void Die(){
            Destroy(transform.parent.gameObject);
        }

        public void StartAttack(){
            Debug.Log("Started Attack");
            enemyController.attacking = true;
        }

        public void EndAttack(){
            Debug.Log("Stopped Attack");
            enemyController.attacking = false;
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
    }
}