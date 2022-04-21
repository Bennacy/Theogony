using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyWeaponManager : MonoBehaviour
    {
        public Transform parent;
        public EnemyWeapons weaponTemplate;
        public BossWeapon bossweaponTemplate;
        public Vector3 positionAdjustment;
        public Quaternion angleAdjustment;
        
        void Awake()
        {
            GameObject weapon = null;

            if (weaponTemplate)
            {
                 weapon = Instantiate(weaponTemplate.weaponPrefab) as GameObject;
                weapon.tag = "EnemyWeapon";
            }
            else if (bossweaponTemplate)
            {
                 weapon = Instantiate(bossweaponTemplate.weaponPrefab) as GameObject;
                 weapon.tag = "BossWeapon";
            }


            if (weapon){
                if(parent){
                    weapon.transform.parent = parent;
                }
            }

            weapon.transform.localPosition = positionAdjustment;
            weapon.transform.localRotation = angleAdjustment;
            
        }
    }
}