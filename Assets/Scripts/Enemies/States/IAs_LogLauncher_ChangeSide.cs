using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_LogLauncher_ChangeSide<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_LogLauncher_ChangeSide(EnemyState changeSide) : base(changeSide)
    {
        //
    }
    
    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        // This function is called ONCE when the State Machine changes to this state //
    }

    public override void UpdateState(float deltaTime)
    {
        // This function is CONSTANTLY called while it is active in the State Machine //
    }

    public override void ExitState()
    {
        // This function is called ONCE before the State Machine changes state //
    }
}
