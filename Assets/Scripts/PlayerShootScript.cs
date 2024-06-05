using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerShootScript : NetworkBehaviour
{
    public static EventHandler OnShoot;
    public static EventHandler OnHammerSwing;
    public static EventHandler<DamageEventArgs> OnDamage;
    private static GunSO gunSO;
    DamageEventArgs damageInfo;
     ManageDamageScript damage;
     RaycastHit hitInfo;
    private float timeSinceLastShot;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask playerLayer;
    void Start()
    {
        GameInput.Instance.OnAttackAction += OnAttackAction;
    }

    private void OnAttackAction(object sender, EventArgs e)
    {
        Shoot();
    }

    public static void OnWeaponChange(GunSO gunSo){
        gunSO = gunSo;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public void Shoot(){
        if(!IsOwner||gunSO==null||PlayerScript.LocalInstance.IsPlayerEmptyHanded()){
            return;
        }
        if (gunSO.currentAmmo > 0)
        {
            if (CanShoot())
            {
                PlayerScript.LocalInstance.GetCurrentWeapon().PlayMuzzleFlash();
                if(PlayerScript.LocalInstance.GetCurrentWeapon().CompareTag("Shotgun"))
                {
                    OnShoot?.Invoke(this,EventArgs.Empty);
                    if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, gunSO.maxDistance,playerLayer))
                    {
                        Debug.Log(hitInfo.transform.name +" || " +hitInfo.transform.tag);
                        if(hitInfo.transform.CompareTag("Player"))
                        {
                            PlayerScript player= GameManagerScript.Instance.GetPlayerFromId(hitInfo.transform.name);
                            damage = player.GetComponent<ManageDamageScript>();
                            
                            damageInfo = new()
                            {
                                playerId = hitInfo.transform.name,
                                gunInfo = gunSO,
                                bulletTransform = transform.forward
                            };
                            DamageEvent_ServerRpc();

                            
                            // DamageLogicScript.OnDamage?.Invoke(this , damageInfo);
                            //damage.ReciveDamage(cameraTransform.forward,gunSO);
                        // player.GetComponent<HealthBarUIScript>().Damage((int)gunSO.damage);
                        //  DamageLogicScript.Instance.ReciveDamage(gunSO,transform.forward,hitInfo.transform.name.ToString());
                        // player.GetComponent<ManageDamageScript>().ReciveDamage(gunSO,cameraTransform.forward);
                        }
                    }
                }
                if(PlayerScript.LocalInstance.GetCurrentWeapon().CompareTag("Hammer"))
                {
                    OnHammerSwing?. Invoke(this,EventArgs.Empty);
                    PlayerAnimatorScript.Instance.HammerSwingAnimation();
                    if (Physics.SphereCast(cameraTransform.position,gunSO.maxDistance,cameraTransform.forward,out hitInfo,playerLayer))
                    {
                        Debug.Log(hitInfo.transform.name +" || " +hitInfo.transform.tag);
                        if(hitInfo.transform.CompareTag("Player"))
                        {
                            PlayerScript player= GameManagerScript.Instance.GetPlayerFromId(hitInfo.transform.name);
                            damage = player.GetComponent<ManageDamageScript>();
                            
                            damageInfo = new()
                            {
                                playerId = hitInfo.transform.name,
                                gunInfo = gunSO,
                                bulletTransform = transform.forward
                            };
                            DamageEvent_ServerRpc();

                            
                        // DamageLogicScript.OnDamage?.Invoke(this , damageInfo);
                        //damage.ReciveDamage(cameraTransform.forward,gunSO);
                        // player.GetComponent<HealthBarUIScript>().Damage((int)gunSO.damage);
                        //  DamageLogicScript.Instance.ReciveDamage(gunSO,transform.forward,hitInfo.transform.name.ToString());
                        // player.GetComponent<ManageDamageScript>().ReciveDamage(gunSO,cameraTransform.forward);
                        }
                    }
                }

                gunSO.currentAmmo--;
                timeSinceLastShot = 0;
            }
        }
    }
    private bool CanShoot() => !gunSO.reloading && timeSinceLastShot > 1f / (gunSO.fireRate / 60f);
    [ServerRpc(RequireOwnership =false)]
    private void SetDamageData_ServerRpc(){
        damage.knockback.Value =  transform.forward * gunSO.damage;
        damage.playerId.Value = (int)hitInfo.transform.name.ToCharArray()[^1] ;
    }

    [ServerRpc(RequireOwnership =false)]
    private void DamageEvent_ServerRpc(){
        DamageEvent_ClientRpc();
    }
    [ClientRpc]
    private void DamageEvent_ClientRpc(){
        OnDamage?.Invoke(this,damageInfo);
    }
}
