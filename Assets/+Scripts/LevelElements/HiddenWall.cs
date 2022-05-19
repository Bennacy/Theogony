using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class HiddenWall : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        public PlayerControllerScript playerControllerScript;
        public float fadeDuration;
        public float fadeTimer;
        public bool fading;
        public Collider coll;
        public Material material;
        
        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            coll = GetComponent<Collider>();
            material = new Material(GetComponent<MeshRenderer>().material);
            material.name = "Local Material";
            GetComponent<MeshRenderer>().material = material;
            fadeTimer = 0;
            fading = false;
        }

        void Update()
        {
            if(fading){
                fadeTimer += Time.deltaTime; 
                Color newCol = material.color;
                newCol.a = Functions.MapValues(fadeTimer, 0, fadeDuration, 1, 0);
                material.color = newCol;
                coll.enabled = false;
            }
            if(material.color.a <= 0){
                Destroy(gameObject);
            }
        }
    }
}