using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Onl_Player_Manager : NetworkBehaviour
{
    private int nextPlayerID = 0; // ID inicial.
    private List<Onl_Player_Controller> connectedPlayers = new List<Onl_Player_Controller>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        // Encuentra al jugador conectado por el clientId.
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        var playerController = playerObject.GetComponent<Onl_Player_Controller>();

        if (playerController != null)
        {
            // Asigna el siguiente playerID y lo incrementa para el próximo jugador.
            playerController.SetPlayerIDServerRPC(nextPlayerID);
            nextPlayerID++;

            // Añadir el jugador a la lista de jugadores conectados.
            connectedPlayers.Add(playerController);
        }
    }

    private void OnDestroy()
    {
        // Limpiar el callback al destruir el objeto para evitar errores.
        if (NetworkManager.Singleton != null && IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}
