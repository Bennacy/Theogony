using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Steering : ScriptableObject
{
    public abstract SteeringData GetSteering(SteeringBehaviorBase steeringbase);
   
}
