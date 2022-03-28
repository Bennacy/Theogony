using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Steering Behavior/Arrive")]
public class ArriveBehavior : Steering
{
    
    public float stopRadius;
    public float slowRadius;
   
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steeringData = new SteeringData();
        Vector3 direction = steeringbase.GetTarget().transform.position - steeringbase.GetPosition();
        float distance = direction.magnitude;
        if(distance < stopRadius)
        {
            steeringbase.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return steeringData;
        }
        float speed;
        if(distance < slowRadius)
        {
            speed = steeringbase.maxAcceleration * (distance / slowRadius);
        }
        else
        {
            speed = steeringbase.maxAcceleration;
        }
        steeringData.linear = (direction.normalized * speed)-steeringbase.GetRigidbody().velocity;
        return steeringData;
    }

    
}
