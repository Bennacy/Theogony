using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class CyclopsAnimationEvents : MonoBehaviour
    {
        private Animator animator;
        private EnemyController enemyController;
        private Collider weaponCollider;
        public bool animating;
        public bool attacking;

        void Start()
        {
            animator = GetComponent<Animator>();
            enemyController = GetComponentInParent<EnemyController>();
        }

        void Update()
        {
            animator.SetBool("Animating", animating);
        }
        
        public void AnimationOver(){
            animating = false;
        }

        public void AnimationStarted(){
            animating = true;
        }
    }
}