using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class Lobbyist : MonoBehaviour
{
    public int m_MaxConnections = 32;
    public string RelayJoinCode = "";
    public string PlayerID = "";
    public TextMeshProUGUI DebugText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI StatusText;
    public TextMeshProUGUI AnnounceText;
    public TextMeshProUGUI UpdateText;
    public static string Text = "";
    public TMP_InputField NamePick;
    public TMP_InputField CodePick;
    public GameObject LoginScreen;

    private void Awake()
    {
        God.HPText = HPText;
        God.StatusText = StatusText;
        God.AnnounceText = AnnounceText;
        God.UpdateText = UpdateText;
    }

    void Start()
    {
        Login();
    }
    
    void Update()
    {
        if (PlayerID == "") return;
        DebugText.text = Text;
        // if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ConnectHost());
        // if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(ConnectClient());
    }

    public void Connect()
    {
        
        God.NamePick = NamePick.text;
        if (CodePick.text == "")
        {
            StartCoroutine(ConnectHost());
        }
        else
        {
            RelayJoinCode = CodePick.text;
            StartCoroutine(ConnectClient());
        }
    }

    public async void Login()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            PlayerID = AuthenticationService.Instance.PlayerId;
            //Text = PlayerID;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public IEnumerator ConnectHost()
    {
        var serverRelayUtilityTask = AllocateRelayServerAndGetJoinCode(m_MaxConnections);
        while (!serverRelayUtilityTask.IsCompleted)
        {
            yield return null;
        }
        if (serverRelayUtilityTask.IsFaulted)
        {
            Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
            yield break;
        }

        var (ipv4address, port, allocationIdBytes, connectionData, key, joinCode) = serverRelayUtilityTask.Result;
        RelayJoinCode = joinCode;
        Text = RelayJoinCode;
// Display the joinCode to the user.

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(ipv4address, port, allocationIdBytes, key, connectionData, true);
        yield return null;
        NetworkManager.Singleton.StartHost();
        LoginScreen.SetActive(false);
        
    }

    public IEnumerator ConnectClient()
    {
        var clientRelayUtilityTask = JoinRelayServerFromJoinCode(RelayJoinCode);

        while (!clientRelayUtilityTask.IsCompleted)
        {
            yield return null;
        }

        if (clientRelayUtilityTask.IsFaulted)
        {
            Debug.LogError("Exception thrown when attempting to connect to Relay Server. Exception: " + clientRelayUtilityTask.Exception.Message);
            yield break;
        }

        var (ipv4address, port, allocationIdBytes, connectionData, hostConnectionData, key) = clientRelayUtilityTask.Result;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(ipv4address, port, allocationIdBytes, key, connectionData, hostConnectionData, true);

        NetworkManager.Singleton.StartClient();
        yield return null;
        LoginScreen.SetActive(false);
    }
    
    public static async Task<(string ipv4address, ushort port, byte[] allocationIdBytes, byte[] connectionData, byte[] key, string joinCode)> AllocateRelayServerAndGetJoinCode(int maxConnections, string region = null)
    {
        Allocation allocation;
        string createJoinCode;
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;
        }

        // Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        // Debug.Log($"server: {allocation.AllocationId}");

        try
        {
            createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch
        {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
        return (dtlsEndpoint.Host, (ushort)dtlsEndpoint.Port, allocation.AllocationIdBytes, allocation.ConnectionData, allocation.Key, createJoinCode);
    }
    
    public static async Task<(string ipv4address, ushort port, byte[] allocationIdBytes, byte[] connectionData, byte[] hostConnectionData, byte[] key)> JoinRelayServerFromJoinCode(string joinCode)
    {
        JoinAllocation allocation;
        try
        {
            allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch
        {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        // Debug.Log($"client: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        // Debug.Log($"host: {allocation.HostConnectionData[0]} {allocation.HostConnectionData[1]}");
        // Debug.Log($"client: {allocation.AllocationId}");

        var dtlsEndpoint = allocation.ServerEndpoints.First(e => e.ConnectionType == "dtls");
        return (dtlsEndpoint.Host, (ushort)dtlsEndpoint.Port, allocation.AllocationIdBytes, allocation.ConnectionData, allocation.HostConnectionData, allocation.Key);
    }
}
