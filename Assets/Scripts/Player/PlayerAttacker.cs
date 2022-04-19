using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerManager playerManager;
        Animator animator;
        public float lightAttackCost;
        public float heavyAttackCost;

        public bool riposteAttack;

        public void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            animator = GetComponentInChildren<Animator>();

        }
        public void HandleLightAttack(weaponItems weaponItem)
        {
            if (riposteAttack){
                animator.Play("Riposte");
                return;
            } 
            
            if (animator.GetBool("Occupied") == true || !playerManager.UpdateStamina(heavyAttackCost))
                return;

            if (weaponItem.currattack == weaponItem.lightAttack.Length)
            {
                weaponItem.currattack = 0;
            }
            
            animator.Play(weaponItem.lightAttack[weaponItem.currattack]);

        }

        public void HandleHeavyAttack(weaponItems weaponItem)
        {
            if (animator.GetBool("Occupied") == true || !playerManager.UpdateStamina(heavyAttackCost))
                return;

            animator.Play(weaponItem.heavyAttack);
        }


        public void HandleParry(weaponItems weaponItem)
        {
           
            if (animator.GetBool("Occupied") == true || !playerManager.UpdateStamina(heavyAttackCost))
                return;

            animator.Play(weaponItem.heavyAttack);
        }

        public void HandleBlock(weaponItems weaponItem)
        {

          
            if (weaponItem.currattack == weaponItem.lightAttack.Length)
            {
                weaponItem.currattack = 0;
            }

            animator.Play(weaponItem.lightAttack[weaponItem.currattack]);
        }


        public void WillRiposte()
        {
            riposteAttack = true;
        }

    }
}
