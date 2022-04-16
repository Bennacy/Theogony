using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyController : MonoBehaviour
    {
        public float maxHealth;
        public float currHealth;
        public int currencyDrop;
        public float iFrames;
        private GlobalInfo globalInfo;
        private Rigidbody rb;
        private ParticleSystem blood;
        public LayerMask viewLayer;
        public Collider[] inRange;
        public float viewRange;
        public float moveSpeed;
        public Transform target;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            blood = GetComponentInChildren<ParticleSystem>();
            rb = GetComponent<Rigidbody>();
            currHealth = maxHealth;
        }

        void Update()
        {
            if(currHealth <= 0){
                Kill();
            }

            inRange = Physics.OverlapSphere(transform.position, viewRange, viewLayer);
            if(inRange.Length != 0){
                target = inRange[0].transform;
            }else{
                target = null;
            }

            if(target != null){
                rb.velocity = GetDirection(transform.position, target.position) * moveSpeed;
            }else{
                rb.velocity = Vector3.zero;
            }
        }

        void Kill(){
            globalInfo.AlterCurrency(currencyDrop);
            Destroy(gameObject);
        }

        public void Damage(float damageTaken){
            currHealth -= damageTaken;
        }

        void OnCollisionEnter(Collision collision){
            if(collision.gameObject.layer == 8){
                DamageCollider enemyController = collision.gameObject.GetComponent<DamageCollider>();
                Damage(10);
                blood.transform.position = collision.contacts[0].point;
                Vector3 direction = GetDirection(collision.transform.position, transform.position);
                direction.y = 0;
                rb.AddForce(direction * 10, ForceMode.Impulse);
                blood.Play();
                // Debug.Log(collision.contacts[0].point);
            }
        }

        private Vector3 GetDirection(Vector3 position1, Vector3 position2){
            return Vector3.Normalize(position2 - position1);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, viewRange);
        }
    }
}