using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Theogony{
    public class PlayerControllerScript : MonoBehaviour
    {
        [Header("References")]
        // public GlobalInfo globalInfo;
        public PlayerManager playerManager;
        public CapsuleCollider coll;
        public Rigidbody rb;
        public GameObject cam;
        public Quaternion camForward;
        private Vector3 movementVector;
        private Animator animator;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        [Space]
        
        [Space]
        [Header("Movement Values")]
        public float walkSpeed;
        public float runSpeed;
        public float backstepSpeed;
        public float rollSpeed;
        private float moveSpeed;
        public float backstepTime;
        public float rollTime;
        public float turnTime;
        public float rollCost;
        public float backstepCost;
        public float runCost;
        [Space]

        [Space]
        [Header("Booleans")]
        public bool canMove;
        private bool stoppedMove;
        public bool running;


        void Start()
        {
            // globalInfo = GlobalInfo.GetGlobalInfo();
            moveSpeed = walkSpeed;
            animator = GetComponentInChildren<Animator>();
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        void Update()
        {
            float camRot = cam.transform.rotation.eulerAngles.y;
            camForward = Quaternion.Euler(0, camRot, 0);
            if((movementVector != Vector3.zero || stoppedMove) && canMove){
                stoppedMove = false;
                Vector3 vel = Vector3.zero;
                if(running){
                    if(!playerManager.UpdateStamina(runCost * Time.deltaTime)){
                        running = false;
                        moveSpeed = walkSpeed;
                        playerManager.staminaSpent = false;
                    }else{
                        StartCoroutine(playerManager.RechargeStamina());
                    }
                }
                
                vel += camForward * movementVector * moveSpeed;
                rb.velocity = vel;
                
                float angle = Vector3.SignedAngle(Vector3.forward, camForward * movementVector, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * turnTime);
            }
        }

        public void Move(InputAction.CallbackContext context){
            Vector2 inputVector2 = context.action.ReadValue<Vector2>();
            movementVector = new Vector3(inputVector2.x, 0, inputVector2.y);
            Debug.Log(movementVector);
            if(context.canceled){
                stoppedMove = true;
                movementVector = Vector3.zero;
            }
        }

        public void LightAttack(InputAction.CallbackContext context){
            if(context.performed){
                canMove = false;
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        public void HeavyAttack(InputAction.CallbackContext context){
            if(context.performed){
                canMove = false;
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        public void Run(InputAction.CallbackContext context){
            if(true){
                float value = context.ReadValue<float>();
                if(context.performed){
                    moveSpeed = runSpeed;
                    running = true;
                }else if(context.canceled){
                    moveSpeed = walkSpeed;
                    running = false;
                }
            }
        }

        public void Roll(InputAction.CallbackContext context){
            if(true){
                if(context.performed && canMove){
                    if(movementVector == Vector3.zero && playerManager.UpdateStamina(backstepCost)){
                        moveSpeed = backstepSpeed;
                        rb.velocity += ((rb.rotation * Vector3.forward) * moveSpeed);
                        StartCoroutine(RollTime(backstepTime));
                    }else if(playerManager.UpdateStamina(rollCost)){
                        moveSpeed = rollSpeed;
                        float angle = Vector3.SignedAngle(Vector3.forward, camForward * movementVector, Vector3.up);
                        transform.rotation = Quaternion.Euler(0, angle, 0);
                        rb.velocity += ((movementVector.normalized) * moveSpeed);
                        StartCoroutine(RollTime(rollTime));
                    }
                }
            }
        }

        private IEnumerator RollTime(float wait){
            // coll.enabled = false;
            canMove = false;
            playerManager.staminaSpent = true;
            yield return new WaitForSeconds(wait);
            coll.enabled = true;
            rb.velocity = Vector3.zero;
            moveSpeed = walkSpeed;
            canMove = true;
            StartCoroutine(playerManager.RechargeStamina());
        }
}
}