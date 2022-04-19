using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Attack")]
    public class actMeleeAttack : Action
    {
        private EnemyController enemyController;
        public override void Startup(FSM fsm){
            enemyController = fsm.enemyController;
        }
        
        public override void Act(FSM fsm){
            if(!fsm.enemyController.attacking){
                string[] possibleAttacks = fsm.enemyController.weapon.possibleAttacks;
                int[] attackWeights = fsm.enemyController.weapon.attackWeights;

                int attackRoll = Random.Range(0, 101);
                int percentSum = 0;
                for(int i = 0; i < possibleAttacks.Length; i++){
                    percentSum += attackWeights[i];
                    if(attackRoll <= percentSum){
                        if(fsm.attackTracker.x == i){
                            if(fsm.attackTracker.y > 1){ //If the same attack has been performed two times in a row
                                return;
                            }
                            else{
                                fsm.attackTracker.y++;
                            }
                        }else{ //If this is a new attack type
                            fsm.attackTracker = new Vector2(i, 1);
                        }
                        fsm.enemyController.animator.Play(possibleAttacks[i]);
                        return;
                    }
                }
            }
        }
    }
}