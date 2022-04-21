using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class Interactable : MonoBehaviour
    {
        public UIController uiController;
        private GlobalInfo globalInfo;
        public PlayerControllerScript playerControllerScript;
        public PlayerInventory playerInventory;
        [Tooltip("0 - Pick up\n1 - Open\n2 - Rest\n3 - Recover\n4 - Fog wall")]
        public int action;
        
        void Start()
        {
            gameObject.layer = 7;
            globalInfo = GlobalInfo.GetGlobalInfo();
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            playerInventory = playerControllerScript.gameObject.GetComponent<PlayerInventory>();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
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
                    GetComponent<Checkpoint>().Sit();
                    break;
                case 3:
                    globalInfo.RecoverCurrency();
                    break;
                case 4:
                    GetComponent<BossBarrier>().Traverse();
                    break;
            }
        }
    }
}