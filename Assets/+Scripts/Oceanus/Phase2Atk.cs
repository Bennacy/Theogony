using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class Phase2Atk : MonoBehaviour
    {
        private float timer = 3f;
        public Vector3 scaleChange = new Vector3(12f, 0f, 12f);
        public Vector3 explosionChange = new Vector3(0, 100f, 0f);
        public BossController bossController;

        Collider damageCollider;
       

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.enabled = false;
        }
        void Update()
        {
           if(bossController.dying){
               Destroy(gameObject);
           }

            timer = timer - Time.deltaTime;
            if (timer < 1.2f)
            {
                transform.localScale = transform.localScale - explosionChange * Time.deltaTime  ;
                damageCollider.enabled = true;

            }
            else
            {
                transform.localScale = transform.localScale - scaleChange * Time.deltaTime ;
            }

            if (timer < 0)
            {
                Destroy(gameObject);
            }
       

           

        }
        void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
            {

                collision.GetComponent<PlayerManager>().Damage(20);

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
