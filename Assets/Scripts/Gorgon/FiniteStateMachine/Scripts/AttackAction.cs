using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Action/Attack")]


public class AttackAction : Action
{
    private float cooldowntimer = 0f;
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        //fsm.GetAgent().SteeringAttack();
        int i = Random.Range(1, 3);
        if (cooldowntimer < 0)
        {
            switch (i)
            {
                case 1:
                    {
                        fsm.GetAgent().GetComponent<MeshRenderer>().material.color = Color.red;
                        Debug.Log("First Attack");
                        cooldowntimer = 1;

                        break;
                    }
                case 2:
                    {
                        fsm.GetAgent().GetComponent<MeshRenderer>().material.color = Color.blue;
                        Debug.Log("Second Attack");
                        cooldowntimer = 1;
                        break;
                    }

            }
        }
        cooldowntimer -= Time.deltaTime;

    }
   
}
