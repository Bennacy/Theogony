using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class ButtonPromptScript : MonoBehaviour
    {
        [Serializable]
        public class ButtonImages{
            [Tooltip("Image order:\n0 - Accept\n1 - Back/Cancel\n2 - Pick up\n3 - Open\n4 - Rest\n5 - Tab left\n6 - Tab right")]
            public Sprite[] images;
            public string[] actionText;
        }
        [Tooltip("Controller types:\n0 - Keyboard\n1 - xBox\n2 - PS4")]
        public ButtonImages[] prompts;
        
        [Range(0,2)]
        public int inputType;

        [Range(0,6)]
        public int action;
        public PlayerInput input;
        [SerializeField]
        private Image promptImage;
        [SerializeField]
        private TextMeshProUGUI promptText;
        public PlayerInventory player;
        public LayerMask interactLayer;
        public float interactRadius = 1;
        public Collider[] inRange;
        private Interactable interactScript;
        private GlobalInfo globalInfo;
        
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
        }

        void Update()
        {
            switch(input.currentControlScheme){
                case "Keyboard":
                    inputType = 0;
                    break;
                case "xBox":
                    inputType = 1;
                    break;
                case "PS4":
                    inputType = 2;
                    break;
            }
            promptText.text = prompts[0].actionText[action];
            promptImage.sprite = prompts[inputType].images[action];
            inRange = Physics.OverlapSphere(player.transform.position, interactRadius, interactLayer);
            
            if(inRange.Length > 0 && !globalInfo.paused){
                interactScript = inRange[0].GetComponent<Interactable>();
                action = interactScript.action + 2;
                promptText.enabled = true;
                promptImage.enabled = true;
            }else{
                promptImage.enabled = false;
                promptText.enabled = false;
            }
        }

        public void Interact(InputAction.CallbackContext context){
            if(context.performed && inRange.Length > 0){
                interactScript = inRange[0].GetComponent<Interactable>();
                interactScript.Interact();
            }
        }
    }
}