using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Theogony{
    public class GlobalInfo : MonoBehaviour
    {
        [Header("References")]
        public static GlobalInfo self;
        public PlayerControllerScript playerControllerScript;
        public UpdateBar health;
        public UpdateBar stamina;
        private UIController uiController;
        public string activeScene;
        [Space]

        [Space]
        [Header("Values")]
        public int currency;
        public Checkpoint lastCheckpoint;
        public Checkpoint[] checkpoints;
        [Space]

        [Space]
        [Header("Player Stats Info")]
        public int totalLevel;
        public int baseLevelCost;
        public int levelCostScaling;
        public int vit;
        public int end;
        public int str;
        public int dex;
        public float vitIncrease;
        public float baseHealth;
        public float endIncrease;
        public float baseStamina;
        [Space]

        [Space]
        [Header("Booleans")]
        public bool paused;
        public bool refreshedScene;
        public bool reloading;
        
        public static GlobalInfo GetGlobalInfo(){
            return(GameObject.FindGameObjectWithTag("GlobalInfo").GetComponent<GlobalInfo>());
        }
        
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            if(self == null){
                self = this;
            }else{
                Destroy(gameObject);
            }
            StartCoroutine(StartFunctions());
        }

        private IEnumerator StartFunctions(){
            yield return new WaitForSeconds(0.3f);
            activeScene = SceneManager.GetActiveScene().name;
            foreach(Checkpoint checkpoint in checkpoints){
                if(checkpoint.sceneName != activeScene){
                    checkpoint.gameObject.SetActive(false);
                }else{
                    checkpoint.gameObject.SetActive(true);
                }
            }
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            health = uiController.transform.Find("HUD").Find("Health").GetComponent<UpdateBar>();
            stamina = uiController.transform.Find("HUD").Find("Stamina").GetComponent<UpdateBar>();

            refreshedScene = false;
        }

        void Update()
        {
            if(refreshedScene){
                StartCoroutine(StartFunctions());
            }
            if(paused){
                Cursor.lockState = CursorLockMode.None;
            }
        }

        #region Player Leveling
        public void PlayerLevel(){
            totalLevel = vit + end + str + dex;
        }

        public int LevelUpCost(){
            return (baseLevelCost + totalLevel * levelCostScaling);
        }

        public void AlterVit(int change){
            vit += change;
            health.UpdateBarWidth(health.origMax + (vitIncrease * vit));
            PlayerLevel();
        }

        public void AlterEnd(int change){
            end += change;
            stamina.UpdateBarWidth(stamina.origMax + (endIncrease * end));
            PlayerLevel();
        }

        public void AlterStr(int change){
            str += change;
            PlayerLevel();
        }

        public void AlterDex(int change){
            dex += change;
            PlayerLevel();
        }
        #endregion

        public bool AlterCurrency(int valueToAdd){
            if(currency + valueToAdd >= 0){
                currency += valueToAdd;
                return true;
            }else{
                return false;
            }
        }

        public IEnumerator TravelTo(Checkpoint destination, bool reload){
            if(destination != lastCheckpoint || reloading){ //Won't run if the player tries to travel to the checkpoint they are resting at
                reloading = true; //Triggers the reload "animation"

                yield return new WaitForSeconds(0.35f);
                playerControllerScript.animator.Play("JumpToSit");
                
                if(uiController.menuInfo)
                    uiController.OpenMenu(uiController.menuInfo.previousMenu);
                uiController.restBackground.SetActive(false);

                uiController.ToggleRest();

                CameraHandler cam = playerControllerScript.cameraHandler;
                Debug.Log(cam);
                cam.transform.position = destination.teleportPosition;
                playerControllerScript.transform.position = destination.teleportPosition;

                playerControllerScript.transform.LookAt(destination.transform.position, Vector3.up);
                lastCheckpoint = destination;
                reloading = false;

                if(reload){
                    ReloadLevel(destination.sceneName);
                }
            }
        }

        public void ReloadLevel(string sceneName){
            reloading = true;
            refreshedScene = true;
            paused = false;
            SceneManager.LoadScene(sceneName);
            StartCoroutine(TravelTo(lastCheckpoint, false));
        }
    }
}