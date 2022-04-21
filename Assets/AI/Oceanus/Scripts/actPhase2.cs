using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Theogony
{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Phase2")]
    public class actPhase2 : Action
    {
        public override void Startup(FSM fsm)
        {
            Debug.Log(" Phase 2");
            return;
        }
        public override void Act(FSM fsm)
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
    }
}
