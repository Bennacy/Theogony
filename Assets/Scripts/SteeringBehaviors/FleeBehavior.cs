using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Steering Behavior/Flee")]
public class FleeBehavior : Steering
{
    
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steeringData = new SteeringData();
        steeringData.linear = Vector3.Normalize(steeringbase.GetPosition() - steeringbase.GetTarget().transform.position);
        steeringData.linear *= steeringbase.maxAcceleration;
        return steeringData;
    }
}
