using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [Header("References")]
    public GameObject backgroundImg;
    public Image[] menuTabs;
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
        backgroundImg.SetActive(paused);
        unselectedC = new Color();
        unselectedC.r = 0;
        Debug.Log(unselectedC);
        selectedC = new Color(175, 175, 175, 100);
    }

    void Update()
    {
        if(paused){
            foreach(Image tab in menuTabs){
                tab.color = unselectedC;
            }
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
        Debug.Log("Pause " + context.phase);
        paused = !paused;
        backgroundImg.SetActive(paused);
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
