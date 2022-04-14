using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class Checkpoint : MonoBehaviour
    {
        private  GlobalInfo globalInfo;
        public UIController uiController;
        public Sprite destinationPreview;
        private PlayerControllerScript playerControllerScript;
        public string locationName;
        public Vector3 teleportPosition;
        public int sortingOrder;
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            teleportPosition = transform.position;
            teleportPosition.x += 2;
        }

        public void Rest(){
            playerControllerScript.animator.Play("SitDown");
            playerControllerScript.canMove = false;
            Vector3 checkpointDirection = (playerControllerScript.transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(checkpointDirection, Vector3.forward, Vector3.up);
            Quaternion rotation = Quaternion.Euler(0, angle + 180, 0);
            playerControllerScript.transform.LookAt(transform.position, Vector3.up);
            
            Vector3 forward = rotation * Vector3.forward;
            playerControllerScript.rb.velocity = -playerControllerScript.walkSpeed / 4 * forward;
            uiController.ToggleRest();
            globalInfo.lastCheckpoint = this;
            if(globalInfo.checkpoints[sortingOrder] == null){
                UnlockCheckpoint();
            }
        }
        private void UnlockCheckpoint(){
            globalInfo.checkpoints[sortingOrder] = this;
        }
    }
}