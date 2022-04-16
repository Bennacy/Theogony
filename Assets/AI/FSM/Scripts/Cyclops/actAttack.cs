using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Attack")]
    public class Attack : Action
    {
        public float attackCooldown;
        public float attackTime;
        
        public override void Act(FSM fsm){
            attackTime += Time.deltaTime;
            if(attackTime > attackCooldown){
                attackTime = 0;
                Debug.Log("Attacking");
            }
        }
    }
}