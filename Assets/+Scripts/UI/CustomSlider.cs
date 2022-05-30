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
        private Vector2 backgroundThreshold;
        private RectTransform canvasTransform;
        private Camera mainCam;
        public RectTransform backgroundTransform;
        public RectTransform filledIndicator;
        private Image filledImage;
        public Sprite filledSprite;
        public Image sliderBall;
        public Sprite ballSprite;
        private Button buttonRef;
        private float fractionSize;
        public int fractionCount;
        public float sliderValue;
        private float lastValue;
        private bool changed;
        private bool held;
        public bool mouseOver;
        private SendSettings sendSettings;
        private MainMenuController mainMenuController;

        void Start()
        {
            sendSettings = GetComponentInParent<ChangeSettings>().sendSettings;
            if(sendSettings){
                mainMenuController = GetComponentInParent<MainMenuController>();
                canvasTransform = mainMenuController.gameObject.GetComponent<RectTransform>();
            }else{
                uiController = GetComponentInParent<UIController>();
                canvasTransform = uiController.gameObject.GetComponent<RectTransform>();
            }
            buttonRef = GetComponentInChildren<Button>();
            mainCam = Camera.main;
            backgroundThreshold.x = backgroundTransform.localPosition.x;
            backgroundThreshold.y = backgroundTransform.localPosition.x + backgroundTransform.sizeDelta.x;
            filledImage = filledIndicator.gameObject.GetComponent<Image>();
            SetValue(sliderValue);
        }

        void Update()
        {
            filledImage.sprite = filledSprite;
            sliderBall.sprite = ballSprite;
            lastValue = sliderValue;
            if(sendSettings){
                if(mainMenuController.sliderChange != 0 && mainMenuController.highlightedBtn == buttonRef){
                    ChangeSlider(mainMenuController.sliderChange);
                    mainMenuController.sliderChange = 0;
                    sliderValue = GetValue();
                }
            }else{
                if(uiController.sliderChange != 0 && uiController.highlightedBtn == buttonRef){
                    ChangeSlider(uiController.sliderChange);
                    uiController.sliderChange = 0;
                    sliderValue = GetValue();
                }
            }

            if(mouseOver && Input.GetMouseButtonDown(0)){
                held = true;
            }

            if(held){
                Vector2 clicked;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, Input.mousePosition, null, out clicked);
                clicked.y = sliderBall.transform.localPosition.y;
                if(clicked.x < backgroundThreshold.x){
                    clicked.x = backgroundThreshold.x;
                }else if(clicked.x > backgroundThreshold.y){
                    clicked.x = backgroundThreshold.y;
                }
                sliderBall.transform.localPosition = clicked;
                sliderValue = GetValue();
                if(Input.GetMouseButtonUp(0)){
                    held = false;
                }
            }

            Vector2 filledSize = filledIndicator.sizeDelta;
            filledSize.x = (sliderBall.transform.localPosition.x - backgroundTransform.localPosition.x);
            filledIndicator.sizeDelta = filledSize;

            if(lastValue != sliderValue){
                changed = true;
            }
        }

        private void ChangeSlider(int sign){
            Vector2 newPos = sliderBall.transform.localPosition;
            newPos.x += sign * fractionSize;
            
            if(newPos.x < Mathf.Round(backgroundTransform.localPosition.x)){
                newPos.x = Mathf.Round(backgroundTransform.localPosition.x);
            }else if(newPos.x > Mathf.Round(backgroundThreshold.y)){
                newPos.x = Mathf.Round(backgroundThreshold.y);
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
            float newPos = Functions.MapValues(sliderValue, sliderRange.x, sliderRange.y, backgroundThreshold.x, backgroundThreshold.y);
            Vector2 ballPos = sliderBall.transform.localPosition;
            ballPos.x = newPos;
            sliderBall.transform.localPosition = ballPos;
        }

        public bool OnValueChanged(){
            if(changed){
                changed = false;
                return true;
            }else{
                return false;
            }
        }

        public void SetMouseOver(bool state){
            mouseOver = state;
        }

        public bool OnMouseOver(){
            return mouseOver;
        }
    }
}