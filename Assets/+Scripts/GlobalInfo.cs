using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Theogony{
    public class GlobalInfo : MonoBehaviour
    {
        [Serializable]
        public class LostCurrency{
            // [Tooltip("Image order:\n0 - Accept\n1 - Back/Cancel\n2 - Pick up\n3 - Open\n4 - Rest\n5 - Tab left\n6 - Tab right")]
            public GameObject prefab;
            public int amount;
            public string scene;
            public Transform transform;
            public bool collected;
        }

        [Header("References")]
        public static GlobalInfo self;
        public PlayerControllerScript playerControllerScript;
        public UpdateBar health;
        public UpdateBar stamina;
        private UIController uiController;
        public string activeScene;
        public LostCurrency lostCurrency;
        [Space]

        [Space]
        [Header("Values")]
        public int currency;
        public float sensitivityX;
        public float sensitivityY;
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
        public bool playerTargetable;
        
        public static GlobalInfo GetGlobalInfo(){
            return(GameObject.FindGameObjectWithTag("GlobalInfo").GetComponent<GlobalInfo>());
        }
        
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            if(self == null){
                // gameObject.AddComponent<Interactable>();
                self = this;
            }else{
                Destroy(gameObject);
            }
            StartCoroutine(StartFunctions(.4f));
        }

        private IEnumerator StartFunctions(float wait){
            yield return new WaitForSeconds(wait);
            activeScene = SceneManager.GetActiveScene().name;
            if(checkpoints.Length > 0){
                foreach(Checkpoint checkpoint in checkpoints){
                    if(checkpoint.sceneName != activeScene){
                        checkpoint.gameObject.SetActive(false);
                    }else{
                        checkpoint.gameObject.SetActive(true);
                    }
                }
            }
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            health = uiController.transform.Find("HUD").Find("Health").GetComponent<UpdateBar>();
            stamina = uiController.transform.Find("HUD").Find("Stamina").GetComponent<UpdateBar>();

            playerControllerScript.playerInput.enabled = true;
            if(lastCheckpoint != null){
                playerControllerScript.transform.position = lastCheckpoint.teleportPosition;
            }

            if(lostCurrency.transform){
                if(lostCurrency.scene == activeScene){
                    lostCurrency.transform.gameObject.SetActive(true);
                }else{
                    lostCurrency.transform.gameObject.SetActive(false);
                }
            }
            reloading = false;
            refreshedScene = false;
        }

        void Update()
        {
            if(refreshedScene){
                StartCoroutine(StartFunctions(.4f));
            }
            if(paused){
                Cursor.lockState = CursorLockMode.None;
            }

            // if(Input.GetKeyDown(KeyCode.P)){
            //     DropCurrency();
            // }
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

        public void PlayerDeath(){
            Debug.Log("You Died");
            Debug.Log(playerControllerScript.transform.position);
            reloading = true;
            playerControllerScript.playerInput.enabled = false;
            playerTargetable = false;
            refreshedScene = true;
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            StartCoroutine(StartFunctions(1.5f));
            playerTargetable = true;
        }

        public void DropCurrency(){
            if(lostCurrency.transform){
                Destroy(lostCurrency.transform.gameObject);
            }
            if(currency > 0){
                GameObject currencyObj = Instantiate(lostCurrency.prefab, transform);
                Interactable interactable = currencyObj.AddComponent<Interactable>();
                lostCurrency.transform = currencyObj.transform;
                lostCurrency.transform.position = playerControllerScript.transform.position;
                lostCurrency.transform.position = new Vector3(lostCurrency.transform.position.x, lostCurrency.transform.localScale.y, lostCurrency.transform.position.z);
                lostCurrency.amount = currency;
                currency = 0;
                lostCurrency.collected = false;
                lostCurrency.scene = activeScene;
                currencyObj.SetActive(false);
            }
        }

        public void RecoverCurrency(){
            Destroy(lostCurrency.transform.gameObject);
            currency += lostCurrency.amount;
            lostCurrency.amount = 0;
            lostCurrency.collected = true;
        }

        public IEnumerator TravelTo(Checkpoint destination, bool reload){
            if(destination != lastCheckpoint || reloading){ //Won't run if the player tries to travel to the checkpoint they are resting at
                reloading = true; //Triggers the reload "animation"
                if(!reload){
                    StartCoroutine(StartFunctions(.4f));
                }

                yield return new WaitForSeconds(0.45f);
                if(playerControllerScript)
                    playerControllerScript.animator.Play("InstantSit");
                
                if(uiController.menuInfo)
                    uiController.OpenMenu(uiController.menuInfo.previousMenu);
                uiController.restBackground.SetActive(false);

                uiController.ToggleRest();

                CameraHandler cam = playerControllerScript.cameraHandler;
                cam.transform.position = destination.teleportPosition;
                playerControllerScript.transform.position = destination.teleportPosition;

                playerControllerScript.transform.LookAt(destination.transform.position, Vector3.up);
                lastCheckpoint = destination;
                playerTargetable = false;

                if(reload){
                    StartCoroutine(ReloadLevel(destination.sceneName));
                }
            }
        }

        public IEnumerator ReloadLevel(string sceneName){
            reloading = true;
            yield return new WaitForSeconds(.4f);
            refreshedScene = true;
            paused = false;
            SceneManager.LoadScene(sceneName);
            StartCoroutine(TravelTo(lastCheckpoint, false));
        }
    }

    public class Functions{
        public static float MapValues(float toMap, float min1, float max1, float min2, float max2){
            return (toMap - min1) * (max2 - min2) / (max1 - min1) + min2;
        }
    }
}
