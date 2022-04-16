using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Action/Attack")]


public class AttackAction : Action
{
    private float cooldowntimer = 0.1f;
    
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        fsm.GetAgent().GoToTarget();

        int i = Random.Range(1, 4);
        if (cooldowntimer < 0)
        {
            // fsm.GetAgent().FacePlayer();
         
            switch (i)
            {
                case 1:
                    {
                        
                        Debug.Log("First Attack");
                        cooldowntimer = 2;
                        fsm.animator.Play("Attack1");

                        break;
                    }
                case 2:
                    {
                       
                        Debug.Log("Second Attack");
                        cooldowntimer = 2;
                        fsm.animator.Play("Attack2");
                        fsm.GetAgent().RotateGorgon();

                        break;
                    }
                case 3:
                    {

                        Debug.Log("Third Attack");
                        cooldowntimer = 2;
                        fsm.animator.Play("Attack3");

                        break;
                    }

            }
        }
       
        
        cooldowntimer -= Time.deltaTime;

    }
    
}
