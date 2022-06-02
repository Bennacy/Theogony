using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    [CreateAssetMenu(menuName = "Items/Weapon Items")]
    public class weaponItems : Items
    {
        public bool twoHanded;
        public GameObject weaponPrefab;
        public Sprite icon;
        public AudioClip equipClip;
        public AudioClip swingClip;
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
            float strBonus = baseDamage * (strScaling * .25f) * (globalInfo.strIncrease.Evaluate(globalInfo.str * .01f));
            float dexBonus = baseDamage * (dexScaling * .25f) * (globalInfo.dexIncrease.Evaluate(globalInfo.dex * .01f));

            float damage = baseDamage + strBonus + dexBonus;
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
