using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    [CreateAssetMenu(menuName = "Items/Boss Weapon")]
    public class BossWeapon : Items
    {
        public GameObject weaponPrefab;
        public float knockback;
        public string[] phase1Attacks;
        public int[] phase1Weights;

        public string[] phase2Attacks;
        public int[] phase2Weights;

        public float damageDealt;
        public weaponItems playerWeapon;
    }
}
