using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for creating enemy behavior states //
public abstract class IAs_Enemy_State
{
    public virtual void EnterState()
    {
        // This function is called ONCE when the State Machine changes to this state //
    }

    public virtual void UpdateState()
    {
        // This function is CONSTANTLY called while it is active in the State Machine //
    }

    public virtual void ExitState()
    {
        // This function is called ONCE before the State Machine changes state //
    }
}
