using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Theogony{
    public class MainMenuController : MonoBehaviour
    {
        public Button highlightedBtn;
        public int buttonIndex;
        public MenuInfo menuInfo;
        public Button[] menuButtons;
        
        public bool overSlider;
        public int sliderChange;
        public bool holdingNavigation;
        public bool firstHold;
        
        public float firstHoldTime;
        public float fastHoldTime;
        public float holdTimer;
        public Vector2 navigationValue;
        public Sprite[] buttonSprites;
        public Color[] buttonTextColors;
        public Vector3[] buttonScale;
        void Start()
        {
            buttonSprites = menuInfo.buttonSprites;
            buttonTextColors = menuInfo.buttonTextColors;
            GetButtons();
        }

        void Update()
        {
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

        public void MouseOver(Button button){
            highlightedBtn = button;
            for(int i = 0; i < menuButtons.Length; i++){
                if(menuButtons[i] == highlightedBtn){
                    buttonIndex = i;
                    return;
                }
            }
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

        public void StartGame(){
            SceneManager.LoadScene("NewStarter");
        }

        public void Quit(){
            Application.Quit();
        }

        #region Menu Navigation
        public void Back(InputAction.CallbackContext context){
            if(context.canceled && menuInfo.previousMenu){
                Debug.Log(menuInfo.previousMenu);
                OpenMenu(menuInfo.previousMenu);
            }
        }

        public void Accept(InputAction.CallbackContext context){
            if(context.performed){
                highlightedBtn.onClick.Invoke();
            }
        }

        public void OpenMenu(GameObject menu){
            Debug.Log(menu);
            menu.SetActive(true);
            
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
    }
}