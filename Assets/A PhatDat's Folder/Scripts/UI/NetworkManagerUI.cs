using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.UI;
using Unity.Networking.Transport.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using System.Collections.Generic;


public class NetworkManagerUI : MonoBehaviour
{
    //Trường này để gán các UI vào 
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    //[SerializeField] private Button createLobbyBtn;
    //[SerializeField] private Button joinLobbyBtn;
    //[SerializeField] private Button listLobbyBtn;
    [SerializeField] private TMP_InputField joinInput;
    [SerializeField] private TextMeshProUGUI codeText;

    private Lobby hostLobby;
    private float heartbeatTimer;

    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log(AuthenticationService.Instance.PlayerId);

        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //createLobbyBtn.onClick.AddListener(CreateLobby);
        //joinLobbyBtn.onClick.AddListener(JoinLobby);
        //listLobbyBtn.onClick.AddListener(ListLobbies);
        hostBtn.onClick.AddListener(CreateRelay);
        clientBtn.onClick.AddListener(() => JoinRelay(joinInput.text));

    }

    //void Update()
    //{
    //    HandleLobbyHeartbeat();
    //}

    //async void HandleLobbyHeartbeat()
    //{
    //    if (hostLobby != null)
    //    {
    //        heartbeatTimer -= Time.deltaTime;
    //        if (heartbeatTimer < 0f)
    //        {
    //            float heartbeatTimerMax = 15;
    //            heartbeatTimer = heartbeatTimerMax;

    //            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
    //        }
    //    }
    //}

    //Hàm tạo relay và code
    async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            if (allocation == null)
            {
                Debug.Log("Allocation failed!");
                return;
            }

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            codeText.text = "Code: " + joinCode;

            var relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e.Message);
        }
    }


    async void JoinRelay(string joinCode)
    {
        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            if (joinAllocation == null)
            {
                Debug.Log("Join allocation failed!");
                //debugText.text = "Debug: Join allocation failed!";
                return;
            }

            var relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e.Message);
        }
    }


    //async void CreateLobby()
    //{
    //    try
    //    {
    //        string lobbyName = "My Lobby";
    //        int maxPlayers = 4;
    //        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

    //        Debug.Log("Create lobby! " + lobby.Name + " " + lobby.MaxPlayers);
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e.Message);
    //    }
    //}

    //async void ListLobbies()
    //{
    //    try
    //    {
    //        QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
    //        {
    //            Count = 25,
    //            Filters = new List<QueryFilter>
    //            {
    //                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
    //            },
    //            Order = new List<QueryOrder>
    //            {
    //                new QueryOrder(false, QueryOrder.FieldOptions.Created)
    //            }
    //        };

    //        QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

    //        Debug.Log("Lobbies found: " + queryResponse.Results.Count);
    //        //debugText.text = "Debug: Lobbies found: " + queryResponse.Results.Count;
    //        foreach (Lobby lobby in queryResponse.Results)
    //        {
    //            Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
    //            //debugText.text = "Debug: " + lobby.Name + " " + lobby.MaxPlayers;
    //        }
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e.Message);
    //    }
    //}

    //async void JoinLobby()
    //{
    //    try
    //    {
    //        QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

    //        await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e.Message);
    //    }
    //}

    //private void Awake()
    //{
    //    hostBtn.onClick.AddListener(() =>
    //    {
    //        if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsServer)
    //        {
    //            NetworkManager.Singleton.StartHost();
    //        }
    //        else
    //        {
    //            Debug.Log("Host is already running.");
    //        }
    //    });

    //    clientBtn.onClick.AddListener(() =>
    //    {
    //        if (!NetworkManager.Singleton.IsClient)
    //        {
    //            NetworkManager.Singleton.StartClient();
    //        }
    //        else
    //        {
    //            Debug.Log("Client is already running.");
    //        }
    //    });
    //}
}
