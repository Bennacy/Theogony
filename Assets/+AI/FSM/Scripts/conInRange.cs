using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Conditions/InRange")]
    public class conInRange : Condition
    {
        [SerializeField]  private bool negation;
        [SerializeField]  private float attackDistance;

        public override bool Test(FSM fsm)
        {
            Transform target = fsm.enemyController.target;
            Vector3 targetDir = target.position - fsm.transform.position;
            float angle = Vector3.Angle(targetDir, fsm.transform.forward);
            float dist = Vector3.Distance(target.position,  fsm.transform.position);

            if(!fsm.enemyController.playerTargetable){
                return negation;
            }

            if(dist < attackDistance){
                return !negation;
            }else{
                return negation;
            }
        }
    }
}