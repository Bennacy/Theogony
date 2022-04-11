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
        public GridControl[] menuWindows;
        public Button highlightedBtn;
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
            unselectedC = new Color(1, 1, 1, .75f);
            selectedC = new Color(.75f, .75f, .75f, .75f);
        }

        void Update()
        {
            if(paused){
                
            }
        }

        public void TogglePause(InputAction.CallbackContext context){
            if(context.canceled){
                paused = !paused;
                backgroundImg.SetActive(paused);
                globalInfo.paused = paused;
                if(paused){
                    inputAction.SwitchCurrentActionMap("UI");
                }else{
                    inputAction.SwitchCurrentActionMap("InGame");
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
    }
}