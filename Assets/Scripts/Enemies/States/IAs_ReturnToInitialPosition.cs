using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_ReturnToInitialPosition<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_ReturnToInitialPosition(EnemyState returning, EnemyState idle, Vector3 initialAngle, float speed) : base(returning)
    {
        idleStateKey = idle;
        targetAngle = initialAngle;
        turningSpeed = speed;
    }

    protected EnemyState idleStateKey;
    protected Vector3 targetAngle;
    protected float turningSpeed;

    public override void UpdateState(float deltaTime)
    {
        stateMachine.GetEnemyTransform().eulerAngles = Vector3.MoveTowards(stateMachine.GetEnemyTransform().eulerAngles, targetAngle, deltaTime*turningSpeed);
        if(stateMachine.GetEnemyTransform().eulerAngles == targetAngle)
        {
            stateMachine.ChangeToState(idleStateKey);
        }
    }
}
