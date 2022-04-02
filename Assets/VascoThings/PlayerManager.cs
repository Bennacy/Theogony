using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;

        public CameraHandler cameraHandler;
        PlayerLocomotaion playerLocomotion;
        public bool isInteracting; 


        private void Awake()
        {
            
        }

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotaion>();
        }


        void Update()
        {
            float delta = Time.deltaTime;
            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
           
        }
        private void LateUpdate()
        {
            inputHandler.l_Input = false;
            inputHandler.h_Input = false;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }
        }


    }
}