using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class DisplayPlayerStats : MonoBehaviour
    {
        public TextMeshProUGUI statText;
        public TextMeshProUGUI[] levelDisplays;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI staminaText;
        public TextMeshProUGUI damageText;
        public GlobalInfo globalInfo;
        private PlayerManager playerManager;
        private PlayerInventory playerInventory;
        private weaponItems weapon;
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            playerInventory = playerManager.GetComponent<PlayerInventory>();
        }


        void Update()
        {
            string finalString = "";
            finalString += "Vitality:\t\t" + (globalInfo.vit+1).ToString();
            finalString += "\nEndurance:\t\t" + (globalInfo.end+1).ToString();
            finalString += "\nStrength:\t\t" + (globalInfo.str+1).ToString();
            finalString += "\nDexterity:\t\t" + (globalInfo.dex+1).ToString();

            for(int i = 0; i < levelDisplays.Length; i++){
                switch(i){
                    case 0:
                        levelDisplays[i].text = "Vitality:\t\t" + (globalInfo.vit + 1);
                        break;
                    case 1:
                        levelDisplays[i].text = "Endurance:\t\t" + (globalInfo.end + 1);
                        break;
                    case 2:
                        levelDisplays[i].text = "Strength:\t\t" + (globalInfo.str + 1);
                        break;
                    case 3:
                        levelDisplays[i].text = "Dexterity:\t\t" + (globalInfo.dex + 1);
                        break;
                }
            }

            weapon = playerInventory.rightWeapon;
            healthText.text = "Max Health:\t\t" + Mathf.Round(playerManager.maxHealth);
            staminaText.text = "Max Stamina:\t" + Mathf.Round(playerManager.maxStamina);
            int currentDamage = Mathf.RoundToInt(weapon.CalculateDamage(globalInfo, false));
            damageText.text = "Damage:\t" + currentDamage;
        }
    }
}