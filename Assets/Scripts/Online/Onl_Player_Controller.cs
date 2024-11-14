using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.Collections.LowLevel.Unsafe;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.TextCore.Text;


public class Onl_Player_Controller : NetworkBehaviour
{
    [SerializeField] private Mov_Player_Controller movController;
    private PlayerInput playerInput;
    [HideInInspector] public bool onlJumpButtonPressed = false;
    [HideInInspector] public bool onlDodgePressed = false;
    [HideInInspector] public bool onlPickUpPressed = false;
    [HideInInspector] public bool onlAttackPressed = false;

    [HideInInspector] public int clientServer;
    [HideInInspector] public Vector2 onlDirection2D;
    [HideInInspector] public bool isSever;

    [HideInInspector] public GameObject playerCard;

    [HideInInspector] public int currentHealthOnl;


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

        if (IsOwner)
        {
            GameObject.FindAnyObjectByType<Loc_Character_Select>().onlController = this;
        }
    }

    public void TryOnlineChooseCharacter(int characterID)
    {
        if (IsOwner)
        {
            if (IsServer) { ChooseCharacterClientRPC(characterID); }
            else { ChooseCharacterServerRPC(characterID); }
        }
    }

    [ServerRpc]
    public void ChooseCharacterServerRPC(int characterID)
    {
        ChooseCharacter(characterID);
        ChooseCharacterClientRPC(characterID);
    }

    [ClientRpc]
    public void ChooseCharacterClientRPC(int characterID)
    {
        ChooseCharacter(characterID);
    }

    private void ChooseCharacter(int characterID)
    {
        movController.ChangeCharacter(characterID);
    }

    void FixedUpdate()
    {
        if (clientServer > 0)
        {
            Vector2 direction2D = playerInput.actions["Direction"].ReadValue<Vector2>();
            MoveServerRPC(direction2D);
        }
    }


    // MOVEMENT INPUT

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
        movController.Grab();
        GrabClientRPC();
        /*
        if (!IsOwner) { return; }
        if (IsServer)
        {
            movController.Grab();
            GrabClientRPC(); // Llama a todos los clientes para ejecutar la acción de Grab
        }
        else
        {
            GrabServerRPC();
        }*/
    }

    // Modificar GrabServerRPC para llamar a GrabClientRPC
    [ServerRpc]
    void GrabServerRPC()
    {
        movController.Grab();
        GrabClientRPC(); // Notifica a todos los clientes cuando el servidor ejecuta Grab
    }

    [ClientRpc]
    void GrabClientRPC()
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

    // ATTACK ACTION
    public void TryOnlineAttack()
    {
        movController.Attack();
        AttackClientRPC();
        /*if (!IsOwner) { return; }
        if (IsServer)
        {
            movController.Attack();
            AttackClientRPC(); // Llama a todos los clientes para ejecutar la acción de Grab
        }
        else
        {
            AttackServerRPC();
        }*/
    }

    // Modificar GrabServerRPC para llamar a GrabClientRPC
    [ServerRpc]
    void AttackServerRPC()
    {
        movController.Grab();
        AttackClientRPC(); // Notifica a todos los clientes cuando el servidor ejecuta Grab
    }

    [ClientRpc]
    void AttackClientRPC()
    {
        movController.Attack();
    }


    // RERECIVE DAMAGE
    public void TryOnlineDamaged(int damage)
    {
        if (!IsOwner) { return; }
        if (IsServer)
        {
            movController.receiveDamageReal(damage);
            DamageClientRPC(damage);
        }
        else
        {
            DamageServerRPC(damage);
        }
    }

    [ServerRpc]
    void DamageServerRPC(int damage)
    {
        movController.receiveDamageReal(damage);
        DamageClientRPC(damage);
    }

    [ClientRpc]
    void DamageClientRPC(int damage)
    {
        movController.receiveDamageReal(damage);
    }

    // HEALTH
    public void Health(int health)
    {
        if (!IsOwner) { return; }
        if (IsServer)
        {
            currentHealthOnl = health;
            SetHealthClientRPC(health);
        }
        else
        {
            SetHealthServerRPC(health);
        }
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        currentHealthOnl = newValue;
    }

    [ServerRpc]
    public void SetHealthServerRPC(int health)
    {
        currentHealthOnl = health;
        SetHealthClientRPC(health);
    }

    [ClientRpc]
    public void SetHealthClientRPC(int health)
    {
        currentHealthOnl = health;
    }


    



    // REVIVE
    public void TryOnlineRevive()
    {
        movController.Revive();
        ReviveClientRPC();
    }

    [ServerRpc]
    void ReviveServerRPC()
    {
        movController.Revive();
        ReviveClientRPC();
    }

    [ClientRpc]
    void ReviveClientRPC()
    {
        movController.Revive();
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
        PlayerID(oldValue, newValue);
    }

    public void PlayerIDSetter(int oldValue, int newValue)
    {
        PlayerID(oldValue, newValue);
        PlayerIDClientRPC(oldValue, newValue);
        /*if (!IsOwner) { return; }
        if (IsServer)
        {
            PlayerID(oldValue, newValue);
            PlayerIDClientRPC(oldValue, newValue); // Llama a todos los clientes para ejecutar la acción de Grab
        }
        else
        {
            PlayerIDServerRPC(oldValue, newValue);
        }*/
    }

    [ServerRpc]
    private void PlayerIDServerRPC(int oldValue, int newValue)
    {
        PlayerID(oldValue, newValue);
    }

    [ClientRpc]
    private void PlayerIDClientRPC(int oldValue, int newValue)
    {
        PlayerID(oldValue,newValue);
    }

    private void PlayerID(int oldValue, int newValue)
    {
        // Notificar al `Mov_Player_Controller` del cambio de ID si es necesario.
        Mov_Player_Controller movController = GetComponent<Mov_Player_Controller>();
        if (movController != null)
        {
            if (newValue != 100)
            { movController.onlineIndex = newValue; }
            movController.ChangeCharacter(-1);
        }
    }
}
