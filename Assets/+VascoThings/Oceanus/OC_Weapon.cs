using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
        public class OC_Weapon : MonoBehaviour
        {
            Collider tridentCollider;
                

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

                    Debug.Log("Hit Player");
                }
            }
        }
}

