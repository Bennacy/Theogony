using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Theogony{
    public class UIController : MonoBehaviour
    {
        [Header("References")]
        public PlayerInput inputAction;
        public Camera mainCam;
        public PlayerControllerScript playerControllerScript;
        public GameObject pauseBackground;
        public GameObject restBackground;
        public GlobalInfo globalInfo;
        public Button highlightedBtn;
        public MenuInfo[] menus;
        public MenuInfo menuInfo;
        public Button[] menuButtons;
        public int buttonIndex;
        public LayerMask UILayer;
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
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            inputAction = playerControllerScript.gameObject.GetComponent<PlayerInput>();
            mainCam = Camera.main;
            globalInfo = GlobalInfo.GetGlobalInfo();
            pauseBackground.SetActive(paused);
        }

        public void Bruh(Button button){
            highlightedBtn = button;
            for(int i = 0; i < menuButtons.Length; i++){
                if(menuButtons[i] == highlightedBtn){
                    buttonIndex = i;
                    return;
                }
            }
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
            if(context.canceled && !restBackground.activeSelf){
                paused = !paused;
                pauseBackground.SetActive(paused);
                globalInfo.paused = paused;
                if(paused){
                    menuInfo = menus[0];
                    menuInfo.gameObject.SetActive(true);
                    GetButtons();
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

        public void ToggleRest(){
            bool resting = !restBackground.activeSelf;
            paused = resting;
            globalInfo.paused = resting;
            restBackground.SetActive(resting);
            if(resting){
                menuInfo = menus[1];
                menuInfo.gameObject.SetActive(true);
                GetButtons();
                inputAction.SwitchCurrentActionMap("UI");
            }else{
                menuInfo.currIndex = buttonIndex = 0;
                restBackground.SetActive(false);
                inputAction.SwitchCurrentActionMap("InGame");
                playerControllerScript.animator.Play("StandUp");
            }
        }

        public void Rest(){
            globalInfo.lastCheckpoint.Rest();
        }

        private void GetButtons(){
            menuButtons = menuInfo.buttons;
            buttonIndex = menuInfo.currIndex;
            highlightedBtn = menuButtons[buttonIndex];
            foreach(Button button in menuButtons){ //Adds an event that highlights buttons that the mouse hovers
                GameObject current = button.gameObject;
                if(current.GetComponent<EventTrigger>() == null){
                    current.AddComponent<EventTrigger>();
                    EventTrigger trigger = current.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerEnter;
                    entry.callback.AddListener(delegate{Bruh(button);});
                    trigger.triggers.Add(entry);
                }
            }
        }

        #region Menu Navigation
        // public void SwapTab(InputAction.CallbackContext context){
        //     if(context.performed){
        //         Vector2 value = context.ReadValue<Vector2>();
        //         if(value.x > 0){
        //             Debug.Log("Right");
        //         }else{
        //             Debug.Log("Left");
        //         }
        //     }
        // }

        public void Back(InputAction.CallbackContext context){
            if(context.canceled){
                if(menuInfo.previousMenu == null){ //If this action returns to the game
                    if(pauseBackground.activeSelf){ //If the pause menu is open
                        menuInfo.currIndex = buttonIndex = 0;
                        pauseBackground.SetActive(false);
                        globalInfo.paused = false;
                        paused = false;
                        inputAction.SwitchCurrentActionMap("InGame");
                    }else if(restBackground.activeSelf){ //If the checkpoint menu is active
                        menuInfo.currIndex = buttonIndex = 0;
                        restBackground.SetActive(false);
                        globalInfo.paused = paused = false;
                        inputAction.SwitchCurrentActionMap("InGame");
                        playerControllerScript.animator.Play("StandUp");
                    }
                }else{ //If this action opens the previous menu
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
            menuInfo.gameObject.SetActive(false);
            if(!menu.GetComponent<MenuInfo>().saveIndex){
                menuInfo.currIndex = 0;
            }
            menuInfo = menu.GetComponent<MenuInfo>();
            if(!menuInfo.closesPrevious){
                menuInfo.previousMenu.gameObject.SetActive(true);
            }
            GetButtons();
        }

        public void GetMenuButtons(){
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
        #endregion

        public void IncreaseLevel(int statIndex){
            switch(statIndex){
                case 0: //Vitality
                    globalInfo.AlterVit(1);
                    break;
                
                case 1: //Endurance
                    globalInfo.AlterEnd(1);
                    break;
                
                case 2: //Strength
                    globalInfo.AlterStr(1);
                    break;
                
                case 3: //Dexterity
                    globalInfo.AlterDex(1);
                    break;
            }
        }
        public void DecreaseLevel(int statIndex){
            switch(statIndex){
                case 0: //Vitality
                    globalInfo.AlterVit(-1);
                    break;
                
                case 1: //Endurance
                    globalInfo.AlterEnd(-1);
                    break;
                
                case 2: //Strength
                    globalInfo.AlterStr(-1);
                    break;
                
                case 3: //Dexterity
                    globalInfo.AlterDex(-1);
                    break;
            }
        }
    }
}