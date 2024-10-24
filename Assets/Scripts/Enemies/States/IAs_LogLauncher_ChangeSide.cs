using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_LogLauncher_ChangeSide<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_LogLauncher_ChangeSide(EnemyState change, EnemyState idle) : base(change)
    {
        idleStateKey = idle;
    }

    protected EnemyState idleStateKey;
    protected Vector3 targetExit, targetEnter, otherSideSpawn;
    protected float sideFactor = 1;
    private bool isDisappearing, isReentering;
    public StateVoidEvent changingSide = new();
    
    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        stateMachine.waitForTime.AddListener(OnWaitOver);
        isDisappearing = true;
        targetExit = new Vector3(stateMachine.GetEnemyTransform().position.x + 5f*sideFactor, stateMachine.GetEnemyTransform().position.y, stateMachine.GetEnemyTransform().position.z);
        targetEnter = new Vector3(stateMachine.GetEnemyTransform().position.x - 18f*sideFactor, stateMachine.GetEnemyTransform().position.y, stateMachine.GetEnemyTransform().position.z);
        otherSideSpawn = new Vector3(targetEnter.x - 5f*sideFactor, targetEnter.y, targetEnter.z);
    }

    public override void UpdateState(float deltaTime)
    {
        if(isDisappearing)
        {
            stateMachine.GetEnemyTransform().position = Vector3.MoveTowards(stateMachine.GetEnemyTransform().position, targetExit, deltaTime*5f);
            if(stateMachine.GetEnemyTransform().position == targetExit)
            {
                isDisappearing = false;
                stateMachine.StartCoroutine("WaitTime", 1f);
            }
            return;
        }

        if(isReentering)
        {
            stateMachine.GetEnemyTransform().position = Vector3.MoveTowards(stateMachine.GetEnemyTransform().position, targetEnter, deltaTime*5f);
            if(stateMachine.GetEnemyTransform().position == targetEnter)
            {
                OnSideChanged();
                stateMachine.ChangeToState(idleStateKey);
            }
        }
    }

    public override void ExitState()
    {
        isReentering = false;
        stateMachine.waitForTime.RemoveListener(OnWaitOver);
    }

    public void OnWaitOver()
    {
        if(isReentering)
        {
            return;
        }
        stateMachine.GetEnemyTransform().position = otherSideSpawn;
        stateMachine.GetEnemyTransform().eulerAngles = new Vector3(stateMachine.GetEnemyTransform().eulerAngles.x, stateMachine.GetEnemyTransform().eulerAngles.y-180*sideFactor, stateMachine.GetEnemyTransform().eulerAngles.z);
        isReentering = true;
    }

    public void OnSideChanged()
    {
        sideFactor *= -1f;
        changingSide.Invoke();
    }
}
