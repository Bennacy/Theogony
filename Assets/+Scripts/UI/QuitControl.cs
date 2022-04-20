using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuitControl : MonoBehaviour
{
    public Button quitBtn;
    public Button confirm;
    public Button cancel;
    public GameObject confirmationBackground;
    
    void Start()
    {

        quitBtn.onClick.AddListener(delegate{ToggleQuit();});
    }

    void Update()
    {
        
    }

    public void ToggleQuit(){
        Debug.Log("A");
    }
}
