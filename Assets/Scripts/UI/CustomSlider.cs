using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Theogony{
    public class CustomSlider : MonoBehaviour
    {
        private GlobalInfo globalInfo;
        private UIController uiController;
        public Vector2 sliderRange;
        public RectTransform backgroundTransform;
        private Image sliderBall;
        private Button buttonRef;
        private float fractionSize;
        public int fractionCount;
        public float sliderValue;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            buttonRef = GetComponentInChildren<Button>();
            sliderBall = transform.Find("SliderBall").gameObject.GetComponent<Image>();

            fractionSize = backgroundTransform.sizeDelta.x / fractionCount;
        }

        void Update()
        {
            if(uiController.sliderChange != 0 && uiController.highlightedBtn == buttonRef){
                ChangeSlider(uiController.sliderChange);
                uiController.sliderChange = 0;
                sliderValue = GetValue();
            }
        }

        private void ChangeSlider(int sign){
            Vector2 newPos = sliderBall.transform.localPosition;
            newPos.x += Mathf.Round(sign * fractionSize);
            if(newPos.x >= Mathf.Round(backgroundTransform.localPosition.x) && newPos.x <= Mathf.Round(backgroundTransform.localPosition.x + backgroundTransform.sizeDelta.x)){
                sliderBall.transform.localPosition = newPos;
            }
        }

        public float GetValue(){
            float ballOffset = sliderBall.transform.localPosition.x - backgroundTransform.localPosition.x;
            float fractions = Mathf.Round(ballOffset / fractionSize);
            float finalValue = Mathf.Round((sliderRange.y - sliderRange.x) / fractionCount * fractions);
            return finalValue + sliderRange.x;
        }
    }
}