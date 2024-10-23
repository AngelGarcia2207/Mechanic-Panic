using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class StateVoidEvent : UnityEvent {}

// Base class for creating enemy behavior states //
public abstract class IAs_Enemy_State<EnemyState> where EnemyState : Enum
{
    // Base constructor //
    public IAs_Enemy_State(EnemyState newKey)
    {
        baseKey = newKey;
    }

    // Main key to identify this state //
    protected EnemyState baseKey;
    // Reference to the state machine using this state instance //
    protected IAs_Enemy_State_Machine<EnemyState> stateMachine;

    public virtual void EnterState(IAs_Enemy_State_Machine<EnemyState> SM)
    {
        stateMachine = SM; // <- Don't forget to add this line //

        // This function is called ONCE when the state machine changes to this state //
    }

    public virtual void UpdateState(float deltaTime)
    {
        // This function is CONSTANTLY called while it is active in the state machine //
    }

    public virtual void ExitState()
    {
        // This function is called ONCE before the state machine changes state //
    }

    public virtual void TriggerEnter(Collider other)
    {
        // This function is called ONCE when a "OnTriggerEnter" is called in the state machine //
    }

    public virtual void TriggerExit(Collider other)
    {
        // This function is called ONCE when a "OnTriggerExit" is called in the state machine //
    }
}