using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class SendSettings : MonoBehaviour
    {
        public int audioVolume;

        void Awake()
        {
            if(GameObject.FindGameObjectWithTag("GlobalInfo")){
                audioVolume = GlobalInfo.GetGlobalInfo().audioVolume;
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}