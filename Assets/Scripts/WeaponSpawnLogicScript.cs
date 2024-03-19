using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnLogicScript : NetworkBehaviour
{
    private Transform spawnedWeapon;
    public static WeaponSpawnLogicScript Instance{get;private set;}

    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private List<Transform> weaponSpawnPoints;
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SpawnWeapon(){
        SpawnWeapon_ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnWeapon_ServerRpc()
    {
        spawnedWeapon = Instantiate(weapons[1].transform, weaponSpawnPoints[0]);
        NetworkObject spawnedWeaponNetworkObject = spawnedWeapon.GetComponent<NetworkObject>();
        spawnedWeaponNetworkObject.Spawn(true);

        WeaponScript  weaponObject = spawnedWeapon.GetComponent<WeaponScript>();
    }
    public Transform GetSpawnedWeapon(){
        return spawnedWeapon;
    }
}
