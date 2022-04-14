using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class Interactable : MonoBehaviour
    {
        public PlayerControllerScript playerControllerScript;
        public PlayerInventory playerInventory;
        [Tooltip("0 - Pick up\n1 - Open\n2 - Rest")]
        public int action;
        
        void Start()
        {
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            playerInventory = playerControllerScript.gameObject.GetComponent<PlayerInventory>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Interact(){
            switch(action){
                case 0:
                    Debug.Log("Pick up");
                    Destroy(gameObject);
                    break;
                case 1:

                    break;
                case 2:
                    break;
            }
        }
    }
}