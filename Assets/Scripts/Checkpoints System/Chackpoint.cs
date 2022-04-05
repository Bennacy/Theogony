using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chackpoint : MonoBehaviour
{
    private GameMaster gm;
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
;    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gm.lastCheckpoinPosition = transform.position;
        }
    }
}
