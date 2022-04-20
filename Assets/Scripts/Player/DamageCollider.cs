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
            if (player.riposteAttack){
                collision.gameObject.GetComponent<EnemyController>().riposteCollider.enabled = false;
                player.playerManager.cameraHandler.ShakeRotation(1f, 0f, 1f, .2f);
                collision.transform.GetComponentInChildren<Animator>().StopPlayback();
                collision.transform.GetComponentInChildren<Animator>().Play("Riposted");
                player.riposteAttack = false;
                Debug.Log("RIP");
            }
        }
    }
}
