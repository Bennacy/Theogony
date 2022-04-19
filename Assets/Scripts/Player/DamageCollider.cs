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
        public bool ripost;

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

        void Update()
        {
            ripost = player.riposteAttack;
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
            Debug.Log("Hit");
            if (collision.gameObject.layer == 3){
                if (ripost){
                    collision.GetComponentInChildren<Animator>().Play("Riposted");
                    Debug.Log("RIP");
                    player.GetComponent<PlayerAttacker>().riposteAttack = false;
                }
                Debug.Log("Hit");
            }
        }
    }
}
