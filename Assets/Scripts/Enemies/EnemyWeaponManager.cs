using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyWeaponManager : MonoBehaviour
    {
        public Transform parent;
        public EnemyWeapons weaponTemplate;
        public Vector3 positionAdjustment;
        public Quaternion angleAdjustment;
        
        void Awake()
        {
            GameObject weapon = Instantiate(weaponTemplate.weaponPrefab) as GameObject;
            if(weapon){
                if(parent){
                    weapon.transform.parent = parent;
                }
            }
            weapon.transform.localPosition = positionAdjustment;
            weapon.transform.localRotation = angleAdjustment;
            weapon.tag = "EnemyWeapon";
        }
    }
}