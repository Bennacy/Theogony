using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class Interactable : MonoBehaviour
    {
        public UIController uiController;
        public float interactRange;
        private GlobalInfo globalInfo;
        public PlayerControllerScript playerControllerScript;
        public PlayerInventory playerInventory;
        [Tooltip("0 - Pick up\n1 - Open\n2 - Rest\n3 - Recover\n4 - Fog wall")]
        public InteractionType action;
        public weaponItems itemGiven;
        public int currencyChange;
        
        void Start()
        {
            gameObject.layer = 7;
            globalInfo = GlobalInfo.GetGlobalInfo();
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            playerInventory = playerControllerScript.gameObject.GetComponent<PlayerInventory>();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
        }

        public void Interact(){
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            if(Vector3.Distance(playerControllerScript.transform.position, transform.position) <= interactRange){
                switch(action){
                    case InteractionType.PickUp:
                        if(!itemGiven){
                            globalInfo.AlterCurrency(currencyChange);
                        }else{
                            playerInventory.PickUp(itemGiven, false);
                        }
                        Destroy(gameObject);
                        break;
                    case InteractionType.Open:

                        break;
                    case InteractionType.Sit:
                        GetComponent<Checkpoint>().Sit();
                        break;
                    case InteractionType.RecoverSouls:
                        globalInfo.RecoverCurrency();
                        break;
                    case InteractionType.BossBarrier:
                        GetComponent<BossBarrier>().Traverse();
                        break;
                }
            }
        }
    }
}