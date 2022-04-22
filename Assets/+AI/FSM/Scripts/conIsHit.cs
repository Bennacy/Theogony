using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Conditions/IsHit")]
    public class conIsHit : Condition
    {
        [SerializeField] private bool negation;

        public override bool Test(FSM fsm)
        {
            EnemyController enemyController = fsm.enemyController;
            
            if(!fsm.enemyController.playerTargetable){
                return negation;
            }

            if(fsm.enemyController.wasHit){
                fsm.enemyController.wasHit = false;
                return !negation;
            }else{
                return negation;
            }
        }
    }
}