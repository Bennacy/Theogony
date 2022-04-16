using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviorBase : MonoBehaviour
{
    public float maxAcceleration;
    public float maxAngularAcceleration;
    public float drag;
    private Rigidbody rb;
    private Steering[] steerings;
    public GorgonFiniteStateMachine fsm;
    public State conditionState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        steerings = GetComponents<Steering>();
        rb.drag = drag;
        
    }


    void FixedUpdate()
    {
        if (fsm.currentState.Equals(conditionState))
        {
            Vector3 acceleration = Vector3.zero;
            float rotation = 0;
            foreach (Steering behavior in steerings)
            {
                SteeringData steeringData = behavior.GetSteering(this);
                acceleration += steeringData.linear;
                rotation += steeringData.angular;
            }
            if (acceleration.magnitude > maxAcceleration)
            {
                acceleration.Normalize();
                acceleration *= maxAcceleration;
            }
            rb.AddForce(acceleration);
            if (rotation != 0)
            {
                rb.rotation = Quaternion.Euler(0, rotation, 0);
            }

        }
    }
}
