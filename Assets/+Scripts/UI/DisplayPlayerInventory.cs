using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class DisplayPlayerInventory : MonoBehaviour
    {
        public bool isLeft;
        public GlobalInfo globalInfo;
        public UIController uIController;
        public PlayerInventory playerInventory;
        
        public RectTransform itemSlotChildren;
        public GameObject itemInteraction;
        public GameObject itemSlotPrefab;
        public int maxCols;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            uIController = GetComponentInParent<UIController>();
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInventory>();
        }

        void Update()
        {
            
        }

        public void ExpandSlots(){
            itemInteraction.GetComponent<MenuInfo>().previousMenu = itemSlotChildren.gameObject;
            foreach(Transform child in itemSlotChildren){
                Destroy(child.gameObject);
            }
            
            MenuInfo menuInfo = itemSlotChildren.gameObject.GetComponent<MenuInfo>();
            int weaponListSize = 0;
            if(isLeft){
                weaponListSize = globalInfo.collectedWeaponsL.Count;
            }else{
                weaponListSize = globalInfo.collectedWeaponsR.Count;
            }
            menuInfo.rowSize = maxCols;

            int rows = ((weaponListSize-1)/maxCols) + 1;
            if(weaponListSize < maxCols){
                int cols = ((weaponListSize) % maxCols);
                itemSlotChildren.sizeDelta = new Vector2(cols * 125 + 25, Mathf.Ceil(rows * 125));
            }else{
                itemSlotChildren.sizeDelta = new Vector2(maxCols * 125 + 25, Mathf.Ceil(rows * 125));
            }

            menuInfo.buttons = new Button[weaponListSize];
            for(int i = 0; i < weaponListSize; i++){
                Button slot = Instantiate(itemSlotPrefab, itemSlotChildren).GetComponent<Button>();
                int col = i % maxCols;
                int row = (i - col) / maxCols;
                slot.transform.localPosition = new Vector2(col * 125 + 75, -row * 125 - 62.5f);
                // slot.onClick.AddListener(delegate{uIController.OpenMenu(itemInteraction);});
                int newIndex = i;
                slot.onClick.AddListener(delegate{UpdateWeapons(newIndex);});
                menuInfo.buttons[i] = slot;
            }

            uIController.OpenMenu(itemSlotChildren.gameObject);
        }

        private void UpdateWeapons(int newIndex){
            Debug.Log(newIndex);
            if(isLeft){
                globalInfo.currentWeaponL = newIndex;
            }else{
                globalInfo.currentWeaponR = newIndex;
            }
            playerInventory.LoadWeapons();
        }
    }
}