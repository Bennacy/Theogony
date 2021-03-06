using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class TravelMenu : MonoBehaviour
    {
        public int maxCheckpoints;
        public int indexOffset;
        private GlobalInfo globalInfo;
        private MenuInfo menuInfo;
        private UIController uiController;
        public Transform buttonHolder;
        public Image destinationImage;
        public Checkpoint[] unlockedCheckpoints;
        public GameObject buttonPrefab;

        void Start()
        {
            // destinationImage = GetComponentInChildren<Image>();
            indexOffset = 0;
        }

        void Update()
        {
            int currentSelection = menuInfo.currIndex;
            destinationImage.sprite = unlockedCheckpoints[currentSelection].destinationPreview;
        }

        void OnEnable()
        {
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            menuInfo = GetComponent<MenuInfo>();
            int arraySize = 0;
            
            foreach(Checkpoint checkpoint in globalInfo.checkpoints){
                if(checkpoint.unlocked){
                    arraySize++;
                }
            }
            foreach(Transform button in buttonHolder){
                Destroy(button.gameObject);
            }

            menuInfo.buttons = new Button[arraySize];
            unlockedCheckpoints = new Checkpoint[arraySize];
            int newArrayIndex = 0;
            foreach(Checkpoint checkpoint in globalInfo.checkpoints){
                if(checkpoint.unlocked){
                    unlockedCheckpoints[newArrayIndex] = checkpoint;
                    CreateButton(checkpoint, newArrayIndex);
                    newArrayIndex++;
                }
            }
            uiController.GetMenuButtons();
        }

        private void CreateButton(Checkpoint checkpointReference, int index){
                GameObject newButton = Instantiate(buttonPrefab, buttonHolder);
                Vector3 buttonPos = newButton.transform.localPosition;
                buttonPos.y -= index * 140 - 70;
                newButton.transform.localPosition = buttonPos;

                TextMeshProUGUI text = newButton.transform.GetComponentInChildren<TextMeshProUGUI>();
                text.text = checkpointReference.locationName;

                newButton.GetComponent<Button>().onClick.AddListener(delegate{StartCoroutine(globalInfo.TravelTo(checkpointReference, true));});
                menuInfo.buttons[index] = newButton.GetComponent<Button>();
        }
    }
}