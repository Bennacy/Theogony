using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class BlockColider : MonoBehaviour
    {
        public Collider blockCollider;

        //  Collider blockCollider;

        private void Awake()
        {
            Transform parentTrans = GameObject.FindGameObjectWithTag("Player").transform;
            
            blockCollider = parentTrans.Find("Block").gameObject.GetComponent<Collider>();
            blockCollider.gameObject.SetActive(true);
            blockCollider.isTrigger = false;
            blockCollider.enabled = false;
        }

        public void EnableCollider()
        {
            blockCollider.enabled = true;
        }

        public void DisableCollider()
        {
            blockCollider.enabled = false;
        }
    }

}
