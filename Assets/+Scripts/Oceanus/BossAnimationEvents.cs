using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class BossAnimationEvents : MonoBehaviour
    {
        private Animator animator;
        private BossController bossController;
        public Collider weaponCollider;
        public bool animating;
        public bool attacking;
        public GameObject Slam;
        public GameObject waterLaser;
        public GameObject phase2Atk;
        public Vector3 positionAdjustment;
        public Quaternion angleAdjustment;
        public Transform parent;

        void Start()
        {
            animator = GetComponent<Animator>();
            bossController = GetComponentInParent<BossController>();
            Collider[] weaponColliders  = transform.GetComponentsInChildren<Collider>();
            foreach(Collider collider in weaponColliders)
            {
                if(collider != GetComponent<Collider>())
                {
                    weaponCollider = collider;
                    return;
                }
            }
        }

        void Update()
        {
            animator.SetBool("Animating", animating);
        }

        public void Die()
        {
            Destroy(transform.parent.gameObject);
        }

        public void StartAttack()
        {
            bossController.attacking = true;
        }

        public void EndAttack()
        {
            bossController.attacking = false;
        }

        public void ColliderOn()
        {
            weaponCollider.enabled = true;
        }

        public void ColliderOff()
        {
            weaponCollider.enabled = false;
        }

        public void AnimationOver()
        {
            animating = false;
        }

        public void AnimationStarted()
        {
            animating = true;
        }

        public void StandUp()
        {
            animator.Play("Standing");
        }

        public void Recover()
        {
            bossController.staggered = false;
        }

        public void InvincibleOn()
        {
            bossController.invincible = true;
        }

        public void InvincibleOff()
        {
            bossController.GetComponent<Rigidbody>().isKinematic = false;
            bossController.invincible = false;
        }
        public void SlamAttack()
        {
            Instantiate(Slam, transform.position, transform.rotation);
        }
        public void Stopfacing()
        {
            GetComponentInParent<FSM>().stopFacing = false;
        }
        public void Continuefacing()
        {
            GetComponentInParent<FSM>().stopFacing = true;
        }

        public void LaserAttack()
        {

           GameObject laser = Instantiate(waterLaser) as GameObject;

            laser.transform.parent = parent;
            laser.transform.localPosition = positionAdjustment;
            laser.transform.localRotation = angleAdjustment;

           //  Instantiate(waterLaser, transform.position, transform.rotation);
        }

        public void Phase2Transition()
        {
            GetComponentInParent<BossController>().phase2Transition = true;
        }

        public void Phase2Atk()
        {
           Instantiate(phase2Atk, transform.position, transform.rotation);            
        }
    }
}