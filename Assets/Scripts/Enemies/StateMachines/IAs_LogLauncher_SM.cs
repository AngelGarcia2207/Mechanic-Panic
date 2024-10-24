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
    [SerializeField] private float shotAimTime = 3f, chargeAimTime = 1f;
    private Vector3 initialRotation;
    private float sideFactor = 1;
    public MachineVoidEvent changeSides;

    void Awake()
    {
        machineStates.Add(LogLauncherStates.Idle,
        new IAs_LogLauncher_Idle<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.Idle, LogLauncherStates.AimLog, LogLauncherStates.AimCharge, LogLauncherStates.ChangeSides, minIdleTime, maxIdleTime));

        machineStates.Add(LogLauncherStates.AimLog,
        new IAs_Aim<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.AimLog, LogLauncherStates.Shoot, LogLauncherStates.Idle, turnSpeed*0.75f, shotAimTime, false));

        machineStates.Add(LogLauncherStates.Shoot,
        new IAs_Shoot<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.Shoot, LogLauncherStates.BackToInitialPose));

        machineStates.Add(LogLauncherStates.AimCharge,
        new IAs_Aim<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.AimCharge, LogLauncherStates.Charge, LogLauncherStates.Idle, turnSpeed, chargeAimTime));

        machineStates.Add(LogLauncherStates.Charge,
        new IAs_ChargeForward_Short<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.Charge, LogLauncherStates.BackToInitialPose, chargeStrenght));
        IAs_ChargeForward_Short<IAs_LogLauncher_SM.LogLauncherStates> temp = machineStates[LogLauncherStates.Charge] as IAs_ChargeForward_Short<IAs_LogLauncher_SM.LogLauncherStates>;
        changeSides.AddListener(temp.OnSideChanged);

        machineStates.Add(LogLauncherStates.BackToInitialPose,
        new IAs_ReturnToInitialPosition<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.BackToInitialPose, LogLauncherStates.Idle, turnSpeed*0.5f));
        IAs_ReturnToInitialPosition<IAs_LogLauncher_SM.LogLauncherStates> temp2 = machineStates[LogLauncherStates.BackToInitialPose] as IAs_ReturnToInitialPosition<IAs_LogLauncher_SM.LogLauncherStates>;
        changeSides.AddListener(temp2.OnSideChanged);

        machineStates.Add(LogLauncherStates.ChangeSides,
        new IAs_LogLauncher_ChangeSide<IAs_LogLauncher_SM.LogLauncherStates>(LogLauncherStates.ChangeSides, LogLauncherStates.Idle));
        IAs_LogLauncher_ChangeSide<IAs_LogLauncher_SM.LogLauncherStates> temp3 = machineStates[LogLauncherStates.ChangeSides] as IAs_LogLauncher_ChangeSide<IAs_LogLauncher_SM.LogLauncherStates>;
        temp3.changingSide.AddListener(OnChangingSides);

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

    public void OnChangingSides()
    {
        sideFactor *= -1f;
        changeSides.Invoke();
    }
}
