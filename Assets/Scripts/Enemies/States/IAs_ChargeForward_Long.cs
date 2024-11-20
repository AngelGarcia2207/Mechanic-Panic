using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_ChargeForward_Long<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_ChargeForward_Long(EnemyState charge, EnemyState idle, EnemyState stun, float speed) : base(charge)
    {
        idleStateKey = idle;
        stunStateKey = stun;
        chargeStrenght = speed;
    }

    protected EnemyState idleStateKey;
    protected EnemyState stunStateKey;
    protected GameObject[] activePlayers;
    protected Transform targetedPlayer;
    protected Vector3 fixedTarget;
    protected float chargeStrenght;
    protected float sideFactor = 1;
    protected bool isPreparing, isCharging, isResting;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {   
        stateMachine = SM;
        activePlayers = stateMachine.GetActivePlayers();
        stateMachine.waitForTime.AddListener(OnWaitOver);
        stateMachine.StartCoroutine("WaitTime", 1f);
        targetedPlayer = activePlayers[0].transform;

        for(int i = 1; i < activePlayers.Length; i++)
        {
            if(Vector3.Distance(activePlayers[i].transform.position, stateMachine.GetEnemyTransform().position)
                < Vector3.Distance(targetedPlayer.position, stateMachine.GetEnemyTransform().position))
            {
                targetedPlayer = activePlayers[i].transform;
            }
        }

        isPreparing = true;
        isCharging = true;
        isResting = true;

        try
        { stateMachine.GetCharacterAnimator().SetInteger("state", 1); }
        catch { }
    }

    public override void UpdateState(float deltaTime)
    {
        if(isPreparing)
        {
            LookAtPlayer(deltaTime);
            return;
        }

        if(isCharging)
        {
            stateMachine.GetEnemyTransform().position = Vector3.MoveTowards(stateMachine.GetEnemyTransform().position, fixedTarget, deltaTime*chargeStrenght);
            if(stateMachine.GetEnemyTransform().position == fixedTarget)
            {
                isCharging = false;
                stateMachine.StartCoroutine("WaitTime", 1f);
            }
            return;
        }

        if(isResting)
        {
            return;
        }

        stateMachine.ChangeToState(idleStateKey);
    }

    public override void ExitState()
    {
        stateMachine.waitForTime.RemoveListener(OnWaitOver);
    }

    public override void CollisionEnter(Collision collision)
    {
        if(isCharging)
        {
            isCharging = false;
            stateMachine.StartCoroutine("WaitTime", 1f);
        }
    }

    public void LookAtPlayer(float deltaTime)
    {
        //Debug.Log("look");
        Transform newTarget = activePlayers[0].transform;

        for(int i = 1; i < activePlayers.Length; i++)
        {
            if(Vector3.Distance(activePlayers[i].transform.position, stateMachine.GetEnemyTransform().position)
                < Vector3.Distance(newTarget.position, stateMachine.GetEnemyTransform().position))
            {
                newTarget = activePlayers[i].transform;
            }
        }

        Vector3 initialAngle = stateMachine.GetEnemyTransform().eulerAngles;
        stateMachine.GetEnemyTransform().LookAt(newTarget);
        Vector3 targetAngle = new Vector3(0, stateMachine.GetEnemyTransform().eulerAngles.y, 0);
        float angle = Mathf.MoveTowardsAngle(initialAngle.y, targetAngle.y, deltaTime*210f);
        stateMachine.GetEnemyTransform().eulerAngles = new Vector3(0, angle, 0);
    }

    public void OnWaitOver()
    {
        if(isPreparing)
        {
            fixedTarget = new Vector3(targetedPlayer.position.x, stateMachine.GetEnemyTransform().position.y, targetedPlayer.position.z);
            isPreparing = false;
        }
        else
        {
            isResting = false;
        }
    }
}