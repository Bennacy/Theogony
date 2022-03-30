using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class PlayerManagerV : MonoBehaviour
    {
        InputHandler inputHandler;

        CameraHandler cameraHandler;
        PlayerLocomotaion playerLocomotion;


        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
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