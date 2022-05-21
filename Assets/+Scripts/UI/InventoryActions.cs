using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
public class InventoryActions : MonoBehaviour
    {
        public UIController uiController;
        private MenuInfo menuInfo;
        void Start()
        {
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
        }

        void Update()
        {
            
        }

        void OnEnable()
        {
            if(!uiController)
                uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            menuInfo = GetComponent<MenuInfo>();
            foreach(Transform child in transform.parent){
                GameObject obj = child.gameObject;
                if(obj != gameObject && obj.activeSelf){
                    menuInfo.previousMenu = obj;
                }
            }
            Debug.Log("Controller: " + uiController + "\nButton: " + uiController.highlightedBtn);
            transform.position = uiController.highlightedBtn.transform.position;
        }
    }
}
