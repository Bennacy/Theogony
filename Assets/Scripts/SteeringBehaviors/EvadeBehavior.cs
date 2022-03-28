using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeBehavior : Steering
{
     public Transform target;
     public float maxprediction;
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steeringData = new SteeringData();
        Vector3 direction = target.transform.position - steeringbase.GetPosition();
        float distance = direction.magnitude;
        float speedTarget = target.GetComponent<Rigidbody>().velocity.magnitude;
        float speedAgent = steeringbase.GetRigidbody().velocity.magnitude;

        float prediction;
        if (speedAgent <= distance / maxprediction)
        {
            prediction = maxprediction;
        }
        else
        {
            prediction = distance / speedAgent;
        }
        Vector3 futureDirection = target.position + (target.GetComponent<Rigidbody>().velocity * prediction);
        steeringData.linear = Vector3.Normalize(steeringbase.GetPosition() - futureDirection);
        steeringData.linear *= steeringbase.maxAcceleration;



        return steeringData;
    }
}
