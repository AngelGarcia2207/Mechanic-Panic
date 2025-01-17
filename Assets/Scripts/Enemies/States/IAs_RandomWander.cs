using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_RandomWander<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_RandomWander(EnemyState idle, EnemyState charge, EnemyState spin) : base(idle)
    {
        chargeStateKey = charge;
        spinStateKey = spin;
    }

    protected EnemyState chargeStateKey;
    protected EnemyState spinStateKey;
    protected GameObject[] activePlayers;
    protected int numberOfWanders;
    protected Vector3 targetPosition, center;
    protected bool hasEnoughTimePassed, awaitingNext;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        activePlayers = stateMachine.GetActivePlayers();
        numberOfWanders = UnityEngine.Random.Range(2, 5);
        targetPosition = GetNewPosition(stateMachine.GetEnemyTransform().position);
        awaitingNext = true;
        stateMachine.waitForTime.AddListener(OnWaitOver);
        stateMachine.StartCoroutine("WaitTime", 1.5f);

        try
        { stateMachine.GetCharacterAnimator().SetInteger("state", 2); }
        catch { }
    }

    public override void UpdateState(float deltaTime)
    {
        activePlayers = stateMachine.GetActivePlayers();
        if(activePlayers.Length == 0)
        {
            return;
        }
        LookAtPlayer(deltaTime);

        if(awaitingNext)
        {
            return;
        }
        
        if(numberOfWanders == 0)
        {
            if(UnityEngine.Random.Range(0, 3) == 0)
            {
                stateMachine.ChangeToState(spinStateKey);
            }
            else
            {
                stateMachine.ChangeToState(chargeStateKey);
            }
            awaitingNext = true;
            return;
        }
        
        if(stateMachine.GetEnemyTransform().position != targetPosition && hasEnoughTimePassed == false)
        {
            stateMachine.GetEnemyTransform().position = Vector3.MoveTowards(stateMachine.GetEnemyTransform().position, targetPosition, deltaTime*3f);
            return;
        }

        stateMachine.StopAllCoroutines();
        hasEnoughTimePassed = true;
        awaitingNext = true;
        numberOfWanders--;
        stateMachine.StartCoroutine("WaitTime", 1.5f);
    }

    public override void ExitState()
    {
        numberOfWanders = 0;
        awaitingNext = false;
        hasEnoughTimePassed = false;
        stateMachine.waitForTime.RemoveListener(OnWaitOver);
    }

    private Vector3 GetNewPosition(Vector3 currentPosition)
    {
        float newX = UnityEngine.Random.Range(0, 5)-2;
        float newZ = UnityEngine.Random.Range(0, 5)-2;
        float dis = Mathf.Sqrt(newX*newX + newZ*newZ);

        int layerMask = LayerMask.GetMask("Default") | LayerMask.GetMask("PlayerBoundary") | LayerMask.GetMask("Enemy");
        
        for(int i = 0; i < 100; i++)
        {
            Debug.DrawRay(currentPosition, new Vector3(newX, 0, newZ), Color.yellow, 2);

            if(Physics.Raycast(currentPosition, new Vector3(newX, 0, newZ), dis, layerMask) == false)
            {
                break;
            }

            newX = UnityEngine.Random.Range(0, 5)-2;
            newZ = UnityEngine.Random.Range(0, 5)-2;
            dis = Mathf.Sqrt(newX*newX + newZ*newZ);
        }

        return new Vector3(currentPosition.x+newX, currentPosition.y, currentPosition.z+newZ);
    }

    public void LookAtPlayer(float deltaTime)
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

        Vector3 initialAngle = stateMachine.GetEnemyTransform().eulerAngles;
        stateMachine.GetEnemyTransform().LookAt(newTarget);
        Vector3 targetAngle = new Vector3(0, stateMachine.GetEnemyTransform().eulerAngles.y, 0);
        float angle = Mathf.MoveTowardsAngle(initialAngle.y, targetAngle.y, deltaTime*210f);
        stateMachine.GetEnemyTransform().eulerAngles = new Vector3(0, angle, 0);
    }

    public void OnWaitOver()
    {
        if(awaitingNext)
        {
            awaitingNext = false;
            hasEnoughTimePassed = false;
            targetPosition = GetNewPosition(stateMachine.GetEnemyTransform().position);
            stateMachine.StartCoroutine("WaitTime", 1f);
        }
        else
        {
            hasEnoughTimePassed = true;
        }
    }

    public void SetCenter(Vector3 center)
    {
        this.center = center;
    }
}
