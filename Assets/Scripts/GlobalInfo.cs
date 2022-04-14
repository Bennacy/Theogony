using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class GlobalInfo : MonoBehaviour
    {
        [Header("References")]
        public static GlobalInfo self;
        public UpdateBar health;
        public UpdateBar stamina;
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
        [Header("Booleans")]
        public bool paused;
        
        public static GlobalInfo GetGlobalInfo(){
            return(GameObject.FindGameObjectWithTag("GlobalInfo").GetComponent<GlobalInfo>());
        }
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if(self == null){
                self = this;
            }else{
                Destroy(gameObject);
            }
        }

        void Update()
        {
            // if(Input.GetKeyDown(KeyCode.Alpha1)){
            //     IncreaseVit();
            // }
        }

        public void AlterVit(int change){
            vit += change;
            health.UpdateBarWidth(health.origMax + (10 * vit));
        }

        public void AlterEnd(int change){
            end += change;
            stamina.UpdateBarWidth(stamina.origMax + (5 * end));
        }

        public void AlterStr(int change){
            str += change;
        }

        public void AlterDex(int change){
            dex += change;
        }

        public bool AlterCurrency(int valueToAdd){
            if(currency + valueToAdd >= 0){
                currency += valueToAdd;
                return true;
            }else{
                return false;
            }
        }
}
}