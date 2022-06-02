using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        private Vector3[] buttonScale;
        public int buttonIndex;
        public LayerMask UILayer;
        public UIAudio uIAudio;
        [Space]

        [Space]
        [Header("Values")]
        public Sprite[] buttonSprites;
        public Color[] buttonTextColors;
        public float firstHoldTime;
        public float fastHoldTime;
        public float holdTimer;
        public Vector2 navigationValue;
        [Space]

        [Space]
        [Header("Booleans")]
        public bool paused;
        public bool overSlider;
        public int sliderChange;
        public bool holdingNavigation;
        public bool firstHold;
        
        void Start()
        {
            mainCam = Camera.main;
            globalInfo = GlobalInfo.GetGlobalInfo();
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            inputAction = playerControllerScript.gameObject.GetComponent<PlayerInput>();
            pauseBackground.SetActive(paused);
            uIAudio = GetComponent<UIAudio>();
        }

        public void MouseOver(Button button){
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
                int btnIndex = 0;
                foreach(Button button in menuButtons){
                    TextMeshProUGUI text = null;
                    if(button.GetComponentInChildren<TextMeshProUGUI>()){
                        text = button.GetComponentInChildren<TextMeshProUGUI>();
                    }
                    
                    Image.Type savedType = button.image.type;
                    if(button != highlightedBtn){
                        if(menuInfo.enlargeButtons){
                            button.transform.localScale = buttonScale[btnIndex];
                        }    

                        if(text != null){
                            text.color = buttonTextColors[0];
                        }
                        button.image.sprite = buttonSprites[0];
                    }else{
                        if(menuInfo.enlargeButtons){
                            button.transform.localScale = buttonScale[btnIndex] * 1.15f;
                        }
                        
                        if(text != null){
                            text.color = buttonTextColors[1];
                        }
                        button.image.sprite = buttonSprites[1];
                    }
                    button.image.type = savedType;
                    btnIndex++;
                }
                overSlider = highlightedBtn.name.Contains("Slider");
                if(holdingNavigation){
                    holdTimer += Time.deltaTime;
                    if(firstHold){
                        if(holdTimer > firstHoldTime){
                            holdTimer = 0;
                            firstHold = false;
                            ActualNavigation();
                        }
                    }else{
                        if(holdTimer > fastHoldTime){
                            holdTimer = 0;
                            firstHold = false;
                            ActualNavigation();
                        }
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
                globalInfo.playerTargetable = false;
                menuInfo = menus[1];
                menuInfo.gameObject.SetActive(true);
                GetButtons();
                inputAction.SwitchCurrentActionMap("UI");
            }else{
                globalInfo.playerTargetable = true;
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
            buttonScale = new Vector3[menuButtons.Length];
            int btnIndex = 0;
            foreach(Button button in menuButtons){ //Adds an event that highlights buttons that the mouse hovers
                GameObject current = button.gameObject;
                if(current.GetComponent<EventTrigger>() == null){
                    current.AddComponent<EventTrigger>();
                    EventTrigger trigger = current.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerEnter;
                    entry.callback.AddListener(delegate{MouseOver(button);});
                    trigger.triggers.Add(entry);
                }
                buttonScale[btnIndex] = button.transform.localScale;
                btnIndex++;
            }
        }

        public void Quit(){
            globalInfo.paused = false;
            SceneManager.LoadScene("TitleScreen");
            // Application.Quit();
        }

        #region Menu Navigation
        public void Back(InputAction.CallbackContext context){
            if(context.canceled){
                uIAudio.source.clip = menuInfo.closeClip;
                if(menuInfo.previousMenu == null){ //If this action returns to the game
                    for(int i = 0; i < menuInfo.buttons.Length; i++){
                        menuInfo.buttons[i].transform.localScale = buttonScale[i];
                    }
            
                    if(pauseBackground.activeSelf){ //If the pause menu is open
                        menuInfo.currIndex = buttonIndex = 0;
                        pauseBackground.SetActive(false);
                        globalInfo.paused = false;
                        paused = false;
                        inputAction.SwitchCurrentActionMap("InGame");
                    }else if(restBackground.activeSelf){ //If the checkpoint menu is active
                        menuInfo.currIndex = buttonIndex = 0;
                        globalInfo.playerTargetable = true;
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
                // uIAudio.source.clip = uIAudio.clips[0];
                // uIAudio.source.Play();
                highlightedBtn.onClick.Invoke();
            }
        }

        public void OpenMenu(GameObject menu){
            menu.SetActive(true);
            MenuInfo newMenu = menu.GetComponent<MenuInfo>();
            if(newMenu.previousMenu){
                if(newMenu.previousMenu.activeSelf){
                    uIAudio.source.clip = menu.GetComponent<MenuInfo>().openClip;
                }
            }

            uIAudio.source.Play();
            
            for(int i = 0; i < menuInfo.buttons.Length; i++){
                menuInfo.buttons[i].transform.localScale = buttonScale[i];
            }
            menuInfo.gameObject.SetActive(false);

            if(!menu.GetComponent<MenuInfo>().saveIndex){
                menuInfo.currIndex = 0;
            }
            menuInfo = menu.GetComponent<MenuInfo>();
            buttonSprites = menuInfo.buttonSprites;
            buttonTextColors = menuInfo.buttonTextColors;
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
                navigationValue = context.ReadValue<Vector2>();
                ActualNavigation();
                holdingNavigation = true;
            }else if(context.canceled){
                navigationValue = context.ReadValue<Vector2>();
                holdingNavigation = false;
                firstHold = true;
                holdTimer = 0;
            }
        }

        private void ActualNavigation(){
            uIAudio.source.clip = uIAudio.clips[0];
            uIAudio.source.Play();
                 
            if(overSlider && navigationValue.x != 0){
                sliderChange = (int)Mathf.Sign(navigationValue.x);
            }

            if(navigationValue.x == 1 && !overSlider){
                buttonIndex++;
                if(buttonIndex >= menuButtons.Length){
                    buttonIndex = 0;
                }
            }else if(navigationValue.x == -1 && !overSlider){
                buttonIndex--;
                if(buttonIndex < 0){
                    buttonIndex = menuButtons.Length - 1;
                }
            }else if(navigationValue.y == -1){
                int futureIndex = buttonIndex + menuInfo.rowSize;
                if(futureIndex >= menuButtons.Length){
                    futureIndex = futureIndex - menuButtons.Length;
                }
                buttonIndex = futureIndex;
            }else if(navigationValue.y == 1){
                int futureIndex = buttonIndex - menuInfo.rowSize;
                if(futureIndex < 0){
                    futureIndex = futureIndex + menuButtons.Length;
                }
                buttonIndex = futureIndex;
            }
            menuInfo.currIndex = buttonIndex;
            highlightedBtn = menuButtons[buttonIndex];
        }
        #endregion

        public void ToggleHUD(InputAction.CallbackContext context){
            if(context.performed){
                GameObject hud = transform.Find("HUD").gameObject;
                hud.SetActive(!hud.activeSelf);
            }
        }
    }
}