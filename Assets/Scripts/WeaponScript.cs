using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class WeaponScript : NetworkBehaviour
{
    [SerializeField] private GunSO gunSO;
    [SerializeField] private ParticleSystem muzzleFlash;

    private float timeSinceLastShot;

    private Transform cameraTransform;
    private IWeaponParent weaponObjectParent;

    private FollowPlayerScript followTransform;
    private void Start() {
        gameObject.transform.position = new Vector3(0,-6,0);//change this to the weapon spawning location
    }
    private void Awake() {
        followTransform = GetComponent<FollowPlayerScript>();
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
        followTransform.SetTargetTransform(weaponObjectParent.GetWeaponHolderTransform(),PlayerScript.LocalInstance.GetCameraTransform());
        PlayerShootScript.OnWeaponChange(gunSO);
    }

    public IWeaponParent GetWeaponParent(){
        return weaponObjectParent;
    }
    public void Shoot(Transform cameraTransform)
    {
        // if(!IsOwner){
        //     return;
        // }
        // this.cameraTransform = cameraTransform;
        // Shoot_ServerRpc();
    }
    [ServerRpc(RequireOwnership =false)]
    private void Shoot_ServerRpc(){
        Shoot_ClientRpc();
    }
    [ClientRpc]
    private void Shoot_ClientRpc(){
       if (gunSO.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo, gunSO.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name +" || " +hitInfo.transform.tag);
                    if(hitInfo.transform.tag == "Player"){
                        PlayerScript player= GameManagerScript.Instance.GetPlayerFromId(hitInfo.transform.name);
                        ManageDamageScript damage = player.GetComponent<ManageDamageScript>();
                       // damage.ReciveDamage(gunSO,cameraTransform.forward);
                       // DamageLogicScript.Instance.ReciveDamage(gunSO,transform.forward,hitInfo.transform.name.ToString());
                       // player.GetComponent<ManageDamageScript>().ReciveDamage(gunSO,cameraTransform.forward);
                    }
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
    public void PlayMuzzleFlash(){
        muzzleFlash.Play();
    }
}
