using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Theogony{
    public class GlobalInfo : MonoBehaviour
    {
        public string firstScene;

        [Header("References")]
        public static GlobalInfo self;
        public PlayerControllerScript playerControllerScript;
        public UpdateBar health;
        public UpdateBar stamina;
        private UIController uiController;
        public string activeScene;
        public LostCurrency lostCurrency;
        // public HiddenWallInfo[] hiddenWalls;
        [Space]

        [Space]
        [Header("Curves")]
        public AnimationCurve levelCost;
        public AnimationCurve healthIncrease;
        public AnimationCurve staminaIncrease;
        public AnimationCurve strIncrease;
        public AnimationCurve dexIncrease;
        [Space]

        [Space]
        [Header("Values")]
        public int currency;
        public float sensitivity;
        public int audioVolume;
        public Checkpoint lastCheckpoint;
        public Checkpoint[] checkpoints;
        [Space]

        [Space]
        [Header("Player Stats Info")]
        public int healCharges;
        public int healLevel;
        public int healBase;
        public int healIncrease;
        public int totalLevel;
        public int baseLevelCost;
        public int levelCostScaling;
        public int vit;
        public int end;
        public int str;
        public int dex;
        public float baseHealth;
        public float baseStamina;
        [Space]

        [Space]
        [Header("Inventory")]
        public List<weaponItems> collectedWeaponsR;
        public List<weaponItems> collectedWeaponsL;
        public int currentWeaponR;
        public int currentWeaponL;
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
            // SendSettings sendSettings = GameObject.Find("SendSettings").GetComponent<SendSettings>();
            // if(sendSettings.gameObject){
            //     audioVolume = sendSettings.audioVolume;
            //     Destroy(sendSettings.gameObject);
            // }

            DontDestroyOnLoad(gameObject);
            if(self == null){
                // SceneManager.LoadScene(firstScene);
                self = this;
            }else{
                Destroy(gameObject);
            }
            StartCoroutine(StartFunctions(.4f));
        }

        private IEnumerator StartFunctions(float wait){
            yield return new WaitForSeconds(wait);
            activeScene = SceneManager.GetActiveScene().name;
            
            // GameObject[] walls = GameObject.FindGameObjectsWithTag("HiddenWall");
            // hiddenWalls = new HiddenWallInfo[walls.Length];
            // for(int i = 0; i < walls.Length; i++){
            //     hiddenWalls[i] = new HiddenWallInfo(walls[i].GetComponent<HiddenWall>());
            //     hiddenWalls[i].wall.gameObject.SetActive(hiddenWalls[i].active);
            // }

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


            lostCurrency.positions[0] = playerControllerScript.transform.position;
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
            lostCurrency.positions = new List<Vector3>();
            reloading = false;
            refreshedScene = false;
        }

        void Update()
        {
            if(activeScene != SceneManager.GetActiveScene().name){
                activeScene = SceneManager.GetActiveScene().name;
                refreshedScene = true;
            }
            if(refreshedScene){
                StartCoroutine(StartFunctions(.4f));
            }
            Debug.Log(playerControllerScript);
            if(playerControllerScript){
                lostCurrency.updateTimer += Time.deltaTime;
                if(lostCurrency.updateTimer > lostCurrency.updateCooldown){
                    lostCurrency.UpdateLastPosition(playerControllerScript.transform.position);
                }

                if(paused && playerControllerScript.playerInput.currentControlScheme == "Keyboard"){
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }else{
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
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
            health.UpdateBarWidth(healthIncrease.Evaluate(vit * .01f) * 10000);
            PlayerLevel();
        }

        public void AlterEnd(int change){
            end += change;
            stamina.UpdateBarWidth(staminaIncrease.Evaluate(end * .01f) * 1000);
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
                lostCurrency.transform.position = lostCurrency.positions[0];
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
                playerControllerScript.playerInput.enabled = false;
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

        void OnDrawGizmosSelected()
        {
            // Gizmos.DrawSphere(lostCurrency.positions[0], 1);
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

    public enum InteractionType{PickUp, Open, Sit, RecoverSouls, BossBarrier}
    
    [Serializable]
    public class LostCurrency{
        public GameObject prefab;
        public int amount;
        public string scene;
        public Transform transform;
        public int delay;
        public float updateCooldown;
        public float updateTimer;
        public int maxListSize;
        public List<Vector3> positions;
        public bool collected;

        public void UpdateLastPosition(Vector3 newPos){
            maxListSize = Mathf.FloorToInt(delay / updateCooldown);

            updateTimer = 0;
            positions.Add(newPos);
            if(positions.Count > maxListSize){
                positions.RemoveAt(0);
            }
        }
    }

    [Serializable]
    public class HiddenWallInfo{
        public bool active;
        public HiddenWall wall;

        public HiddenWallInfo(HiddenWall wallRef){
            wall = wallRef;
        }
    }

    public class Functions{
        public static float MapValues(float toMap, float min1, float max1, float min2, float max2){
            return (toMap - min1) * (max2 - min2) / (max1 - min1) + min2;
        }

        public static Vector3 Perpendicular(Vector3 v1, Vector3 v2){
            return Vector3.Cross(v1,v2);
        }
    }
}
