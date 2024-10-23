using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_LogLauncher_Idle<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_LogLauncher_Idle(EnemyState idle, EnemyState shoot, EnemyState charge, EnemyState change, int min, int max) : base(idle)
    {
        aimLogStateKey = shoot;
        aimChargeStateKey = charge;
        changeSidesStateKey = change;
        minIdle = min;
        maxIdle = max;
        idleCount = 0;
        idleLimit = UnityEngine.Random.Range(2, 3);
    }

    protected EnemyState aimLogStateKey;
    protected EnemyState aimChargeStateKey;
    protected EnemyState changeSidesStateKey;
    protected GameObject[] activePlayers;
    protected Transform closestPlayer;
    protected int minIdle, maxIdle;
    protected int idleCount, idleLimit;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        activePlayers = stateMachine.GetActivePlayers();
        stateMachine.waitForTime.AddListener(OnWaitOver);
        stateMachine.StartCoroutine("WaitTime", UnityEngine.Random.Range(minIdle, maxIdle));
    }

    public override void UpdateState(float deltaTime)
    {
        if(idleCount >= idleLimit)
        {
            Reset();
        }
        
        closestPlayer = GetClosestPlayer();

        if(Vector3.Distance(closestPlayer.position, stateMachine.GetEnemyTransform().position) <= 8f)
        {
            if(UnityEngine.Random.Range(1, 90) > Mathf.Pow(1.75f, Vector3.Distance(closestPlayer.position, stateMachine.GetEnemyTransform().position)))
            {
                stateMachine.StopCoroutine("WaitTime");
                stateMachine.ChangeToState(aimChargeStateKey);
            }
        }
    }

    public override void ExitState()
    {
        idleCount++;
        stateMachine.waitForTime.RemoveListener(OnWaitOver);
    }

    public void OnWaitOver()
    {
        stateMachine.ChangeToState(aimLogStateKey);
    }

    private Transform GetClosestPlayer()
    {
        Transform newTarget = activePlayers[0].transform;

        for(int i = 1; i < activePlayers.Length; i++)
        {
            if(Vector3.Distance(activePlayers[i].transform.position, stateMachine.GetEnemyTransform().position)
                < Vector3.Distance(newTarget.position, stateMachine.GetEnemyTransform().position))
            {
                newTarget = activePlayers[i].transform;
            }
        }

        return newTarget;
    }

    private void Reset()
    {
        idleCount = -1;
        idleLimit = UnityEngine.Random.Range(2, 3);
        stateMachine.ChangeToState(changeSidesStateKey);
    }
}
