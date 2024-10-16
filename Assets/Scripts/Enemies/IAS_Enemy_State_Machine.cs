using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This state machine specifically handles states of type "IAs_Enemy_State" //
public class IAS_Enemy_State_Machine : MonoBehaviour
{
    [SerializeField] private IAs_Enemy_State startingState;
    [SerializeField] private IAs_Enemy_State currentState;
    [SerializeField] private GameObject owner;
    private bool isMachinePaused;

    void Start()
    {
        ChangeToState(startingState);
    }
    
    void Update()
    {
        if(isMachinePaused == false)
        {
            currentState.UpdateState();
        }
    }

    // Use this function to end current state and initialize next state //
    public void ChangeToState(IAs_Enemy_State newState)
    {
        if(currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    // Use this function to pause current state. If a parameter is given, the machine will pause for the given time (in seconds) //
    // and then resume automatically                                                                                             //
    public void PauseMachine(float pauseTime = 0f)
    {
        if(pauseTime <= 0f)
        {
            isMachinePaused = true;
        }
        else
        {
            isMachinePaused = true;
            StartCoroutine(DelayResume(pauseTime));
        }
    }
    
    // Use this function to resume a paused machine //
    public void ResumeMachine() { isMachinePaused = false;}

    // Coroutine for the automatic resume //
    IEnumerator DelayResume(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResumeMachine();
    }
}
