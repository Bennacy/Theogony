using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Stagger")]
    public class actStagger : Action
    {
        public override void Startup(FSM fsm){
            Debug.Log("Staggered");
            fsm.enemyController.attacking = false;
            fsm.enemyController.animator.StopPlayback();
            fsm.enemyController.animator.Play("Staggered");
            fsm.staggerTimer = 3;
        }
        public override void Act(FSM fsm)
        {
            fsm.staggerTimer -= Time.deltaTime;
            if(fsm.staggerTimer <= 0){
                fsm.enemyController.staggered = false;
                fsm.enemyController.parryCollider.enabled = false;
            }
        }
    }
}