using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Theogony {
    public class RewardRoom : MonoBehaviour
    {
        [SerializeField]
        private Transform[] spawningPositions;
        [SerializeField]
        private GameObject GorgonPrefab;
        [SerializeField]
        private GameObject StatuePrefab;
        private bool status = false;
        public PlayerControllerScript playerControllerScript;
        public float interactRange;
        void Start()
        {
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            foreach (Transform transform in spawningPositions)
            {
                Instantiate(StatuePrefab, transform.position, Quaternion.identity);
            }
            FaceArena("GorgonStatue");
        }

        void Update()
        {
            if (Vector3.Distance(playerControllerScript.transform.position, transform.position) <= interactRange)
            {
                if (status == false)
                {
                    foreach (Transform transform in spawningPositions)
                    {
                        Instantiate(GorgonPrefab, transform.position, Quaternion.identity);
                    }
                    DestroyAll("GorgonStatue");
                    status = true;
                }
            }
        }

        void DestroyAll(string tag)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            for (int i = 0; i < objects.Length; i++)
            {
                Destroy(objects[i]);
            }
        }

        public void FaceArena(string tag)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            for (int i = 0; i < objects.Length; i++)
            {
                Transform enemy = objects[i].transform;
                Vector3 direction = (transform.position - enemy.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * 150f);
            }
        }

        
    }
}
