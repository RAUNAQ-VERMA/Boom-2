using System;
using Unity.Netcode;
using UnityEngine;

public class WeaponScript : NetworkBehaviour
{
 // Start is called before the first frame update
    [SerializeField] public GunSO gunSO;

    private float timeSinceLastShot;


    private IWeaponParent weaponObjectParent;

    private FollowPlayerScript followTransform;
    private void Awake() {
        followTransform = GetComponent<FollowPlayerScript>();
    }

    private void GameInput_OnAttack(object sender, EventArgs e)
    {
        Debug.Log("Attacking");
        Shoot();
    }
    public static void SpawnWeapon(){
        WeaponSpawnLogicScript.Instance.SpawnWeapon();
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
         //   Debug.LogError("IWeaponParent already has a weapon");
        }

        weaponObjectParent.SetCurrentWeapon(this);
        GetComponent<BoxCollider>().enabled = false;
        followTransform.SetTargetTransform(weaponObjectParent.GetWeaponHolderTransform());
    }

    public IWeaponParent GetWeaponParent(){
        return weaponObjectParent;
    }
    public void Shoot()
    {
        if (gunSO.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunSO.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);
                }
                gunSO.currentAmmo--;
                timeSinceLastShot = 0;

            }
        }

    }
    private bool CanShoot() => !gunSO.reloading && timeSinceLastShot > 1f / (gunSO.fireRate / 60f);

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward);
    }
}
