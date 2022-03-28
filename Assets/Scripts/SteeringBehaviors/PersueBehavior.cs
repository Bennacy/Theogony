using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Steering Behavior/Persue")]
public class PersueBehavior : Steering
{
    
     public float maxprediction;
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {

     
        SteeringData steeringData = new SteeringData();
        Vector3 direction = steeringbase.GetTarget().transform.position - steeringbase.GetPosition();
        float distance = direction.magnitude;
        float speedTarget = steeringbase.GetTarget().GetComponent<Rigidbody>().velocity.magnitude;
        float speedAgent= steeringbase.GetRigidbody().velocity.magnitude;

        float prediction;
        if(speedAgent<= distance / maxprediction)
        {
            prediction = maxprediction;
        }
        else
        {
            prediction = distance / speedAgent;
        }
        Vector3 futureDirection = steeringbase.GetTarget().transform.position +(steeringbase.GetTarget().GetComponent<Rigidbody>().velocity*prediction);
        steeringData.linear = Vector3.Normalize(futureDirection - steeringbase.GetPosition());
        steeringData.linear *= steeringbase.maxAcceleration;

        

        return steeringData;
    }
}
