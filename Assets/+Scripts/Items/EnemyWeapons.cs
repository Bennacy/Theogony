using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName ="Items/Enemy Weapon")]
    public class EnemyWeapons : Items
    {
        public GameObject weaponPrefab;
        public AudioClip impactClip;
        public float knockback;
        public string[] possibleAttacks;
        public int[] attackWeights;
        public float damageDealt;
        public weaponItems droppedWeapon;
    }
}