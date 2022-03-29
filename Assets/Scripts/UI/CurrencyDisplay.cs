using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    public GlobalInfo globalInfo;
    public Text currencyText;
    public Text changeText;
    public float changeTime;
    private float changeTimer;
    private int currency;
    private int oldCurrency;
    private int currencyTarget;
    private float changeAmount;
    private bool changing;

    void Start()
    {
        globalInfo = GlobalInfo.GetGlobalInfo();
        changing = false;
        currency = oldCurrency = globalInfo.currency;
    }

    void Update()
    {
        if(!changing){
            currency = globalInfo.currency;
        }

        if(oldCurrency != currency && !changing){
            changing = true;
            currencyTarget = currency;
            Debug.Log("Currency Target: " + currencyTarget);
            currency = oldCurrency;
            CurrChange(currencyTarget - currency);
        }

        if(changing){
            int change = Mathf.CeilToInt(Time.deltaTime * (currencyTarget - oldCurrency) / changeTime);
            if(change <= 0 && currencyTarget > oldCurrency){
                change = 1;
            }else if(change >= 0 && currencyTarget < oldCurrency){
                change = -1;
            }
            currency += change;
            if(Mathf.Abs(currencyTarget - currency) <= Mathf.CeilToInt(Mathf.Abs(currencyTarget - oldCurrency))/10){
                currency = currencyTarget;
                changing = false;
            }
        }

        if(!changing){
            oldCurrency = currency;
        }

        currencyText.text = currency.ToString();
    }

    void CurrChange(float changed){
        string toDisplay = "";

        if(changed > 0){
            toDisplay += "+";
        }

        toDisplay += changed.ToString();
        changeText.text = toDisplay;

        StartCoroutine(ChangeNums());
    }

    IEnumerator ChangeNums(){
        changeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(changeTime);
        changeText.gameObject.SetActive(false);
    }
}