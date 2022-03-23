using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BRUH

public class PlayerMov : MonoBehaviour
{
    public Rigidbody rb;
    public float walkSpeed;
    public float runSpeed;
    private float moveSpeed;
    public float turnTime;
    public Camera cam;

    void Start()
    {
        
    }

    void Update()
    {
        float camRot = cam.transform.rotation.eulerAngles.y;
        Vector3 camForward = Quaternion.Euler(0, camRot, 0) * Vector3.forward;

        if(Input.GetKey(KeyCode.LeftShift)){
            moveSpeed = runSpeed;
        }else{
            moveSpeed = walkSpeed;
        }

        Vector3 vel = Vector3.zero;
        if(Input.GetKey(KeyCode.W)){
            vel += (camForward * moveSpeed);
        }
        if(Input.GetKey(KeyCode.S)){
            vel += (Quaternion.Euler(0, 180, 0) * camForward * moveSpeed);
        }
        if(Input.GetKey(KeyCode.D)){
            vel += (Quaternion.Euler(0, 90, 0) * camForward * moveSpeed);
        }
        if(Input.GetKey(KeyCode.A)){
            vel += (Quaternion.Euler(0, -90, 0) * camForward * moveSpeed);
        }
        rb.velocity = vel;
        Vector3 movementDir = vel.normalized;
        // Debug.Log(camForward);
        if(movementDir != Vector3.zero){
            float angle = Vector3.SignedAngle(Vector3.forward, movementDir, Vector3.up);
            Debug.Log(angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * turnTime);
        }
    }
}
