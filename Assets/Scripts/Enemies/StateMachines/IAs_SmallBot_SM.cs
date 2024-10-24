using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_SmallBot_SM : IAs_Enemy_State_Machine<IAs_SmallBot_SM.SmallBotStates>
{
    public enum SmallBotStates
    {
        Idle,
        Charge,
        Spin,
        Stunned
    }

    [SerializeField] float chargeStrenght;
    [SerializeField] float stunTime;

    void Awake()
    {
        machineStates.Add(SmallBotStates.Idle,
        new IAs_RandomWander<IAs_SmallBot_SM.SmallBotStates>(SmallBotStates.Idle, SmallBotStates.Charge, SmallBotStates.Spin));

        machineStates.Add(SmallBotStates.Charge,
        new IAs_ChargeForward_Long<IAs_SmallBot_SM.SmallBotStates>(SmallBotStates.Charge, SmallBotStates.Idle, SmallBotStates.Stunned, chargeStrenght));

        machineStates.Add(SmallBotStates.Spin,
        new IAs_SpinAttack<IAs_SmallBot_SM.SmallBotStates>(SmallBotStates.Spin, SmallBotStates.Idle, SmallBotStates.Stunned));

        machineStates.Add(SmallBotStates.Stunned,
        new IAs_Stunned<IAs_SmallBot_SM.SmallBotStates>(SmallBotStates.Stunned, SmallBotStates.Idle, stunTime));

        currentState = machineStates[SmallBotStates.Idle];
    }
}
