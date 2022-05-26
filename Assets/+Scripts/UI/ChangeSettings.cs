using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class ChangeSettings : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        public CustomSlider audioSlider;
        public CustomDropdown dropdown;
        public CustomToggle audioToggle;
        public string[] qualityNames;
        public int savedAudio;
        private bool previouslyToggled;

        void Start()
        {
            dropdown = GetComponentInChildren<CustomDropdown>();
            audioToggle = GetComponentInChildren<CustomToggle>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            qualityNames = QualitySettings.names;
            audioSlider.SetValue(globalInfo.audioVolume);
            savedAudio = Mathf.RoundToInt(audioSlider.GetValue());
            audioToggle.active = (globalInfo.audioVolume == 0);
        }

        void Update()
        {
            if(dropdown.ValueChanged()){
                Debug.Log(dropdown.currentOption);
                QualitySettings.SetQualityLevel(dropdown.currentOption, true);
            }

            int audioVolume = Mathf.RoundToInt(audioSlider.GetValue());
            
            // if(!audioToggle.active){
            //     audioSlider.SetValue(0);
            // }else{
            //     audioSlider.SetValue(savedAudio);
            // }
            if(audioToggle.active){
                if(audioVolume > 0 && previouslyToggled){
                    audioToggle.active = false;
                }
                if(!previouslyToggled){
                    savedAudio = audioVolume;
                    audioSlider.SetValue(0);
                    globalInfo.audioVolume = 0;
                }
            }
            if(!audioToggle.active && previouslyToggled){
                audioSlider.SetValue(savedAudio);
            }

            // if(audioVolume > 0 && audioToggle.active && !previouslyToggled){
            // }

            if(audioSlider.GetValue() <= 0.4f){
                audioSlider.SetValue(0);
                audioToggle.active = true;
            }

            if(audioVolume > 0 && !audioToggle.active){
                globalInfo.audioVolume = audioVolume;
            }

            // globalInfo.audioVolume = audioVolume;
            // audioToggle.active = (globalInfo.audioVolume == 0);
            // if(globalInfo.audioVolume > 0){
            //     savedAudio = globalInfo.audioVolume;
            // }
            previouslyToggled = audioToggle.active;
        }
    }
}