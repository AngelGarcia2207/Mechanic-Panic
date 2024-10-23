using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_ChargeForward_Short<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_ChargeForward_Short(EnemyState charge, EnemyState idle, float strenght) : base(charge)
    {
        idleStateKey = idle;
        chargeStrenght = strenght;
        acceleration = strenght;
    }

    protected EnemyState idleStateKey;
    protected Transform targetedPlayer;
    protected Vector3 fixedTarget;
    protected Vector3 returnPosition;
    protected float chargeStrenght, acceleration;
    protected float sideFactor = 1;
    protected bool isPreparing, isCharging, isResting, isBacking;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {   
        stateMachine = SM;
        stateMachine.waitForTime.AddListener(OnWaitOver);
        stateMachine.StartCoroutine("WaitTime", 1f);
        returnPosition = stateMachine.GetEnemyTransform().position;

        GameObject[] activePlayers = stateMachine.GetActivePlayers();
        targetedPlayer = activePlayers[0].transform;

        for(int i = 1; i < activePlayers.Length; i++)
        {
            if(Vector3.Distance(activePlayers[i].transform.position, stateMachine.GetEnemyTransform().position)
                < Vector3.Distance(targetedPlayer.position, stateMachine.GetEnemyTransform().position))
            {
                targetedPlayer = activePlayers[i].transform;
            }
        }

        fixedTarget = new Vector3(targetedPlayer.position.x, stateMachine.GetEnemyTransform().position.y, targetedPlayer.position.z);

        isPreparing = true;
        isCharging = true;
        isResting = true;
        isBacking = true;
    }

    public override void UpdateState(float deltaTime)
    {
        if(isPreparing)
        {
            return;
        }

        if(isCharging)
        {
            stateMachine.GetEnemyTransform().position = Vector3.MoveTowards(stateMachine.GetEnemyTransform().position, fixedTarget, deltaTime*acceleration);
            if(Vector3.Distance(stateMachine.GetEnemyTransform().position, fixedTarget) == 0f || Vector3.Distance(stateMachine.GetEnemyTransform().position, returnPosition) >= 5f)
            {
                isCharging = false;
                stateMachine.StartCoroutine("WaitTime", 2f);
            }
            acceleration *= 1.05f;
            return;
        }

        if(isResting)
        {
            return;
        }

        if(isBacking)
        {
            stateMachine.GetEnemyTransform().position = Vector3.MoveTowards(stateMachine.GetEnemyTransform().position, returnPosition, deltaTime*chargeStrenght);
            if(Vector3.Distance(stateMachine.GetEnemyTransform().position, returnPosition) == 0f)
            {
                isBacking = false;
            }
            return;
        }

        stateMachine.ChangeToState(idleStateKey);
    }

    public override void ExitState()
    {
        stateMachine.waitForTime.RemoveListener(OnWaitOver);
    }

    public void OnWaitOver()
    {
        if(isPreparing)
        {
            isPreparing = false;
        }
        else
        {
            isResting = false;
        }
    }

    public void OnSideChanged()
    {
        sideFactor *= -1;
    }
}
