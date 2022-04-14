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
        }

        void Update()
        {
            
        }

        void OnEnable()
        {
            menuInfo = GetComponent<MenuInfo>();
            foreach(Transform child in transform.parent){
                GameObject obj = child.gameObject;
                if(obj != gameObject && obj.activeSelf){
                    menuInfo.previousMenu = obj;
                }
            }
            transform.position = uiController.highlightedBtn.transform.position;
        }
    }
}
