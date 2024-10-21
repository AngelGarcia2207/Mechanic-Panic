using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_Aim<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_Aim(EnemyState aim, EnemyState attack, EnemyState idle, float speed, bool aimingClose = true) : base(aim)
    {
        attackStateKey = attack;
        isAimingToClosest = aimingClose;
        turnSpeed = speed;
    }

    protected EnemyState attackStateKey;
    protected EnemyState idle;
    protected GameObject[] activePlayers;
    protected Transform targetPlayer;
    protected float turnSpeed;
    private bool isAimingToClosest;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        activePlayers = stateMachine.GetActivePlayers();
        if(activePlayers.Length <= 0)
        {
            stateMachine.ChangeToState(idle);
        }
        stateMachine.waitForTime.AddListener(OnAttackReady);
        if(isAimingToClosest)
        {
            stateMachine.StartCoroutine("WaitTime", 2f);
        }
        else
        {
            stateMachine.StartCoroutine("WaitTime", 3f);
        }
    }

    public override void UpdateState(float deltaTime)
    {
        targetPlayer = GetTargetPlayer();
        Vector3 initialAngle = stateMachine.GetEnemyTransform().eulerAngles;
        stateMachine.GetEnemyTransform().LookAt(targetPlayer);
        Vector3 targetAngle = new Vector3(0, stateMachine.GetEnemyTransform().eulerAngles.y, 0);
        stateMachine.GetEnemyTransform().eulerAngles = Vector3.MoveTowards(initialAngle, targetAngle, deltaTime*turnSpeed);
        //stateMachine.GetEnemyTransform().eulerAngles = new Vector3(0, stateMachine.GetEnemyTransform().eulerAngles.y, 0);
        //Debug.Log("Aim");
    }

    public override void ExitState()
    {
        stateMachine.waitForTime.RemoveListener(OnAttackReady);
    }

    public void OnAttackReady()
    {
        stateMachine.ChangeToState(attackStateKey);
    }

    private Transform GetTargetPlayer()
    {
        Transform newTarget = activePlayers[0].transform;

        for(int i = 1; i < activePlayers.Length; i++)
        {
            if(isAimingToClosest)
            {
                if(Vector3.Distance(activePlayers[i].transform.position, stateMachine.GetEnemyTransform().position)
                    < Vector3.Distance(newTarget.position, stateMachine.GetEnemyTransform().position))
                {
                    newTarget = activePlayers[i].transform;
                }
            }
            else
            {
                if(Vector3.Distance(activePlayers[i].transform.position, stateMachine.GetEnemyTransform().position)
                    > Vector3.Distance(newTarget.position, stateMachine.GetEnemyTransform().position))
                {
                    newTarget = activePlayers[i].transform;
                }
            }
        }

        return newTarget;
    }
}
