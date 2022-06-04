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
        public List<GameObject> riposteEnemies;

        private void Awake()
        {
            hitEnemies = new List<GameObject>();
            riposteEnemies = new List<GameObject>();
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

        public void ResetList(){
            hitEnemies = new List<GameObject>();
            riposteEnemies = new List<GameObject>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            GameObject rootObject = collision.gameObject;
            while(rootObject.transform.parent != null){
                rootObject = rootObject.transform.parent.gameObject;
            }
            foreach(GameObject hit in riposteEnemies){
                if(rootObject == hit){
                    return;
                }
            }
            riposteEnemies.Add(rootObject);
            
            if(rootObject.GetComponent<HiddenWall>()){
                rootObject.GetComponent<HiddenWall>().fading = true;
            }

            if (player.riposteAttack && !collision.gameObject.GetComponentInParent<EnemyController>().invincible){
                player.playerManager.cameraHandler.ShakeRotation(1f, 0f, 1f, .2f);
                collision.transform.parent.GetComponentInChildren<Animator>().StopPlayback();
                collision.transform.parent.GetComponentInChildren<Animator>().Play("Riposted");
            }
        }
    }
}
