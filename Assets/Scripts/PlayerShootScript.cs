using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using System;

public class PlayerShootScript : NetworkBehaviour
{
    
    private static GunSO gunSO;
    private float timeSinceLastShot;
    [SerializeField] private Transform cameraTransform;
    void Start()
    {
        GameInput.Instance.OnAttackAction += OnAttackAction;
    }

    private void OnAttackAction(object sender, EventArgs e)
    {
        Shoot(cameraTransform);
    }

    public static void OnWeaponChange(GunSO gunSo){
        gunSO = gunSo;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward);
    }

    public void Shoot(Transform cameraTransform){
        this.cameraTransform = cameraTransform;
        Shoot_ServerRpc();
    }

    [ServerRpc(RequireOwnership =false)]
    private void Shoot_ServerRpc(){
        Shoot_ClientRpc();
    }

    [ClientRpc]
    private void Shoot_ClientRpc(){
        if(!IsOwner||gunSO==null){
            return;
        }
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
                        damage.ReciveDamage(gunSO,cameraTransform.forward);
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
}
