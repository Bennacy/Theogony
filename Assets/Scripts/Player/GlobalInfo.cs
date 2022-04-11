using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    public class GlobalInfo : MonoBehaviour
    {
        [Header("References")]
        public static GlobalInfo self;
       // public UpdateBar health;
        [Space]

        [Space]
        [Header("Values")]
        public int currency;
        public int vit;
        public int end;
        public int str;
        public int dex;
        [Space]

        [Space]
        [Header("Bools")]
        public bool paused;

        public static GlobalInfo GetGlobalInfo()
        {
            return (GameObject.FindGameObjectWithTag("GlobalInfo").GetComponent<GlobalInfo>());
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (self == null)
            {
                self = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                IncreaseVit();
            }
        }

        public void IncreaseVit()
        {
            vit++;
           // health.UpdateBarWidth(health.origMax + (10 * vit));
        }

        public bool AlterCurrency(int valueToAdd)
        {
            if (currency + valueToAdd >= 0)
            {
                currency += valueToAdd;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}