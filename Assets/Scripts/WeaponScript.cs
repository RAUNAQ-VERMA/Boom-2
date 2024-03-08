using Unity.Netcode;
using UnityEngine;

public class WeaponScript : NetworkBehaviour
{
 // Start is called before the first frame update
    [SerializeField] private GunSO gunSO;
//    [SerializeField] private Transform muzzle;

    private float timeSinceLastShot;


    private IWeaponParent weaponObjectParent;

    private FollowPlayerScript followTransform;

    private void Start() {
       
    }
    private void Awake() {
        followTransform = GetComponent<FollowPlayerScript>();
    }

    public static void SpawnWeapon(IWeaponParent weaponObjectParent){
        WeaponSpawnScript.Instance.SpawnWeapon(weaponObjectParent);
    }
    public void SetWeaponParent(IWeaponParent weaponObjectParent)
    {
        SetWeaponParent_ServerRpc(weaponObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetWeaponParent_ServerRpc(NetworkObjectReference weaponReference){
        SetWeaponParent_ClientRpc(weaponReference);
    }

    [ClientRpc]
    private void SetWeaponParent_ClientRpc(NetworkObjectReference weaponReference){
        weaponReference.TryGet(out NetworkObject weaponNetworkObject);
        IWeaponParent weaponObjectParent = weaponNetworkObject.GetComponent<IWeaponParent>();

        if(this.weaponObjectParent != null){
            //remove weapons
        }
        this.weaponObjectParent = weaponObjectParent;

        if(weaponObjectParent.HasWeapon()){
            Debug.LogError("IWeaponParent already has a weapon");
        }

        weaponObjectParent.SetCurrentWeapon(this);
        followTransform.SetTargetTransform(weaponObjectParent.GetWeaponHolderTransform());
    }

    public IWeaponParent GetWeaponParent(){
        return weaponObjectParent;
    }






















    // private void Update()
    // {
    //     timeSinceLastShot += Time.deltaTime;
    //     Debug.DrawRay(muzzle.position, muzzle.forward);
    // }
    // public void Shoot()
    // {

    //     if (gunSO.currentAmmo > 0)
    //     {
    //         if (CanShoot())
    //         {
    //             if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunSO.maxDistance))
    //             {
    //                 Debug.Log(hitInfo.transform.name);
    //             }
    //             gunSO.currentAmmo--;
    //             timeSinceLastShot = 0;

    //         }
    //     }

    // }
    // private bool CanShoot() => !gunSO.reloading && timeSinceLastShot > 1f / (gunSO.fireRate / 60f);

}
