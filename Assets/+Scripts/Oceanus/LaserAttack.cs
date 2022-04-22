using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class LaserAttack : MonoBehaviour
    {
        private float timer = 2f;
        float accelx, accelz, accely = 0;
        public GameObject pivotpoint;
        private GameObject boss;

        void Awake()
        {
            boss = GameObject.FindGameObjectWithTag("Oceanus");
        }

            // Update is called once per frame
            void Update()
        {
            if ( boss.GetComponent<BossController>().currHealth <= 0)
            {
                Destroy(gameObject);
            }
             
            timer = timer - Time.deltaTime;
            if (timer < 0)
            {
                Destroy(gameObject);
            }

            accelx = accelx - 0 * Time.deltaTime;
            accely = Input.acceleration.y;
            accelz = Input.acceleration.z;
            pivotpoint.transform.Rotate(accelx * Time.deltaTime, accely * Time.deltaTime, accelz * Time.deltaTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            {

                collision.GetComponent<PlayerManager>().Damage(10);

            }
        }
        void OnTriggerStay(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            {

                collision.GetComponent<PlayerManager>().Damage(1);

            }
        }
    }
}
