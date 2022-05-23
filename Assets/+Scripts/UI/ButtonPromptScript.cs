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
            [Tooltip("Image order:\n0 - Accept\n1 - Back/Cancel\n2 - Pick up\n3 - Open\n4 - Rest\n5 - Recover\n6 - Fog wall\n7 - Tab left\n8 - Tab right")]
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
        public TextMeshProUGUI confirmText;
        public Image confirmImage;
        public TextMeshProUGUI backText;
        public Image backImage;
        
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            input = player.gameObject.GetComponent<PlayerInput>();
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

            confirmText.text = "Confirm";
            confirmImage.sprite = prompts[inputType].images[0];
            backText.text = "Back";
            backImage.sprite = prompts[inputType].images[1];

            inRange = Physics.OverlapSphere(player.transform.position, interactRadius, interactLayer);

            
            if(inRange.Length > 0 && !globalInfo.paused){
                int index = 0;
                for(int i = 0; i < inRange.Length; i++){
                    interactScript = inRange[i].GetComponent<Interactable>();
                    if(interactScript.interactRange > Vector3.Distance(player.transform.position, interactScript.transform.position)){
                        index = i;
                        break;
                    }
                }
                if(Vector3.Distance(player.transform.position, interactScript.transform.position) <= interactScript.interactRange){
                    action = (int)interactScript.action + 2;
                    promptText.enabled = true;
                    promptImage.enabled = true;

                    backText.enabled = false;
                    backImage.enabled = false;
                    confirmText.enabled = false;
                    confirmImage.enabled = false;
                }
            }else{
                promptImage.enabled = false;
                promptText.enabled = false;

                if(globalInfo.paused){
                    backText.enabled = true;
                    backImage.enabled = true;
                    confirmText.enabled = true;
                    confirmImage.enabled = true;
                }else{
                    backText.enabled = false;
                    backImage.enabled = false;
                    confirmText.enabled = false;
                    confirmImage.enabled = false;
                }
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