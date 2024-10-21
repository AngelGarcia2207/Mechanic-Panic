using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAs_LogLauncher_SM : IAs_Enemy_State_Machine<IAs_LogLauncher_SM.LogLauncherStates>
{
    public enum LogLauncherStates
    {
        Idle,
        AimLog,
        Shoot,
        AimCharge,
        Charge,
        BackToInitialPose,
        ChangeSides
    }

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int minProjectileStrenght = 750, maxProjectileStrenght = 1250;
    [SerializeField] private float chargeStrenght = 5f, turnSpeed = 60f;
    [SerializeField] private int minIdleTime = 2, maxIdleTime = 5;
    private Vector3 initialRotation;

    void Awake()
    {
        machineStates.Add(LogLauncherStates.Idle,
        new IAs_LogLauncher_Idle<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.Idle, LogLauncherStates.AimLog, LogLauncherStates.AimCharge, minIdleTime, maxIdleTime));

        machineStates.Add(LogLauncherStates.AimLog,
        new IAs_Aim<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.AimLog, LogLauncherStates.Shoot, LogLauncherStates.Idle, turnSpeed*0.75f, false));

        machineStates.Add(LogLauncherStates.Shoot,
        new IAs_Shoot<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.Shoot, LogLauncherStates.BackToInitialPose));

        machineStates.Add(LogLauncherStates.AimCharge,
        new IAs_Aim<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.AimCharge, LogLauncherStates.Charge, LogLauncherStates.Idle, turnSpeed));

        machineStates.Add(LogLauncherStates.Charge,
        new IAs_ChargeForward_Short<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.Charge, LogLauncherStates.BackToInitialPose, chargeStrenght));

        initialRotation = owner.transform.eulerAngles;
        machineStates.Add(LogLauncherStates.BackToInitialPose,
        new IAs_ReturnToInitialPosition<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.BackToInitialPose, LogLauncherStates.Idle, initialRotation, turnSpeed*0.5f));

        // change sides

        currentState = machineStates[LogLauncherStates.Idle];
    }
    
    public override void InstantiateObject()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, this.transform.position, new Quaternion(0, 0, 0, 0));
        newProjectile.transform.Rotate(new Vector3(90f, 90f, -owner.transform.eulerAngles.y-90f));
        newProjectile.transform.Translate(new Vector3(0f, -3f, -2f));
        float shotStrenght = Random.Range(minProjectileStrenght, maxProjectileStrenght);
        newProjectile.GetComponent<Rigidbody>().AddForce(new Vector3(-shotStrenght*Mathf.Sin(Mathf.Deg2Rad*newProjectile.transform.eulerAngles.y),
        0, -shotStrenght*Mathf.Cos(Mathf.Deg2Rad*newProjectile.transform.eulerAngles.y)));
        Destroy(newProjectile, 3f);
    }
}
