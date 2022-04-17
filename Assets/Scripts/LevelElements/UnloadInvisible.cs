using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{ 
    public class UnloadInvisible : MonoBehaviour
    {
        public Camera cam;
        public MeshRenderer[] renderers;
        public Collider[] colliders;
        public MonoBehaviour[] scripts;
        public bool toggled;
        public Vector2 seenThreshold = new Vector2(-1, 2);

        void Start()
        {
            cam = Camera.main;
            renderers = GetComponentsInChildren<MeshRenderer>();
            colliders = GetComponentsInChildren<Collider>();
            scripts = GetComponentsInChildren<MonoBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {
             Vector3 screenPoint = cam.WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > seenThreshold.x && screenPoint.x > seenThreshold.x && screenPoint.y > seenThreshold.x && screenPoint.x < seenThreshold.y && screenPoint.y < seenThreshold.y;

            if(onScreen && !toggled){
                ToggleAll();
            }else if(!onScreen && toggled){
                ToggleAll();
            }
        }
        
        private void ToggleAll(){
            toggled = !toggled;
            foreach(MeshRenderer component in renderers){
                if(component != this){
                    component.enabled = toggled;
                }
            }
            foreach(Collider component in colliders){
                if(component != this){
                    // component.enabled = toggled;
                }
            }
            foreach(MonoBehaviour component in scripts){
                if(component != this){
                    component.enabled = toggled;
                }
            }
        }
    }
}