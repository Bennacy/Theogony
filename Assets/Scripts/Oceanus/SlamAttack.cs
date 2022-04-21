using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class SlamAttack : MonoBehaviour
    {
        private float timer = 1.5f;

        // Update is called once per frame
        void Update()
        {
            timer = timer - Time.deltaTime;
            if (timer < 0)
            {
                Destroy(gameObject);
            }
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            {

                collision.GetComponent<PlayerManager>().Damage(10);

            }
        }
    }
}
