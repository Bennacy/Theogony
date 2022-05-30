using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class CustomToggle : MonoBehaviour
    {
        public Button button;
        public bool active;
        public Image toggledImage;
        public Sprite toggledSprite;
        
        void Start()
        {
            button = GetComponentInChildren<Button>();
            button.onClick.AddListener(delegate{ChangeValue();});
            toggledImage.sprite = toggledSprite;
        }

        void Update()
        {
            toggledImage.enabled = active;
        }

        public void ChangeValue(){
            active = !active;
        }
    }
}