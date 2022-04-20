using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Theogony{
    public class PlayerManager : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        private PlayerControllerScript playerControllerScript;
        private Rigidbody rb;
        public CameraHandler cameraHandler;
        public EnemyController parryEnemy;
        public float maxHealth;
        public float maxStamina;
        public float currHealth;
        public float currStamina;
        public float staminaRecharge;
        public int[] levels;
        public bool staminaSpent;

        void Start()
        {
            playerControllerScript = GetComponent<PlayerControllerScript>();
            rb = GetComponent<Rigidbody>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            cameraHandler = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraHandler>();
            maxHealth = globalInfo.baseHealth + (globalInfo.vit * globalInfo.vitIncrease);
            maxStamina = globalInfo.baseStamina + (globalInfo.end * globalInfo.endIncrease);
            currHealth = maxHealth;
            currStamina = maxStamina;
        }

        void Update()
        {
            maxHealth = globalInfo.baseHealth + (globalInfo.vit * globalInfo.vitIncrease);
            maxStamina = globalInfo.baseStamina + (globalInfo.end * globalInfo.endIncrease);
            if(!staminaSpent && currStamina < maxStamina){
                currStamina += staminaRecharge * Time.deltaTime;
            }
            if(currHealth < 0){
                Die();
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

        private void Die(){
            GetComponent<PlayerInput>().enabled = false;
            globalInfo.playerTargetable = false;
            GetComponentInChildren<Animator>().Play("Die");
        }

        public void Riposte(){
            GetComponentInChildren<Animator>().Play("Riposte");
            parryEnemy.GetComponent<FSM>().staggerTimer = float.PositiveInfinity;
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

        void OnTriggerEnter(Collider collision){
            if(collision.gameObject.tag == "EnemyWeapon"){
                EnemyWeapons weapon = collision.gameObject.GetComponentInParent<EnemyWeaponManager>().weaponTemplate;
                StartCoroutine(Knockback(collision, weapon.knockback));
                Damage(weapon.damageDealt);
            }
        }

        private void Damage(float deduction){
            currHealth -= deduction;
        }

        private IEnumerator Knockback(Collider collision, float knockback){
            playerControllerScript.canMove = false;
            Vector3 direction = GetDirection(collision.transform.position, transform.position);
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