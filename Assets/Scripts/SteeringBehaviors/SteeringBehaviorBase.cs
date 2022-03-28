using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviorBase : MonoBehaviour
{
    public float maxAcceleration;
    public float maxAngularAcceleration;
    public float drag;
    private Rigidbody rb;
    public GameObject target;
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        
    }
    public GameObject GetTarget()
    {
        return target;
    }
    public Vector3 GetPosition()
    {
        return rb.transform.position;
    }
    public Transform GetTransform()
    {
        return rb.transform;
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public void ExecuteSteerings(Steering[] steerings)
    {
        Vector3 acceleration = Vector3.zero;
        float rotation = 0;
        foreach(Steering behavior in steerings)
        {
            SteeringData steeringData = behavior.GetSteering(this);
            acceleration += steeringData.linear;
            rotation += steeringData.angular;
        }
        if(acceleration.magnitude> maxAcceleration)
        {
            acceleration.Normalize();
            acceleration *= maxAcceleration;
        }
       /* if (rotation > maxAngularAcceleration)
        {
            rotation = maxAcceleration;
        }*/
        rb.AddForce(acceleration);
        if(rotation != 0)
        {
            rb.rotation = Quaternion.Euler(0, rotation, 0);
        }
        
    }
}
