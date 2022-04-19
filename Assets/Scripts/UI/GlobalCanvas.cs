using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Theogony{
    public class GlobalCanvas : MonoBehaviour
    {
        private static GlobalCanvas self;
        public GlobalInfo globalInfo;
        private Image image;
        public float fadeinSpeed;
        public float fadeoutSpeed;
        public bool startedFade;

        void Start()
        {
            if(self == null){
                self = this;
                DontDestroyOnLoad(gameObject);
                globalInfo = GlobalInfo.GetGlobalInfo();
                image = GetComponentInChildren<Image>();
            }else{
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // fadeinSpeed = globalInfo.fadeSpeed;
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
                while(color.a < 1){
                    color.a += (fadeinSpeed * Time.deltaTime);
                    Debug.Log("fade out " + color);
                    image.color = color;
                    yield return null;
                }
            }else{
                while(color.a > 0){
                    color.a -= fadeoutSpeed * Time.deltaTime;
                    Debug.Log("fade in " + color);
                    image.color = color;
                    yield return null;
                }
                image.enabled = false;
            }
        }
    }
}