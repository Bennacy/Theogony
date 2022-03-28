using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public PlayerManager playerManager;
    public Image staminaFront;
    public Image staminaTrans;
    public float decreaseSpeed;
    private float maxWidth;
    private float currWidth;
    private float oldWidth;
    private bool loweringStam;

    void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        RectTransform frontTrans = (RectTransform)staminaFront.transform;
        RectTransform transTrans = (RectTransform)staminaTrans.transform;
        maxWidth = frontTrans.sizeDelta.x;
    }

    void Start()
    {
    }

    void Update()
    {
        RectTransform frontTrans = (RectTransform)staminaFront.transform;
        RectTransform transTrans = (RectTransform)staminaTrans.transform;
        currWidth = playerManager.currStamina * maxWidth / playerManager.maxStamina;

        if(loweringStam && transTrans.sizeDelta.x > frontTrans.sizeDelta.x){
            Vector2 temp = transTrans.sizeDelta;
            temp.x -= decreaseSpeed * Time.deltaTime;
            transTrans.sizeDelta = temp;
        }else if(transTrans.sizeDelta.x < frontTrans.sizeDelta.x){
            loweringStam = false;
            transTrans.sizeDelta = frontTrans.sizeDelta;
        }
        if(oldWidth > currWidth){
            StartCoroutine(DecreaseStamina());
        }
        frontTrans.sizeDelta = new Vector2(currWidth, frontTrans.sizeDelta.y);
        oldWidth = currWidth;
    }

    private IEnumerator DecreaseStamina(){
        yield return new WaitForSeconds(0.1f);
        loweringStam = true;
    }
}
