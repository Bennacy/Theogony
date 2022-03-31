using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Condition/CanSee")]


public class CanSeeCondition : Condition
{
    [SerializeField]
    private bool negation;
    [SerializeField]
    private float viewAngle;
    [SerializeField]
    private float viewDistance;
  
    public override bool Test(GorgonFiniteStateMachine fsm)
    {
        Transform target = fsm.GetAgent().target;
        Vector3 direction = target.position - fsm.transform.position;
        float distance = direction.magnitude;
        float angle = Vector3.Angle(direction.normalized, fsm.transform.forward);
        if (angle < viewAngle && distance < viewDistance &&  fsm.cooldowntimer < 0)// && !Physics.Linecast(fsm.GetAgent().transform.position,target.position))
        {
            return !negation;
        }
        else
        {
            fsm.cooldowntimer -= Time.deltaTime;

        }
        return negation;


    }
}
