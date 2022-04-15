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
        private bool confirming;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            uiController = transform.parent.gameObject.GetComponent<UIController>();
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
                confirmationText.text = "Spend " + totalCost + " currency\nto level up?";
            }else{
                confirmationText.text = "Cancel level up?";
            }
        }

        public int GetUpgradeCost(int levels){
            if(levels == 1){
                return globalInfo.LevelUpCost();
            }else if(levels == 0){
                return 0;
            }else{
                int finalCost = GetUpgradeCost(levels - 1);
                finalCost += (globalInfo.baseLevelCost + globalInfo.levelCostScaling * (levels - 1 + globalInfo.totalLevel));
                return finalCost;
            }
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
            GameObject menuSelection = transform.Find("MenuSelection").gameObject;

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