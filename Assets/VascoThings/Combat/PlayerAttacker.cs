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
            animHandler.Play(weaponItem.lightAttack[0]);
            Debug.Log("light attack");
        }

        public void HandleHeavyAttack(weaponItems weaponItem)
        {
            animHandler.Play(weaponItem.heavyAttack);
        }
    }
}
