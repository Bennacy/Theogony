using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class BossBarrier : MonoBehaviour
    {
        private GlobalInfo globalInfo;
        private PlayerControllerScript player;
        public Vector3[] traversePoints;
        private bool traversing;
        public float traverseSpeed;
        private bool canTraverse;
        private bool disabled;
        public GameObject boss;

        void Start()
        {
            globalInfo = GlobalInfo.GetGlobalInfo();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            traversing = false;
            canTraverse = true;
            boss.SetActive(false);
        }

        void Update()
        {
            Vector3 finalPos = GetWorldCoordinates(traversePoints[1]);
            finalPos.y = player.transform.position.y;
            if(traversing && Vector3.Distance(player.transform.position, finalPos) > 0.5f){
                player.transform.position = Vector3.MoveTowards(player.transform.position, finalPos, traverseSpeed*Time.deltaTime);
                player.transform.LookAt(finalPos);
                player.animator.SetFloat("Speed", 1);
                DisablePlayerColliders();
            }else if(traversing){
                gameObject.layer = 0;
                traversing = false;
                player.animator.SetFloat("Speed", 0);
                EnablePlayerColliders();
            }
        }

        private Vector3 GetWorldCoordinates(Vector3 relative){
            relative.y -= transform.localScale.y / 2;
            relative += transform.position;
            return relative;
        }

        private void DisablePlayerColliders(){
            if(!disabled){
                player.canMove = false;
                Collider[] colliders = player.GetComponentsInChildren<Collider>();
                foreach(Collider collider in colliders){
                    collider.enabled = false;
                }
                player.GetComponent<Rigidbody>().useGravity = false;
                disabled = true;
            }
        }

        private void EnablePlayerColliders(){
            player.canMove = true;
            Collider[] colliders = player.gameObject.GetComponentsInChildren<Collider>();
            foreach(Collider collider in colliders){
                collider.enabled = true;
            }
            player.GetComponent<Rigidbody>().useGravity = true;
            disabled = false;
        }

        public void Traverse(){
            if(!canTraverse)
                return;
            
            Vector3 newPos = GetWorldCoordinates(traversePoints[0]);
            newPos.y = player.transform.position.y;
            player.transform.position = newPos;
            traversing = true;
            boss.SetActive(true);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            foreach(Vector3 position in traversePoints){
                Gizmos.DrawWireSphere(GetWorldCoordinates(position), 1);
            }
        }
    }
}