using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTest2 : State
{
    public FSM fsm => GetComponent<FSM>();
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if(true)
        {
            fsm.ChangeState<StateTest>();
        }
        else
        {

        }
    }

    
}

