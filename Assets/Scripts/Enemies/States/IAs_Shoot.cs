using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_Shoot<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_Shoot(EnemyState shoot, EnemyState idle) : base(shoot)
    {
        idleStateKey = idle;
    }

    protected EnemyState idleStateKey;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        stateMachine.InstantiateObject();
        stateMachine.ChangeToState(idleStateKey);
    }
}