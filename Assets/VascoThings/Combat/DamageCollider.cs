using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony {
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public GameObject player;
        public bool ripost;

            private void Awake()
            {
                damageCollider = GetComponent<Collider>();
                damageCollider.gameObject.SetActive(true);
                damageCollider.isTrigger = true;
                damageCollider.enabled = false;

            }

        public void Update()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            ripost = player.GetComponent<PlayerAttacker>().riposteAttack;

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
        
            if (collision.tag == "Enemy")
            {
                if (ripost)
                {
                    collision.GetComponentInChildren<Animator>().Play("Riposted");
                    player.GetComponent<PlayerAttacker>().riposteAttack = false;
                }
                Debug.Log("Hit");
            }
        }
    }

}
