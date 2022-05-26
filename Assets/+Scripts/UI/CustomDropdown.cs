using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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

        [HideInInspector]
        public GameObject optionTemplate;

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



    
    // [CustomEditor(typeof(CustomDropdown))]
    // [CanEditMultipleObjects]
    // [ExecuteInEditMode]
    // public class CustomDropdownEditor : Editor{
    //     CustomDropdown dropdown;
    //     bool pressedCreate;
    //     string optionName = "Option name";

    //     public override void OnInspectorGUI()
    //     {
    //         dropdown = (CustomDropdown)target;
    //         string addOptionText = "Add Option";
    //         if(pressedCreate){
    //             addOptionText = "Close";
    //         }

    //         if(GUILayout.Button(addOptionText)){
    //             pressedCreate = !pressedCreate;
    //         }

    //         if(pressedCreate){
    //             GUILayout.BeginHorizontal();
    //             optionName = GUILayout.TextField(optionName);
    //             if(GUILayout.Button("Create")){
    //                 GameObject newOption = Instantiate(dropdown.optionTemplate, dropdown.optionHolder.transform);
    //                 newOption.name = optionName;
    //                 newOption.GetComponentInChildren<TextMeshProUGUI>().text = newOption.name;
    //                 int optionIndex = dropdown.optionHolder.transform.childCount;
    //                 newOption.GetComponent<Button>().onClick.AddListener(delegate{dropdown.UpdateOption(optionIndex - 1);});
    //                 pressedCreate = false;
    //                 optionName = "Option name";
    //             }
    //             GUILayout.EndHorizontal();
    //         }

    //         DrawDefaultInspector();
    //     }
    // }
}