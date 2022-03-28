using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyNavMesh : MonoBehaviour
{
   
    public Transform target;
    private NavMeshAgent agent;
    public float health = 10;
    private SteeringBehaviorBase steering;
    public Steering[] chase_steerings;
    public Steering[] attack_steerings;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {


    }

 
    public bool IsAtDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public void GoToTarget()
    {
      //  steering.ExecuteSteerings(chase_steerings);
       agent.SetDestination(target.position - new Vector3(1, 0, 1));
    }
    
    public float GetHealth()
    {
        return health;
    }
   
    public void StopAction()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }
    public void SteeringAttack()
    {
        steering.ExecuteSteerings(attack_steerings);
    }
   
    


}
