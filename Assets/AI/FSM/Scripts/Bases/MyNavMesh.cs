using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Theogony{
    public class MyNavMesh : MonoBehaviour
    {

        public Transform target;
        public NavMeshAgent agent;
        private EnemyController enemyController;
        private GlobalInfo globalInfo;
        public float health = 10;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            enemyController = GetComponent<EnemyController>();
            target = enemyController.target;
            globalInfo = GlobalInfo.GetGlobalInfo();
        }

        public void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100f);
        }
        public void GoToTarget()
        {
            FaceTarget();
            agent.SetDestination(target.position);
        }

        public void SetTarget(Vector3 newTarget){
            agent.SetDestination(newTarget);
        }

        public void Stop()
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        public bool IsAtDestination(){
            if(!agent.pathPending){
                if(agent.remainingDistance <= agent.stoppingDistance){
                        return true;
                }
            }
            return false;
        }

        public void RotateGorgon()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            Vector3 angle = lookRotation.eulerAngles;
            angle.y += 90;
            lookRotation = Quaternion.Euler(angle);
            transform.rotation = lookRotation;
        }
    }
}