using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInfo : MonoBehaviour
{
    [Header("References")]
    public static GlobalInfo self;
    [Space]

    [Space]
    [Header("Values")]
    public int currency;
    [Space]

    [Space]
    [Header("Bools")]
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
