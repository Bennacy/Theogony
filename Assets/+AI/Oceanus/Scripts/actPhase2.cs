using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Theogony
{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Phase2")]
    public class actPhase2 : Action
    {
        public override void Startup(FSM fsm)
        {
            fsm.bossController.animator.Play("Phase2");
            return;
        }
        public override void Act(FSM fsm)
        {

            


        }
    }
}
