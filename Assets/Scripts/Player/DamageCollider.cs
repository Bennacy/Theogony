using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony {
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        private GlobalInfo globalInfo;
        public PlayerAttacker player;
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
            gameObject.tag = "PlayerWeapon";
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
            gameObject.layer = 8;
            
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttacker>();
        }

        public void EnableCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableCollider()
        {
            damageCollider.enabled = false;
        }
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == 3){
                collision.transform.parent.GetComponentInChildren<Animator>().Play("Riposted");
                collision.GetComponentInParent<FSM>().staggerTimer = float.PositiveInfinity;
                collision.enabled = false;
                player.riposteAttack = false;
                Debug.Log("RIP");
            }
        }
    }
}
