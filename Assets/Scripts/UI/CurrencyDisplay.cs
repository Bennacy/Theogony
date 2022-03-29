using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    public GlobalInfo globalInfo;
    public Text changeNums;
    public Text txt;
    public float changeTime;
    private int currency;
    private int oldCurrency;
    private float changeAmount;
    private bool changing;

    void Start()
    {
        globalInfo = GlobalInfo.GetGlobalInfo();
        currency = globalInfo.currency;
        oldCurrency = currency;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!changing){
            currency = globalInfo.currency;
        }else{
            currency += Mathf.RoundToInt((Time.deltaTime * changeAmount) / changeTime);
        }
        txt.text = currency.ToString();

        if(oldCurrency != currency && !changing){
            changeAmount = currency - oldCurrency;
            CurrChange(changeAmount);
        }
        if(!changing)
            oldCurrency = currency;

    }

    void CurrChange(float changed){
        string toDisplay = "";

        if(changed > 0){
            toDisplay += "+";
        }

        toDisplay += changed.ToString();
        changeNums.text = toDisplay;

        StartCoroutine(ChangeNums());
    }

    IEnumerator ChangeNums(){
        changing = true;
        changeNums.gameObject.SetActive(true);
        yield return new WaitForSeconds(changeTime);
        changeNums.gameObject.SetActive(false);
        changing = false;
        currency = globalInfo.currency;
    }
}
