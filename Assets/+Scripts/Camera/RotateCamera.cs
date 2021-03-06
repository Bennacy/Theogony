using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private float range;
    public Transform player;
    private int runOnce = 0;
    public MeshRenderer m;

     

    void FixedUpdate()
    {
        Vector3 camToPly = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, player.position.z - transform.position.z);
        Vector3 camToPlayer = (player.position - transform.position);

        RaycastHit targ;

        if(runOnce > 0){
            m.enabled = true;
        }

        float range = Vector3.Distance(player.position, transform.position);
        Physics.Raycast(transform.position, camToPlayer, out targ, range);
        
        if(targ.collider.gameObject.GetComponent<MeshRenderer>()){
            runOnce ++;
            m = targ.collider.gameObject.GetComponent<MeshRenderer>();
            m.enabled = false;
        }
    }
}
