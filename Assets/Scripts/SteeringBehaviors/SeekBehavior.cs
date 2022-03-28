using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Steering Behavior/Seek")]
public class SeekBehavior : Steering
{
  
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steeringData = new SteeringData();
        steeringData.linear = Vector3.Normalize(steeringbase.GetTarget().transform.position - steeringbase.GetPosition());
        steeringData.linear *= steeringbase.maxAcceleration;
        return steeringData;
    }

   
}
