using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class SteeringData
    {
        public Vector3 linear;
        public float angular;
        public SteeringData()
        {
            linear = Vector3.zero;
            angular = 0;
        }
    }
}
