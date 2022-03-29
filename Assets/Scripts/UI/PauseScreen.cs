using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject backgroundImg;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePause(InputAction.CallbackContext context){
        Debug.Log("Pause " + context.phase);
    }
}
