using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Máquina de estados
public partial class PlayerStateMachine
{
	public PlayerState idleState = new PlayerIdle();
	public PlayerState walkingState = new PlayerWalking();
	public PlayerState jumpingState = new PlayerJumping();
	public PlayerState dodgingState = new PlayerDodging();
	public PlayerState attackingState = new PlayerAttacking();
	public PlayerState holdingState = new PlayerHolding();
	public PlayerState stunnedState = new PlayerStunned();

	private PlayerState currentState;

	public PlayerStateMachine()
    {
        idleState.SetPossibleTransitions(new List<PlayerState> { walkingState, jumpingState, dodgingState, attackingState, holdingState, stunnedState });
        walkingState.SetPossibleTransitions(new List<PlayerState> { idleState, jumpingState, dodgingState, attackingState, holdingState, stunnedState });
        jumpingState.SetPossibleTransitions(new List<PlayerState> { idleState, dodgingState, holdingState, stunnedState });
        dodgingState.SetPossibleTransitions(new List<PlayerState>()); // Al esquivar, no puede transicionar a otros estados
        attackingState.SetPossibleTransitions(new List<PlayerState> { dodgingState, stunnedState });
        holdingState.SetPossibleTransitions(new List<PlayerState> { walkingState, jumpingState, dodgingState, stunnedState });
        stunnedState.SetPossibleTransitions(new List<PlayerState>()); // Al estar aturdido, no puede transicionar a otros estados

        currentState = idleState;
    }

	// Cambiar de estado, si es que el actual permite la transición
	public bool ChangeState(PlayerState state)
	{
		if (currentState.IsAPossibleTransition(state))
		{
			currentState.Exit();
			state.Enter();
			currentState = state;

			Debug.Log(currentState);

			return true;
		}
		else
		{
			return false;
		}
	}

	// Regresar a Idle
	public void returnToIdle()
	{
		currentState.Exit();
		currentState = idleState;
		currentState.Enter();

		Debug.Log(currentState);
	}
}

// Clase de estado base, de la cual heredan y sobreescriben
public partial class PlayerState
{
    private List<PlayerState> possibleTransitions;

	// Método para verificar si una transición es posible
	virtual public bool IsAPossibleTransition(PlayerState state)
	{
		if (possibleTransitions.Contains(state))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	virtual public void SetPossibleTransitions(List<PlayerState> statesList)
	{
		possibleTransitions = statesList;
	}

	virtual public void Enter()
	{
		return;
	}

	virtual public void Exit()
	{
		return;
	}

	virtual public void SMUpdate()
    {
        return;
    }
}

public partial class PlayerIdle : PlayerState
{

}

public partial class PlayerWalking : PlayerState
{

}

public partial class PlayerJumping : PlayerState
{

}

public partial class PlayerDodging : PlayerState
{

}

public partial class PlayerAttacking : PlayerState
{

}

public partial class PlayerHolding : PlayerState
{

}

public partial class PlayerStunned : PlayerState
{

}