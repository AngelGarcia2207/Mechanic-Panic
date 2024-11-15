using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Onl_Player_Manager : NetworkBehaviour
{
    private int nextPlayerID = 0; // ID inicial.
    private List<Onl_Player_Controller> connectedPlayersOnl = new List<Onl_Player_Controller>();
    private List<Mov_Player_Controller> connectedPlayersMov = new List<Mov_Player_Controller>();


    public List<GameObject> playerCards = new List<GameObject>();

    [HideInInspector] public int remainingLivesOnl;

    [SerializeField] private TextMeshProUGUI playerIDsText;

    void Start()
    {
        StartCoroutine(ShowPlayersIDs());
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
        else
        {
            StartCoroutine(ShowPlayersIDs());
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Encuentra al jugador conectado por el clientId.
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        var onlController = playerObject.GetComponent<Onl_Player_Controller>();
        var movController = playerObject.GetComponent<Mov_Player_Controller>();

        if (onlController != null)
        {
            movController.onlManager = this;
            // Asigna el siguiente playerID y lo incrementa para el próximo jugador.
            onlController.SetPlayerIDServerRPC(nextPlayerID);
            nextPlayerID++;

            // Añadir el jugador a la lista de jugadores conectados.
            connectedPlayersOnl.Add(onlController);
            connectedPlayersMov.Add(movController);
            movController.AddPlayerCardToNetManager(this);
            ChangeAllCharacterssss();
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        // Intenta obtener el jugador desconectado usando la lista de jugadores conectados.
        Onl_Player_Controller disconnectedPlayer = connectedPlayersOnl.Find(p => p.OwnerClientId == clientId);

        if (disconnectedPlayer != null)
        {
            Debug.Log($"Player {disconnectedPlayer.playerID.Value} disconnected. PlayerCard: {disconnectedPlayer.playerCard}");

            // Eliminar el playerCard de la lista y destruirlo visualmente.
            if (disconnectedPlayer.playerCard != null)
            {
                playerCards.Remove(disconnectedPlayer.playerCard);
                Destroy(disconnectedPlayer.playerCard); // Destruir el objeto de la interfaz.
            }

            // Remover al jugador de la lista de jugadores conectados.
            connectedPlayersOnl.Remove(disconnectedPlayer);
            connectedPlayersMov.Remove(disconnectedPlayer.GetComponent<Mov_Player_Controller>());
        }

        // Decrementar el ID para el próximo jugador.
        nextPlayerID--;
    }

    public void ChangeAllCharacterssss()
    {
        StartCoroutine(WaitToChange());
    }
    IEnumerator WaitToChange()
    {
        yield return new WaitForSeconds(1f);

        foreach (Onl_Player_Controller player in connectedPlayersOnl)
        {
            player.PlayerIDSetter(0, player.playerID.Value);
        }
    }

    private void OnDestroy()
    {
        // Limpiar el callback al destruir el objeto para evitar errores.
        if (NetworkManager.Singleton != null && IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }



    // Remaining Lives General
    public void TryRemLivesOnline(int value)
    {
        if (!IsOwner) { return; }
        if (IsServer)
        {
            remainingLivesOnl = value;
            RemLivesClientRPC(value);
        }
        else
        {
            RemLivesServerRPC(value);
        }
    }

    private void OnRemLivesChanged(int oldValue, int newValue)
    {
        remainingLivesOnl = newValue;
    }

    [ServerRpc]
    public void RemLivesServerRPC(int value)
    {
        remainingLivesOnl = value;
        RemLivesClientRPC(value);
    }

    [ClientRpc]
    public void RemLivesClientRPC(int value)
    {
        remainingLivesOnl = value;
    }

    IEnumerator ShowPlayersIDs()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);
            string playersIDs = "";
            foreach (Onl_Player_Controller player in connectedPlayersOnl)
            {
                playersIDs += player.playerID.Value + " ";
            }
            playerIDsText.text = playersIDs;
        }
    }
}
