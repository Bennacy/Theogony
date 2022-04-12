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
        public Button highlightedBtn;
        public MenuInfo menuInfo;
        public Button[] menuButtons;
        public int buttonIndex;
        [Space]

        [Space]
        [Header("Values")]
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
            // unselectedC = new Color(1, 1, 1, .75f);
            // selectedC = new Color(.75f, .75f, .75f, .75f);
            menuButtons = menuInfo.buttons;
            highlightedBtn = menuButtons[buttonIndex];
            menuInfo.gameObject.SetActive(true);
        }

        void Update()
        {
            if(paused){
                foreach(Button button in menuButtons){
                    if(button != highlightedBtn){
                        button.image.color = unselectedC;
                    }else{
                        button.image.color = selectedC;
                    }
                }
            }
        }

        public void TogglePause(InputAction.CallbackContext context){
            if(context.canceled){
                paused = !paused;
                backgroundImg.SetActive(paused);
                globalInfo.paused = paused;
                if(paused){
                    highlightedBtn = menuButtons[buttonIndex];
                    inputAction.SwitchCurrentActionMap("UI");
                }else{
                    inputAction.SwitchCurrentActionMap("InGame");
                    while(menuInfo.previousMenu != null){
                        menuInfo.currIndex = buttonIndex = 0;
                        OpenMenu(menuInfo.previousMenu);
                    }
                }
            }
        }

        public void SwapTab(InputAction.CallbackContext context){
            if(context.performed){
                Vector2 value = context.ReadValue<Vector2>();
                if(value.x > 0){
                    Debug.Log("Right");
                }else{
                    Debug.Log("Left");
                }
            }
        }

        public void Back(InputAction.CallbackContext context){
            if(context.canceled){
                if(menuInfo.previousMenu == null){
                    menuInfo.currIndex = buttonIndex = 0;
                    backgroundImg.SetActive(false);
                    globalInfo.paused = false;
                    paused = false;
                    inputAction.SwitchCurrentActionMap("InGame");
                }else{
                    OpenMenu(menuInfo.previousMenu);
                }
            }
        }

        public void Accept(InputAction.CallbackContext context){
            if(context.performed){
                highlightedBtn.onClick.Invoke();
            }
        }

        public void OpenMenu(GameObject menu){
            menu.SetActive(true);
            // highlightedBtn = menuInfo.gameObject.GetComponent<Button>();
            menuInfo.gameObject.SetActive(false);
            if(!menu.GetComponent<MenuInfo>().saveIndex){
                menuInfo.currIndex = 0;
            }
            menuInfo = menu.GetComponent<MenuInfo>();
            if(!menuInfo.closesPrevious){
                menuInfo.previousMenu.gameObject.SetActive(true);
            }
            menuButtons = menuInfo.buttons;
            buttonIndex = menuInfo.currIndex;
            highlightedBtn = menuButtons[menuInfo.currIndex];
        }

        public void NavigateMenu(InputAction.CallbackContext context){
            if(context.performed){
                Vector2 value = context.ReadValue<Vector2>();
                if(value.x == 1){
                    buttonIndex++;
                    if(buttonIndex >= menuButtons.Length){
                        buttonIndex = 0;
                    }
                }else if(value.x == -1){
                    buttonIndex--;
                    if(buttonIndex < 0){
                        buttonIndex = menuButtons.Length - 1;
                    }
                }else if(value.y == -1){
                    int futureIndex = buttonIndex + menuInfo.rowSize;
                    if(futureIndex >= menuButtons.Length){
                        futureIndex = futureIndex - menuButtons.Length;
                    }
                    buttonIndex = futureIndex;
                }else if(value.y == 1){
                    int futureIndex = buttonIndex - menuInfo.rowSize;
                    if(futureIndex < 0){
                        futureIndex = futureIndex + menuButtons.Length;
                    }
                    buttonIndex = futureIndex;
                }
                menuInfo.currIndex = buttonIndex;
                highlightedBtn = menuButtons[buttonIndex];
            }
        }
    }
}