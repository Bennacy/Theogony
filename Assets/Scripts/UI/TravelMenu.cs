using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class TravelMenu : MonoBehaviour
    {
        private GlobalInfo globalInfo;
        private MenuInfo menuInfo;
        private UIController uiController;
        private Image destinationImage;
        public Checkpoint[] unlockedCheckpoints;
        public GameObject buttonPrefab;

        void Start()
        {
            destinationImage = GetComponentInChildren<Image>();
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
                if(checkpoint != null){
                    arraySize++;
                }
            }
            menuInfo.buttons = new Button[arraySize];
            unlockedCheckpoints = new Checkpoint[arraySize];
            int newArrayIndex = 0;
            foreach(Checkpoint checkpoint in globalInfo.checkpoints){
                if(checkpoint != null){
                    unlockedCheckpoints[newArrayIndex] = checkpoint;
                    CreateButton(checkpoint, newArrayIndex);
                    newArrayIndex++;
                }
            }

            uiController.GetMenuButtons();
        }

        private void CreateButton(Checkpoint checkpointReference, int index){
                GameObject newButton = Instantiate(buttonPrefab, transform);
                Vector3 buttonPos = newButton.transform.localPosition;
                buttonPos.y -= index * 75;
                newButton.transform.localPosition = buttonPos;

                TextMeshProUGUI text = newButton.transform.GetComponentInChildren<TextMeshProUGUI>();
                text.text = checkpointReference.locationName;

                newButton.GetComponent<Button>().onClick.AddListener(delegate{StartCoroutine(globalInfo.TravelTo(checkpointReference));});
                menuInfo.buttons[index] = newButton.GetComponent<Button>();
        }
    }
}