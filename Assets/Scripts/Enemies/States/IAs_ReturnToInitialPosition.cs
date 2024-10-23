using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_ReturnToInitialPosition<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_ReturnToInitialPosition(EnemyState returning, EnemyState idle, float speed) : base(returning)
    {
        idleStateKey = idle;
        turningSpeed = speed;
    }

    protected EnemyState idleStateKey;
    protected Vector3 targetAngle = new Vector3(0, 270, 0);
    protected float turningSpeed;
    protected float sideFactor = 1;

    public override void UpdateState(float deltaTime)
    {
        stateMachine.GetEnemyTransform().eulerAngles = Vector3.MoveTowards(stateMachine.GetEnemyTransform().eulerAngles, targetAngle, deltaTime*turningSpeed);
        if(stateMachine.GetEnemyTransform().eulerAngles == targetAngle)
        {
            stateMachine.ChangeToState(idleStateKey);
        }
    }

    public void OnSideChanged()
    {
        targetAngle = new Vector3(0, targetAngle.y-180*sideFactor, 0);
        sideFactor *= -1;
    }
}
