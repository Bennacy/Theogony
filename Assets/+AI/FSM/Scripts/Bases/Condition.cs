using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool Test(FSM fsm);
    }
}