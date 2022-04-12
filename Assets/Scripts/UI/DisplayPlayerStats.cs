using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class DisplayPlayerStats : MonoBehaviour
    {
        public TextMeshProUGUI statText;
        public GlobalInfo globalInfo;
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
        }


        void Update()
        {
            string finalString = "";
            finalString += "Vitality:\t\t" + globalInfo.vit+1;
            finalString += "\nEndurance:\t\t" + globalInfo.end+1;
            finalString += "\nStrength:\t\t" + globalInfo.str+1;
            finalString += "\nDexterity:\t\t" + globalInfo.dex+1;

            statText.text = finalString;
        }
    }
}