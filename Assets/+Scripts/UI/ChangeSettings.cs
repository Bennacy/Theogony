using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class ChangeSettings : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        public SendSettings sendSettings;
        public CustomSlider audioSlider;
        public CustomDropdown dropdown;
        public CustomToggle audioToggle;
        public int savedAudio;
        private bool previouslyToggled;

        void Start()
        {
            dropdown = GetComponentInChildren<CustomDropdown>();
            audioToggle = GetComponentInChildren<CustomToggle>();

            if(sendSettings){
                audioSlider.SetValue(sendSettings.audioVolume);
                savedAudio = Mathf.RoundToInt(audioSlider.GetValue());
                audioToggle.active = (sendSettings.audioVolume == 0);
            }else{
                globalInfo = GlobalInfo.GetGlobalInfo();
                audioSlider.SetValue(globalInfo.audioVolume);
                savedAudio = Mathf.RoundToInt(audioSlider.GetValue());
                audioToggle.active = (globalInfo.audioVolume == 0);
            }
        }

        void Update()
        {
            if(dropdown.ValueChanged()){
                Debug.Log(dropdown.currentOption);
                QualitySettings.SetQualityLevel(dropdown.currentOption, true);
            }

            int audioVolume = Mathf.RoundToInt(audioSlider.GetValue());
            
            if(audioToggle.active){
                if(audioVolume > 0 && previouslyToggled){
                    audioToggle.active = false;
                }
                if(!previouslyToggled){
                    savedAudio = audioVolume;
                    audioSlider.SetValue(0);
                    if(sendSettings){
                        sendSettings.audioVolume = 0;
                    }else{
                        globalInfo.audioVolume = 0;
                    }
                    previouslyToggled = true;
                }
            }
            if(!audioToggle.active && previouslyToggled){
                audioSlider.SetValue(savedAudio);
            }

            if(audioSlider.GetValue() <= 0.4f){
                if(!previouslyToggled){
                    savedAudio = 10;
                }
                audioSlider.SetValue(0);
                audioToggle.active = true;
            }

            if(audioVolume > 0 && !audioToggle.active){
                if(sendSettings){
                    sendSettings.audioVolume = audioVolume;
                }else{
                    globalInfo.audioVolume = audioVolume;
                }
            }

            AudioListener.volume = audioVolume;

            previouslyToggled = audioToggle.active;
        }
    }
}