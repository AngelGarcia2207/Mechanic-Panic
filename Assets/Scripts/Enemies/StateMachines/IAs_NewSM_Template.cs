using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In the parameters of the base state machine, use the enum you defined for THIS state machine //
// ANDD DON'T FORGET "using System" //
public class IAs_NewSM_Template : IAs_Enemy_State_Machine<IAs_NewSM_Template.ExampleEnum>
{
    // Create an enum with the names of all the needed states for this state machine //
    public enum ExampleEnum
    {
        State1,
        State2,
        State3,
        State4
    }

    // Add some references here if necessary

    // Define the starting state in this function //
    void Awake()
    {
        // Add all the necessary states to machineStates //

                                     // Change this //
        currentState = machineStates[ExampleEnum.State1];
    }
}
