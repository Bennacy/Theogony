using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class PersueBehavior : Steering
    {
        public Transform target;
        public float maxprediction;

        public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
        {
            Debug.Log("Peruse");
            SteeringData steeringData = new SteeringData();
            Vector3 direction = target.transform.position - transform.position;
            float distance = direction.magnitude;
            float speedTarget = target.GetComponent<Rigidbody>().velocity.magnitude;
            float speedAgent = GetComponent<Rigidbody>().velocity.magnitude;

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
            steeringData.linear = Vector3.Normalize(futureDirection - (transform.position - new Vector3(1f, 0f, 1f)));
            steeringData.linear *= steeringbase.maxAcceleration;

            return steeringData;


        }
    }
}