using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Conditions/FacingTarget")]
    public class conFacingTarget : Condition
    {
        [SerializeField] private bool negation;
        [SerializeField] private float angleThreshold;
        [SerializeField] private bool larger;

        public override bool Test(FSM fsm)
        {
            Transform target = fsm.GetNavMesh().target;
            Vector3 targetDir = target.position - fsm.transform.position;
            float angle = Vector3.Angle(targetDir, fsm.transform.forward);

            if(!fsm.enemyController.playerTargetable){
                return negation;
            }

            if(!larger){
                if((angle < angleThreshold)){
                    return !negation;
                }else{
                    return negation;
                }
            }else{
                if((angle > angleThreshold)){
                    return !negation;
                }else{
                    return negation;
                }
            }
        }
    }
}