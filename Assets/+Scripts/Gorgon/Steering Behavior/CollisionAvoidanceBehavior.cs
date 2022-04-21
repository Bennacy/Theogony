using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceBehavior : Steering
{
    private Transform[] targets;
    [SerializeField] private float radius;

    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steering = new SteeringData();
        float shortestTime = float.PositiveInfinity;
        Transform firstTarget = null;
        float firstMinSeparation = 0, firstDistance = 0, firstRadius = 0;
        Vector3 firstRelativePos = Vector3.zero, firstRelativeVel = Vector3.zero;

        foreach (Transform target in targets)
        {
            Vector3 relativePos = transform.position - target.position;
            Vector3 relativeVel = GetComponent<Rigidbody>().velocity - target.GetComponent<Rigidbody>().velocity;
            float distance = relativePos.magnitude;
            float relativeSpeed = relativeVel.magnitude;

            if (relativeSpeed == 0)
                continue;

            float timeToCollision = -1 * Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

            Vector3 separation = relativePos + relativeVel * timeToCollision;
            float minSeparation = separation.magnitude;

            if (minSeparation > radius + radius)
                continue;

            if ((timeToCollision > 0) && (timeToCollision < shortestTime))
            {
                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
                firstRadius = radius;
            }
        }

        if (firstTarget == null)
            return steering;

        if (firstMinSeparation <= 0 || firstDistance < radius + firstRadius)
            steering.linear = transform.position - firstTarget.position;
        else
            steering.linear = firstRelativePos + firstRelativeVel * shortestTime;

        steering.linear.Normalize();
        steering.linear *= steeringbase.maxAcceleration;

        return steering;
    }

    void Start()
    {
        SteeringBehaviorBase[] agents = FindObjectsOfType<SteeringBehaviorBase>();
        targets = new Transform[agents.Length - 1];
        int count = 0;
        foreach (SteeringBehaviorBase agent in agents)
        {
            if (agent.gameObject != gameObject)
            {
                targets[count] = agent.transform;
                count++;
            }
        }
    }
}