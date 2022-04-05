using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class PlayerAttacker : MonoBehaviour
    {
        Animator animHandler;

        public void Awake()
        {
            animHandler = GetComponentInChildren<Animator>();

        }
        public void HandleLightAttack(weaponItems weaponItem)
        {
            if (animHandler.GetBool("Combo") == true)
                return;

            if (weaponItem.currattack == weaponItem.lightAttack.Length)
            {
                weaponItem.currattack = 0;
            }
            animHandler.Play(weaponItem.lightAttack[weaponItem.currattack]);

             weaponItem.currattack++;          

        }

        public void HandleHeavyAttack(weaponItems weaponItem)
        {
            animHandler.Play(weaponItem.heavyAttack);
        }
    }
}
