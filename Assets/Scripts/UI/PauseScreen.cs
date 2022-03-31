using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [Header("References")]
    public PlayerInput input;
    public GameObject backgroundImg;
    public GlobalInfo globalInfo;
    public Text menuHeader;
    public Image[] menuTabs;
    public GameObject[] menuWindows;
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
                menuWindows[i].SetActive(false);
            }
            menuWindows[menuIndex].SetActive(true);
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
        paused = !paused;
        // input.enabled = paused;
        backgroundImg.SetActive(paused);
        globalInfo.paused = paused;
    }

    public void Back(InputAction.CallbackContext context){
        if(context.canceled){
            backgroundImg.SetActive(false);
            globalInfo.paused = false;
            paused = false;
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
