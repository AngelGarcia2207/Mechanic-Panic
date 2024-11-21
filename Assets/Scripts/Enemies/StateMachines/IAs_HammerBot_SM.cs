using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_HammerBot_SM : IAs_Enemy_State_Machine<IAs_HammerBot_SM.HammerBotStates>
{
    public enum HammerBotStates
    {
        Idle,
        Attack,
        Spin,
        Stunned
    }

    [SerializeField] public float AttackStrenght = 7f;
    [SerializeField] public float stunTime = 3f;
    [SerializeField] private Vector3 center;

    void Awake()
    {
        /*machineStates.Add(HammerBotStates.Idle,
        new IAs_RandomWander<IAs_HammerBot_SM.HammerBotStates>(HammerBotStates.Idle, HammerBotStates.Attack, HammerBotStates.Spin));
        IAs_RandomWander<IAs_HammerBot_SM.HammerBotStates> temp = machineStates[HammerBotStates.Idle] as IAs_RandomWander<IAs_HammerBot_SM.HammerBotStates>;
        temp.SetCenter(center);

        machineStates.Add(HammerBotStates.Attack,
        new IAs_AttackForward_Long<IAs_HammerBot_SM.HammerBotStates>(HammerBotStates.Attack, HammerBotStates.Idle, HammerBotStates.Stunned, AttackStrenght));

        machineStates.Add(HammerBotStates.Spin,
        new IAs_SpinAttack<IAs_HammerBot_SM.HammerBotStates>(HammerBotStates.Spin, HammerBotStates.Idle, HammerBotStates.Stunned));

        machineStates.Add(HammerBotStates.Stunned,
        new IAs_Stunned<IAs_HammerBot_SM.HammerBotStates>(HammerBotStates.Stunned, HammerBotStates.Idle, stunTime));

        currentState = machineStates[HammerBotStates.Idle];*/
    }

    public void UpdateCenter(Vector3 center)
    {
        IAs_RandomWander<IAs_HammerBot_SM.HammerBotStates> temp = machineStates[HammerBotStates.Idle] as IAs_RandomWander<IAs_HammerBot_SM.HammerBotStates>;
        temp.SetCenter(center);
    }
}
