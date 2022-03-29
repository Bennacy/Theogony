using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Sword;
    public bool CanAttack = true;
    public float AttackCooldown = 1.0f; 
    public void SwordAttack()
    {
       // Animator animator = Sword.GetComponent<Animator>(); 
       // animator.SetTrigger("Attack");
    }

}
