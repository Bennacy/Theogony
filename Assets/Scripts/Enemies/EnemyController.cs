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

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            // rb = GetComponent<Rigidbody>();
            currHealth = maxHealth;
        }

        void Update()
        {
            // rb.velocity = Vector3.back;
            if(currHealth <= 0){
                Kill();
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
                // Debug.Log(collision.contacts[0].point);
            }
        }
    }
}