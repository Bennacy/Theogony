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

        public void LoadWeapons(){
            rightWeapon = globalInfo.collectedWeaponsR[globalInfo.currentWeaponR];
            leftWeapon = globalInfo.collectedWeaponsL[globalInfo.currentWeaponL];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        void Update()
        {
            // if(Input.GetKeyDown(KeyCode.RightArrow)){
            //     globalInfo.currentWeaponR++;
            //     LoadWeapons();
            // }
            // if(Input.GetKeyDown(KeyCode.LeftArrow)){
            //     globalInfo.currentWeaponR--;
            //     LoadWeapons();
            // }
        }

        public void PickUp(weaponItems weapon, bool isLeft){
            if(isLeft){
                foreach(weaponItems testing in globalInfo.collectedWeaponsL){
                    if(testing.itemName == weapon.itemName){
                        return;
                    }
                }
                globalInfo.collectedWeaponsL.Add(weapon);
            }else{
                foreach(weaponItems testing in globalInfo.collectedWeaponsR){
                    if(testing.itemName == weapon.itemName){
                        return;
                    }
                }
                globalInfo.collectedWeaponsR.Add(weapon);
            }

            GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<ItemFade>().NewItem(weapon);
        }
    }
}

