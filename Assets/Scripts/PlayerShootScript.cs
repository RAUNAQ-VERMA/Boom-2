using UnityEngine;
using Unity.Netcode;
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
    }

    public void Shoot(Transform cameraTransform){
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
                    if(hitInfo.transform.tag == "Player")
                    {
                        PlayerScript player= GameManagerScript.Instance.GetPlayerFromId(hitInfo.transform.name);
                        ManageDamageScript damage = player.GetComponent<ManageDamageScript>();
                        DamageEventArgs damageInfo = new()
                        {
                            playerId = hitInfo.transform.name,
                            gunInfo = gunSO,
                            bulletTransform = transform.forward
                        };
                        //OnDamage?.Invoke(this,damageInfo);
                        DamageLogicScript.OnDamage?.Invoke(this , damageInfo);
                        //damage.ReciveDamage(gunSO,cameraTransform.forward);
                      //  DamageLogicScript.Instance.ReciveDamage(gunSO,transform.forward,hitInfo.transform.name.ToString());
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
