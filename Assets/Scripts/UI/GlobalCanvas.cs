using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Theogony{
    public class GlobalCanvas : MonoBehaviour
    {
        private static GlobalCanvas self;
        private GlobalInfo globalInfo;
        private Image image;

        void Start()
        {
            if(self == null){
                self = this;
            }else{
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            globalInfo = GlobalInfo.GetGlobalInfo();
            image = GetComponentInChildren<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            image.enabled = globalInfo.reloading;
        }
    }
}