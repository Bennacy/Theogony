using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool l_Input;
        public bool h_Input;

        PlayerController inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerController();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            AttackInput(delta);

        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

      private void AttackInput(float delta)
      {
            inputActions.PlayerActions.LightAttack.performed += inputActions => l_Input = true;
            inputActions.PlayerActions.HeavyAttack.performed += inputActions => h_Input = true;

            if (l_Input)
            {
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
               
            }

            if (h_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
                Debug.Log("heavy attack");
            }
        }
    }
}