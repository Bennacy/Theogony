using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Theogony{
    public class UpdateBar : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        public PlayerManager playerManager;
        public Image barFront;
        public Image barTrans;
        public Image barBackground;
        public float decreaseSpeed;
        public float origWidth;
        public float origMax;
        private float maxWidth;
        private float currWidth;
        private float oldWidth;
        private bool lowering;

        [Header("0-Stamina, 1-Health")]
        [Range(0,1)]
        public int barType;

        void Awake()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            RectTransform frontTrans = (RectTransform)barFront.transform;
            RectTransform transTrans = (RectTransform)barTrans.transform;
            switch(barType){
                case 0:
                    origMax = globalInfo.baseStamina;
                    UpdateBarWidth(globalInfo.staminaIncrease.Evaluate(globalInfo.end * .01f) * 1000);
                    break;
                case 1:
                    origMax = globalInfo.baseHealth;
                    UpdateBarWidth(globalInfo.healthIncrease.Evaluate(globalInfo.vit * .01f) * 10000);
                    break;
            }
            // maxWidth = frontTrans.sizeDelta.x;
        }

        void Start()
        {
        }

        void Update()
        {
            RectTransform frontTrans = (RectTransform)barFront.transform;
            RectTransform transTrans = (RectTransform)barTrans.transform;
            switch(barType){
                case 0:
                    currWidth = playerManager.currStamina * maxWidth / playerManager.maxStamina;
                    break;
                case 1:
                    currWidth = playerManager.currHealth * maxWidth / playerManager.maxHealth;
                    break;
            }

            if(lowering && transTrans.sizeDelta.x > frontTrans.sizeDelta.x){
                Vector2 temp = transTrans.sizeDelta;
                temp.x -= decreaseSpeed * Time.deltaTime;
                transTrans.sizeDelta = temp;
            }else if(transTrans.sizeDelta.x < frontTrans.sizeDelta.x){
                lowering = false;
                transTrans.sizeDelta = frontTrans.sizeDelta;
            }

            frontTrans.sizeDelta = new Vector2(currWidth, frontTrans.sizeDelta.y);
            
            if(oldWidth > currWidth){
                if(oldWidth - currWidth > 2){
                    StartCoroutine(DecreaseBar());
                }else{
                    transTrans.sizeDelta = frontTrans.sizeDelta;
                }
            }
            oldWidth = currWidth;
        }

        private IEnumerator DecreaseBar(){
            yield return new WaitForSeconds(.5f);
            lowering = true;
        }

        public void UpdateBarWidth(float newValue){
            maxWidth = (newValue * origWidth / origMax);
            RectTransform bar = (RectTransform)barBackground.transform;
            bar.sizeDelta = new Vector2(maxWidth, bar.sizeDelta.y);
        }
    }
}