using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public abstract class Action : ScriptableObject
    {
        public abstract void Startup(FSM fsm); // Acts as Start() function, is called once when the state changes
        public abstract void Act(FSM fsm); // Acts as Update() funtion, is called every frame
    }
}