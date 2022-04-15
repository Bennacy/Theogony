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
        public Image sliderBall;
        private Button buttonRef;
        private float fractionSize;
        public int fractionCount;
        public float sliderValue;
        private float lastValue;
        private bool changed;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
            buttonRef = GetComponentInChildren<Button>();

            SetValue(globalInfo.sensitivity);
        }

        void OnEnable()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            SetValue(globalInfo.sensitivity);
        }

        void Update()
        {
            lastValue = sliderValue;
            if(uiController.sliderChange != 0 && uiController.highlightedBtn == buttonRef){
                ChangeSlider(uiController.sliderChange);
                uiController.sliderChange = 0;
                sliderValue = GetValue();
            }
            if(lastValue != sliderValue){
                changed = true;
            }
        }

        private void ChangeSlider(int sign){
            Vector2 newPos = sliderBall.transform.localPosition;
            newPos.x += sign * fractionSize;
            
            if(newPos.x < Mathf.Round(backgroundTransform.localPosition.x)){
                newPos.x = Mathf.Round(backgroundTransform.localPosition.x);
            }else if(newPos.x > Mathf.Round(backgroundTransform.localPosition.x + backgroundTransform.sizeDelta.x)){
                newPos.x = Mathf.Round(backgroundTransform.localPosition.x + backgroundTransform.sizeDelta.x);
            }
            sliderBall.transform.localPosition = newPos;
        }

        public float GetValue(){
            fractionSize = backgroundTransform.sizeDelta.x / fractionCount;
            float ballOffset = sliderBall.transform.localPosition.x - backgroundTransform.localPosition.x;
            float fractions = ballOffset / fractionSize;
            float finalValue = (sliderRange.y - sliderRange.x) / fractionCount * fractions;
            sliderValue = finalValue + sliderRange.x;
            return sliderValue;
        }

        public void SetValue(float newValue){
            sliderValue = newValue;
            float newPos = Functions.MapValues(sliderValue, sliderRange.x, sliderRange.y, backgroundTransform.localPosition.x, backgroundTransform.localPosition.x + backgroundTransform.sizeDelta.x);
            Vector2 ballPos = sliderBall.transform.localPosition;
            ballPos.x = newPos;
            sliderBall.transform.localPosition = ballPos;
        }

        public bool ValueChanged(){
            if(changed){
                changed = false;
                return true;
            }else{
                return false;
            }
        }
    }
}