using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class EnemyWorldCanvas : MonoBehaviour
    {
        public PlayerControllerScript playerControllerScript;
        public EnemyController enemyController;
        public BossController bossController;
        public CameraHandler cam;
        public GameObject healthBar;
        public GlobalInfo globalInfo;
        public RectTransform barTransform;
        private float maxWidth;
        
        void Start()
        {
            bossController = GetComponentInParent<BossController>(); 
            enemyController = GetComponentInParent<EnemyController>();
            playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            cam = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraHandler>();
            globalInfo = GlobalInfo.GetGlobalInfo();
            maxWidth = barTransform.sizeDelta.x;
        }

        void Update()
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y = cam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(rotation);

            bool active = false;
            if (bossController)
            {
                active = (cam.lockOnTarget == transform.parent || bossController.currHealth != bossController.maxHealth) && Vector3.Distance(bossController.transform.position, bossController.target.position) < 15;
                healthBar.SetActive(active);

                Vector3 newSize = barTransform.sizeDelta;
                newSize.x = bossController.currHealth * maxWidth / bossController.maxHealth;
                barTransform.sizeDelta = newSize;
            }else if (enemyController)
            {
                active = (cam.lockOnTarget == transform.parent || enemyController.currHealth != enemyController.maxHealth) && Vector3.Distance(enemyController.transform.position, enemyController.target.position) < 15;
                healthBar.SetActive(active);

                Vector3 newSize = barTransform.sizeDelta;
                newSize.x = enemyController.currHealth * maxWidth / enemyController.maxHealth;
                barTransform.sizeDelta = newSize;
            }
        }
    }
}