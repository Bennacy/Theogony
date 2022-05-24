using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask obstacleLayers;
    private float range;
    public Transform player;
    private int runOnce = 0;
    public MeshRenderer m;

     

    void Update()
    {

        if( transform.position.y <= 10f)
        {
            transform.position =  new Vector3( transform.position.x ,10.1f ,  transform.position.z);
        }
        Vector3 camToPly = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, player.position.z - transform.position.z);
        Vector3 camToPlayer = (player.position - transform.position);

        RaycastHit targ;

        if(runOnce > 0){
            m.enabled = true;
        }
        Debug.Log(transform.position);

        Debug.DrawRay(transform.position, Vector3.forward, Color.red);
        float range = Vector3.Distance(player.position, transform.position);
        Physics.Raycast(transform.position, camToPlayer, out targ, range);
        runOnce ++;
        // Debug.Log(targ.collider.gameObject.name);
        
        m = targ.collider.gameObject.GetComponent<MeshRenderer>();
        m.enabled = false;


    }
    
    void OnDrawGizmos()
    {
        // Debug.Log(transform.position);
        Gizmos.DrawSphere(transform.position, 1);
    }
}
