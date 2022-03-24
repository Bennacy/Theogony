using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputTest : MonoBehaviour
{
    public Rigidbody rb;
    public float walkSpeed;
    public float runSpeed;
    private float moveSpeed;
    public float rollForce;
    public float turnTime;
    public Camera cam;
    public Vector3 camForward;

    void Start()
    {
        
    }

    void Update()
    {
        // Debug.Log(input);
    }

    public void Move(InputAction.CallbackContext context){
        Vector2 inputVector2 = context.action.ReadValue<Vector2>();
        Vector3 inputVector = new Vector3(inputVector2.x, 0, inputVector2.y);
        
        float camRot = cam.transform.rotation.eulerAngles.y;
        camForward = Quaternion.Euler(0, camRot, 0) * Vector3.forward;

        rb.velocity += inputVector * moveSpeed;
        Debug.Log(inputVector);
    }
}
