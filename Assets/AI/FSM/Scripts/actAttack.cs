using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Attack")]
    public class actAttack : Action
    {
        private EnemyController enemyController;
        public override void Startup(FSM fsm){
            enemyController = fsm.enemyController;
        }
        
        public override void Act(FSM fsm){
            if(!enemyController.attacking){
                string[] possibleAttacks = enemyController.weapon.possibleAttacks;
                int[] attackWeights = enemyController.weapon.attackWeights;

                int attackRoll = Random.Range(0, 101);
                int percentSum = 0;
                for(int i = 0; i < possibleAttacks.Length; i++){
                    percentSum += attackWeights[i];
                    if(attackRoll <= percentSum){
                        enemyController.animator.Play(possibleAttacks[i]);
                        return;
                    }
                }
            }
        }
    }
}