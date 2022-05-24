using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private float range;
    public Transform player;
    private int runOnce = 0;
    public MeshRenderer m;

     

    void FixedUpdate()
    {

        if( transform.position.y <= 10f)
        {
            transform.position =  new Vector3( transform.position.x ,10.1f ,  transform.position.z);
        }
        Vector3 camToPly = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, player.position.z - transform.position.z);

        RaycastHit verification;
        RaycastHit targ;
       
        // Debug.Log(transform.position.z);
/*
        Physics.Raycast(transform.position, camToPly, out verification, Mathf.Infinity);

        if(targ.collider.gameObject.name == verification.collider.gameObject.name)
        {
            return;
        }else
        {
              m.enabled = true;
        }     
*/  
    if(runOnce>0){
        m.enabled = true;
        }

        Physics.Raycast(transform.position, camToPly, out targ, Mathf.Infinity);
        runOnce ++;
        Debug.Log(targ.collider.gameObject.name);
        
        m = targ.collider.gameObject.GetComponent<MeshRenderer>();
        m.enabled = false;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*
        if(Input.GetKey(KeyCode.UpArrow)){

        }
        if(Input.GetKey(KeyCode.DownArrow)){
            
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            Vector3 rot = transform.rotation.eulerAngles;
            rot.y -= 10 * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rot);
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            Vector3 rot = transform.rotation.eulerAngles;
            rot.y += 10 * Time.deltaTime;
            transform.rotation = Quaternion.Euler(rot);
        }
        */
    }
}
