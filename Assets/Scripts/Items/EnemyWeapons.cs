using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName ="Items/Enemy Weapon")]
    public class EnemyWeapons : Items
    {
        public GameObject weaponPrefab;
        public string[] possibleAttacks;
        public int[] attackWeights;
    }
}