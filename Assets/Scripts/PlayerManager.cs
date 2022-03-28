using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float maxHealth;
    public float maxStamina;
    public float currHealth;
    public float currStamina;
    public int[] levels;

    void Start()
    {
        currHealth = maxHealth;
        currStamina = maxStamina;
    }

    void Update()
    {
        
    }

    public bool UpdateStamina(float changeBy){
        if(currStamina - changeBy >= 0){
            currStamina -= changeBy;
            return true;
        }else{
            return false;
        }
    }
}
