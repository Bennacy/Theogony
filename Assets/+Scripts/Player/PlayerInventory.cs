using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony { 
public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;
        GlobalInfo globalInfo;
        public weaponItems rightWeapon;
        public weaponItems leftWeapon;

        private void Awake()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            LoadWeapons();
        }

        private void LoadWeapons(){
            rightWeapon = globalInfo.collectedWeaponsR[globalInfo.currentWeaponR];
            leftWeapon = globalInfo.collectedWeaponsL[globalInfo.currentWeaponL];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                globalInfo.currentWeaponR++;
                LoadWeapons();
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                globalInfo.currentWeaponR--;
                LoadWeapons();
            }
        }

        public void PickUp(weaponItems weapon, bool isLeft){
            if(isLeft){
                globalInfo.collectedWeaponsL.Add(weapon);
            }else{
                globalInfo.collectedWeaponsR.Add(weapon);
            }
        }
    }
}

