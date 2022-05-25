using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class SetQuality : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        public CustomDropdown dropdown;
        public string[] qualityNames;

        void Start()
        {
            dropdown = GetComponentInChildren<CustomDropdown>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            qualityNames = QualitySettings.names;
        }

        void Update()
        {
            if(dropdown.ValueChanged()){
                Debug.Log(dropdown.currentOption);
                QualitySettings.SetQualityLevel(dropdown.currentOption, true);
            }
        }
    }
}