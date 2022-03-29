using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float maxHealth;
    public float maxStamina;
    public float currHealth;
    public float currStamina;
    public float staminaRecharge;
    public int[] levels;
    public bool staminaSpent;

    void Start()
    {
        currHealth = maxHealth;
        currStamina = maxStamina;
    }

    void Update()
    {
        if(!staminaSpent && currStamina < maxStamina){
            currStamina += staminaRecharge * Time.deltaTime;
        }
    }

    public bool UpdateStamina(float changeBy){
        if(currStamina - changeBy >= 0){
            currStamina -= changeBy;
            return true;
        }else{
            return false;
        }
    }

    public IEnumerator RechargeStamina(){
        staminaSpent = true;
        yield return new WaitForSeconds(1);
        staminaSpent = false;
    }
}
