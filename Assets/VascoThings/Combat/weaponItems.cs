using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    [CreateAssetMenu(menuName = "Items/Weapon Items")]
    public class weaponItems : Items
    {

        public GameObject weaponPrefab;
        public bool isUnarmed;
        public string[] lightAttack;
        public string heavyAttack;
    }
}
