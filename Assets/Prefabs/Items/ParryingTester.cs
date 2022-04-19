using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class ParryingTester : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private GameObject mob;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GotParried()
        {
            animator.Play("Parried");
            mob.GetComponent<Riposte>().GotParried();
        }
    }
}
