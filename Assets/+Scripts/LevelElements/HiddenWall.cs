using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class HiddenWall : MonoBehaviour
    {
        public GlobalInfo globalInfo;
        public PlayerControllerScript controller;
        public PlayerControllerScript playerControllerScript;
        public float fadeDuration;
        public float fadeTimer;
        public bool fading;
        public Collider coll;
        public Material material;
        public bool active;
        
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
                active = false;
                // foreach(HiddenWallInfo info in globalInfo.hiddenWalls){
                //     if(info.wall == this){
                //         info.active = false;
                //         break;
                //     }
                // }
            }
            if(material.color.a <= 0){
                Destroy(gameObject);
            }
            if(controller){
                if(controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Roll")){
                    fading = true;
                }
            }
        }

        void OnCollisionEnter(Collision collisionInfo)
        {
            if(collisionInfo.gameObject.tag == "Player"){
                controller = collisionInfo.gameObject.GetComponent<PlayerControllerScript>();
            }
        }

        void OnCollisionExit(Collision collisionInfo)
        {
            if(collisionInfo.gameObject.tag == "Player"){
                controller = null;
            }
        }
    }
}