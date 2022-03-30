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
    public void FacePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100f);
    }
    public void GoToTarget()
    {
       FacePlayer();
       agent.SetDestination(target.position - new Vector3(1f, 0, 1f));
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
    public void RotateGorgon()
    {
       
            transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        
    }





}
