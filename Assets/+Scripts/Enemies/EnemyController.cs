using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Theogony{
    public class EnemyController : MonoBehaviour
    {
        public AudioClip hitClip;
        public AudioClip dieClip;
        public AudioClip walkClip;
        public AudioSource audioSource;

        public float maxHealth;
        public float currHealth;
        public int currencyDrop;
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
        public bool staggered;
        public EnemyWeapons weapon;
        public GameObject dropPreset;
        public bool invincible;

        [HideInInspector]
        public bool dying;
        public Collider riposteCollider;
        private MonoBehaviour[] scripts;
        public bool wasHit;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            blood = GetComponentInChildren<ParticleSystem>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            currHealth = maxHealth;
            audioSource = GetComponent<AudioSource>();
            weapon = GetComponent<EnemyWeaponManager>().weaponTemplate;
            dying = false;
        }

        public void Update()
        {
            animator.SetFloat("Velocity", Vector3.Magnitude(navMeshAgent.velocity));
            playerTargetable = globalInfo.playerTargetable;
        }

        public void Damage(float damageTaken){
            currHealth -= damageTaken;
            wasHit = true;
            if(currHealth > 0)
                audioSource.PlayOneShot(hitClip);
        }

        void OnTriggerEnter(Collider collision){
            if(collision.gameObject.layer == 8 && collision.gameObject.tag == "PlayerWeapon"){
                if(invincible)
                    return;
                
                DamageCollider collider = collision.GetComponent<DamageCollider>();
                foreach(GameObject hit in collider.hitEnemies){
                    if(hit == gameObject){
                        return;
                    }
                }
                collider.hitEnemies.Add(gameObject);

                weaponItems weapon = collision.gameObject.GetComponentInParent<PlayerInventory>().rightWeapon;
                Damage(weapon.CalculateDamage(globalInfo, collision.GetComponentInParent<PlayerAttacker>().riposteAttack));
                collision.GetComponentInParent<PlayerAttacker>().riposteAttack = false;
                riposteCollider.enabled = false;
                if(weapon.knockback - knockbackResistance > 0){
                    Knockback(collision, weapon.knockback - knockbackResistance);
                }
                blood.Play();
            }
        }

        private void Knockback(Collider collision, float knockback){
                Vector3 direction = GetDirection(collision.transform.position, transform.position);
                direction.y = 0;
                rb.AddForce(direction * knockback, ForceMode.Impulse);
        }

        private Vector3 GetDirection(Vector3 position1, Vector3 position2){
            return Vector3.Normalize(position2 - position1);
        }

        public void GotParried(){
            GetComponent<Rigidbody>().isKinematic = true;
            staggered = true;
            riposteCollider.enabled = true;
        }

        void OnDrawGizmosSelected()
        {
            for(int i = 0; i < patrolWaypoints.Length; i++){
                Vector3 waypointPos = patrolWaypoints[i];
                if(i == waypointIndex){
                    Gizmos.color = Color.red;
                }else{
                    Gizmos.color = Color.white;
                }
                Gizmos.DrawWireSphere(waypointPos, 1);
                Gizmos.color = Color.white;
                int j = i;
                j++;
                if(j >= patrolWaypoints.Length){
                    j = 0;
                }
                Gizmos.DrawLine(waypointPos, waypointPos + ((patrolWaypoints[j] - patrolWaypoints[i]) / 2));
            }
        }
    }
}