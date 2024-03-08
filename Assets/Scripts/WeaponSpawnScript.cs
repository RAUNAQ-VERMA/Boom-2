using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnScript : MonoBehaviour
{
    public Transform spawnedWeapon;
    public static WeaponSpawnScript Instance{get;private set;}
    [SerializeField] private List<GameObject> weapons;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    public void SpawnWeapon(IWeaponParent weaponObjectParent){
        SpawnWeapon_ServerRpc(weaponObjectParent);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnWeapon_ServerRpc(IWeaponParent weaponObjectParent)
    {
        // Instantiate the weapon on the server and is not setting its parent to the weaponHolder
        SpawnWeapon_ClientRpc(weaponObjectParent);
    }

    [ClientRpc]
    private void SpawnWeapon_ClientRpc(IWeaponParent weaponObjectParent)
    {
        // Instantiate the weapon on the server and set its parent to the weaponHolder
        spawnedWeapon = Instantiate(weapons[1].transform);
        NetworkObject spawnedWeaponNetworkObject = spawnedWeapon.GetComponent<NetworkObject>();
        spawnedWeaponNetworkObject.Spawn(true);

        WeaponScript  weaponObject = spawnedWeapon.GetComponent<WeaponScript>();

        //this is for equiping the weapon on spawn
        //weaponObject.SetWeaponParent(weaponObjectParent);
        
    }
    public Transform GetSpawnedWeapon(){
        return spawnedWeapon;
    }
}
