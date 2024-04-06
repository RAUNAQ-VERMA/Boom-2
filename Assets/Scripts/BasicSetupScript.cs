using Unity.Netcode;
using UnityEngine;

public class BasicSetupScript : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
    private Camera sceneCamera;

    public override void OnNetworkSpawn(){
        string networkId = GetComponent<NetworkObject>().OwnerClientId.ToString();
        PlayerScript player = GetComponent<PlayerScript>();
        GameManagerScript.Instance.RegisterPlayer(networkId,player);
        Debug.Log(networkId);
    }
    void Start()
    {
        if(!IsLocalPlayer){
            for (int i = 0; i<componentsToDisable.Length; i++){
                componentsToDisable[i].enabled =false;
            }
        }
        else{
            sceneCamera = Camera.main;
            if(sceneCamera!=null){
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable() {
        if(sceneCamera!=null){
            sceneCamera.gameObject.SetActive(true);
        }
        GameManagerScript.Instance.UnRegisterPlayer(transform.name);
    }
}
