using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;


public class Onl_Player_Controller : NetworkBehaviour
{
    public Mov_Player_Controller movController;
    private PlayerInput playerInput;
    [HideInInspector] public bool onlJumpButtonPressed = false;
    [HideInInspector] public bool onlDodgePressed = false;
    [HideInInspector] public bool onlPickUpPressed = false;
    [HideInInspector] public bool onlAttackPressed = false;

    [HideInInspector] public int clientServer;
    [HideInInspector] public Vector2 onlDirection2D;
    [HideInInspector] public bool isSever;

    [HideInInspector] public GameObject playerCard;

    void Start()
    {
        clientServer = 0;
        if (IsServer && IsLocalPlayer) { clientServer = 2; }
        else if (IsClient && IsLocalPlayer) { clientServer = 1; }

        isSever = IsServer;

        playerInput = GetComponent<PlayerInput>();

        // Suscribirse al cambio de valor de la NetworkVariable.
        playerID.OnValueChanged += OnPlayerIDChanged;

        movController = GetComponent<Mov_Player_Controller>();
    }

    void FixedUpdate()
    {
        if (clientServer > 0)
        {
            Vector2 direction2D = playerInput.actions["Direction"].ReadValue<Vector2>();
            MoveServerRPC(direction2D);
        }
    }

    [ServerRpc]
    void MoveServerRPC(Vector2 _input)
    {
        onlDirection2D = _input;
    }

    private void OnJump()
    {
        if (!IsOwner) { return; }
        if (IsServer) { Jump(); }
        else { JumpServerRPC(); }
    }

    [ServerRpc]
    void JumpServerRPC() { Jump(); }

    void Jump()
    {
        onlJumpButtonPressed = !onlJumpButtonPressed;
    }


    // DODGE INPUT
    private void OnDodgeHold()
    {
        if (!IsOwner) { return; }
        if (IsServer) { DodgeHold(); }
        else { DodgeHoldServerRPC(); }
    }

    [ServerRpc]
    void DodgeHoldServerRPC() { DodgeHold(); }

    void DodgeHold()
    {
        onlDodgePressed = !onlDodgePressed;
    }

    // PICKUP INPUT
    private void OnPickUpHold()
    {
        if (!IsOwner) { return; }
        if (IsServer) { PickUpHold(); }
        else { PickUpHoldServerRPC(); }
    }

    [ServerRpc]
    void PickUpHoldServerRPC() { PickUpHold(); }

    void PickUpHold()
    {
        onlPickUpPressed = !onlPickUpPressed;
    }

    // GRAB ACTION
    public void TryOnlineGrab()
    {
        if (!IsOwner) { return; }
        if (IsServer) { Grab(); }
        else { Grab(); }
    }

    [ServerRpc]
    void GrabServerRPC() { Grab(); }

    void Grab()
    {
        movController.Grab();
    }

    // ATTACK INPUT
    private void OnAttackHold()
    {
        if (!IsOwner) { return; }
        if (IsServer) { AttackHold(); }
        else { AttackHoldServerRPC(); }
    }

    [ServerRpc]
    void AttackHoldServerRPC() { AttackHold(); }

    void AttackHold()
    {
        onlAttackPressed = !onlAttackPressed;
    }



    // Asegúrate de que `playerCard` se establezca cuando se crea en `Mov_Player_Controller`.
    public void SetPlayerCard(GameObject card)
    {
        playerCard = card;
    }

    // Player ID

    // NetworkVariable para sincronizar el playerID entre todos los clientes.
    public NetworkVariable<int> playerID = new NetworkVariable<int>();

    // Método que el manager llama para asignar el ID.
    [ServerRpc(RequireOwnership = false)]
    public void SetPlayerIDServerRPC(int id)
    {
        if (IsServer)
        {
            playerID.Value = id;
        }
    }

    /*private void Start()
    {
        // Suscribirse al cambio de valor de la NetworkVariable.
        playerID.OnValueChanged += OnPlayerIDChanged;
    }*/

    private void OnPlayerIDChanged(int oldValue, int newValue)
    {
        // Notificar al `Mov_Player_Controller` del cambio de ID si es necesario.
        Mov_Player_Controller movController = GetComponent<Mov_Player_Controller>();
        if (movController != null)
        {
            movController.onlineIndex = newValue;
            movController.ChangeCharacter();
        }
    }
}
