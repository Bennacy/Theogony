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
        public Image currentWeapon;
        
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
            if(isLeft){
                currentWeapon.sprite = globalInfo.collectedWeaponsL[globalInfo.currentWeaponL].icon;
            }else{
                currentWeapon.sprite = globalInfo.collectedWeaponsR[globalInfo.currentWeaponR].icon;
            }
        }

        public void ExpandSlots(){
            itemInteraction.GetComponent<MenuInfo>().previousMenu = itemSlotChildren.gameObject;
            foreach(Transform child in itemSlotChildren){
                Destroy(child.gameObject);
            }
            
            MenuInfo menuInfo = itemSlotChildren.gameObject.GetComponent<MenuInfo>();
            int weaponListSize = 0;
            List<weaponItems> weaponList;
            if(isLeft){
                weaponList = globalInfo.collectedWeaponsL;
                currentWeapon.sprite = weaponList[globalInfo.currentWeaponL].icon;
            }else{
                weaponList = globalInfo.collectedWeaponsR;
                currentWeapon.sprite = weaponList[globalInfo.currentWeaponR].icon;
            }
            weaponListSize = weaponList.Count;
            menuInfo.rowSize = maxCols;

            int rows = ((weaponListSize-1)/maxCols) + 1;
            if(weaponListSize < maxCols){
                int cols = ((weaponListSize) % maxCols);
                itemSlotChildren.sizeDelta = new Vector2(cols * 200 + 25, Mathf.Ceil(rows * 200));
            }else{
                itemSlotChildren.sizeDelta = new Vector2(maxCols * 200 + 25, Mathf.Ceil(rows * 200));
            }

            menuInfo.buttons = new Button[weaponListSize];
            for(int i = 0; i < weaponListSize; i++){
                Button slot = Instantiate(itemSlotPrefab, itemSlotChildren).GetComponent<Button>();
                int col = i % maxCols;
                int row = (i - col) / maxCols;
                slot.transform.localPosition = new Vector2(col * 200 + 112.5f, -row * 200 - 100);
                
                int newIndex = i;
                Image icon = slot.transform.Find("Icon").gameObject.GetComponent<Image>();
                icon.sprite = weaponList[newIndex].icon;
                slot.onClick.AddListener(delegate{UpdateWeapons(newIndex);});
                menuInfo.buttons[i] = slot;
            }

            uIController.OpenMenu(itemSlotChildren.gameObject);
        }

        private void UpdateWeapons(int newIndex){
            if(isLeft){
                globalInfo.currentWeaponL = newIndex;
            }else{
                globalInfo.currentWeaponR = newIndex;
            }
            playerInventory.LoadWeapons();
        }
    }
}