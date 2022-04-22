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
        public float rotationSpeed;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            enemyController = GetComponent<EnemyController>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            globalInfo = GlobalInfo.GetGlobalInfo();
        }

        public void FaceTarget()
        {
            Vector3 targetPos = target.position;
            targetPos.y = 0;
            Vector3 selfPos = transform.position;
            selfPos.y = 0;
            Vector3 direction = (targetPos - selfPos).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
    }
}