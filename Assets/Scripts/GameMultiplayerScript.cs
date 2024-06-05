using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMultiplayerScript : NetworkBehaviour
{
    private Transform spawnedWeapon;

    private WeaponScript currentWeapon;
    public static GameMultiplayerScript Instance{get;private set;}

    public static int MAX_PLAYER_AMOUNT = 2;
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private List<Transform> weaponSpawnPoints;

    int weaponIndex=0;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost(){
        NetworkManager.Singleton.StartHost();
    }
    public void StartClient(){
        NetworkManager.Singleton.StartClient();
    }

    public void SpawnWeapon(){
        SpawnWeapon_ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnWeapon_ServerRpc()
    {
        spawnedWeapon = Instantiate(weapons[weaponIndex%weapons.Count].transform, weaponSpawnPoints[0]);
        weaponIndex++;
        NetworkObject spawnedWeaponNetworkObject = spawnedWeapon.GetComponent<NetworkObject>();
        spawnedWeaponNetworkObject.Spawn(true);

        WeaponScript  weaponObject = spawnedWeapon.GetComponent<WeaponScript>();
    }
    public void DespawnWeapon(WeaponScript weapon){
        this.currentWeapon = weapon;
        DespawnWeapon_ServerRPC();
    }
    [ServerRpc(RequireOwnership =false)]
    public void DespawnWeapon_ServerRPC(){
        Destroy(currentWeapon);
        currentWeapon.GetComponent<NetworkObject>().Despawn();
        Debug.Log(currentWeapon.name+" despawned");
    }
    public Transform GetSpawnedWeapon(){
        return spawnedWeapon;
    }
}
