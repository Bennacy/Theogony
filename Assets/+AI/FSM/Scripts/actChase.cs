using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Chase")]
    public class actChase : Action
    {
        public override void Startup(FSM fsm){
            fsm.GetNavMesh().GoToTarget();
        }

        public override void Act(FSM fsm){
<<<<<<< HEAD:Assets/+AI/FSM/Scripts/actChase.cs
=======

          //  Debug.Log("Chasing " + fsm.enemyController.target);
>>>>>>> new-main-vasco:Assets/AI/FSM/Scripts/actChase.cs
            fsm.GetNavMesh().GoToTarget();
        }
    }
}