using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Theogony{
    public class ButtonSpriteOverride : MonoBehaviour
    {

        public GlobalInfo globalInfo;
        public UIController uIController;
        public Sprite[] newSprites;
        public Button[] buttonsToOverride;
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            uIController = GetComponentInParent<UIController>();
            // buttonsToOverride = GetComponentsInChildren<Button>();
        }
        
        void LateUpdate()
        {
            foreach(Button button in buttonsToOverride){
                Image.Type savedType = button.image.type;
                if(button != uIController.highlightedBtn){
                    button.image.sprite = newSprites[0];
                }else{
                    button.image.sprite = newSprites[1];
                }
                button.image.type = savedType;
            }
        }
    }
}