using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Theogony{
    public class PauseScreen : MonoBehaviour
    {
        [Header("References")]
        public PlayerInput inputAction;
        public GameObject backgroundImg;
        public GlobalInfo globalInfo;
        public Text menuHeader;
        public Image[] menuTabs;
        public GridControl[] menuWindows;
        public Button highlightedBtn;
        [Space]

        [Space]
        [Header("Values")]
        public int menuIndex;
        public Color unselectedC;
        public Color selectedC;
        [Space]

        [Space]
        [Header("Booleans")]
        public bool paused;
        
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            backgroundImg.SetActive(paused);
            unselectedC = new Color(1, 1, 1, .75f);
            selectedC = new Color(.75f, .75f, .75f, .75f);
        }

        void Update()
        {
            if(paused){
                for(int i = 0; i < menuTabs.Length; i++){
                    menuTabs[i].color = unselectedC;
                    menuWindows[i].gameObject.SetActive(false);
                }
                menuWindows[menuIndex].gameObject.SetActive(true);
                menuTabs[menuIndex].color = selectedC;

                switch(menuIndex){
                    case 0:
                        break;
                    
                    case 1:
                        break;
                    
                    case 2:
                        break;
                    
                    case 3:
                        break;
                }
            }
        }

        public void TogglePause(InputAction.CallbackContext context){
            if(context.performed){
                paused = !paused;
                Debug.Log(paused);
                // input.enabled = paused;
                backgroundImg.SetActive(paused);
                globalInfo.paused = paused;
                if(paused){
                    inputAction.SwitchCurrentActionMap("UI");
                }else{
                    inputAction.SwitchCurrentActionMap("InGame");
                }
            }
        }

        public void Back(InputAction.CallbackContext context){
            if(context.canceled){
                backgroundImg.SetActive(false);
                globalInfo.paused = false;
                paused = false;
                inputAction.SwitchCurrentActionMap("InGame");
            }
        }

        public void Accept(InputAction.CallbackContext context){
            if(context.performed){
                highlightedBtn.onClick.Invoke();
            }
        }

        public void NavigateMenu(InputAction.CallbackContext context){
            Vector2 navigate = context.ReadValue<Vector2>();
            if(context.performed){
                Debug.Log(navigate);
                if(navigate.x == -1){
                    menuWindows[menuIndex].PrevCol();
                }else if(navigate.x == 1){
                    menuWindows[menuIndex].NextCol();
                }else if(navigate.y == -1){
                    menuWindows[menuIndex].PrevRow();
                }else if(navigate.y == 1){
                    menuWindows[menuIndex].NextRow();
                }
            }
        }

        public void SwapMenu(InputAction.CallbackContext context){
            if(context.performed && paused){
                string name = context.action.name;
                if(name.Contains("Left")){
                    menuIndex--;
                    if(menuIndex < 0){
                        menuIndex = 3;
                    }
                }else{
                    menuIndex++;
                    if(menuIndex > 3){
                        menuIndex = 0;
                    }
                }
            }
        }
    }
}