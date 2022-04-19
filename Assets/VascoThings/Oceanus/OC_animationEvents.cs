using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class OC_animationEvents : MonoBehaviour
    {
        Animator OC_animHandler;
        OC_Weapon tridentCollider;
     

        private void Awake()
        {

            OC_animHandler = GetComponent<Animator>();
            tridentCollider = GetComponentInChildren<OC_Weapon>();



        }

        public void EnableCollider()
        {
            tridentCollider.EnableCollider();
        }

        public void DisableCollider()
        {
            tridentCollider.DisableCollider();
        }
    }
}
