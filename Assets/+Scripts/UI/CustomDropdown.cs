using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class CustomDropdown : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        public UIController uIController;
        public string optionString;
        public TextMeshProUGUI optionText;
        public bool dropdownOpen;
        public GameObject optionHolder;
        public Button[] options;
        public int optionIndex;
        public Button parentButton;

        void Start()
        {
            dropdownOpen = false;
            globalInfo = GlobalInfo.GetGlobalInfo();
            options = optionHolder.GetComponentsInChildren<Button>();
            optionString = options[optionIndex].gameObject.name;
            foreach (Button option in options){
                option.GetComponentInChildren<TextMeshProUGUI>().text = option.gameObject.name;
            }
            optionText.text = optionString;
            uIController = GetComponentInParent<UIController>();
            parentButton.onClick.AddListener(delegate{ToggleDropdown();});
        }

        void Update()
        {
            
        }

        public void ToggleDropdown(){
            dropdownOpen = !dropdownOpen;
            MenuInfo optionMenu = optionHolder.GetComponent<MenuInfo>();
            if(dropdownOpen){
                uIController.OpenMenu(optionHolder);
                optionMenu.currIndex = optionIndex;
            }else{
                uIController.OpenMenu(optionMenu.previousMenu);
            }
        }

        public void UpdateOption(int newIndex){
            optionIndex = newIndex;
            optionString = options[optionIndex].gameObject.name;
            optionText.text = optionString;
            ToggleDropdown();
        }
    }
}