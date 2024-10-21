using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Use this event to call void functions in any state //
[System.Serializable] public class MachineVoidEvent : UnityEvent {}

// This state machine specifically handles states of type "IAs_Enemy_State" //
public abstract class IAs_Enemy_State_Machine<EnemyState> : MonoBehaviour where EnemyState : Enum
{
    // Dictionary with all available states and their keys //
    protected Dictionary<EnemyState, IAs_Enemy_State<EnemyState>> machineStates = new();
    // Current state script //
    protected IAs_Enemy_State<EnemyState> currentState;
    // Main script of the enemy that owns this state machine //
    [SerializeField] protected Ene_EnemyTest owner;
    // Main animator of the enemy //
    [SerializeField] protected Animator characterAnimator;
    // Control booleans //
    private bool isMachinePaused = false, isTransitioning = false;
    // Void event for wait time //
    public MachineVoidEvent waitForTime;

    // This function initializes the starting state //
    void Start()
    {
        currentState.EnterState(this);
    }
    
    // This function processes the current state every frame or changes to a new state //
    void Update()
    {
        if(isMachinePaused == false && isTransitioning == false)
        {
            currentState.UpdateState(Time.deltaTime);
        }
    }

    // Use this function to end current state and initialize next state //
    public void ChangeToState(EnemyState newStateKey)
    {
        isTransitioning = true;
        if(currentState != null)
        {
            currentState.ExitState();
        }
        currentState = machineStates[newStateKey];
        currentState.EnterState(this);
        isTransitioning = false;
    }

    // Use this function to call a "TriggerEnter" response in current state //
    void OnTriggerEnter(Collider other)
    {
        currentState.TriggerEnter(other);
    }

    // Use this function to call a "TriggerExit" response in current state //
    void OnTriggerExit(Collider other)
    {
        currentState.TriggerExit(other);
    }

    // Use this function to create an object and do something with it //
    public virtual void InstantiateObject()
    {
        // Do something with an the object //
    }

    public GameObject[] GetActivePlayers()
    {
        return GameObject.FindGameObjectsWithTag("Player");
    }

    public Transform GetEnemyTransform()
    {
        return owner.transform;
    }

    // Use this function to pause current state. If a parameter is given, the machine will pause for the given time (in seconds) //
    // and then resume automatically                                                                                             //
    public void PauseMachine(float pauseTime = 0f)
    {
        isMachinePaused = true;
        if(pauseTime > 0f)
        {
            StartCoroutine(DelayResume(pauseTime));
        }
    }
    
    // Use this function to resume a paused machine //
    public void ResumeMachine()
    {
        isMachinePaused = false;
    }

    // Coroutine for the automatic resume //
    private IEnumerator DelayResume(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResumeMachine();
    }

    // Use this function to wait in the current state WITHOUT stopping it //
    public IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        waitForTime.Invoke();
    }
}
