using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehavior : Steering
{
    private Transform[] targets;
    [SerializeField] private float threshold = 2f;
    [SerializeField] private float decayCoefficient = -25f;

    void Start() { 
        SteeringBehaviorBase[] agents = FindObjectsOfType<SteeringBehaviorBase>();
        targets = new Transform[agents.Length - 1]; int count = 0; 
        foreach (SteeringBehaviorBase agent in agents) {
            if (agent.gameObject != gameObject) {
                targets[count] = agent.transform; count++;
            }
        }
    }
    public override SteeringData GetSteering(SteeringBehaviorBase steeringbase)
    {
        SteeringData steering = new SteeringData(); 
        foreach (Transform target in targets)
        { Vector3 direction = target.transform.position -  transform.position; float distance = direction.magnitude;
            if (distance < threshold) {
                float strength = Mathf.Min(decayCoefficient / (distance * distance), steeringbase.maxAcceleration);
                direction.Normalize(); 
                steering.linear += strength * direction; 
            }
        }
        return steering;
    }
}
