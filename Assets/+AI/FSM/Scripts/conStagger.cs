using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Conditions/Stagger")]
    public class conStagger : Condition
    {
        public bool negation;
        public override bool Test(FSM fsm)
        {
        if(fsm.enemyController.staggered){
            return !negation;
        }
            return negation;
        }
    }
}