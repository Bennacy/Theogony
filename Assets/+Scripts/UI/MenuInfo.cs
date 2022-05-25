using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Theogony{
    public class MenuInfo : MonoBehaviour
    {
        public GameObject previousMenu;
        [Tooltip("If the previous menu is closed when this one is opened")]
        public bool closesPrevious;
        public bool saveIndex;
        public Button[] buttons;
        public int rowSize;
        public int currIndex;
        public Sprite[] buttonSprites;
        public Color[] buttonTextColors;
    }
}