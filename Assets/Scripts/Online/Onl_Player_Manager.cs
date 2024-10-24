using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Onl_Player_Manager : NetworkBehaviour
{
    private int nextPlayerID = 0; // ID inicial.
    private List<Onl_Player_Controller> connectedPlayers = new List<Onl_Player_Controller>();

    public List<GameObject> playerCards = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
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
            connectedPlayers.Add(onlController);
            movController.AddPlayerCardToNetManager(this);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        // Intenta obtener el jugador desconectado usando la lista de jugadores conectados.
        Onl_Player_Controller disconnectedPlayer = connectedPlayers.Find(p => p.OwnerClientId == clientId);

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
            connectedPlayers.Remove(disconnectedPlayer);
        }

        // Decrementar el ID para el próximo jugador.
        nextPlayerID--;
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
}
