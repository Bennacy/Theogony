using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Conditions/CanSee")]
    public class conCanSee : Condition
    {
        [SerializeField]  private LayerMask visionLayer;
        [SerializeField]  private RaycastHit seeing;
        [SerializeField]  private bool negation;
        [SerializeField]  private float viewAngle;
        [SerializeField]  private float viewDistance;

        public override bool Test(FSM fsm)
        {
            Transform target = null;

            if (fsm.enemyController)
            {
                 target = fsm.enemyController.target;
            }
            else if (fsm.bossController)
            {
                 target = fsm.bossController.target;
            }
           
            Vector3 targetDir = target.position - fsm.transform.position;
            float angle = Vector3.Angle(targetDir, fsm.transform.forward);
            float dist = Vector3.Distance(target.position,  fsm.transform.position);

            if (fsm.enemyController)
            {
                if (!fsm.enemyController.playerTargetable)
                {
                    return negation;
                }
            }
            else if (fsm.bossController)
            {
                if (!fsm.bossController.playerTargetable)
                {
                    return negation;
                }
            }
           

            fsm.visionCountdown -= Time.deltaTime;
            if((angle < viewAngle) && (dist < viewDistance)){
                Debug.Log("InView");
                
                if(!VisionBlocked(fsm, target, dist)){
                    fsm.visionCountdown = fsm.visionMemory;
                    Debug.Log("Sees");
                    return !negation;
                }else{
                    Debug.Log("Can't see");
                    if(fsm.visionCountdown <= 0){
                        // Debug.Log("Forgor :skull:");
                        return negation;
                    }else{
                        // Debug.Log("Remembers");
                        return !negation;
                    }
                }
            }else{
                if(fsm.visionCountdown <= 0){
                    // Debug.Log("Forgor :skull:");
                    return negation;
                }else{
                    // Debug.Log("Remembers");
                    return !negation;
                }
            }
        }

        private bool VisionBlocked(FSM fsm, Transform target, float distanceToPlayer){
            Vector3 targetDir = (target.position - fsm.transform.position).normalized;

            Physics.Raycast(fsm.transform.position, targetDir, out RaycastHit hit, distanceToPlayer, visionLayer);

            Debug.Log(hit.collider != null);
            return hit.collider != null;
        }
    }
}