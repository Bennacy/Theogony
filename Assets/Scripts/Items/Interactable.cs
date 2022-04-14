using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class Interactable : MonoBehaviour
    {
        public UIController uiController;
        public PlayerControllerScript playerControllerScript;
        public PlayerInventory playerInventory;
        [Tooltip("0 - Pick up\n1 - Open\n2 - Rest")]
        public int action;
        
        void Start()
        {
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
                    playerControllerScript.animator.Play("SitDown");
                    playerControllerScript.canMove = false;
                    Vector3 checkpointDirection = (playerControllerScript.transform.position - transform.position).normalized;
                    float angle = Vector3.SignedAngle(checkpointDirection, Vector3.forward, Vector3.up);
                    Quaternion rotation = Quaternion.Euler(0, angle + 180, 0);
                    playerControllerScript.transform.LookAt(transform.position, Vector3.up);
                    // playerControllerScript.transform.rotation = rotation;
                    
                    Vector3 forward = rotation * Vector3.forward;
                    playerControllerScript.rb.velocity = -playerControllerScript.walkSpeed / 4 * forward;
                    uiController.ToggleRest();
                    break;
            }
        }
    }
}