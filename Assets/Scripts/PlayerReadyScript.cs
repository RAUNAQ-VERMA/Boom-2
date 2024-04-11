using System.Collections.Generic;
using Unity.Netcode;

public class PlayerReadyScript : NetworkBehaviour
{

    public static PlayerReadyScript Instance{get;private set;}

    private Dictionary<ulong,bool> playerReadyDictionary;

    private void Awake() {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady(){
        SetPlayerReady_ServerRpc();
    }

    [ServerRpc(RequireOwnership =false)]
    private void SetPlayerReady_ServerRpc(ServerRpcParams rpcParams= default){
        playerReadyDictionary[rpcParams.Receive.SenderClientId] = true;
        bool allClientsReady = true;
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds){
            if(!playerReadyDictionary.ContainsKey(clientId)||!playerReadyDictionary[clientId]){
                allClientsReady=false;
                break;
            }
        }
        if(allClientsReady){
            GameLobbyScript.Instance.DeleteLobby();
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }
}
