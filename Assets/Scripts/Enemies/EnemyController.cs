using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyController : MonoBehaviour
    {
        public float maxHealth;
        public float currHealth;
        public int currencyDrop;
        private GlobalInfo globalInfo;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            currHealth = maxHealth;
        }

        void Update()
        {
            if(currHealth <= 0){
                Kill();
            }
        }

        void Kill(){
            globalInfo.AlterCurrency(currencyDrop);
            Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "PlayerWeapon"){
                Debug.Log("Hit");
                currHealth -= 10;
            }
        }
    }
}