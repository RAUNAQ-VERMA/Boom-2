using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnLogicScript : NetworkBehaviour
{
    private Transform spawnedWeapon;
    public static WeaponSpawnLogicScript Instance{get;private set;}
    IWeaponParent weaponObjectParent;
    [SerializeField] private List<GameObject> weapons;
    // Start is called before the first frame update

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SpawnWeapon(){
       // this.weaponObjectParent = weaponObjectParent; 
        SpawnWeapon_ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnWeapon_ServerRpc()
    {
        // Instantiate the weapon on the server and is not setting its parent to the weaponHolder
        //SpawnWeapon_ClientRpc(weaponObjectParent);
        spawnedWeapon = Instantiate(weapons[1].transform);
        NetworkObject spawnedWeaponNetworkObject = spawnedWeapon.GetComponent<NetworkObject>();
        spawnedWeaponNetworkObject.Spawn(true);

        WeaponScript  weaponObject = spawnedWeapon.GetComponent<WeaponScript>();
    }

    // [ClientRpc]
    // private void SpawnWeapon_ClientRpc(IWeaponParent weaponObjectParent)
    // {
    //     //Instantiate the weapon on the server and set its parent to the weaponHolder
        
    //     //this is for equiping the weapon on spawn
    //the below code is cut and pasted to serverRpc
    //     spawnedWeapon = Instantiate(weapons[1].transform);
    //     NetworkObject spawnedWeaponNetworkObject = spawnedWeapon.GetComponent<NetworkObject>();
    //     spawnedWeaponNetworkObject.Spawn(true);

    //     WeaponScript  weaponObject = spawnedWeapon.GetComponent<WeaponScript>();
    //     weaponObject.SetWeaponParent(weaponObjectParent);
        
    // }
    public Transform GetSpawnedWeapon(){
        return spawnedWeapon;
    }
}
