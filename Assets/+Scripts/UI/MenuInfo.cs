using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInfo : MonoBehaviour
{
    public GameObject previousMenu;
    public bool closesPrevious;
    public bool saveIndex;
    public Button[] buttons;
    public int rowSize;
    public int currIndex;
    public Sprite[] buttonSprites;
}
