using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public abstract class Action : ScriptableObject
    {
        public abstract void Startup(FSM fsm);
        public abstract void Act(FSM fsm);
    }
}