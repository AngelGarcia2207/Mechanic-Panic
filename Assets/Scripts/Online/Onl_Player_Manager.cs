using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Onl_Player_Manager : NetworkBehaviour
{
    private int nextPlayerID = 0; // ID inicial.
    private List<Onl_Player_Controller> connectedPlayersOnl = new List<Onl_Player_Controller>();
    private List<Mov_Player_Controller> connectedPlayersMov = new List<Mov_Player_Controller>();


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

    IEnumerator WaitToChange()
    {
        yield return new WaitForSeconds(5f);
        int i = 0;
        foreach (Onl_Player_Controller player in connectedPlayersOnl)
        {
            player.PlayerIDSetter(0, i);
            i++;
        }
    }

    public void ChangeAllCharacterssss()
    {
        StartCoroutine(WaitToChange());
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
