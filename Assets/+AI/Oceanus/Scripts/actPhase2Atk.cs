using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Attack2")]
    public class actPhase2Atk : Action
    {
        private BossController bossController;
      
      
        public override void Startup(FSM fsm)
        {
            bossController = fsm.bossController;
            
        }

        public override void Act(FSM fsm)
        {

            if (fsm.bossController)
            {
                if (!fsm.bossController.attacking)
                {
                    string[] possibleAttacks = fsm.bossController.weapon.phase2Attacks;
                    int[] attackWeights = fsm.bossController.weapon.phase2Weights;

                    int attackRoll = Random.Range(0, 101);
                    int percentSum = 0;
                    for (int i = 0; i < possibleAttacks.Length; i++)
                    {
                        percentSum += attackWeights[i];
                        if (attackRoll <= percentSum)
                        {
                            if (fsm.attackTracker.x == i)
                            {
                                if (fsm.attackTracker.y > 1)
                                { //If the same attack has been performed two times in a row
                                    return;
                                }
                                else
                                {
                                    fsm.attackTracker.y++;
                                }
                            }
                            else
                            { //If this is a new attack type
                                fsm.attackTracker = new Vector2(i, 1);
                            }
                            fsm.bossController.animator.Play(possibleAttacks[i]);
                            return;
                        }
                    }
                }
            }
           
            

        }
    }
}