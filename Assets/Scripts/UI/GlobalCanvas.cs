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
        public float fadeinSpeed;
        public float fadeoutSpeed;
        private bool startedFade;

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
            if(globalInfo.reloading && !startedFade){
                startedFade = true;
                StartCoroutine(FadeOut(true));
            }
            if(!globalInfo.reloading && startedFade){
                startedFade = false;
                StartCoroutine(FadeOut(false));
            }
        }

        public IEnumerator FadeOut(bool fadingIn){
            Color color = image.color;
            if(fadingIn){
                image.enabled = true;
                while(image.color.a < 1){
                    color.a += (fadeinSpeed * Time.deltaTime);
                    image.color = color;
                    yield return null;
                }
            }else{
                while(image.color.a > 0){
                    color.a -= fadeoutSpeed * Time.deltaTime;
                    image.color = color;
                    yield return null;
                }
                image.enabled = false;
            }
        }
    }
}