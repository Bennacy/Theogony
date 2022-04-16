using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public abstract class Action : ScriptableObject
    {
        // public abstract void Start(FSM fsm);
        public abstract void Act(FSM fsm);
    }
}