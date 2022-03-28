using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Steering Behavior/LookAt")]
public class LookAtBehavior : Steering
{
   
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steeringData = new SteeringData();
        Vector3 direction = Vector3.Normalize(steeringbase.GetTarget().transform.position -steeringbase.GetPosition());

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        steeringData.angular = Mathf.LerpAngle(steeringbase.GetTransform().rotation.eulerAngles.y, angle, steeringbase.maxAngularAcceleration * Time.deltaTime);
  
        return steeringData;


    }
}
