using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class ParryColider : MonoBehaviour
    {

        Collider parryCollider;

        private void Awake()
        {
            parryCollider = GetComponent<Collider>();
            parryCollider.gameObject.SetActive(true);
            parryCollider.isTrigger = true;
            parryCollider.enabled = false;

        }

        public void EnableCollider()
        {
            parryCollider.enabled = true;
        }

        public void DisableCollider()
        {
            parryCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "EnemyWeapon")
            {
                Debug.Log("Hit weapon");
                collision.GetComponentInParent<EnemyController>().GotParried();
            }
        }
    }
    
}
