using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

namespace Theogony{
    public class PlayerManager : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        private PlayerControllerScript playerControllerScript;
        private Rigidbody rb;
        public CameraHandler cameraHandler;
        public EnemyController riposteEnemy;
        public float maxHealth;
        public float maxStamina;
        public float currHealth;
        public float currStamina;
        public float staminaRecharge;
        public float blockingRecharge;
        public float normalRecharge;
        public int[] levels;
        public bool staminaSpent;
        public bool wasHit;
        public int healCharges;

        void Start()
        {
            playerControllerScript = GetComponent<PlayerControllerScript>();
            rb = GetComponent<Rigidbody>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            cameraHandler = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraHandler>();
            maxHealth = globalInfo.healthIncrease.Evaluate(globalInfo.vit * .01f) * 10000;
            maxStamina = globalInfo.staminaIncrease.Evaluate(globalInfo.end * .01f) * 1000;
            currHealth = maxHealth;
            currStamina = maxStamina;
            healCharges = globalInfo.healCharges;
        }

        void Update()
        {
            maxHealth = globalInfo.healthIncrease.Evaluate(globalInfo.vit * .01f) * 10000;
            maxStamina = globalInfo.staminaIncrease.Evaluate(globalInfo.end * .01f) * 1000;
            if(currStamina < maxStamina){
                currStamina += staminaRecharge * Time.deltaTime;
            }
            if(currHealth < 0){
                Die();
            }

            if(riposteEnemy){
                if(!riposteEnemy.riposteCollider.enabled){
                    riposteEnemy = null;
                    GetComponent<PlayerAttacker>().riposteAttack = false;
                }
            }

            if(Input.GetKeyDown(KeyCode.G)){
                // cameraHandler.ShakePosition(.1f, .1f, .1f, .3f);
                // cameraHandler.ShakeRotation(1f, 0f, 1f, .3f);
            }
        }

        void LateUpdate()
        {
            // if (cameraHandler != null)
            // {
            //     float delta = Time.deltaTime;
            //     cameraHandler.HandleCameraRotation(delta);
            //     cameraHandler.FollowTarget(delta);
            // }
            // wasHit = false;
        }

        void FixedUpdate()
        {
            if (cameraHandler != null)
            {
                float delta = Time.fixedDeltaTime;
                cameraHandler.HandleCameraRotation(delta);
                cameraHandler.FollowTarget(delta);
            }
        }

        private void Die(){
            if(GetComponent<PlayerInput>().enabled)
                globalInfo.DropCurrency();
            GetComponent<PlayerInput>().enabled = false;
            globalInfo.playerTargetable = false;
            GetComponentInChildren<Animator>().Play("Die");
        }

        public void Riposte(){
            Vector3 ripostePos = riposteEnemy.transform.position + RipostePosition();
            ripostePos.y = transform.position.y;
            transform.position = ripostePos;
            transform.LookAt(riposteEnemy.transform);
            cameraHandler.LoseLockOn();
            rb.isKinematic = true;
            globalInfo.playerTargetable = false;
            GetComponentInChildren<Animator>().Play("Riposte");
            riposteEnemy.GetComponent<FSM>().staggerTimer = float.PositiveInfinity;
            StartCoroutine(EndRiposte(1));
        }

        private Vector3 RipostePosition(){
            Vector3 forward = riposteEnemy.transform.forward;
            // forward *= riposteEnemy.riposteDistance;
            return forward;
        }

        public IEnumerator EndRiposte(float wait){
            yield return new WaitForSeconds(wait);
            rb.isKinematic = false;
            globalInfo.playerTargetable = true;
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
        
        public void Heal(InputAction.CallbackContext context){
            if(context.performed){
                Animator animator = GetComponentInChildren<Animator>();
                if(healCharges > 0 && !animator.GetBool("Occupied")){
                    string animName = "Drink";
                    if(animator.GetCurrentAnimatorStateInfo(0).IsName("FastDrink") || animator.GetCurrentAnimatorStateInfo(0).IsName("Drink"))
                        animName = "FastDrink";

                    animator.Play(animName);
                    healCharges--;
                    currHealth += globalInfo.healBase + (globalInfo.healLevel * globalInfo.healIncrease);
                    if(currHealth > maxHealth){
                        currHealth = maxHealth;
                    }
                }
            }
        }

        public void Damage(float deduction){
            currHealth -= deduction;
        }

        public void StaminaDamage(float deduction){
            currStamina -= deduction;
            if(currStamina < 0){
                currStamina = 0;
                StanceBreak();
            }
        }

        public void StanceBreak(){
            Debug.Log("Stance Broken");
            Animator animator = GetComponentInChildren<Animator>();
            WeaponSlotManager manager = GetComponentInChildren<WeaponSlotManager>();
            manager.DisableBlockCollider();
            manager.ResetEvents();
            manager.EnableKinematic();
            
            playerControllerScript.blocking = false;
            playerControllerScript.canMove = false;
            animator.Play("StanceBreak");
        }

        public IEnumerator Knockback(Transform weapon, float knockback){
            playerControllerScript.canMove = false;
            Vector3 direction = GetDirection(weapon.position, transform.position);
            direction.y = 0;
            rb.AddForce(direction * knockback, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f);
            playerControllerScript.canMove = true;
        }

        private Vector3 GetDirection(Vector3 position1, Vector3 position2){
            return Vector3.Normalize(position2 - position1);
        }
    }
}