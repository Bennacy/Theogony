using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class ItemFade : MonoBehaviour
    {
        public float fadeSpeed;
        public float holdTime;
        private float holdTimer;
        public TextMeshProUGUI itemName;
        public Image weaponIcon;
        private bool fadingIn;
        private bool holding;
        private bool fadingOut;

        void Start()
        {
            
        }

        void Update()
        {
            if(fadingIn){
                Color nameColor = itemName.color;
                nameColor.a += fadeSpeed * Time.deltaTime;
                itemName.color = nameColor;
                
                Color iconColor = weaponIcon.color;
                iconColor.a += fadeSpeed * Time.deltaTime;
                weaponIcon.color = iconColor;

                if(nameColor.a >= 1){
                    fadingIn = false;
                    holding = true;
                }
            }

            if(holding){
                holdTimer += Time.deltaTime;
                if(holdTimer >= holdTime){
                    holding = false;
                    holdTimer = 0;
                    fadingOut = true;
                }
            }

            if(fadingOut){
                Color nameColor = itemName.color;
                nameColor.a -= fadeSpeed * Time.deltaTime;
                itemName.color = nameColor;
                
                Color iconColor = weaponIcon.color;
                iconColor.a -= fadeSpeed * Time.deltaTime;
                weaponIcon.color = iconColor;

                if(nameColor.a <= 0){
                    fadingOut = false;
                }
            }
        }

        public void NewItem(weaponItems weapon){
            itemName.text = "\t" + weapon.itemName;
            weaponIcon.sprite = weapon.icon;

            fadingIn = true;
        }
    }
}