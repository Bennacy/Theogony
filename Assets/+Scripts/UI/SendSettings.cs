using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class SendSettings : MonoBehaviour
    {
        public float audioVolume;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}