using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class PlayerAttacker : MonoBehaviour
    {
        Animator animator;

        public void Awake()
        {
            animator = GetComponentInChildren<Animator>();

        }
        public void HandleLightAttack(weaponItems weaponItem)
        {

          

            if (weaponItem.currattack == weaponItem.lightAttack.Length)
            {
                weaponItem.currattack = 0;
            }
            animator.Play(weaponItem.lightAttack[weaponItem.currattack]);

            // weaponItem.currattack++;          

        }

        public void HandleHeavyAttack(weaponItems weaponItem)
        {
            if (animator.GetBool("Occupied") == true)
                return;

            animator.Play(weaponItem.heavyAttack);
        }
    }
}
