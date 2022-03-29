using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyNavMesh : MonoBehaviour
{

    public Transform target;
    private NavMeshAgent agent;
    public float health = 10;



    void Start()
    {
      
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {


    }
    
    public void GoToTarget()
    {
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

   
    


}
