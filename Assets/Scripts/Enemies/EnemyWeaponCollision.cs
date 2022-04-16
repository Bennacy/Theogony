using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyWeaponCollision : MonoBehaviour
    {
        Collider damageCollider;
        private GlobalInfo globalInfo;
        public List<GameObject> hitEnemies;

        private void Awake()
        {
            hitEnemies = new List<GameObject>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            Rigidbody rb;
            if(GetComponent<Rigidbody>() == null){
                rb = gameObject.AddComponent<Rigidbody>();
            }else{
                rb = GetComponent<Rigidbody>();
            }
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            gameObject.tag = "EnemyWeapon";
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = false;
            damageCollider.enabled = false;
            gameObject.layer = 8;
        }
    }
}