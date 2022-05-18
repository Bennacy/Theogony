using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Theogony
{
[CreateAssetMenu(menuName = "AI/FSM/Actions/Die")]
    public class actDie : Action
    {
        public override void Startup(FSM fsm)
        {
            return;
        }
        public override void Act(FSM fsm)
        {
            if (fsm.bossController)
            {
                fsm.bossController.dying = true;
                foreach (MonoBehaviour script in fsm.gameObject.GetComponents<MonoBehaviour>())
                {
                    if (script != fsm.bossController && script != fsm)
                    {
                        Destroy(script);
                    }
                }
                foreach (Collider collider in fsm.gameObject.GetComponentsInChildren<Collider>())
                {
                    Destroy(collider);
                }
                Destroy(fsm.gameObject.GetComponent<NavMeshAgent>());
                Destroy(fsm.gameObject.GetComponentInChildren<Canvas>().gameObject);
                Destroy(fsm.gameObject.GetComponent<Rigidbody>());

                fsm.bossController.animator.StopPlayback();
                fsm.bossController.animator.Play("Die");
                GlobalInfo.GetGlobalInfo().AlterCurrency(fsm.bossController.currencyDrop);
                if (GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraHandler>().lockOnTarget == fsm.transform)
                {
                    GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraHandler>().lockOnTarget = null;
                }
                Destroy(fsm.bossController);
                Destroy(fsm);
            }
            else if (fsm.enemyController)
            {
                fsm.enemyController.dying = true;
                foreach (MonoBehaviour script in fsm.gameObject.GetComponents<MonoBehaviour>())
                {
                    if (script != fsm.enemyController && script != fsm)
                    {
                        Destroy(script);
                    }
                }
                foreach (Collider collider in fsm.gameObject.GetComponentsInChildren<Collider>())
                {
                    Destroy(collider);
                }
                Destroy(fsm.gameObject.GetComponent<NavMeshAgent>());
                Destroy(fsm.gameObject.GetComponentInChildren<Canvas>().gameObject);
                Destroy(fsm.gameObject.GetComponent<Rigidbody>());

                fsm.enemyController.animator.StopPlayback();
                fsm.enemyController.animator.Play("Die");
                GlobalInfo.GetGlobalInfo().AlterCurrency(fsm.enemyController.currencyDrop);
                if (GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraHandler>().lockOnTarget == fsm.transform)
                {
                    GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraHandler>().lockOnTarget = null;
                }
                Destroy(fsm.enemyController);
                Destroy(fsm);
            }
           
        }
    }
}
