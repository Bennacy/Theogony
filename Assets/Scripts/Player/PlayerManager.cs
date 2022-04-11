using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Theogony{
    public class PlayerManager : MonoBehaviour
    {
        // public GlobalInfo globalInfo;
        public CameraHandler cameraHandler;
        public float mouseX;
        public float mouseY;
        public float maxHealth;
        public float maxStamina;
        public float currHealth;
        public float currStamina;
        public float staminaRecharge;
        public int[] levels;
        public bool staminaSpent;

        void Start()
        {
            // globalInfo = GlobalInfo.GetGlobalInfo();
            currHealth = maxHealth;
            currStamina = maxStamina;
        }

        void Update()
        {
            if(!staminaSpent && currStamina < maxStamina){
                currStamina += staminaRecharge * Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            if (cameraHandler != null)
            {
                float delta = Time.deltaTime;
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta);
            }
        }

        public bool UpdateStamina(float changeBy){
            if(currStamina - changeBy >= 0){
                currStamina -= changeBy;
                return true;
            }else{
                return false;
            }
        }

        public IEnumerator RechargeStamina(){
            staminaSpent = true;
            yield return new WaitForSeconds(1);
            staminaSpent = false;
        }

        public void TestDelta(InputAction.CallbackContext context){
            Vector2 value = context.ReadValue<Vector2>();
            mouseX = value.x;
            mouseY = value.y;
        }
    }
}