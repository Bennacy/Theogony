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
        public int currentOption;
        private int previousIndex;
        public Image arrowImage;
        public Sprite[] arrowSprites;
        public Button parentButton;

        void Start()
        {
            dropdownOpen = false;
            globalInfo = GlobalInfo.GetGlobalInfo();
            options = optionHolder.GetComponentsInChildren<Button>();
            optionString = options[currentOption].gameObject.name;
            foreach (Button option in options){
                option.GetComponentInChildren<TextMeshProUGUI>().text = option.gameObject.name;
            }
            optionText.text = optionString;
            uIController = GetComponentInParent<UIController>();
            parentButton.onClick.AddListener(delegate{ToggleDropdown();});
            arrowImage.sprite = arrowSprites[0];
        }

        void Update()
        {
            
        }

        public void ToggleDropdown(){
            dropdownOpen = !optionHolder.activeSelf;
            MenuInfo optionMenu = optionHolder.GetComponent<MenuInfo>();
            if(dropdownOpen){
                arrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                uIController.OpenMenu(optionHolder);
                optionMenu.currIndex = currentOption;
            }else{
                arrowImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                uIController.OpenMenu(optionMenu.previousMenu);
            }
        }

        public void UpdateOption(int newIndex){
            previousIndex = -1;
            currentOption = newIndex;
            optionString = options[currentOption].gameObject.name;
            optionText.text = optionString;
            ToggleDropdown();
        }

        public bool ValueChanged(){
            bool changed = (previousIndex != currentOption);
            previousIndex = currentOption;
            return changed;
        }
    }



    
}