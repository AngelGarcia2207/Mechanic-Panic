using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Máquina de estados
public partial class PlayerStateMachine : MonoBehaviour
{
	public PlayerState idle;
	public PlayerState move;
	public PlayerState jump;
	public PlayerState jumpMove;
	public PlayerState dodge;
	public PlayerState grab;
	public PlayerState attack;
	public PlayerState moveAttack;
	public PlayerState hold;
	public PlayerState hurl;
	public PlayerState parry;
	public PlayerState stunned;
	public PlayerState dead;

	private PlayerState currentState;
	
	public Animator animator;

	public void Initialize(Animator animator)
    {
		idle = new PlayerIdle(animator);
		move = new PlayerMove(animator);
		jump = new PlayerJump(animator);
		jumpMove = new PlayerJumpMove(animator);
		dodge = new PlayerDodge(animator);
		grab = new PlayerGrab(animator);
		attack = new PlayerAttack(animator);
		moveAttack = new PlayerMoveAttack(animator);
		hold = new PlayerHold(animator);
		hurl = new PlayerHurl(animator);
		parry = new PlayerParry(animator);
		stunned = new PlayerStunned(animator);
		dead = new PlayerDead(animator);

        idle.SetPossibleTransitions(new List<PlayerState> { idle, move, jump, dodge, grab, attack, hold, stunned, dead });
        move.SetPossibleTransitions(new List<PlayerState> { idle, move, jump, dodge, grab, moveAttack, hold, stunned, dead });
        jump.SetPossibleTransitions(new List<PlayerState> { idle, jump, jumpMove, dodge, hold, stunned, dead});
		jumpMove.SetPossibleTransitions(new List<PlayerState> { idle, move, jumpMove, dodge, hold, stunned, dead });
        dodge.SetPossibleTransitions(new List<PlayerState>()); // Al esquivar, no puede transicionar a otros estados
		grab.SetPossibleTransitions(new List<PlayerState> { dodge, stunned, dead });
        attack.SetPossibleTransitions(new List<PlayerState> { dodge, stunned, dead });
		moveAttack.SetPossibleTransitions(new List<PlayerState> { idle, dodge, stunned, dead });
        hold.SetPossibleTransitions(new List<PlayerState> { hurl, dodge, stunned, dead });
		hurl.SetPossibleTransitions(new List<PlayerState> { dodge, stunned, dead });
		parry.SetPossibleTransitions(new List<PlayerState>()); // Al hacer parry, no puede transicionar a otros estados
        stunned.SetPossibleTransitions(new List<PlayerState> { dead }); // Al estar aturdido, no puede transicionar a otros estados
		dead.SetPossibleTransitions(new List<PlayerState>()); // Al estar muerto, no puede transicionar a otros estados

        currentState = idle;

		this.animator = animator;
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
		if (currentState.IsAPossibleTransition(state) && currentState != state)
		{
			StopAllCoroutines();
			currentState.Exit();
			currentState = state;
			currentState.Enter();
		}
	}

	// Cambiar de estado, con una duración antes de regresar a Idle
	public void ChangeState(PlayerState state, float delay)
	{
		if (currentState.IsAPossibleTransition(state) && currentState != state)
		{
			StopAllCoroutines();
			currentState.Exit();
			currentState = state;
			currentState.Enter();
			StartCoroutine(ActionDelay(delay));
		}
	}
	
	// Regresar a Idle
	public void ReturnToIdle()
	{
		StopAllCoroutines();
		currentState.Exit();
		currentState = idle;
		currentState.Enter();
	}

	IEnumerator ActionDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
        ReturnToIdle();
	}

	public PlayerState GetCurrentState()
	{
		return currentState;
	}
}

// Clase de estado base, de la cual heredan y sobreescriben
public partial class PlayerState
{
    private List<PlayerState> possibleTransitions;

	public Animator animator;

	public PlayerState(Animator animator)
    {
		this.animator = animator;
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

	public void ChangeAnimation(string animation)
	{
		animator.CrossFade(animation, 0.05f);
	}
}

public partial class PlayerIdle : PlayerState
{
    public PlayerIdle(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("idle");
    }
}

public partial class PlayerMove : PlayerState
{
    public PlayerMove(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("move");
    }
}

public partial class PlayerJump : PlayerState
{
    public PlayerJump(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("jump");
    }
}

public partial class PlayerJumpMove : PlayerState
{
    public PlayerJumpMove(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("jump");
    }
}

public partial class PlayerDodge : PlayerState
{
    public PlayerDodge(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("dodge");
    }
}

public partial class PlayerGrab : PlayerState
{
    public PlayerGrab(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("grab");
    }
}

public partial class PlayerAttack : PlayerState
{
    public PlayerAttack(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("attack");
    }
}

public partial class PlayerMoveAttack : PlayerState
{
    public PlayerMoveAttack(Animator animator) : base(animator)
    {
    }
}

public partial class PlayerHold : PlayerState
{
    public PlayerHold(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("hold");
    }
}

public partial class PlayerHurl : PlayerState
{
    public PlayerHurl(Animator animator) : base(animator)
    {
    }
	
	public override void Enter()
    {
        ChangeAnimation("hurl");
    }
}

public partial class PlayerParry : PlayerState
{
	public PlayerParry(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("parry");
    }
}

public partial class PlayerStunned : PlayerState
{
    public PlayerStunned(Animator animator) : base(animator)
    {
    }
}

public partial class PlayerDead : PlayerState
{
	public PlayerDead(Animator animator) : base(animator)
    {
    }

	public override void Enter()
    {
        ChangeAnimation("death");
    }
}