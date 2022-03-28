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
    public float backstepSpeed;
    private float moveSpeed;
    public float rollSpeed;
    public float backstepTime;
    public float rollTime;
    public float turnTime;
    private bool stoppedMove;
    public bool canMove;
    public Camera cam;
    public Quaternion camForward;
    private Vector3 movementVector;

    void Start()
    {
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        float camRot = cam.transform.rotation.eulerAngles.y;
        camForward = Quaternion.Euler(0, camRot, 0);
        if((movementVector != Vector3.zero || stoppedMove) && canMove){
            stoppedMove = false;
            Vector3 vel = Vector3.zero;
            
            vel += camForward * movementVector * moveSpeed;
            rb.velocity = vel;
            
            float angle = Vector3.SignedAngle(Vector3.forward, movementVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * turnTime);
        }
    }

    public void Move(InputAction.CallbackContext context){
        Vector2 inputVector2 = context.action.ReadValue<Vector2>();
        movementVector = new Vector3(inputVector2.x, 0, inputVector2.y);
        if(context.canceled){
            stoppedMove = true;
            movementVector = Vector3.zero;
        }
    }

    public void Run(InputAction.CallbackContext context){
        float value = context.ReadValue<float>();
        if(context.performed && canMove){
            moveSpeed = runSpeed;
            // pressedTimer = 0;
            // pressedRoll = true;
        }else if(context.canceled){
            moveSpeed = walkSpeed;
            // pressedTimer = 0;
            // pressedRoll = false;
        }
    }

    public void Roll(InputAction.CallbackContext context){
        if(context.performed){
            if(movementVector == Vector3.zero){
                moveSpeed = backstepSpeed;
                rb.velocity += ((rb.rotation * Vector3.forward) * moveSpeed);
                StartCoroutine(RollTime(backstepTime));
            }else{
                moveSpeed = rollSpeed;
                rb.velocity += ((rb.rotation * Vector3.forward) * moveSpeed);
                StartCoroutine(RollTime(rollTime));
            }
        }
    }

    private IEnumerator RollTime(float wait){
        Debug.Log("Rolling");
        canMove = false;
        yield return new WaitForSeconds(wait);
        rb.velocity = Vector3.zero;
        moveSpeed = walkSpeed;
        Debug.Log("Stopped");
        canMove = true;
    }
}
