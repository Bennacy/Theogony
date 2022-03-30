using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Action/Attack")]


public class AttackAction : Action
{
    private float cooldowntimer = 0.2f;
    
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        fsm.GetAgent().FacePlayer();

        int i = Random.Range(1, 3);
        if (cooldowntimer < 0)
        {
            switch (i)
            {
                case 1:
                    {
                        fsm.GetAgent().GetComponent<MeshRenderer>().material.color = Color.red;
                        Debug.Log("First Attack");
                        cooldowntimer = 2;
                        fsm.gorgonAnimator.Play("Attack1");

                        break;
                    }
                case 2:
                    {
                        fsm.GetAgent().GetComponent<MeshRenderer>().material.color = Color.blue;
                        Debug.Log("Second Attack");
                        cooldowntimer = 2;
                        fsm.gorgonAnimator.Play("Attack2");
                       // fsm.GetAgent().RotateGorgon();

                        break;
                    }

            }
        }
        cooldowntimer -= Time.deltaTime;

    }
    
}
