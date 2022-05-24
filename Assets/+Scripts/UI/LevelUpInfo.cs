using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class LevelUpInfo : MonoBehaviour
    {
        public int totalCost;
        public int totalLevels;
        public int vitLevels;
        public int endLevels;
        public int strLevels;
        public int dexLevels;
        public TextMeshProUGUI[] levelDisplays;
        private UIController uiController;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI confirmationText;
        public GlobalInfo globalInfo;
        public GameObject confirmation;
        public GameObject levelMenu;
        public TextMeshProUGUI healthUpdate;
        public TextMeshProUGUI staminaUpdate;
        public TextMeshProUGUI damageUpdate;
        private bool confirming;
        private PlayerManager playerManager;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            uiController = transform.parent.gameObject.GetComponent<UIController>();
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        }

        void Update()
        {
            for(int i = 0; i < levelDisplays.Length; i++){
                switch(i){
                    case 0:
                        levelDisplays[i].text = "Vitality:\t" + (globalInfo.vit + vitLevels + 1);
                        break;
                    case 1:
                        levelDisplays[i].text = "Endurance:\t" + (globalInfo.end + endLevels + 1);
                        break;
                    case 2:
                        levelDisplays[i].text = "Strength:\t" + (globalInfo.str + strLevels + 1);
                        break;
                    case 3:
                        levelDisplays[i].text = "Dexterity:\t" + (globalInfo.dex + dexLevels + 1);
                        break;
                }
            }

            costText.text = "Total cost:\t" + totalCost;
            if(confirming){
                confirmationText.text = "Spend " + totalCost + " Drachma\nto level up?";
            }else{
                confirmationText.text = "Cancel level up?";
            }

            healthUpdate.text = "Health:  " + Mathf.Round(playerManager.maxHealth) + "     -     " + Mathf.Round(globalInfo.healthIncrease.Evaluate((vitLevels + globalInfo.vit) * .01f) * 10000);
            staminaUpdate.text = "Stamina:  " + Mathf.Round(playerManager.maxStamina) + "     -     " + Mathf.Round(globalInfo.staminaIncrease.Evaluate((endLevels + globalInfo.end) * .01f) * 1000);

            weaponItems weapon = playerManager.GetComponent<PlayerInventory>().rightWeapon;
            int currentDamage = Mathf.RoundToInt(weapon.CalculateDamage(globalInfo, false));
            float strBonus = weapon.baseDamage * (weapon.strScaling * .25f) * (globalInfo.strIncrease.Evaluate((strLevels + globalInfo.str) * .01f));
            float dexBonus = weapon.baseDamage * (weapon.dexScaling * .25f) * (globalInfo.dexIncrease.Evaluate((dexLevels + globalInfo.dex) * .01f));
            damageUpdate.text = "Damage:  " + currentDamage + "     -     " + Mathf.RoundToInt(weapon.baseDamage + strBonus + dexBonus);
        }

        public int GetUpgradeCost(int levels){
            int finalCost = 0;
            for(int i = 1; i < levels+1; i++){
                finalCost += Mathf.RoundToInt(globalInfo.levelCost.Evaluate((globalInfo.totalLevel + levels) * .01f) * 1000);
            }
            return finalCost;

            // if(levels == 1){
            //     return globalInfo.LevelUpCost();
            // }
            // if(levels == 0){
            //     return 0;
            // }else{
            //     int finalCost = GetUpgradeCost(levels - 1);
            //     finalCost += (globalInfo.baseLevelCost + globalInfo.levelCostScaling * (levels - 1 + globalInfo.totalLevel));
            //     return finalCost;
            // }
        }

        public void TempIncrease(int stat){
            int sign = (int)Mathf.Sign(stat);

            if(GetUpgradeCost(totalLevels + sign) > globalInfo.currency){
                return;
            }

            stat = Mathf.Abs(stat);
            switch(stat){
                case 1:
                    if(vitLevels + sign >= 0 && vitLevels + sign + globalInfo.vit <= 98){
                        vitLevels += sign;
                    }
                    break;
                case 2:
                    if(endLevels + sign >= 0 && endLevels + sign + globalInfo.end <= 98){
                        endLevels += sign;
                    }
                    break;
                case 3:
                    if(strLevels + sign >= 0 && strLevels + sign + globalInfo.str <= 98){
                        strLevels += sign;
                    }
                    break;
                case 4:
                    if(dexLevels + sign >= 0 && dexLevels + sign + globalInfo.dex <= 98){
                        dexLevels += sign;
                    }
                    break;
            }
            totalLevels = vitLevels + endLevels + strLevels + dexLevels;
            totalCost = GetUpgradeCost(totalLevels);
        }

        public void OpenConfirmation(){
            uiController.OpenMenu(confirmation);
            confirming = true;
        }

        public void ConfirmationYes(){
            GameObject menuSelection = transform.Find("RestMenuSelection").gameObject;

            if(confirming){
                globalInfo.AlterCurrency(-totalCost);
                globalInfo.AlterVit(vitLevels);
                globalInfo.AlterEnd(endLevels);
                globalInfo.AlterStr(strLevels);
                globalInfo.AlterDex(dexLevels);
                vitLevels = endLevels = strLevels = dexLevels = totalLevels = totalCost = 0;
                confirmation.SetActive(false);
                levelMenu.SetActive(false);
                confirming = false;
            }else{
                confirmation.SetActive(false);
                levelMenu.SetActive(false);
                vitLevels = endLevels = strLevels = dexLevels = totalLevels = totalCost = 0;
            }

            uiController.OpenMenu(menuSelection);
        }

        public void ConfirmationNo(){
            uiController.OpenMenu(levelMenu);
        }
    }
}