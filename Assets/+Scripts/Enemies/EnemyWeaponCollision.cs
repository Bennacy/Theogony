using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyWeaponCollision : MonoBehaviour
    {
        Collider damageCollider;
        private GlobalInfo globalInfo;
        private Transform playerBlock;
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
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.name == "Block"){
                playerBlock = other.transform;
            }
            
            if(other.tag == "Player" && !GetComponentInParent<EnemyController>().staggered){
                EnemyWeapons weapon = GetComponentInParent<EnemyWeaponManager>().weaponTemplate;
                PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();

                if(playerBlock != null){
                    playerBlock = null;
                    manager.StaminaDamage(weapon.damageDealt / 3);
                }else{
                    StartCoroutine(manager.Knockback(transform, weapon.knockback));
                    manager.wasHit = true;
                    manager.Damage(weapon.damageDealt);
                }
            }
        }
    }
}