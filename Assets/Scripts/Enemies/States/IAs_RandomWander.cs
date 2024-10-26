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
        hasEnoughTimePassed = false;
        awaitingNext = false;
        stateMachine.waitForTime.AddListener(OnWaitOver);
        stateMachine.StartCoroutine("WaitTime", 2f);
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
        /*Debug.Log(center);
        Debug.Log(new Vector3(currentPosition.x+newX, currentPosition.y, currentPosition.z+newZ));*/

        /*while(currentPosition.x + newX > center.x + 7 || currentPosition.x + newX < center.x - 7)
        {
            newX = UnityEngine.Random.Range(0, 5)-2;
        }
        while(currentPosition.z + newZ > center.z + 4.5f || currentPosition.z + newZ < center.z - 4.5f)
        {
            newZ = UnityEngine.Random.Range(0, 5)-2;
        }*/

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
