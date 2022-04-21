using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    [CreateAssetMenu(menuName = "Items/Weapon Items")]
    public class weaponItems : Items
    {
        public GameObject weaponPrefab;
        public float riposteMultiplier;
        public bool isUnarmed;
        public float knockback;
        public string[] lightAttack;
        public string heavyAttack;
        public int currattack = 0;
        public float baseDamage;
        public float strScaling;
        public float dexScaling;

        public float CalculateDamage(GlobalInfo globalInfo, bool riposte){
            //As of now, each level simply adds 5%*scaling to the total weapon damage
            float damage = baseDamage + ((0.05f * strScaling) * globalInfo.str * baseDamage) + ((0.5f * dexScaling) * globalInfo.dex);
            if(riposte)
                damage *= riposteMultiplier;
            return damage;
        }

        private float PreviousDamage(int level, int scaling){
            if(level == 0){
                return 0;
            }
            float previous = PreviousDamage(level - 1, scaling);
            float finalDamage = previous + ((scaling * .05f) * previous);
            return finalDamage;
        }
    }
}
