using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_SpinAttack<EnemyState> : IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    public IAs_SpinAttack(EnemyState spin, EnemyState idle, EnemyState stunned) : base(spin)
    {
        idleStateKey = idle;
        stunnedStateKey = stunned;
    }

    protected EnemyState idleStateKey;
    protected EnemyState stunnedStateKey;
    protected Vector3 initialAngle, targetAngle;
    protected int count = 0;
    protected float acceleration = 1;

    public override void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM;
        stateMachine.audioClips.swayAudio();
        initialAngle = stateMachine.GetEnemyTransform().eulerAngles;
        if(initialAngle.y + 120 > 360)
        {
            targetAngle = new Vector3(0, initialAngle.y+120-360, 0);
        }
        else
        {
            targetAngle = new Vector3(0, initialAngle.y+120, 0);
        }

        try
        { stateMachine.GetCharacterAnimator().SetInteger("state", 3); }
        catch { }
    }

    public override void UpdateState(float deltaTime)
    {
        if(count == 3)
        {
            stateMachine.ChangeToState(idleStateKey);
            return;
        }

        initialAngle = stateMachine.GetEnemyTransform().eulerAngles;

        if(stateMachine.GetEnemyTransform().eulerAngles != targetAngle)
        {
            float angle = Mathf.MoveTowardsAngle(initialAngle.y, targetAngle.y, deltaTime*360f*acceleration);
            stateMachine.GetEnemyTransform().eulerAngles = new Vector3(0, angle, 0);
            acceleration *= 1.04f;
            return;
        }

        if(initialAngle.y + 120 > 360)
        {
            targetAngle = new Vector3(0, initialAngle.y+120-360, 0);
        }
        else
        {
            targetAngle = new Vector3(0, initialAngle.y+120, 0);
        }

        count++;
    }

    public override void ExitState()
    {
        count = 0;
        acceleration = 1;
    }
}
