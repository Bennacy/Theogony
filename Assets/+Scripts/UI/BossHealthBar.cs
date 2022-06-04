using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class BossHealthBar : MonoBehaviour
    {
        public string bossName;
        public GlobalInfo globalInfo;
        public BossController bossController;
        public TextMeshProUGUI text;
        public BossBarrier barrier;
        public Image barFront;
        public Image barTrans;
        public Image barBackground;
        public float decreaseSpeed;
        private float maxWidth;
        private float currWidth;
        private float oldWidth;
        private bool lowering;
        public bool activated;

        void Awake()
        {            
            globalInfo = GlobalInfo.GetGlobalInfo();
            text.text = bossName;
            RectTransform rect = (RectTransform)barBackground.transform;
            maxWidth = rect.sizeDelta.x;
        }

        void Update()
        {
            if(!bossController){
                gameObject.SetActive(false);
            }

            if(bossController.gameObject.activeSelf && !barrier.traversing && !activated){
                activated = true;
                foreach(Transform child in transform){
                    child.gameObject.SetActive(true);
                }
            }

            RectTransform frontTrans = (RectTransform)barFront.transform;
            RectTransform transTrans = (RectTransform)barTrans.transform;

            currWidth = bossController.currHealth * maxWidth / bossController.maxHealth;
            if(lowering && transTrans.sizeDelta.x > frontTrans.sizeDelta.x){
                Vector2 temp = transTrans.sizeDelta;
                temp.x -= decreaseSpeed * Time.deltaTime;
                transTrans.sizeDelta = temp;
            }else if(transTrans.sizeDelta.x < frontTrans.sizeDelta.x){
                lowering = false;
                transTrans.sizeDelta = frontTrans.sizeDelta;
            }
            if(oldWidth > currWidth){
                StartCoroutine(DecreaseBar());
            }
            frontTrans.sizeDelta = new Vector2(currWidth, frontTrans.sizeDelta.y);
            oldWidth = currWidth;
        }

        private IEnumerator DecreaseBar(){
            yield return new WaitForSeconds(0.1f);
            lowering = true;
        }
    }
}