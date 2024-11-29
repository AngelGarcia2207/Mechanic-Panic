using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;

public class OnlTestRelay : MonoBehaviour
{
    public TMP_InputField joinCodeInput;
    public TextMeshProUGUI numeroTexto;

    [SerializeField] private GameObject characterSelection;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            string show = "Signed in " + AuthenticationService.Instance.PlayerId;
            Debug.Log(show);
            numeroTexto.text = show;
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // Register for player connection event
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerConnected;
        }
    }

    private void OnPlayerConnected(ulong clientId)
    {
        // Activate character selection for the local client
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Player joined. Activating character selection.");
            characterSelection.SetActive(true);
        }
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            string show = "Join Code: " + joinCode;
            Debug.Log(show);
            numeroTexto.text = show;

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData);

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinRelay()
    {
        string joinCode = joinCodeInput.text;

        try
        {
            string show = "Joining relay with code: " + joinCode;
            Debug.Log(show);
            numeroTexto.text = show;

            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
