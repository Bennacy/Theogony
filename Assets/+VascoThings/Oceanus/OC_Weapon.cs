using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
        public class OC_Weapon : MonoBehaviour
        {
            Collider tridentCollider;
            public float OC_Damage;
                

            private void Awake()
            {
                tridentCollider = GetComponent<Collider>();
                tridentCollider.gameObject.SetActive(true);
                tridentCollider.isTrigger = true;
              //tridentCollider.enabled = false;

            }


            public void EnableCollider()
            {
                tridentCollider.enabled = true;
            }

            public void DisableCollider()
            {
                tridentCollider.enabled = false;
            }

            private void OnTriggerEnter(Collider collision)
            {

                if (collision.tag == "Player")
                {
                collision.GetComponent<PlayerManager>().Damage(OC_Damage);
                    Debug.Log("Hit Player");
                }
            }
        }
}

