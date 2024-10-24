using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_Stunned<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_Stunned(EnemyState stunned, EnemyState idle, float time) : base(stunned)
    {
        idleStateKey = idle;
        stunTime = time;
    }

    protected EnemyState idleStateKey;
    protected float stunTime;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        stateMachine.waitForTime.AddListener(OnWaitOver);
        stateMachine.StartCoroutine("WaitTime", stunTime);
    }

    public override void ExitState()
    {
        stateMachine.waitForTime.RemoveListener(OnWaitOver);
    }

    public void OnWaitOver()
    {
        stateMachine.ChangeToState(idleStateKey);
    }
}
