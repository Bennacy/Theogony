using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class Checkpoint : MonoBehaviour
    {
        private  GlobalInfo globalInfo;
        public UIController uiController;
        public Sprite destinationPreview;
        public PlayerControllerScript playerControllerScript;
        public string locationName;
        public string sceneName;
        public bool unlocked;
        public Vector3 teleportPosition;
        public int sortingOrder;
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();

            DontDestroyOnLoad(gameObject);
            if(globalInfo.checkpoints[sortingOrder] == null){
                globalInfo.checkpoints[sortingOrder] = this;
            }else{
                Destroy(gameObject);
            }
            teleportPosition = transform.position;
            teleportPosition.x += 2;
            StartCoroutine(StartFunctions());
        }

        void Update()
        {
            if(globalInfo.refreshedScene){
                StartCoroutine(StartFunctions());
            }
        }

        void OnEnable()
        {
            StartCoroutine(StartFunctions());
        }

        private IEnumerator StartFunctions(){
            yield return new WaitForSeconds(.5f);
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
        }

        public void Sit(){
            playerControllerScript.animator.Play("SitDown");
            playerControllerScript.canMove = false;
            teleportPosition = playerControllerScript.transform.position;
            Vector3 checkpointDirection = (playerControllerScript.transform.position - transform.position).normalized;
            float angle = Vector3.SignedAngle(checkpointDirection, Vector3.forward, Vector3.up);
            Quaternion rotation = Quaternion.Euler(0, angle + 180, 0);
            playerControllerScript.transform.LookAt(transform.position, Vector3.up);
            
            Vector3 forward = rotation * Vector3.forward;
            playerControllerScript.rb.velocity = -playerControllerScript.walkSpeed / 4 * forward;
            uiController.ToggleRest();
            globalInfo.lastCheckpoint = this;
            if(!globalInfo.checkpoints[sortingOrder].unlocked){
                UnlockCheckpoint();
            }
        }

        public void Rest(){
            globalInfo.ReloadLevel(sceneName);
        }
        private void UnlockCheckpoint(){
            globalInfo.checkpoints[sortingOrder].unlocked = true;
        }
    }
}