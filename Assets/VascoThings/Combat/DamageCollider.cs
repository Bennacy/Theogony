using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony {
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;

            private void Awake()
            {
                damageCollider = GetComponent<Collider>();
                damageCollider.gameObject.SetActive(true);
                damageCollider.isTrigger = true;
                damageCollider.enabled = false;

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
            if (collision.tag == "Enemy")
            {
                //create function to take damage
                Debug.Log("Hit");
            }
        }
    }

}
