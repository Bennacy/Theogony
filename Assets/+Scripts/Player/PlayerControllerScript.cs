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
        public PlayerInput playerInput;
        public CapsuleCollider damageColl;
        public Rigidbody rb;
        public GameObject cam;
        public CameraHandler cameraHandler;
        public Quaternion camForward;
        public Vector3 movementVector;
        public Animator animator;
        private PlayerAttacker playerAttacker;
        private PlayerInventory playerInventory;
        [Space]
        
        [Space]
        [Header("Movement Values")]
        public float walkSpeed;
        public float runSpeed;
        public float backstepSpeed;
        public float rollSpeed;
        public float drinkSpeed;
        public float moveSpeed;
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
        public bool blocking;



        void Start()
        {
            // globalInfo = GlobalInfo.GetGlobalInfo();
            moveSpeed = walkSpeed;
            animator = GetComponentInChildren<Animator>();
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            cam = GameObject.FindGameObjectWithTag("Camera");
            cameraHandler = cam.GetComponent<CameraHandler>();
            cameraHandler = playerManager.cameraHandler;
            playerInput = GetComponent<PlayerInput>();
            Debug.Log(playerInput.currentControlScheme);
        }

        // void OnGUI()
        // {
        //     GUI.Label(new Rect(0, 0, 100, 100), (playerInput.currentControlScheme));
        // }

        void Update()
        {
            animator.SetBool("Blocking", blocking);

            if(Input.GetKeyDown(KeyCode.Alpha1)){
                playerInput.SwitchCurrentControlScheme("Keyboard");
            }
            if(Input.GetKeyDown(KeyCode.Alpha2)){
                playerInput.SwitchCurrentControlScheme("XBox");
            }
            if(Input.GetKeyDown(KeyCode.Alpha3)){
                playerInput.SwitchCurrentControlScheme("PS4");
            }
            float camRot = cam.transform.rotation.eulerAngles.y;
            camForward = Quaternion.Euler(0, camRot, 0);
            if((movementVector != Vector3.zero || stoppedMove) && canMove){
                stoppedMove = false;
                Vector3 vel = Vector3.zero;
                if(running){
                    Vector3.Normalize(movementVector);
                    if(!playerManager.UpdateStamina(runCost * Time.deltaTime)){
                        running = false;
                        moveSpeed = walkSpeed;
                        // playerManager.staminaSpent = false;
                    }else{
                        // StartCoroutine(playerManager.RechargeStamina());
                    }
                }
                
                vel += camForward * movementVector * moveSpeed;
                rb.velocity = vel;
                

                animator.SetFloat("Speed", rb.velocity.magnitude);
                if(canMove && rb.velocity.magnitude > 0.1f){
                    // animator.speed = rb.velocity.magnitude / moveSpeed;
                }
            }
            float angle = transform.rotation.eulerAngles.y;
            if(cameraHandler.lockOnTarget != null && canMove){
                Vector3 direction = cameraHandler.lockOnTarget.position - transform.position;
                Vector3.Normalize(direction);
                angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            }else if((movementVector != Vector3.zero || stoppedMove) && canMove){
                angle = Vector3.SignedAngle(Vector3.forward, camForward * movementVector, Vector3.up);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * turnTime);
        }

        public void Move(InputAction.CallbackContext context){
            Vector2 inputVector2 = context.action.ReadValue<Vector2>();
            movementVector = new Vector3(inputVector2.x, 0, inputVector2.y);
           
            if(context.canceled){
                stoppedMove = true;
                movementVector = Vector3.zero;
            }
        }

        public void LightAttack(InputAction.CallbackContext context){
            if(context.performed){
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        public void HeavyAttack(InputAction.CallbackContext context){
            if(context.performed){
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }


        public void Parry(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
              
                canMove = false;
                playerAttacker.HandleParry(playerInventory.leftWeapon);
            }
        }


        public void Block(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                canMove = false;
                blocking = true;
                // playerAttacker.HandleBlock(playerInventory.leftWeapon);
            }
            if(context.canceled && blocking){
                canMove = true;
                blocking = false;
                GetComponentInChildren<WeaponSlotManager>().ResetEvents();
                GetComponentInChildren<WeaponSlotManager>().DisableBlockCollider();
            }
        }

        public void Run(InputAction.CallbackContext context){
            float value = context.ReadValue<float>();
            if(context.performed){
                moveSpeed = runSpeed;
                running = true;
            }else if(context.canceled){
                moveSpeed = walkSpeed;
                running = false;
            }
        }

        public void Roll(InputAction.CallbackContext context){
            if(true){
                if(context.performed && canMove && !animator.GetBool("Occupied")){ //Backstep
                    if(movementVector == Vector3.zero && playerManager.UpdateStamina(backstepCost)){
                        rb.useGravity = false;
                        damageColl.enabled = false;
                        moveSpeed = backstepSpeed;
                        rb.velocity += ((rb.rotation * Vector3.forward) * moveSpeed);
                        animator.Play("Backstep");
                        canMove = false;
                        // playerManager.staminaSpent = true;
                    }else if(playerManager.UpdateStamina(rollCost)){ //Roll
                        rb.useGravity = false;
                        damageColl.enabled = false;
                        moveSpeed = rollSpeed;
                        float angle = Vector3.SignedAngle(Vector3.forward, camForward * movementVector, Vector3.up);
                        transform.rotation = Quaternion.Euler(0, angle, 0);
                        rb.velocity += (camForward * movementVector * moveSpeed);
                        animator.Play("Roll");
                        animator.speed = 1.5f;
                        canMove = false;
                        // playerManager.staminaSpent = true;
                    }
                }
            }
        }

        public void FinishRoll(){
            damageColl.enabled = true;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            moveSpeed = walkSpeed;
            canMove = true;
            StartCoroutine(playerManager.RechargeStamina());
        }
    }
}