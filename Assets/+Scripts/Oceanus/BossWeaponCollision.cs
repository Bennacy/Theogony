using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class BossWeaponCollision : MonoBehaviour
    {
        Collider damageCollider;
        private GlobalInfo globalInfo;
        public List<GameObject> hitEnemies;

        private void Awake()
        {
            hitEnemies = new List<GameObject>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            Rigidbody rb;
            if (GetComponent<Rigidbody>() == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            else
            {
                rb = GetComponent<Rigidbody>();
            }
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            gameObject.tag = "BossWeapon";
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        void OnTriggerEnter(Collider other)
        {    
            if(other.tag == "Player"){
                BossWeapon weapon = GetComponentInParent<EnemyWeaponManager>().bossweaponTemplate;
                PlayerManager manager = other.gameObject.GetComponent<PlayerManager>();
                StartCoroutine(manager.Knockback(transform, weapon.knockback));
                manager.wasHit = true;
                manager.Damage(weapon.damageDealt);
            }
        }
    }
}
