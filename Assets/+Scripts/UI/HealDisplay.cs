using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Theogony{
    public class HealDisplay : MonoBehaviour
    {
        public PlayerManager playerManager;
        public GlobalInfo globalInfo;
        public TextMeshProUGUI text;

        void Start()
        {
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        void Update()
        {
            text.text = playerManager.healCharges.ToString();
        }
    }
}