using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehavior : Steering
{
    [SerializeField] private float avoidDistance = 1f;
    [SerializeField] private float lookahead = 2f;
    [SerializeField] private float sideViewAngle = 45f;
    [SerializeField] public LayerMask obstacle;

    public override SteeringData GetSteering(SteeringBehaviorBase
                                             steeringbase)
    {
        SteeringData steering = new SteeringData();
        Vector3[] rayVector = new Vector3[3];
        rayVector[0] = GetComponent<Rigidbody>().velocity;
        rayVector[0].Normalize();
        rayVector[0] *= lookahead;
        float rayOrientation = Mathf.Atan2(
                                   GetComponent<Rigidbody>().velocity.z,
                                   GetComponent<Rigidbody>().velocity.x);
        float rightRayOrientation = rayOrientation + (sideViewAngle *
                                                      Mathf.Deg2Rad);
        float leftRayOrientation = rayOrientation - (sideViewAngle *
                                                      Mathf.Deg2Rad);
        rayVector[1] = new Vector3(Mathf.Cos(rightRayOrientation), 0,
                               Mathf.Sin(rightRayOrientation));
        rayVector[1].Normalize();
        rayVector[1] *= lookahead;
        rayVector[2] = new Vector3(Mathf.Cos(leftRayOrientation), 0,
                                   Mathf.Sin(leftRayOrientation));
        rayVector[2].Normalize();
        rayVector[2] *= lookahead;
        for (int i = 0; i < rayVector.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayVector[i], out hit,
                                            lookahead, obstacle))
            {

                Vector3 target = hit.point + (hit.normal * avoidDistance);
                steering.linear = target - transform.position;
                steering.linear.Normalize();
                steering.linear *= steeringbase.maxAcceleration;
                break;
            }
        }
        return steering;
    }
}

