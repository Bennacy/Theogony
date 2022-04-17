using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class Riposte : MonoBehaviour
    {

        public bool canRiposte;
        private float riposteTimer = 3;
        Collider riposteCollider;

        void Start()
        {
            riposteCollider = GetComponent<Collider>();
            riposteCollider.gameObject.SetActive(true);
            riposteCollider.isTrigger = true;
            riposteCollider.enabled = false;
        }

        
        void Update()
        {
            if (canRiposte)
            {
                riposteCollider.enabled = true;
                riposteTimer = riposteTimer- Time.deltaTime;
            }
            if(riposteTimer < 0)
            {
                riposteCollider.enabled = false;
            }
        }

        public void GotParried()
        {
            canRiposte = true;
            riposteTimer = 3;
        }

        private void OnTriggerEnter(Collider collision)
        {

            if (collision.tag == "Player" && canRiposte)
            {
                //create function to parry
                collision.GetComponent<PlayerAttacker>().WillRiposte();

                Debug.Log("riposted");
            }
        }
    }
}
