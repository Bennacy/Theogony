using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public abstract class Steering : MonoBehaviour
    {
        [SerializeField] private float weight = 1f;
        public abstract SteeringData GetSteering(SteeringBehaviorBase steeringbase);
        public float GetWeight() { return weight; }

    }
}
