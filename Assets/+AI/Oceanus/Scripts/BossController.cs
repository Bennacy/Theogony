using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Theogony
{
    public class BossController : MonoBehaviour
    {
        public float maxHealth;
        public float currHealth;
        public int currencyDrop;
        private GlobalInfo globalInfo;
        public Animator animator;
        private NavMeshAgent navMeshAgent;
        private Rigidbody rb;
        private ParticleSystem blood;
      

        public bool attacking;
        public Transform target;
        public bool playerTargetable;
        public bool staggered;
        public BossWeapon weapon;
        public bool invincible;

        [HideInInspector]
        public bool dying;
        public Collider parryCollider;
        private MonoBehaviour[] scripts;

        public bool phase2Transition;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            blood = GetComponentInChildren<ParticleSystem>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            currHealth = maxHealth;
            weapon = GetComponent<EnemyWeaponManager>().bossweaponTemplate;
            dying = false;
        }

        void Update()
        {
            if (currHealth <= 0 && !dying)
            {
                // Kill();
            }
           
            playerTargetable = globalInfo.playerTargetable;
        }

        public void Damage(float damageTaken)
        {
            currHealth -= damageTaken;
        }

        private Vector3 GetDirection(Vector3 position1, Vector3 position2)
        {
            return Vector3.Normalize(position2 - position1);
        }

        public void GotParried()
        {
            GetComponent<Rigidbody>().isKinematic = true;
            staggered = true;
            parryCollider.enabled = true;
        }

      
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == 8 && collision.gameObject.tag == "PlayerWeapon")
            {
                if (invincible)
                    return;
                weaponItems weapon = collision.gameObject.GetComponentInParent<PlayerInventory>().rightWeapon;
                Damage(weapon.CalculateDamage(globalInfo, staggered));
                blood.transform.position = transform.position;
                blood.Play();
            }
        }
    }
}