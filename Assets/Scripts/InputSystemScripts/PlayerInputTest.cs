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
    public float rollForce;
    public float turnTime;
    private bool stoppedMove;
    public bool canMove;
    private bool pressedRoll;
    private float pressedTimer;
    public Camera cam;
    public Vector3 camForward;
    private Vector3 movementVector;

    void Start()
    {
        moveSpeed = walkSpeed;
    }

    void Update()
    {
        if((movementVector != Vector3.zero || stoppedMove) && canMove){
            stoppedMove = false;
            Vector3 vel = Vector3.zero;
            
            float camRot = cam.transform.rotation.eulerAngles.y;

            vel += Quaternion.Euler(0, camRot, 0) * movementVector * moveSpeed;
            rb.velocity = vel;
            
            float angle = Vector3.SignedAngle(Vector3.forward, movementVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * turnTime);
        }

        if(pressedRoll){
            if(rb.velocity != Vector3.zero){
                pressedRoll = false;
                pressedTimer = 0;
            }
            pressedTimer += Time.deltaTime;
            if(pressedTimer > 0.5f){
                pressedRoll = false;
                pressedTimer = 0;
                StartCoroutine(BackStep());
            }
        }
    }

    public void Move(InputAction.CallbackContext context){
        Vector2 inputVector2 = context.action.ReadValue<Vector2>();
        movementVector = new Vector3(inputVector2.x, 0, inputVector2.y);
        if(context.canceled){
            stoppedMove = true;
            movementVector = Vector3.zero;
        }
        Debug.Log(inputVector2);
    }

    public void Run(InputAction.CallbackContext context){
        float value = context.ReadValue<float>();
        if(context.performed){
            moveSpeed = runSpeed;
            pressedTimer = 0;
            pressedRoll = true;
        }else if(context.canceled){
            moveSpeed = walkSpeed;
            pressedTimer = 0;
            pressedRoll = false;
        }
    }

    public IEnumerator BackStep(){
        canMove = false;
        Debug.Log("A");
        Vector3 vel = Vector3.zero;
        
        float camRot = cam.transform.rotation.eulerAngles.y;
        camRot += 180;

        vel += Quaternion.Euler(0, camRot, 0) * movementVector * backstepSpeed;
        rb.velocity = vel;
        
        float angle = Vector3.SignedAngle(Vector3.forward, movementVector, Vector3.up);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        yield return new WaitForSeconds(.5f);
        canMove = true;
    }
}
