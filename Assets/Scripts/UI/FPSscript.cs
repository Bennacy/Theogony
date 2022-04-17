using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSscript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log((int)(1.0f / Time.smoothDeltaTime));
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());        
    }
}
