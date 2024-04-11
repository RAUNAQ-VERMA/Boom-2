
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLobbyScript : MonoBehaviour
{
    private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
    private float heartbeatTimer;
    private float listLobbiesTimer;
    private float heartbeatTimerMax = 15f;
    private Lobby joinedLobby;
    public static GameLobbyScript Instance{get;private set;}

    public EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    public class OnLobbyListChangedEventArgs:EventArgs{
        public List<Lobby> lobbyList;
    }
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
    }
    void Update()
    {
        HandleHeartbeat();
        HandlePeriodicListingLobbies();
    }
    private async Task<Allocation> AllocateRelay(){
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(GameMultiplayerScript.MAX_PLAYER_AMOUNT-1);
            return allocation;
        }
        catch(RelayServiceException e){
            Debug.Log(e);
            return default;
        }
    }
    private async Task<string> GetRelayJoinCode(Allocation allocation){
        try{
           string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
           return relayJoinCode;
        }
        catch(RelayServiceException e){
            Debug.Log(e);
            return default;
        }
    }
    private async Task<JoinAllocation> JoinRelay(string joinCode){
        try{
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }
        catch(RelayServiceException e){
            Debug.Log(e);
            return default;
        }
    }
    private void HandlePeriodicListingLobbies(){
        if(joinedLobby==null&&AuthenticationService.Instance.IsSignedIn&&SceneManager.GetActiveScene().name==Loader.Scene.LobbyScene.ToString()){
            listLobbiesTimer-=Time.deltaTime;
            if(listLobbiesTimer<=0f){
                float listLobbiesTimerMax = 3f;
                listLobbiesTimer = listLobbiesTimerMax;
                ListLobbies();
            }
        }
    }

    private async void InitializeUnityAuthentication(){
        if(UnityServices.State != ServicesInitializationState.Initialized){
            InitializationOptions initializationOptions = new();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0,10000).ToString());
            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void HandleHeartbeat(){
        if(IsLobbyHost()){
            heartbeatTimer-=Time.deltaTime;
            if(heartbeatTimer<=0){
                heartbeatTimer = heartbeatTimerMax;
                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }

        }
    }
    private bool IsLobbyHost(){
        return joinedLobby!=null&&joinedLobby.HostId ==AuthenticationService.Instance.PlayerId;
    }
    public async void ListLobbies(){
        try{
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Filters = new List<QueryFilter>{
                    new(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
                }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            
            OnLobbyListChanged?.Invoke(this,new OnLobbyListChangedEventArgs{
                lobbyList = queryResponse.Results
            });
        }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void CreateLobby(string lobbyName, bool isPrivate){
        try{
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,GameMultiplayerScript.MAX_PLAYER_AMOUNT,new CreateLobbyOptions{
            IsPrivate = isPrivate,
        });

        Allocation allocation = await AllocateRelay();
        string relayJoinCode =await GetRelayJoinCode(allocation);

        await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id,new UpdateLobbyOptions{
            Data = new Dictionary<string, DataObject>{
                {KEY_RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member,relayJoinCode)}
            }
        });

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation,"dtls"));
        GameMultiplayerScript.Instance.StartHost();
        Loader.LoadNetwork(Loader.Scene.CharacterLobbyScene);
        }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }
    public async void QuickJoinLobby(){
        try{
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            
            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation,"dtls"));

            GameMultiplayerScript.Instance.StartClient();
        }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }
    public async void JoinLobbyWithCode(string lobbyCode){
        try{
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation,"dtls"));

            GameMultiplayerScript.Instance.StartClient();
        }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }
    public async void JoinLobbyWithId(string lobbyId){
        try{
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyId);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation,"dtls"));

            GameMultiplayerScript.Instance.StartClient();
        }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }
    public Lobby GetLobby(){
        return joinedLobby;
    }
    public async void DeleteLobby(){
       try{
        if(joinedLobby!=null){
         await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
       }
       }
       catch(LobbyServiceException e){
        Debug.Log(e);
       }
    }
}
