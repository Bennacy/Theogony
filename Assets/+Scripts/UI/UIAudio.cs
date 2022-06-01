using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class UIAudio : MonoBehaviour
    {
        public AudioSource source;
        public AudioClip[] clips;

        void Start()
        {
            
        }

        void Update()
        {
            // Debug.Log(Mathf.RoundToInt(Time.time));
            // if(Mathf.RoundToInt(Time.time) % 2 == 0 && !source.isPlaying){
            //     source.clip = clips[Mathf.RoundToInt(Time.time) / 2];
            //     source.Play();
            // }
        }
    }
}