using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Máquina de estados
public partial class PlayerStateMachine
{
	public PlayerState idle;
	public PlayerState move;
	public PlayerState jump;
	public PlayerState jumpMove;
	public PlayerState dodge;
	public PlayerState attack;
	public PlayerState moveAttack;
	public PlayerState hold;
	public PlayerState stunned;

	private PlayerState currentState;
	
	public Animator spriteAnimator;

	public PlayerStateMachine(Animator spriteAnimator)
    {
		idle = new PlayerIdle(spriteAnimator);
		move = new PlayerMove(spriteAnimator);
		jump = new PlayerJump(spriteAnimator);
		jumpMove = new PlayerJumpMove(spriteAnimator);
		dodge = new PlayerDodge(spriteAnimator);
		attack = new PlayerAttack(spriteAnimator);
		moveAttack = new PlayerMoveAttack(spriteAnimator);
		hold = new PlayerHold(spriteAnimator);
		stunned = new PlayerStunned(spriteAnimator);

        idle.SetPossibleTransitions(new List<PlayerState> { idle, move, jump, dodge, attack, hold, stunned });
        move.SetPossibleTransitions(new List<PlayerState> { idle, move, jump, dodge, moveAttack, hold, stunned });
        jump.SetPossibleTransitions(new List<PlayerState> { idle, jumpMove, dodge, hold, stunned });
		jumpMove.SetPossibleTransitions(new List<PlayerState> { idle, move, jumpMove, dodge, hold, stunned });
        dodge.SetPossibleTransitions(new List<PlayerState>()); // Al esquivar, no puede transicionar a otros estados
        attack.SetPossibleTransitions(new List<PlayerState> { dodge, stunned });
		moveAttack.SetPossibleTransitions(new List<PlayerState> { dodge, stunned });
        hold.SetPossibleTransitions(new List<PlayerState> { move, jump, dodge, stunned });
        stunned.SetPossibleTransitions(new List<PlayerState>()); // Al estar aturdido, no puede transicionar a otros estados

        currentState = idle;

		this.spriteAnimator = spriteAnimator;
    }

	// Comprobar si una transión es válida
	public bool AvailableTransition(PlayerState state)
	{
		if (currentState.IsAPossibleTransition(state))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	// Cambiar de estado, si es que el actual permite la transición
	public void ChangeState(PlayerState state)
	{
		if (currentState.IsAPossibleTransition(state))
		{
			currentState.Exit();
			state.Enter();
			currentState = state;
		}
	}
	
	// Regresar a Idle
	public void returnToIdle()
	{
		currentState.Exit();
		currentState = idle;
		currentState.Enter();
	}
}

// Clase de estado base, de la cual heredan y sobreescriben
public partial class PlayerState
{
    private List<PlayerState> possibleTransitions;

	public Animator spriteAnimator;

	public PlayerState(Animator spriteAnimator)
    {
		this.spriteAnimator = spriteAnimator;
    }

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
    public PlayerIdle(Animator spriteAnimator) : base(spriteAnimator)
    {
    }
}

public partial class PlayerMove : PlayerState
{
    public PlayerMove(Animator spriteAnimator) : base(spriteAnimator)
    {
    }

	public override void Enter()
    {
        spriteAnimator.SetBool("IsMoving", true);
    }

	public override void Exit()
    {
        spriteAnimator.SetBool("IsMoving", false);
    }
}

public partial class PlayerJump : PlayerState
{
    public PlayerJump(Animator spriteAnimator) : base(spriteAnimator)
    {
    }

	public override void Enter()
    {
        spriteAnimator.SetBool("IsJumping", true);
    }

	public override void Exit()
    {
        spriteAnimator.SetBool("IsJumping", false);
    }
}

public partial class PlayerJumpMove : PlayerState
{
    public PlayerJumpMove(Animator spriteAnimator) : base(spriteAnimator)
    {
    }

	public override void Enter()
    {
        spriteAnimator.SetBool("IsJumping", true);
    }

	public override void Exit()
    {
        spriteAnimator.SetBool("IsJumping", false);
    }
}

public partial class PlayerDodge : PlayerState
{
    public PlayerDodge(Animator spriteAnimator) : base(spriteAnimator)
    {
    }
}

public partial class PlayerAttack : PlayerState
{
    public PlayerAttack(Animator spriteAnimator) : base(spriteAnimator)
    {
    }
}

public partial class PlayerMoveAttack : PlayerState
{
    public PlayerMoveAttack(Animator spriteAnimator) : base(spriteAnimator)
    {
    }
}

public partial class PlayerHold : PlayerState
{
    public PlayerHold(Animator spriteAnimator) : base(spriteAnimator)
    {
    }
}

public partial class PlayerStunned : PlayerState
{
    public PlayerStunned(Animator spriteAnimator) : base(spriteAnimator)
    {
    }
}