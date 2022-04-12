using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
public class ItemInteract : MonoBehaviour
    {
        public PauseScreen pauseScreen;
        void Start()
        {
            
        }

        void Update()
        {
            
        }

        void OnEnable()
        {
            transform.position = pauseScreen.highlightedBtn.transform.position;
        }
    }
}
