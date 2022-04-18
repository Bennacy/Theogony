using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Theogony{
    public class EnemyController : MonoBehaviour
    {
        public float maxHealth;
        public float currHealth;
        public int currencyDrop;
        public float iFrames;
        private GlobalInfo globalInfo;
        public Animator animator;
        private NavMeshAgent navMeshAgent;
        private Rigidbody rb;
        private ParticleSystem blood;
        public Vector3[] patrolWaypoints;
        public int waypointIndex;
        public float knockbackResistance;
        public bool attacking;
        public Transform target;
        public bool playerTargetable;
        public EnemyWeapons weapon;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            blood = GetComponentInChildren<ParticleSystem>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            currHealth = maxHealth;
            weapon = GetComponent<EnemyWeaponManager>().weaponTemplate;
        }

        void Update()
        {
            if(currHealth <= 0){
                Kill();
            }
            animator.SetFloat("Velocity", Vector3.Magnitude(navMeshAgent.velocity));
            playerTargetable = globalInfo.playerTargetable;
        }

        void Kill(){
            globalInfo.AlterCurrency(currencyDrop);
            Destroy(gameObject);
        }

        public void Damage(float damageTaken){
            currHealth -= damageTaken;
        }

        void OnCollisionEnter(Collision collision){
            if(collision.gameObject.layer == 8 && collision.gameObject.tag == "PlayerWeapon"){
                weaponItems weapon = collision.gameObject.GetComponentInParent<PlayerInventory>().rightWeapon;
                Debug.Log(weapon);
                Damage(weapon.CalculateDamage(globalInfo));
                blood.transform.position = collision.contacts[0].point;
                if(weapon.knockback - knockbackResistance > 0){
                    Knockback(collision, weapon.knockback - knockbackResistance);
                }
                blood.Play();
            }
        }

        private void Knockback(Collision collision, float knockback){
                Vector3 direction = GetDirection(collision.transform.position, transform.position);
                direction.y = 0;
                rb.AddForce(direction * knockback, ForceMode.Impulse);
        }

        private Vector3 GetDirection(Vector3 position1, Vector3 position2){
            return Vector3.Normalize(position2 - position1);
        }

        void OnDrawGizmosSelected()
        {
            for(int i = 0; i < patrolWaypoints.Length; i++){
                if(i == waypointIndex){
                    Gizmos.color = Color.red;
                }else{
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawWireSphere(patrolWaypoints[i], 1);
            }
        }
    }
}