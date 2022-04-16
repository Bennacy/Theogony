using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryingTester : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotParried()
    {
        animator.Play("Parried");
    }
}
